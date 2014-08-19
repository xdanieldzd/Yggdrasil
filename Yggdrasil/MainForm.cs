using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.WindowsAPICodePack.Dialogs;

using Yggdrasil.Helpers;
using Yggdrasil.Controls;

namespace Yggdrasil
{
    public partial class MainForm : Form
    {
        GameDataManager gameDataManager;

        public MainForm()
        {
            InitializeComponent();

            SetFormTitle();
            tsslStatus.Text = "Ready";

            if (Configuration.TableEntryEditorSplitter != -1) tableEntryEditor.SplitterPosition = Configuration.TableEntryEditorSplitter;
            if (Configuration.MessageEditorSplitter != -1) messageEditor.SplitterPosition = Configuration.MessageEditorSplitter;

            gameDataManager = new GameDataManager();
            gameDataManager.ItemDataPropertyChangedEvent += new PropertyChangedEventHandler(gameDataManager_ItemDataPropertyChangedEvent);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (gameDataManager.DataHasChanged &&
                MessageBox.Show("Data has been changed. Discard changes and quit without saving?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No);
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            InitializeTabPage(e.TabPage);
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
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
                fbd.ShowNewFolderButton = false;
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK) LoadGameData(fbd.SelectedPath);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gameDataManager.DataHasChanged)
            {
                int changedFiles = gameDataManager.SaveAllChanges();

                if (changedFiles == 0)
                    tsslStatus.Text = "No changes to save";
                else
                    tsslStatus.Text = string.Format("Data saved; {0} {1} changed", changedFiles, (changedFiles == 1 ? "file" : "files"));

                tableEntryEditor.Refresh();
                saveToolStripMenuItem.Enabled = false;
            }
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
            MessageBox.Show(string.Format("{0} {1}\n\nWritten 2014 by xdaniel - https://github.com/xdanieldzd/", Application.ProductName, VersionManagement.CreateVersionString(Application.ProductVersion)),
                "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void gameDataManager_ItemDataPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            if (gameDataManager.DataHasChanged)
            {
                tsslStatus.Text = string.Format("Ready; {0} {1} changed", gameDataManager.ChangedDataCount, (gameDataManager.ChangedDataCount != 1 ? "entries" : "entry"));
                saveToolStripMenuItem.Enabled = true;
            }
            else
            {
                tsslStatus.Text = "Ready";
                saveToolStripMenuItem.Enabled = false;
            }
        }

        private void SetFormTitle()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0} {1}", Application.ProductName, VersionManagement.CreateVersionString(Application.ProductVersion));
            if (gameDataManager != null && gameDataManager.IsInitialized) stringBuilder.AppendFormat(" - [{0}]", gameDataManager.Header.GameTitle);
            this.Text = stringBuilder.ToString();
        }

        private void LoadGameData(string path)
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                IEditorControl editorControl = (IEditorControl)tabPage.Controls.OfType<Control>().FirstOrDefault(x => x is IEditorControl);
                if (editorControl.IsInitialized()) editorControl.Terminate();
                tabPage.Tag = null;
            }

            gameDataManager.ReadGameDirectory(Configuration.LastDataPath = path);

            tableEntryEditor.Enabled = messageEditor.Enabled = (gameDataManager != null && gameDataManager.IsInitialized);

            if (gameDataManager.IsInitialized)
            {
                SetFormTitle();
                InitializeTabPage(tabControl.SelectedTab);

                tsslStatus.Text = "Data loaded";
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
