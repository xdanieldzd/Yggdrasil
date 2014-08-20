using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

using Yggdrasil.Forms;
using Yggdrasil.FileTypes;
using Yggdrasil.Helpers;
using Yggdrasil.TableParsers;

namespace Yggdrasil
{
    public class GameDataManager
    {
        static readonly string[] messageDirs = new string[]
        {
            "data\\Data\\CharaSel", "data\\Data\\Dungeon", "data\\Data\\Event", "data\\Data\\Opening", "data\\Data\\Param", "data\\Data\\SaveLoad", "data\\Data\\Battle"
        };

        static readonly string[] dataDirs = new string[]
        {
            "data\\Data\\Param", "data\\Data\\Battle", "data\\Data\\Event"
        };

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

        public bool IsInitialized { get; private set; }

        public FontRenderer FontRenderer { get; private set; }
        public List<TBB> MessageFiles { get; private set; }

        List<TBB> dataTableFiles;
        List<BaseParser> parsedData;

        List<BaseParser> changedParsedData;
        public bool DataHasChanged { get { return (changedParsedData != null && changedParsedData.Count > 0); } }
        public int ChangedDataCount { get { return (changedParsedData != null ? changedParsedData.Count : -1); } }
        public event PropertyChangedEventHandler ItemDataPropertyChangedEvent;

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
            IsInitialized = false;

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            loadWaitWorker = new BackgroundWorker();
            loadWaitWorker.WorkerReportsProgress = true;
            loadWaitWorker.DoWork += ((s, e) =>
            {
                try
                {
                    loadWaitWorker.ReportProgress(-1, "Reading game directory...");
                    ReadHeaderIdentify();

                    PrepareDirectoryUnpack();

                    loadWaitWorker.ReportProgress(-1, "Generating character map...");
                    EtrianString.GameVersion = Version;

                    loadWaitWorker.ReportProgress(-1, "Initializing font renderer...");
                    FontRenderer = new FontRenderer(this, Path.Combine(path, mainFontFilename));

                    MessageFiles = ReadDataTablesByExtension(".mbb", messageDirs);
                    dataTableFiles = ReadDataTablesByExtension(".tbb", dataDirs);

                    EnsureMessageTableIntegrity();

                    parsedData = ParseDataTables();
                    changedParsedData = new List<BaseParser>();

                    loadWaitWorker.ReportProgress(-1, "Fetching item names...");
                    FetchItemNames();

                    IsInitialized = true;
                }
                catch (GameDataManagerException gameException)
                {
                    MessageBox.Show(
                        string.Format("{0}{1}{1}{2}", gameException.Message, Environment.NewLine, "Please ensure you've selected a valid game data directory."), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        string.Format("{0} occured: {1}{2}{2}{3}", exception.GetType().FullName, exception.Message, Environment.NewLine, "Please contact a developer about this message."), "Exception",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
            loadWaitWorker.ProgressChanged += ((s, e) =>
            {
                loadWaitForm.PrintStatus(e.UserState as string);
                Program.Logger.LogMessage(e.UserState as string);
            });
            loadWaitWorker.RunWorkerCompleted += ((s, e) =>
            {
                loadWaitForm.Close();
                stopwatch.Stop();
                Program.Logger.LogMessage("Game directory read in {0:0.000} sec...", stopwatch.Elapsed.TotalSeconds);
            });
            loadWaitWorker.RunWorkerAsync();

            loadWaitForm = new DataLoadWaitForm();
            loadWaitForm.ShowDialog(Program.MainForm);
        }

        public int SaveAllChanges()
        {
            if (parsedData == null || changedParsedData == null) return 0;

            List<TBB.ITable> needToSave = new List<TBB.ITable>();
            List<string> changedFiles = new List<string>();

            foreach (BaseParser data in changedParsedData)
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

            changedParsedData.Clear();

            return changedFiles.Distinct().Count();
        }

        private void ReadHeaderIdentify()
        {
            loadWaitWorker.ReportProgress(-1, "Reading header data...");

            if (!File.Exists(Path.Combine(DataPath, "header.bin"))) throw new GameDataManagerException("File header.bin not found.");

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

                default: throw new GameDataManagerException("Unsupported game data.");
            }

            loadWaitWorker.ReportProgress(-1, string.Format("Identified '{0} {1}' as {2} version...", Header.GameTitle, Header.GameCode, Version));
        }

        private void PrepareDirectoryUnpack()
        {
            /* Do we need to decompress anything? */
            string checkPath = Path.Combine(DataPath, "data\\Data\\Event", "DUN_01F.evt");
            bool needToUnpack = !File.Exists(checkPath);

            if (needToUnpack)
            {
                loadWaitWorker.ReportProgress(-1, "Preparing to decompress files...");

                List<Tuple<string, string, string, bool, bool>> dirExtTuples = new List<Tuple<string, string, string, bool, bool>>
                { 
                    /* Path, Original Name/Ext, New Name/Ext, Localized?, ARM9-Autofix? */
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Event", ".cmp", ".evt", false, false),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\MapDat", "_ydd.cmp", ".ydd", false, false),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\MapDat", "_ymd.cmp", ".ymd", false, false),

                    new Tuple<string, string, string, bool, bool>("data\\Data\\Param", "BarQuestData.cmp", "BarQuestData.tbb", false, true),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Param", "BarQuestMess.cmp", "BarQuestMess.mbb", true, true),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Param", "BarQuestName.cmp", "BarQuestName.mbb", true, true),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Param", "BtlItemInfo.cmp", "BtlItemInfo.tbb", false, true),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Param", "CampText.cmp", "CampText.mbb", true, true),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Param", "FacilityText.cmp", "FacilityText.mbb", true, true),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Param", "GovernmentMissionData.cmp", "GovernmentMissionData.tbb", false, true),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Param", "GovernmentMissionMess.cmp", "GovernmentMissionMess.mbb", true, true),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Param", "GovernmentMissionName.cmp", "GovernmentMissionName.mbb", true, true),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Param", "GovernmentMissionPrize.cmp", "GovernmentMissionPrize.mbb", true, true),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Param", "ItemChoiceInfo.cmp", "ItemChoiceInfo.mbb", true, true),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Param", "itemIllInfo.cmp", "itemIllInfo.mbb", true, true),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Param", "ItemInfo.cmp", "ItemInfo.mbb", true, true),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Param", "ItemName.cmp", "ItemName.mbb", true, true),
                    
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Battle", "BtlMess.cmp", "BtlMess.mbb", true, true),
                };

                List<Tuple<string, string, string, bool, bool>> dirExtTuplesLocalized = new List<Tuple<string, string, string, bool, bool>>();

                if (Version == Versions.European)
                {
                    dirExtTuplesLocalized.AddRange(dirExtTuples.Where(x => !x.Item4));

                    foreach (Tuple<string, string, string, bool, bool> dirExt in dirExtTuples.Where(x => x.Item4))
                    {
                        int sourcePeriodIdx = dirExt.Item2.IndexOf('.');
                        string sourceFileName = dirExt.Item2.Substring(0, sourcePeriodIdx);
                        string sourceFileExt = dirExt.Item2.Substring(sourcePeriodIdx);

                        int destPeriodIdx = dirExt.Item3.IndexOf('.');
                        string destFileName = dirExt.Item3.Substring(0, sourcePeriodIdx);
                        string destFileExt = dirExt.Item3.Substring(sourcePeriodIdx);

                        foreach (KeyValuePair<Languages, string> langSuffix in langSuffixes)
                        {
                            dirExtTuplesLocalized.Add(new Tuple<string, string, string, bool, bool>(
                                dirExt.Item1,
                                string.Format("{0}{1}{2}", sourceFileName, langSuffix.Value, sourceFileExt),
                                string.Format("{0}{1}{2}", destFileName, langSuffix.Value, destFileExt),
                                false,
                                dirExt.Item5));
                        }
                    }
                }

                /* Find and decompress files */
                foreach (Tuple<string, string, string, bool, bool> dirExt in (Version == Versions.European ? dirExtTuplesLocalized : dirExtTuples))
                {
                    string localDataPath = Path.Combine(DataPath, dirExt.Item1);
                    List<string> filePathsAll = Directory.EnumerateFiles(localDataPath, "*.*", SearchOption.AllDirectories).ToList();

                    List<string> filePaths = filePathsAll
                        .Where(x => x.ToLowerInvariant().EndsWith(dirExt.Item2.ToLowerInvariant()) || Path.GetFileName(x.ToLowerInvariant()) == dirExt.Item2.ToLowerInvariant())
                        .ToList();

                    foreach (string filePath in filePaths)
                    {
                        loadWaitWorker.ReportProgress(-1, string.Format("Decompressing {0}...", Path.GetFileName(filePath)));

                        string newPath = Path.Combine(Path.GetDirectoryName(filePath), filePath.Replace(dirExt.Item2, dirExt.Item3));

                        bool isCompressed;
                        byte[] fileData = Helpers.Decompressor.Decompress(filePath, out isCompressed);
                        if (isCompressed)
                        {
                            BinaryWriter writer = new BinaryWriter(File.Create(newPath));
                            writer.Write(fileData);
                            writer.Close();

                            File.Delete(filePath);
                        }
                    }
                }

                /* Read ARM9 binary */
                BinaryReader arm9Reader = new BinaryReader(File.Open(Path.Combine(DataPath, "arm9.bin"), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
                byte[] arm9Data = new byte[arm9Reader.BaseStream.Length];
                arm9Reader.Read(arm9Data, 0, arm9Data.Length);
                arm9Reader.Close();

                /* Apply ARM9 patches */
                loadWaitWorker.ReportProgress(-1, "Patching ARM9 binary...");

                bool eventsPatched = false, mapsPatched = false;
                for (int i = 0; i < arm9Data.Length; i += 4)
                {
                    string check = string.Empty;

                    /* Event data */
                    if (!eventsPatched && i < arm9Data.Length - 8)
                    {
                        check = Encoding.ASCII.GetString(arm9Data, i, 8);
                        if (check == "MIS_\0\0\0\0")
                        {
                            Buffer.BlockCopy(Encoding.ASCII.GetBytes(".evt"), 0, arm9Data, i + 0x1C, 4);
                            eventsPatched = true;
                        }
                    }

                    /* Map data */
                    if (!mapsPatched && i < arm9Data.Length - 8)
                    {
                        check = Encoding.ASCII.GetString(arm9Data, i, 8);
                        if (check == "_ymd.cmp")
                        {
                            Buffer.BlockCopy(Encoding.ASCII.GetBytes(".ymd\0\0\0\0"), 0, arm9Data, i, 8);
                            Buffer.BlockCopy(Encoding.ASCII.GetBytes(".ydd\0\0\0\0"), 0, arm9Data, i + 0xC, 8);
                            mapsPatched = true;
                        }
                    }
                }

                /* Data/message tables */
                foreach (Tuple<string, string, string, bool, bool> e in dirExtTuples.Where(x => x.Item5))
                {
                    string originalData = e.Item1.Substring(e.Item1.IndexOf('\\') + 1).Replace('\\', '/') + "/" + e.Item2;
                    string replacedData = e.Item1.Substring(e.Item1.IndexOf('\\') + 1).Replace('\\', '/') + "/" + e.Item3;

                    for (int i = 0; i < arm9Data.Length; i += 4)
                    {
                        if (i < arm9Data.Length - originalData.Length)
                        {
                            string check = Encoding.ASCII.GetString(arm9Data, i, originalData.Length);
                            if (check.StartsWith(originalData)) Buffer.BlockCopy(Encoding.ASCII.GetBytes(replacedData), 0, arm9Data, i, replacedData.Length);
                        }
                    }
                }

                /* Write patched ARM9 binary */
                BinaryWriter arm9Writer = new BinaryWriter(File.Open(Path.Combine(DataPath, "arm9.bin"), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
                arm9Writer.Write(arm9Data);
                arm9Writer.Close();
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

        private List<BaseParser> ParseDataTables()
        {
            List<BaseParser> parsedData = new List<BaseParser>();

            List<Type> typesWithAttrib = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetCustomAttributes(typeof(ParserUsage), false).Length > 0).ToList();
            foreach (Type type in typesWithAttrib)
            {
                foreach (ParserUsage attrib in type.GetCustomAttributes(false).Where(x => x is ParserUsage))
                {
                    loadWaitWorker.ReportProgress(-1, string.Format("Parsing {0}...", attrib.FileName));

                    TBB tableFile = dataTableFiles.FirstOrDefault(x => Path.GetFileName(x.Filename) == attrib.FileName);

                    if (tableFile == null)
                        throw new FileNotFoundException(string.Format("Table file {0} not found in loaded files", attrib.FileName));

                    if (attrib.TableNo >= tableFile.NumTables)
                        throw new ArgumentException(string.Format("Table number {0} does not exist in file", attrib.TableNo));

                    if (!(tableFile.Tables[attrib.TableNo] is TBB.TBL1))
                        throw new InvalidCastException(string.Format("Table number {0} is of wrong type {1}", attrib.TableNo, tableFile.Tables[attrib.TableNo].GetType().Name));

                    TBB.TBL1 table = (tableFile.Tables[attrib.TableNo] as TBB.TBL1);
                    for (int i = 0; i < table.Data.Length; i++)
                        parsedData.Add((BaseParser)Activator.CreateInstance(type, new object[] { this, table, i, (PropertyChangedEventHandler)ItemDataPropertyChanged }));
                }
            }

            return parsedData;
        }

        private void FetchItemNames()
        {
            ItemNames = new Dictionary<ushort, string>();
            ItemNames.Add(0, "(None)");
            foreach (BaseParser parser in parsedData.Where(x => (x is EquipItemParser || x is MiscItemParser)))
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
            changedParsedData = parsedData.Where(x => x.HasChanged).ToList();

            var handler = ItemDataPropertyChangedEvent;
            if (handler != null) handler(sender, e);
            return;

            System.Windows.Forms.MessageBox.Show(string.Format("Property {0} in {1} (0x{2:X}) changed; new value is {3} (0x{3:X})",
                e.PropertyName, sender.GetType().Name, sender.GetHashCode(), sender.GetProperty(e.PropertyName)));
        }

        public TBB GetMessageFile(string filename)
        {
            filename = (Version == Versions.European ? string.Format("{0}{1}", filename, langSuffixes[Language]) : filename);
            TBB messageFile = MessageFiles.FirstOrDefault(x => x.Filename != null && Path.GetFileName(x.Filename).StartsWith(filename));

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
            foreach (Type type in typesWithAttrib.OrderBy(x => ((PrioritizedDescription)x.GetAttribute<PrioritizedDescription>()).Priority))
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
