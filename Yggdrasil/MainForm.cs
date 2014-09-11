using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

            gameDataManager.ItemDataPropertyChangedEvent += new PropertyChangedEventHandler(gameDataManager_ItemDataPropertyChangedEvent);
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
            e.Cancel = ((gameDataManager.DataHasChanged || gameDataManager.MessageFileHasChanged) &&
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            if (gameDataManager.DataHasChanged || gameDataManager.MessageFileHasChanged)
            {
                int changeCount = (gameDataManager.ChangedDataCount + gameDataManager.ChangedMessageFileCount);
                StatusText = string.Format("Ready; {0} {1} changed", changeCount, (changeCount != 1 ? "entries" : "entry"));
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
            if (CommonOpenFileDialog.IsPlatformSupported)
            {
                CommonOpenFileDialog ofd = new CommonOpenFileDialog();
                ofd.IsFolderPicker = true;
                ofd.InitialDirectory = Configuration.LastDataPath;
                ofd.Title = "Select game folder";
                if (ofd.ShowDialog() == CommonFileDialogResult.Ok) LoadGameData(ofd.FileName);
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Please select folder with extracted Etrian Odyssey game data.";
                fbd.SelectedPath = Configuration.LastDataPath;
                fbd.ShowNewFolderButton = false;
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK) LoadGameData(fbd.SelectedPath);
            }
        }

        private void CommandSave()
        {
            if (gameDataManager.DataHasChanged || gameDataManager.MessageFileHasChanged)
            {
                int changedFiles = gameDataManager.SaveAllChanges();

                if (changedFiles == 0)
                    StatusText = "No changes to save";
                else
                    StatusText = string.Format("Data saved; {0} {1} changed", changedFiles, (changedFiles == 1 ? "file" : "files"));

                tableEntryEditor.Refresh();
                saveToolStripButton.Enabled = saveToolStripMenuItem.Enabled = false;
            }
        }

        private void CommandAbout()
        {
            string description = System.Reflection.Assembly.GetExecutingAssembly().GetAttribute<System.Reflection.AssemblyDescriptionAttribute>().Description;

            MessageBox.Show(string.Format("{0} - {1}\n\nWritten 2014 by xdaniel - https://github.com/xdanieldzd/", Program.TitleString, description),
                "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            tableEntryEditor.Enabled = messageEditor.Enabled = (gameDataManager != null && gameDataManager.IsInitialized);
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
    }
}
