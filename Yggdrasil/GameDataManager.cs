using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Reflection;

using Yggdrasil.Forms;
using Yggdrasil.FileTypes;
using Yggdrasil.Helpers;
using Yggdrasil.TableParsers;

namespace Yggdrasil
{
    public class GameDataManager
    {
        static readonly string[] messageDirs = new string[] { "data\\Data\\CharaSel", "data\\Data\\Dungeon", "data\\Data\\Event", "data\\Data\\Opening", "data\\Data\\Param", "data\\Data\\SaveLoad" };
        static readonly string[] dataDirs = new string[] { "data\\Data\\Param", "data\\Data\\Battle" };

        public enum Versions { Invalid, European, American, Japanese };
        public enum Languages { German, English, Spanish, French, Italian };

        public string DataPath { get; private set; }

        public Header Header { get; private set; }
        public Versions Version { get; private set; }
        public Languages Language { get; set; }

        string mainFontFilename;
        Dictionary<Languages, string> langSuffixes = new Dictionary<Languages, string>()
        {
            { Languages.German, "_DE" },
            { Languages.English, "_EN" },
            { Languages.Spanish, "_ES" },
            { Languages.French, "_FR" },
            { Languages.Italian, "_IT" }
        };

        DataLoadWaitForm loadWaitForm;
        BackgroundWorker loadWaitWorker;

        public bool IsInitialized { get { return (Version != Versions.Invalid); } }
        public bool DataHasChanged { get { return (parsedData != null && parsedData.Any(x => x.HasChanged)); } }

        public FontRenderer FontRenderer { get; private set; }
        public List<TBB> MessageFiles { get; private set; }

        List<TBB> dataTableFiles;
        List<BaseParser> parsedData;

        public static Dictionary<ushort, string> ItemNames { get; private set; }

        public GameDataManager()
        {
            MessageFiles = null;
            dataTableFiles = null;
            parsedData = null;
        }

        public void ReadGameDirectory(string path)
        {
            DataPath = path;

            ReadHeaderIdentify();

            loadWaitWorker = new BackgroundWorker();
            loadWaitWorker.WorkerReportsProgress = true;
            loadWaitWorker.DoWork += ((s, e) =>
            {
                loadWaitWorker.ReportProgress(-1, "Generating character map...");
                EtrianString.GameVersion = Version;

                loadWaitWorker.ReportProgress(-1, "Initializing font renderer...");
                FontRenderer = new FontRenderer(this, Path.Combine(path, mainFontFilename));

                MessageFiles = ReadDataTablesByExtension(".mbb", messageDirs);
                dataTableFiles = ReadDataTablesByExtension(".tbb", dataDirs);

                EnsureMessageTableIntegrity();

                parsedData = new List<BaseParser>();
                parsedData.AddRange(ParseDataTable("Item.tbb"));
                parsedData.AddRange(ParseDataTable("ItemCompound.tbb"));

                loadWaitWorker.ReportProgress(-1, "Fetching item names...");
                FetchItemNames();
            });
            loadWaitWorker.ProgressChanged += ((s, e) =>
            {
                loadWaitForm.PrintStatus(e.UserState as string);
            });
            loadWaitWorker.RunWorkerCompleted += ((s, e) =>
            {
                loadWaitForm.Close();
            });
            loadWaitWorker.RunWorkerAsync();

            loadWaitForm = new DataLoadWaitForm();
            loadWaitForm.ShowDialog(Program.MainForm);
        }

        public int SaveAllChanges()
        {
            if (parsedData == null) return 0;

            List<TBB.ITable> needToSave = new List<TBB.ITable>();
            List<string> changedFiles = new List<string>();

            foreach (BaseParser data in parsedData.Where(x => x.HasChanged))
            {
                data.Save();
                needToSave.Add(data.ParentTable);
            }

            foreach (TBB.ITable table in needToSave.Distinct())
            {
                table.Save();

                TBB file = table.GetParent();
                if (!file.IsCompressed)
                {
                    changedFiles.Add(file.Filename);
                    BinaryWriter writer = new BinaryWriter(File.Open(file.Filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
                    writer.Write(file.Data);
                    writer.Close();
                }
                else
                    throw new NotImplementedException("Saving to compressed file not implemented");
            }

            return changedFiles.Distinct().Count();
        }

        private void ReadHeaderIdentify()
        {
            Header = new FileTypes.Header(this, Path.Combine(DataPath, "header.bin"));
            switch (Header.GameCode)
            {
                case "AKYP":
                    Version = Versions.European;
                    Language = Languages.English;
                    mainFontFilename = "data\\Data\\Tex\\Font\\Font14x11_00.cmp";
                    break;

                case "AKYE":
                    Version = Versions.American;
                    mainFontFilename = "data\\Data\\Tex\\Font\\Font10x5_00.cmp";
                    break;

                case "AKYJ":
                    Version = Versions.Japanese;
                    mainFontFilename = "data\\Data\\Tex\\Font\\Font10x10_00.cmp";
                    break;

                default: throw new Exception("Unsupported game data");
            }
        }

        private List<TBB> ReadDataTablesByExtension(string extension, string[] directories)
        {
            if (extension == null || extension == string.Empty) throw new ArgumentException("No extension given");
            if (directories == null) throw new ArgumentNullException("Directories is null");

            List<TBB> dataTables = new List<TBB>();

            foreach (string directory in directories)
            {
                List<string> filePaths = Directory.EnumerateFiles(Path.Combine(DataPath, directory), "*.*", SearchOption.AllDirectories)
                    .Where(x => x.ToLowerInvariant().EndsWith(extension) || x.ToLowerInvariant().EndsWith(".cmp"))
                    .ToList();

                foreach (string filePath in filePaths)
                {
                    TBB tbb = new TBB(this, filePath);
                    if (tbb.IsValid()) dataTables.Add(tbb);

                    loadWaitWorker.ReportProgress(-1, string.Format("Reading {0}...", Path.GetFileName(filePath)));
                }
            }

            return dataTables;
        }

        private void EnsureMessageTableIntegrity()
        {
            List<TBB> filesToRemove = new List<TBB>();
            foreach (TBB messageFile in MessageFiles)
            {
                bool removeFile = true;
                foreach (TBB.ITable table in messageFile.Tables)
                {
                    if (table is TBB.MTBL) removeFile = false;
                }

                if (removeFile) filesToRemove.Add(messageFile);
            }

            foreach (TBB file in filesToRemove) MessageFiles.Remove(file);
        }

        private List<BaseParser> ParseDataTable(string tableFileName)
        {
            TBB tableFile = dataTableFiles.FirstOrDefault(x => Path.GetFileName(x.Filename) == tableFileName);
            if (tableFile == null) throw new ArgumentException("Invalid table file name");

            loadWaitWorker.ReportProgress(-1, string.Format("Parsing {0}...", tableFileName));

            List<BaseParser> parsedData = new List<BaseParser>();

            List<Type> typesWithAttrib = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetCustomAttributes(typeof(ParserUsage), false).Length > 0).ToList();
            foreach (Type type in typesWithAttrib)
            {
                foreach (ParserUsage attrib in type.GetCustomAttributes(false).Where(x => x is ParserUsage && (x as ParserUsage).FileName == tableFileName))
                {
                    TBB.TBL1 table = (tableFile.Tables[attrib.TableNo] as TBB.TBL1);

                    for (int i = 0; i < table.Data.Length; i++)
                    {
                        parsedData.Add((BaseParser)Activator.CreateInstance(type, new object[] { this, table, i, (PropertyChangedEventHandler)ItemDataPropertyChanged }));
                    }
                }
            }
            return parsedData;
        }

        private void FetchItemNames()
        {
            ItemNames = new Dictionary<ushort, string>();

            ItemNames.Add(0, "(None)");

            foreach (BaseParser parser in parsedData.Where(x => x is BaseItemParser && !(x is ItemCompoundParser)))
            {
                ushort itemNumber = (parser as BaseItemParser).ItemNumber;
                ItemNames.Add(itemNumber, GetItemName(itemNumber));
            }
        }

        public string GetItemName(ushort itemNumber)
        {
            string value = GetMessageString("ItemName", 0, itemNumber - 1);
            return (value != string.Empty ? value : "(Unnamed)");
        }

        public string GetItemDescription(ushort itemNumber)
        {
            string value = GetMessageString("ItemInfo", 0, itemNumber - 1);
            return (value != string.Empty ? value : "(Unnamed)");
        }

        private void ItemDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            return;

            System.Windows.Forms.MessageBox.Show(string.Format("Property {0} in {1} (0x{2:X}) changed; new value is {3} (0x{3:X})",
                e.PropertyName, sender.GetType().Name, sender.GetHashCode(), sender.GetProperty(e.PropertyName)));
        }

        public TBB GetMessageFile(string filename)
        {
            filename = (Version == Versions.European ? string.Format("{0}{1}", filename, langSuffixes[Language]) : filename);
            TBB messageFile = MessageFiles.FirstOrDefault(x => Path.GetFileName(x.Filename).StartsWith(filename));

            if (messageFile == null) throw new ArgumentException("Message file could not be found");
            return messageFile;
        }

        public EtrianString GetMessageString(string filename, int tableNo, int messageNo)
        {
            TBB messageFile = GetMessageFile(filename);
            return (messageFile.Tables[tableNo] as TBB.MTBL).Messages[messageNo];
        }

        public IList<T> GetParsedData<T>()
        {
            return parsedData.Where(x => x is T).Cast<T>().ToList();
        }

        public IList<Tuple<Type, IList<BaseParser>>> GetAllParsedData(bool mustSupportSave)
        {
            List<Tuple<Type, IList<BaseParser>>> output = new List<Tuple<Type, IList<BaseParser>>>();

            List<Type> typesWithAttrib = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetCustomAttributes(typeof(ParserUsage), false).Length > 0).ToList();
            foreach (Type type in typesWithAttrib)
            {
                if (mustSupportSave)
                {
                    MethodInfo mi = type.GetMethod("Save", BindingFlags.Instance | BindingFlags.Public);
                    if (mi.DeclaringType == typeof(BaseParser)) continue;
                }

                output.Add(new Tuple<Type, IList<BaseParser>>(type, parsedData.Where(x => x.GetType() == type).ToList()));
            }

            return output;
        }
    }
}
