using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using Microsoft.WindowsAPICodePack.Dialogs;

using Yggdrasil.Forms;
using Yggdrasil.Helpers;
using Yggdrasil.Controls;

namespace Yggdrasil
{
    public partial class MainForm : ModernForm
    {
        GameDataManager gameDataManager;

        public string StatusText
        {
            get { return tsslStatus.Text; }
            set
            {
                if (InvokeRequired) BeginInvoke(new Action(() => { tsslStatus.Text = value; }));
                else tsslStatus.Text = value;
            }
        }

        public MainForm()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

            InitializeComponent();

            SetFormTitle();
            StatusText = "Ready";

            gameDataManager = new GameDataManager();
            gameDataManager.Language = Configuration.Language;

            gameDataManager.GameDataPropertyChangedEvent += new PropertyChangedEventHandler(gameDataManager_ItemDataPropertyChangedEvent);
            gameDataManager.SelectedLanguageChangedEvent += new EventHandler(gameDataManager_SelectedLanguageChangedEvent);

            foreach (GameDataManager.Languages language in Enum.GetValues(typeof(GameDataManager.Languages)))
            {
                gameLanguageToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem(language.ToString(), null, new EventHandler((s, e) =>
                {
                    ToolStripMenuItem menuItem = (s as ToolStripMenuItem);
                    Configuration.Language = gameDataManager.Language = (GameDataManager.Languages)menuItem.Tag;

                    foreach (ToolStripMenuItem checkMenuItem in gameLanguageToolStripMenuItem.DropDownItems)
                        checkMenuItem.Checked = (checkMenuItem.Tag is GameDataManager.Languages && (GameDataManager.Languages)checkMenuItem.Tag == Configuration.Language);
                })) { Tag = language, Checked = (language == Configuration.Language) });
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = ((gameDataManager.AnyDataHasChanged) &&
                MessageBox.Show("Data has been changed. Discard changes and quit without saving?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No);
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            InitializeTabPage(e.TabPage);
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandOpenFolder();
        }

        private void openFolderToolStripButton_Click(object sender, EventArgs e)
        {
            CommandOpenFolder();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSave();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            CommandSave();
        }

        private void unpackROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandUnpackROM();
        }

        private void unpackROMToolStripButton_Click(object sender, EventArgs e)
        {
            CommandUnpackROM();
        }

        private void repackROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandRepackROM();
        }

        private void repackROMToolStripButton_Click(object sender, EventArgs e)
        {
            CommandRepackROM();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dumpMainFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fontName = System.IO.Path.GetFileNameWithoutExtension(gameDataManager.MainFontFilename);
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Dump main font";
            sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            sfd.FileName = fontName + ".txt";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                gameDataManager.MainFontRenderer.DumpMkwinfont(sfd.FileName, fontName);
                StatusText = "Main font dumped";
            }
        }

        private void dumpSmallFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fontName = System.IO.Path.GetFileNameWithoutExtension(gameDataManager.SmallFontFilename);
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Dump small font";
            sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            sfd.FileName = fontName + ".txt";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                gameDataManager.SmallFontRenderer.DumpMkwinfont(sfd.FileName, fontName);
                StatusText = "Small font dumped";
            }
        }

        private void showMessageLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Logger.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandAbout();
        }

        private void aboutToolStripButton_Click(object sender, EventArgs e)
        {
            CommandAbout();
        }

        private void gameDataManager_ItemDataPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            if (gameDataManager.AnyDataHasChanged)
            {
                int changeCount = (gameDataManager.ChangedDataCount + gameDataManager.ChangedMessageFileCount + gameDataManager.ChangedMapTileCount);
                StatusText = string.Format("Ready; {0} changes", changeCount);
                saveToolStripButton.Enabled = saveToolStripMenuItem.Enabled = true;
            }
            else
            {
                StatusText = "Ready";
                saveToolStripButton.Enabled = saveToolStripMenuItem.Enabled = false;
            }
        }

        private void gameDataManager_SelectedLanguageChangedEvent(object sender, EventArgs e)
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                IEditorControl editorControl = (IEditorControl)tabPage.Controls.OfType<Control>().FirstOrDefault(x => x is IEditorControl);
                if (editorControl != null && editorControl.IsInitialized()) editorControl.Rebuild();
            }
        }

        private void CommandOpenFolder()
        {
            string selectedPath;
            if (GUIHelpers.ShowFolderBrowser("Select game folder", "Please select folder with extracted Etrian Odyssey game data.", Configuration.LastDataPath, out selectedPath) == System.Windows.Forms.DialogResult.OK)
                LoadGameData(selectedPath);
        }

        private void CommandSave()
        {
            if (gameDataManager.AnyDataHasChanged)
            {
                int changedFiles = gameDataManager.SaveAllChanges();

                if (changedFiles == 0)
                    StatusText = "No changes to save";
                else
                    StatusText = string.Format("Data saved; {0} {1} changed", changedFiles, (changedFiles == 1 ? "file" : "files"));

                tableEntryEditor.Refresh();
                floorMapEditor.Refresh();
                saveToolStripButton.Enabled = saveToolStripMenuItem.Enabled = false;
            }
        }

        private void CommandUnpackROM()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select source ROM";
            ofd.Filter = "Nintendo DS ROMs (*.nds)|*.nds|All Files (*.*)|*.*";
            ofd.InitialDirectory = Path.GetDirectoryName(Configuration.LastROMPath);
            ofd.FileName = Path.GetFileName(Configuration.LastROMPath);
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string outputPath;
                if (GUIHelpers.ShowFolderBrowser("Select output folder", "Please select folder for extracted game data.", Configuration.LastDataPath, out outputPath) != System.Windows.Forms.DialogResult.OK) return;

                if (Directory.EnumerateFileSystemEntries(outputPath).Any() &&
                    MessageBox.Show("The selected folder is not empty. Continue anyway?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                    return;

                Program.Logger.LogMessage(true, "Extracting game data from file {0}...", Path.GetFileName(ofd.FileName));

                RunNdstool(string.Format(
                    @"-x ""{0}"" -9 ""{1}\\arm9.bin"" -7 ""{1}\\arm7.bin"" -y9 ""{1}\\y9.bin"" -y7 ""{1}\\y7.bin"" -d ""{1}\\data"" -y ""{1}\\overlay"" -t ""{1}\\banner.bin"" -h ""{1}\\header.bin""",
                    ofd.FileName, outputPath));

                Configuration.LastROMPath = ofd.FileName;
                LoadGameData(outputPath);
            }
        }

        private void CommandRepackROM()
        {
            string inputPath;
            if (GUIHelpers.ShowFolderBrowser("Select game folder", "Please select folder with extracted game data.", Configuration.LastDataPath, out inputPath) != System.Windows.Forms.DialogResult.OK) return;

            if (gameDataManager.IsInitialized && inputPath == gameDataManager.DataPath && (gameDataManager.AnyDataHasChanged))
            {
                if (MessageBox.Show("Data has been changed. Save changes before creating ROM file?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    CommandSave();
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Select destination ROM";
            sfd.Filter = "Nintendo DS ROMs (*.nds)|*.nds|All Files (*.*)|*.*";
            sfd.InitialDirectory = Path.GetDirectoryName(Configuration.LastROMPath);
            sfd.FileName = Path.GetFileName(Configuration.LastROMPath);
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<string> files = Directory.EnumerateFileSystemEntries(inputPath).ToList();
                if (!files.Contains(Path.Combine(inputPath, "arm9.bin")) ||
                    !files.Contains(Path.Combine(inputPath, "arm7.bin")) ||
                    !files.Contains(Path.Combine(inputPath, "y9.bin")) ||
                    !files.Contains(Path.Combine(inputPath, "y7.bin")) ||
                    !files.Contains(Path.Combine(inputPath, "data")) ||
                    !files.Contains(Path.Combine(inputPath, "overlay")) ||
                    !files.Contains(Path.Combine(inputPath, "banner.bin")) ||
                    !files.Contains(Path.Combine(inputPath, "header.bin")))
                {
                    throw new FileNotFoundException("File required to repack ROM was not found");
                }

                Program.Logger.LogMessage(true, "Creating ROM file {0} from extracted data...", Path.GetFileName(sfd.FileName));

                RunNdstool(string.Format(
                    @"-c ""{0}"" -9 ""{1}\\arm9.bin"" -7 ""{1}\\arm7.bin"" -y9 ""{1}\\y9.bin"" -y7 ""{1}\\y7.bin"" -d ""{1}\\data"" -y ""{1}\\overlay"" -t ""{1}\\banner.bin"" -h ""{1}\\header.bin""",
                    sfd.FileName, inputPath));

                Configuration.LastROMPath = sfd.FileName;
                Program.Logger.LogMessage(true, "Ready; file created");
            }
        }

        private void RunNdstool(string arguments)
        {
            Cursor.Current = Cursors.WaitCursor;

            string ndstoolPath = Path.Combine(Path.GetTempPath(), "ndstool.exe");
            File.WriteAllBytes(ndstoolPath, Properties.Resources.ndstool);
            if (!File.Exists(ndstoolPath)) throw new FileNotFoundException("Could not extract ndstool from resources to temp directory");

            ProcessStartInfo startInfo = new ProcessStartInfo(ndstoolPath, arguments);
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process ndstool = Process.Start(startInfo);
            ndstool.WaitForExit();

            File.Delete(ndstoolPath);

            Cursor.Current = Cursors.Default;
        }

        private void CommandAbout()
        {
            string description = System.Reflection.Assembly.GetExecutingAssembly().GetAttribute<System.Reflection.AssemblyDescriptionAttribute>().Description;
            string copyright = System.Reflection.Assembly.GetExecutingAssembly().GetAttribute<System.Reflection.AssemblyCopyrightAttribute>().Copyright;

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0} - {1}\n", Program.TitleString, description);
            builder.AppendLine(copyright);
            builder.AppendLine();
            builder.Append("Contains ndstool 1.31 by Rafael Vuijk (DarkFader)");
            MessageBox.Show(builder.ToString(), "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SetFormTitle()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(Program.TitleString);
            if (gameDataManager != null && gameDataManager.IsInitialized) stringBuilder.AppendFormat(" - [{0}]", gameDataManager.Header.GameTitle);
            this.Text = stringBuilder.ToString();
        }

        private void LoadGameData(string path)
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                IEditorControl editorControl = (IEditorControl)tabPage.Controls.OfType<Control>().FirstOrDefault(x => x is IEditorControl);
                if (editorControl != null && editorControl.IsInitialized()) editorControl.Terminate();
                tabPage.Tag = null;
            }

            gameDataManager.ReadGameDirectory(Configuration.LastDataPath = path);

            tableEntryEditor.Enabled = messageEditor.Enabled = floorMapEditor.Enabled = (gameDataManager != null && gameDataManager.IsInitialized);
            gameLanguageToolStripMenuItem.Enabled = (gameDataManager != null && gameDataManager.Version == GameDataManager.Versions.European);

            if (gameDataManager.IsInitialized)
            {
                SetFormTitle();
                InitializeTabPage(tabControl.SelectedTab);

                StatusText = "Data loaded";
                if (gameDataManager.Version != GameDataManager.Versions.Japanese)
                    dumpMainFontToolStripMenuItem.Enabled = dumpSmallFontToolStripMenuItem.Enabled = true;
            }
        }

        private void InitializeTabPage(TabPage tabPage)
        {
            if ((gameDataManager != null && gameDataManager.IsInitialized) && tabPage.Tag == null)
            {
                IEditorControl editorControl = (IEditorControl)tabPage.Controls.OfType<Control>().FirstOrDefault(x => x is IEditorControl);
                if (editorControl != null && !editorControl.IsInitialized()) editorControl.Initialize(gameDataManager);
                tabPage.Tag = gameDataManager;
            }
        }
    }
}
