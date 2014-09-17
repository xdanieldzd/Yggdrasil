using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

using Yggdrasil.Forms;
using Yggdrasil.FileHandling;
using Yggdrasil.FileHandling.TableHandling;
using Yggdrasil.Helpers;
using Yggdrasil.TableParsing;
using Yggdrasil.Attributes;
using Yggdrasil.TextHandling;

namespace Yggdrasil
{
    public class GameDataManager
    {
        static readonly string[] archiveDirs = new string[]
        {
            "data\\Data\\Event"
        };

        static readonly string[] messageDirs = new string[]
        {
            "data\\Data\\CharaSel", "data\\Data\\Dungeon", "data\\Data\\Event", "data\\Data\\Opening", "data\\Data\\Param", "data\\Data\\SaveLoad", "data\\Data\\Battle"
        };

        static readonly string[] dataDirs = new string[]
        {
            "data\\Data\\Param", "data\\Data\\Battle", "data\\Data\\Event"
        };

        static readonly string ItemNameFile = "ItemName";
        static readonly string ItemInfoFile = "ItemInfo";
        static readonly string EnemyNameFile = "EnemyName";
        static readonly string EnemyInfoFile = "EnemyInfo";
        static readonly string PlayerSkillNameFile = "PlayerSkillName";
        static readonly string CampSkillInfoFile = "CampSkillInfo";
        static readonly string CampSkillExeInfoFile = "CampSkillExeInfo";

        public enum Versions { Invalid, European, American, Japanese };
        public enum Languages { English, German, Spanish, French, Italian };

        public string DataPath { get; private set; }

        public HeaderFile Header { get; private set; }
        public Versions Version { get; private set; }

        Languages language;
        public Languages Language
        {
            get { return language; }
            set
            {
                language = value;
                var handler = SelectedLanguageChangedEvent;
                if (handler != null) handler(this, new EventArgs());
            }
        }
        public event EventHandler SelectedLanguageChangedEvent;

        public string MainFontFilename { get; private set; }
        public string SmallFontFilename { get; private set; }
        public int MainFontInfoOffset { get; private set; }
        public int SmallFontInfoOffset { get; private set; }

        public Dictionary<Languages, string> LanguageSuffixes = new Dictionary<Languages, string>()
        {
            { Languages.German, "_DE" },
            { Languages.English, "_EN" },
            { Languages.Spanish, "_ES" },
            { Languages.French, "_FR" },
            { Languages.Italian, "_IT" }
        };

        ProgressDialog loadWaitDialog;
        BackgroundWorker loadWaitWorker;

        public bool IsInitialized { get; private set; }

        public FontRenderer MainFontRenderer { get; private set; }
        public FontRenderer SmallFontRenderer { get; private set; }

        List<ArchiveFile> archives;

        public List<TableFile> MessageFiles { get; private set; }
        List<TableFile> changedMessageFiles;
        public bool MessageFileHasChanged { get { return (changedMessageFiles != null && changedMessageFiles.Count > 0); } }
        public int ChangedMessageFileCount { get { return (changedMessageFiles != null ? changedMessageFiles.Count : -1); } }

        List<TableFile> dataTableFiles;
        public List<BaseParser> ParsedData { get; private set; }

        List<BaseParser> changedParsedData;
        public bool DataHasChanged { get { return (changedParsedData != null && changedParsedData.Count > 0); } }
        public int ChangedDataCount { get { return (changedParsedData != null ? changedParsedData.Count : -1); } }
        public event PropertyChangedEventHandler ItemDataPropertyChangedEvent;

        public static Dictionary<ushort, string> ItemNames { get; private set; }
        public static Dictionary<ushort, string> ItemDescriptions { get; private set; }
        public static Dictionary<ushort, string> EnemyNames { get; private set; }
        public static Dictionary<ushort, string> EnemyDescriptions { get; private set; }
        public static Dictionary<ushort, string> PlayerSkillNames { get; private set; }
        public static Dictionary<ushort, string> PlayerSkillShortDescriptions { get; private set; }
        public static Dictionary<ushort, string> PlayerSkillDescriptions { get; private set; }

        public static Dictionary<ushort, string> EncounterDescriptions { get; private set; }
        public static Dictionary<ushort, string> GeneralItemNames { get; private set; }

        public static List<string> AINames { get; private set; }
        public static List<string> SpriteNames { get; private set; }

        public GameDataManager() { }

        public void ReadGameDirectory(string path)
        {
            DataPath = path;
            IsInitialized = false;

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            System.Threading.Thread workerThread = null;

            loadWaitWorker = new BackgroundWorker();
            loadWaitWorker.WorkerReportsProgress = true;
            loadWaitWorker.DoWork += ((s, e) =>
            {
                try
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
                    workerThread = System.Threading.Thread.CurrentThread;

                    loadWaitWorker.ReportProgress(-1, "Reading game directory...");
                    ReadHeaderIdentify();

                    PrepareDirectoryUnpack();

                    loadWaitWorker.ReportProgress(-1, "Generating character map...");
                    EtrianString.GameVersion = Version;

                    //TEMP
                    //TilePalettePair f = new TilePalettePair(this, Path.Combine(path, @"data\Data\Tex\top2"), 256, 64);
                    //TEMP

                    loadWaitWorker.ReportProgress(-1, "Initializing font renderers...");
                    ushort characterCount = (ushort)((SmallFontInfoOffset - MainFontInfoOffset) / 2);
                    MainFontRenderer = new FontRenderer(this, Path.Combine(path, MainFontFilename + ".ntft"), MainFontInfoOffset, characterCount);
                    SmallFontRenderer = new FontRenderer(this, Path.Combine(path, SmallFontFilename + ".ntft"), SmallFontInfoOffset, characterCount);

                    MessageFiles = ReadDataTablesByExtension(new string[] { ".mbb", ".tbb" }, messageDirs);
                    dataTableFiles = ReadDataTablesByExtension(new string[] { ".tbb" }, dataDirs);

                    archives = ReadArchiveFiles(archiveDirs);

                    EnsureMessageTableIntegrity();
                    GenerateDictionariesLists();

                    changedMessageFiles = new List<TableFile>();

                    ParsedData = ParseDataTables();
                    changedParsedData = new List<BaseParser>();

                    CleanStringDictionaries();
                    GenerateOtherDictionaries();

                    loadWaitWorker.ReportProgress(-1, "Finished loading.");
                    IsInitialized = true;
                }
                catch (System.Threading.ThreadAbortException) { }
                catch (Exceptions.GameDataManagerException gameException)
                {
                    GUIHelpers.ShowErrorMessage(
                        "Application Error", "An error occured while loading the game data.", "Please ensure you've selected a valid game data directory.",
                        gameException.Message, gameException.ToString(), (IntPtr)Program.MainForm.Invoke(new Func<IntPtr>(() => { return Program.MainForm.Handle; })));
                }
#if !DEBUG
                catch (Exception exception)
                {
                    GUIHelpers.ShowErrorMessage(
                        "Application Error", "The application performed an illegal operation.", "Please contact a developer with the details of this message.",
                        exception.Message, exception.ToString(), (IntPtr)Program.MainForm.Invoke(new Func<IntPtr>(() => { return Program.MainForm.Handle; })));
                }
#endif
            });
            loadWaitWorker.ProgressChanged += ((s, e) =>
            {
                string message = (e.UserState as string);
                if (loadWaitDialog.Cancel)
                {
                    workerThread.Abort();
                    Program.Logger.LogMessage("Loading aborted.");
                    Program.MainForm.StatusText = "Ready";
                    return;
                }

                loadWaitDialog.Details = message;
                Program.Logger.LogMessage(true, message);
            });
            loadWaitWorker.RunWorkerCompleted += ((s, e) =>
            {
                loadWaitDialog.Close();
                stopwatch.Stop();
                Program.Logger.LogMessage("Game directory read in {0:0.000} sec...", stopwatch.Elapsed.TotalSeconds);
            });
            loadWaitWorker.RunWorkerAsync();

            loadWaitDialog = new ProgressDialog() { Title = "Loading", InstructionText = "Loading game data.", Text = "Please wait while the game data is being loaded...", ParentForm = Program.MainForm };
            loadWaitDialog.Show(Program.MainForm);
        }

        public int SaveAllChanges()
        {
            if (ParsedData == null || changedParsedData == null) return 0;

            List<BaseFile> changedFiles = new List<BaseFile>();

            foreach (BaseParser data in changedParsedData)
            {
                data.Save();
                if (!changedFiles.Contains(data.ParentTable.TableFile)) changedFiles.Add(data.ParentTable.TableFile);
            }

            changedFiles.AddRange(changedMessageFiles);

            List<ArchiveFile> distinctArchives = changedFiles.Where(x => x.ArchiveFile != null).Select(y => y.ArchiveFile).Distinct().ToList();
            changedFiles.RemoveAll(x => x.ArchiveFile != null);
            changedFiles.AddRange(distinctArchives);

            foreach (BaseFile file in changedFiles) file.Save();

            changedParsedData.Clear();
            changedMessageFiles.Clear();

            return changedFiles.Count();
        }

        private void ReadHeaderIdentify()
        {
            loadWaitWorker.ReportProgress(-1, "Reading header data...");

            if (!File.Exists(Path.Combine(DataPath, "header.bin"))) throw new Exceptions.GameDataManagerException("File header.bin could not be found.");

            Header = new FileHandling.HeaderFile(this, Path.Combine(DataPath, "header.bin"));
            switch (Header.GameCode)
            {
                case "AKYP":
                    Version = Versions.European;
                    MainFontFilename = "data\\Data\\Tex\\Font\\Font14x11_00";
                    SmallFontFilename = "data\\Data\\Tex\\Font\\Font10x9_00";
                    MainFontInfoOffset = 0xB79B4;
                    SmallFontInfoOffset = MainFontInfoOffset + 0x180;
                    break;

                case "AKYE":
                    Version = Versions.American;
                    MainFontFilename = "data\\Data\\Tex\\Font\\Font10x5_00";
                    SmallFontFilename = "data\\Data\\Tex\\Font\\Font8x4_00";
                    MainFontInfoOffset = 0xD5C04;
                    SmallFontInfoOffset = MainFontInfoOffset + 0xC0;
                    break;

                case "AKYJ":
                    Version = Versions.Japanese;
                    MainFontFilename = "data\\Data\\Tex\\Font\\Font10x10_00";
                    SmallFontFilename = "data\\Data\\Tex\\Font\\Font8x8_00";
                    MainFontInfoOffset = SmallFontInfoOffset = -1;
                    break;

                default: throw new Exceptions.GameDataManagerException("Unsupported game data.");
            }

            Program.Logger.LogMessage("Identified game '{0} {1}' as {2} version.", Header.GameTitle, Header.GameCode, Version);
        }

        private void PrepareDirectoryUnpack()
        {
            /* Do we need to decompress anything? */
            string checkPath = Path.Combine(DataPath, "data\\Data\\Event", "DUN_01F.evt");
            if (File.Exists(checkPath)) return;

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

                    new Tuple<string, string, string, bool, bool>("data\\Data\\Tex\\Font", Path.GetFileName(MainFontFilename) + ".cmp", Path.GetFileName(MainFontFilename) + ".ntft", false, false),
                    new Tuple<string, string, string, bool, bool>("data\\Data\\Tex\\Font", Path.GetFileName(SmallFontFilename) + ".cmp", Path.GetFileName(SmallFontFilename) + ".ntft", false, false),
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

                    foreach (KeyValuePair<Languages, string> langSuffix in LanguageSuffixes)
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
                if (!Directory.Exists(localDataPath)) continue;

                List<string> filePathsAll = Directory.EnumerateFiles(localDataPath, "*.*", SearchOption.AllDirectories).ToList();
                List<string> filePaths = filePathsAll
                    .Where(x => x.ToLowerInvariant().EndsWith(dirExt.Item2.ToLowerInvariant()) || Path.GetFileName(x.ToLowerInvariant()) == dirExt.Item2.ToLowerInvariant())
                    .ToList();

                foreach (string filePath in filePaths)
                {
                    loadWaitWorker.ReportProgress(-1, string.Format("Decompressing {0}...", Path.GetFileName(filePath)));

                    string newPath = Path.Combine(Path.GetDirectoryName(filePath), filePath.Replace(dirExt.Item2, dirExt.Item3));

                    MemoryStream decompressed = DataCompression.StreamHelper.Decompress(new MemoryStream(File.ReadAllBytes(filePath)));
                    BinaryWriter writer = new BinaryWriter(File.Create(newPath));
                    writer.Write(decompressed.ToArray());
                    writer.Close();

                    File.Delete(filePath);
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

        private List<ArchiveFile> ReadArchiveFiles(string[] directories)
        {
            if (directories == null) throw new ArgumentNullException("Directories is null");

            archives = new List<ArchiveFile>();

            foreach (string directory in directories)
            {
                string localDataPath = Path.Combine(DataPath, directory);
                if (!Directory.Exists(localDataPath)) continue;

                List<string> filePaths = Directory.EnumerateFiles(localDataPath, "*.bin", SearchOption.AllDirectories).ToList();
                foreach (string filePath in filePaths)
                {
                    loadWaitWorker.ReportProgress(-1, string.Format("Reading {0}...", Path.GetFileName(filePath)));

                    ArchiveFile archive = new ArchiveFile(this, filePath);
                    archives.Add(archive);
                }
            }

            foreach (ArchiveFile archive in archives)
            {
                foreach (TableFile tableFile in archive.Blocks.Where(x => x is TableFile))
                {
                    if (tableFile.Tables.Any(x => x is MessageTable)) MessageFiles.Add(tableFile);
                }
            }

            return archives.OrderBy(x => x.Filename).ToList();
        }

        private List<TableFile> ReadDataTablesByExtension(string[] extensions, string[] directories)
        {
            if (extensions == null || extensions.Length == 0) throw new ArgumentException("No extension given");
            if (directories == null) throw new ArgumentNullException("Directories is null");

            List<TableFile> dataTables = new List<TableFile>();

            foreach (string directory in directories)
            {
                string localDataPath = Path.Combine(DataPath, directory);
                if (!Directory.Exists(localDataPath)) continue;

                List<string> filePaths = Directory.EnumerateFiles(localDataPath, "*.*", SearchOption.AllDirectories)
                    .Where(x => extensions.Contains(Path.GetExtension(x.ToLowerInvariant())))
                    .ToList();

                foreach (string filePath in filePaths)
                {
                    TableFile tbb = new TableFile(this, filePath);
                    dataTables.Add(tbb);

                    loadWaitWorker.ReportProgress(-1, string.Format("Reading {0}...", Path.GetFileName(filePath)));
                }
            }

            return dataTables.OrderBy(x => x.Filename).ToList();
        }

        private void EnsureMessageTableIntegrity()
        {
            if (MessageFiles == null) return;

            List<TableFile> filesToRemove = new List<TableFile>();
            foreach (TableFile messageFile in MessageFiles)
            {
                bool removeFile = true;
                foreach (BaseTable table in messageFile.Tables)
                {
                    if (table is MessageTable) removeFile = false;
                }

                if (removeFile) filesToRemove.Add(messageFile);
            }

            foreach (TableFile file in filesToRemove) MessageFiles.Remove(file);
        }

        private List<BaseParser> ParseDataTables()
        {
            List<BaseParser> parsedData = new List<BaseParser>();

            List<Type> typesWithAttrib = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetCustomAttributes(typeof(ParserDescriptor), false).Length > 0).ToList();
            foreach (Type type in typesWithAttrib)
            {
                foreach (ParserDescriptor attrib in type.GetCustomAttributes(false).Where(x => x is ParserDescriptor))
                {
                    loadWaitWorker.ReportProgress(-1, string.Format("Parsing {0}...", attrib.FileName));

                    TableFile tableFile =
                        dataTableFiles.FirstOrDefault(x => Path.GetFileName(x.Filename) == attrib.FileName || Path.GetFileNameWithoutExtension(x.Filename) == Path.GetFileNameWithoutExtension(attrib.FileName));

                    if (tableFile == null)
                        throw new FileNotFoundException(string.Format("Table file {0} not found in loaded files", attrib.FileName));

                    if (attrib.TableNo >= tableFile.NumTables)
                        throw new ArgumentException(string.Format("Table number {0} does not exist in file", attrib.TableNo));

                    if (!(tableFile.Tables[attrib.TableNo] is DataTable))
                        throw new InvalidCastException(string.Format("Table number {0} is of wrong type {1}", attrib.TableNo, tableFile.Tables[attrib.TableNo].GetType().Name));

                    DataTable table = (tableFile.Tables[attrib.TableNo] as DataTable);
                    for (int i = 0; i < table.Data.Length; i++)
                        parsedData.Add((BaseParser)Activator.CreateInstance(type, new object[] { this, table, i, (PropertyChangedEventHandler)ItemDataPropertyChanged }));
                }
            }

            return parsedData;
        }

        private void GenerateDictionariesLists()
        {
            ItemNames = GenerateStringDictionary(ItemNameFile, 0);
            ItemDescriptions = GenerateStringDictionary(ItemInfoFile, 0);
            EnemyNames = GenerateStringDictionary(EnemyNameFile, 0);
            EnemyDescriptions = GenerateStringDictionary(EnemyInfoFile, 0);
            PlayerSkillNames = GenerateStringDictionary(PlayerSkillNameFile, 0);
            PlayerSkillShortDescriptions = GenerateStringDictionary(CampSkillExeInfoFile, 0);
            PlayerSkillDescriptions = GenerateStringDictionary(CampSkillInfoFile, 0);

            loadWaitWorker.ReportProgress(-1, "Reticulating splines...");
            SpriteNames = Directory.EnumerateFiles(Path.Combine(DataPath, "data\\Data\\Tex\\Enemy"), "*.*", SearchOption.AllDirectories).Select(x => Path.GetFileNameWithoutExtension(x).Substring(3)).ToList();
            AINames = Directory.EnumerateFiles(Path.Combine(DataPath, "data\\Data\\Battle"), "ai_*.tbb", SearchOption.AllDirectories).Select(x => Path.GetFileNameWithoutExtension(x).Substring(3)).ToList();
        }

        private Dictionary<ushort, string> GenerateStringDictionary(string filename, int tableNo)
        {
            loadWaitWorker.ReportProgress(-1, string.Format("Generating dictionary for {0}, table {1}...", filename, tableNo));

            Dictionary<ushort, string> dict = new Dictionary<ushort, string>();
            dict.Add(0, "(None)");

            TableFile messageFile = GetMessageFile(filename);
            MessageTable messageTable = (messageFile.Tables[tableNo] as MessageTable);

            for (ushort number = 1; number <= messageTable.NumMessages; number++) dict.Add(number, messageTable.Messages[number - 1].ConvertedString);

            return dict;
        }

        private void CleanStringDictionaries()
        {
            loadWaitWorker.ReportProgress(-1, "Cleaning various dictionaries...");

            CleanStringDictionary(ItemNames, typeof(BaseItemParser), "ItemNumber");
            CleanStringDictionary(ItemDescriptions, typeof(BaseItemParser), "ItemNumber");
            CleanStringDictionary(EnemyNames, typeof(EnemyDataParser), "EnemyNumber");
            CleanStringDictionary(EnemyDescriptions, typeof(EnemyDataParser), "EnemyNumber");
            CleanStringDictionary(PlayerSkillNames, typeof(PlayerSkillReqParser), "SkillNumber");
            CleanStringDictionary(PlayerSkillShortDescriptions, typeof(PlayerSkillReqParser), "SkillNumber");
            CleanStringDictionary(PlayerSkillDescriptions, typeof(PlayerSkillReqParser), "SkillNumber");
        }

        private void CleanStringDictionary(Dictionary<ushort, string> dict, Type parserType, string propertyName)
        {
            foreach (ushort key in dict.Keys
                .Except<ushort>(ParsedData.Where(x => parserType.IsAssignableFrom(x.GetType())).Select(y => (ushort)y.GetProperty(propertyName)))
                .Where(x => x != 0)
                .ToList())
                dict.Remove(key);
        }

        private void GenerateOtherDictionaries()
        {
            loadWaitWorker.ReportProgress(-1, "Generating additional dictionaries...");

            EncounterDescriptions = new Dictionary<ushort, string>();
            foreach (EncounterParser parser in ParsedData.Where(x => (x is EncounterParser))) EncounterDescriptions.Add(parser.EncounterNumber, parser.EntryDescription);

            GeneralItemNames = new Dictionary<ushort, string>();
            GeneralItemNames.Add(0, "(None)");
            foreach (MiscItemParser parser in ParsedData.Where(x => (x is MiscItemParser))) GeneralItemNames.Add(parser.ItemNumber, parser.Name);
        }

        public void ItemDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            changedParsedData = ParsedData.Where(x => x.HasChanged).ToList();
            changedMessageFiles = MessageFiles.Where(x => x.Tables.Any(y => (y is MessageTable) && (y as MessageTable).HasChanges)).ToList();

            if (sender is EncounterParser) GenerateOtherDictionaries();

            if (sender.GetProperty(e.PropertyName) is string)
            {
                if (sender is BaseItemParser)
                {
                    BaseItemParser parser = (sender as BaseItemParser);
                    ItemNames[parser.ItemNumber] = parser.Name;
                    ItemDescriptions[parser.ItemNumber] = parser.Description;

                    SetMessageString(ItemNameFile, 0, parser.ItemNumber - 1, parser.Name);
                    SetMessageString(ItemInfoFile, 0, parser.ItemNumber - 1, parser.Description);
                }

                if (sender is EnemyDataParser)
                {
                    EnemyDataParser parser = (sender as EnemyDataParser);
                    EnemyNames[parser.EnemyNumber] = parser.Name;
                    EnemyDescriptions[parser.EnemyNumber] = parser.Description;

                    SetMessageString(EnemyNameFile, 0, parser.EnemyNumber - 1, parser.Name);
                    SetMessageString(EnemyInfoFile, 0, parser.EnemyNumber - 1, parser.Description);

                    GenerateOtherDictionaries();
                }

                if (sender is PlayerSkillReqParser)
                {
                    PlayerSkillReqParser parser = (sender as PlayerSkillReqParser);
                    PlayerSkillNames[parser.SkillNumber] = parser.Name;
                    PlayerSkillShortDescriptions[parser.SkillNumber] = parser.ShortDescription;
                    PlayerSkillDescriptions[parser.SkillNumber] = parser.Description;

                    SetMessageString(PlayerSkillNameFile, 0, parser.SkillNumber - 1, parser.Name);
                    SetMessageString(CampSkillExeInfoFile, 0, parser.SkillNumber - 1, parser.ShortDescription);
                    SetMessageString(CampSkillInfoFile, 0, parser.SkillNumber - 1, parser.Description);
                }
            }

            var handler = ItemDataPropertyChangedEvent;
            if (handler != null) handler(sender, e);
        }

        public TableFile GetMessageFile(string filename)
        {
            filename = (Version == Versions.European ? string.Format("{0}{1}", filename, LanguageSuffixes[Language]) : filename);
            TableFile messageFile = MessageFiles.FirstOrDefault(x => x.Filename != null && Path.GetFileName(x.Filename).StartsWith(filename));

            if (messageFile == null) throw new ArgumentException("Message file could not be found");
            return messageFile;
        }

        public void SetMessageString(string filename, int tableNo, int messageNo, string message)
        {
            TableFile messageFile = GetMessageFile(filename);
            SetMessageString((messageFile.Tables[tableNo] as MessageTable), messageNo, message);
        }

        public void SetMessageString(MessageTable table, int messageNo, string message)
        {
            table.Messages[messageNo].Update(message);
        }

        public List<TreeNode> GenerateTreeNodes(Type parserType, Action<TreeNode, List<BaseParser>> customChildCreator = null)
        {
            List<TreeNode> nodes = new List<TreeNode>();
            List<BaseParser> currentParsers = ParsedData.Where(x => x.GetType() == parserType).ToList();

            List<ParserDescriptor> parserDescriptors = parserType.GetAttributes<ParserDescriptor>();
            if (parserDescriptors.Count > 1)
            {
                foreach (ParserDescriptor parserDescriptor in parserDescriptors.OrderBy(x => x.Priority).OrderBy(x => x.Description))
                {
                    string path = Path.GetDirectoryName(parserDescriptor.Description);
                    string nodeName = Path.GetFileName(parserDescriptor.Description);

                    TreeNode parentNode = nodes.FirstOrDefault(x => x.Name == path);
                    if (parentNode == null) nodes.Add(parentNode = new TreeNode(path) { Name = path });

                    List<BaseParser> descriptorParsers = currentParsers.Where(x => Path.GetFileName(x.ParentTable.TableFile.Filename) == parserDescriptor.FileName).ToList();
                    TreeNode childNode = new TreeNode(nodeName) { Tag = descriptorParsers };

                    if (customChildCreator != null)
                        customChildCreator.Invoke(childNode, descriptorParsers);
                    else
                    {
                        foreach (BaseParser parser in descriptorParsers) childNode.Nodes.Add(new TreeNode(parser.EntryDescription) { Tag = parser });
                    }

                    parentNode.Nodes.Add(childNode);
                }
            }
            else
            {
                TreeNode parserNode = new TreeNode(parserType.GetAttribute<ParserDescriptor>().Description) { Tag = currentParsers };

                if (customChildCreator != null)
                    customChildCreator.Invoke(parserNode, currentParsers);
                else
                {
                    foreach (BaseParser parser in currentParsers) parserNode.Nodes.Add(new TreeNode(parser.EntryDescription) { Tag = parser });
                }

                nodes.Add(parserNode);
            }

            return nodes;
        }
    }
}
