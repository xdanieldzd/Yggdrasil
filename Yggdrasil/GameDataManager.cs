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
                FontRenderer = new FontRenderer(this, Directory.EnumerateFiles(path, mainFontFilename, SearchOption.AllDirectories).FirstOrDefault());

                MessageFiles = ReadDataTablesByExtension(".mbb", messageDirs);
                dataTableFiles = ReadDataTablesByExtension(".tbb", dataDirs);

                EnsureMessageTableIntegrity();

                parsedData = new List<BaseParser>();
                parsedData.AddRange(ParseDataTable("Item.tbb"));
                parsedData.AddRange(ParseDataTable("ItemCompound.tbb"));
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

        public void SaveAllChanges()
        {
            if (parsedData == null) return;

            foreach (BaseParser data in parsedData.Where(x => x.HasChanged))
            {
                throw new NotImplementedException("Saving not implemented");
            }
        }

        private void ReadHeaderIdentify()
        {
            Header = new FileTypes.Header(this, Path.Combine(DataPath, "header.bin"));
            switch (Header.GameCode)
            {
                case "AKYP":
                    Version = Versions.European;
                    Language = Languages.English;
                    mainFontFilename = "Font14x11_00.cmp";
                    break;

                case "AKYE":
                    Version = Versions.American;
                    mainFontFilename = "Font10x5_00.cmp";
                    break;

                case "AKYJ":
                    Version = Versions.Japanese;
                    mainFontFilename = "Font10x10_00.cmp";

                    if (false)
                    {
                        ushort gameChar = 0x004C;
                        ushort sjisChar = 0x824F;
                        StreamWriter sw = File.CreateText(@"C:\temp\EOJPN-TABLE.txt");
                        Encoding sjis = Encoding.GetEncoding(932);
                        /*for (int i = sjisChar; i < 0xEAA2; i++)
                        {
                            string str = sjis.GetString(BitConverter.GetBytes(sjisChar.Reverse()));
                            if (str != "・" &&
                                str != "ゎ" && str != "ゐ" && str != "ゑ" &&
                                str != "ヮ" && str != "ヰ" && str != "ヱ")
                            {
                                sw.WriteLine("{{ 0x{0:X4}, '{1}' }},", gameChar, str);
                                gameChar++;
                            }
                            sjisChar++;
                            if (gameChar == 0x012B)
                            {
                                sw.WriteLine("{{ 0x{0:X4}, '{1}' }},", gameChar++, "Ⅱ");
                                sw.WriteLine("{{ 0x{0:X4}, '{1}' }},", gameChar++, "Ⅲ");
                                sjisChar = 0x889F;
                            }
                        }*/

                        gameChar = 0x12D; sjisChar = 0x889F;
                        for (int i = sjisChar; i < 0xEAA2; i++)
                        {
                            string str = sjis.GetString(BitConverter.GetBytes(sjisChar.Reverse()));
                            if (str != "・")
                            {
                                sw.WriteLine("{{ 0x{0:X4}, '{1}' }},", gameChar, str);
                                gameChar++;
                            }
                            sjisChar++;
                        }
                        sw.Flush();
                        sw.Close();
                    }
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

                    foreach (byte[] data in table.Data)
                    {
                        parsedData.Add((BaseParser)Activator.CreateInstance(type, new object[] { this, table, data, (PropertyChangedEventHandler)ItemDataPropertyChanged }));
                    }
                }
            }
            return parsedData;
        }

        private void ItemDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
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
    }
}
