namespace Yggdrasil
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpTableData = new System.Windows.Forms.TabPage();
            this.tpMessages = new System.Windows.Forms.TabPage();
            this.tableEntryEditor = new Yggdrasil.Controls.TableEntryEditor();
            this.messageEditor = new Yggdrasil.Controls.MessageEditor();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tpTableData.SuspendLayout();
            this.tpMessages.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(684, 24);
            this.menuStrip.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFolderToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.openFolderToolStripMenuItem.Text = "&Open Folder...";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.saveToolStripMenuItem.Text = "&Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(190, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 540);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(684, 22);
            this.statusStrip.TabIndex = 1;
            // 
            // tsslStatus
            // 
            this.tsslStatus.Name = "tsslStatus";
            this.tsslStatus.Size = new System.Drawing.Size(22, 17);
            this.tsslStatus.Text = "---";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpTableData);
            this.tabControl.Controls.Add(this.tpMessages);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(684, 516);
            this.tabControl.TabIndex = 2;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            // 
            // tpTableData
            // 
            this.tpTableData.Controls.Add(this.tableEntryEditor);
            this.tpTableData.Location = new System.Drawing.Point(4, 22);
            this.tpTableData.Name = "tpTableData";
            this.tpTableData.Padding = new System.Windows.Forms.Padding(3);
            this.tpTableData.Size = new System.Drawing.Size(676, 490);
            this.tpTableData.TabIndex = 1;
            this.tpTableData.Text = "Data Tables";
            this.tpTableData.UseVisualStyleBackColor = true;
            // 
            // tpMessages
            // 
            this.tpMessages.Controls.Add(this.messageEditor);
            this.tpMessages.Location = new System.Drawing.Point(4, 22);
            this.tpMessages.Name = "tpMessages";
            this.tpMessages.Padding = new System.Windows.Forms.Padding(3);
            this.tpMessages.Size = new System.Drawing.Size(676, 490);
            this.tpMessages.TabIndex = 0;
            this.tpMessages.Text = "Messages";
            this.tpMessages.UseVisualStyleBackColor = true;
            // 
            // tableEntryEditor
            // 
            this.tableEntryEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableEntryEditor.Enabled = false;
            this.tableEntryEditor.Location = new System.Drawing.Point(3, 3);
            this.tableEntryEditor.Name = "tableEntryEditor";
            this.tableEntryEditor.Size = new System.Drawing.Size(670, 484);
            this.tableEntryEditor.SplitterPosition = 222;
            this.tableEntryEditor.TabIndex = 6;
            // 
            // messageEditor
            // 
            this.messageEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageEditor.Enabled = false;
            this.messageEditor.Location = new System.Drawing.Point(3, 3);
            this.messageEditor.Name = "messageEditor";
            this.messageEditor.Size = new System.Drawing.Size(670, 484);
            this.messageEditor.SplitterPosition = 222;
            this.messageEditor.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 562);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tpTableData.ResumeLayout(false);
            this.tpMessages.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpMessages;
        private Controls.MessageEditor messageEditor;
        private System.Windows.Forms.TabPage tpTableData;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private Controls.TableEntryEditor tableEntryEditor;
    }
}