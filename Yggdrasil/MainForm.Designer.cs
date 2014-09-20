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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.unpackROMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.repackROMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpMainFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpSmallFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.showMessageLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameLanguageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpTableData = new System.Windows.Forms.TabPage();
            this.tableEntryEditor = new Yggdrasil.Controls.TableEntryEditor();
            this.tpMessages = new System.Windows.Forms.TabPage();
            this.messageEditor = new Yggdrasil.Controls.MessageEditor();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.openFolderToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.unpackROMToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.repackROMToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.tpFloorMaps = new System.Windows.Forms.TabPage();
            this.floorMapEditor = new Yggdrasil.Controls.FloorMapEditor();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tpTableData.SuspendLayout();
            this.tpMessages.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tpFloorMaps.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(704, 24);
            this.menuStrip.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFolderToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.unpackROMToolStripMenuItem,
            this.repackROMToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.openFolderToolStripMenuItem.Text = "&Open Folder...";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.saveToolStripMenuItem.Text = "&Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(194, 6);
            // 
            // unpackROMToolStripMenuItem
            // 
            this.unpackROMToolStripMenuItem.Name = "unpackROMToolStripMenuItem";
            this.unpackROMToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.unpackROMToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.unpackROMToolStripMenuItem.Text = "&Unpack ROM...";
            this.unpackROMToolStripMenuItem.Click += new System.EventHandler(this.unpackROMToolStripMenuItem_Click);
            // 
            // repackROMToolStripMenuItem
            // 
            this.repackROMToolStripMenuItem.Name = "repackROMToolStripMenuItem";
            this.repackROMToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.repackROMToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.repackROMToolStripMenuItem.Text = "&Repack ROM";
            this.repackROMToolStripMenuItem.Click += new System.EventHandler(this.repackROMToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(194, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dumpMainFontToolStripMenuItem,
            this.dumpSmallFontToolStripMenuItem,
            this.toolStripMenuItem2,
            this.showMessageLogToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // dumpMainFontToolStripMenuItem
            // 
            this.dumpMainFontToolStripMenuItem.Enabled = false;
            this.dumpMainFontToolStripMenuItem.Name = "dumpMainFontToolStripMenuItem";
            this.dumpMainFontToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.dumpMainFontToolStripMenuItem.Text = "Dump &Main Font...";
            this.dumpMainFontToolStripMenuItem.Click += new System.EventHandler(this.dumpMainFontToolStripMenuItem_Click);
            // 
            // dumpSmallFontToolStripMenuItem
            // 
            this.dumpSmallFontToolStripMenuItem.Enabled = false;
            this.dumpSmallFontToolStripMenuItem.Name = "dumpSmallFontToolStripMenuItem";
            this.dumpSmallFontToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.dumpSmallFontToolStripMenuItem.Text = "Dump &Small Font...";
            this.dumpSmallFontToolStripMenuItem.Click += new System.EventHandler(this.dumpSmallFontToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(181, 6);
            // 
            // showMessageLogToolStripMenuItem
            // 
            this.showMessageLogToolStripMenuItem.Name = "showMessageLogToolStripMenuItem";
            this.showMessageLogToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.showMessageLogToolStripMenuItem.Text = "Show Message &Log...";
            this.showMessageLogToolStripMenuItem.Click += new System.EventHandler(this.showMessageLogToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameLanguageToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // gameLanguageToolStripMenuItem
            // 
            this.gameLanguageToolStripMenuItem.Name = "gameLanguageToolStripMenuItem";
            this.gameLanguageToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.gameLanguageToolStripMenuItem.Text = "Game &Language...";
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
            this.statusStrip.Location = new System.Drawing.Point(0, 555);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(704, 22);
            this.statusStrip.TabIndex = 2;
            // 
            // tsslStatus
            // 
            this.tsslStatus.Name = "tsslStatus";
            this.tsslStatus.Size = new System.Drawing.Size(22, 17);
            this.tsslStatus.Text = "---";
            this.tsslStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpTableData);
            this.tabControl.Controls.Add(this.tpFloorMaps);
            this.tabControl.Controls.Add(this.tpMessages);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 49);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(704, 506);
            this.tabControl.TabIndex = 1;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            // 
            // tpTableData
            // 
            this.tpTableData.Controls.Add(this.tableEntryEditor);
            this.tpTableData.Location = new System.Drawing.Point(4, 22);
            this.tpTableData.Name = "tpTableData";
            this.tpTableData.Padding = new System.Windows.Forms.Padding(3);
            this.tpTableData.Size = new System.Drawing.Size(696, 480);
            this.tpTableData.TabIndex = 0;
            this.tpTableData.Text = "Data Tables";
            this.tpTableData.UseVisualStyleBackColor = true;
            // 
            // tableEntryEditor
            // 
            this.tableEntryEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableEntryEditor.Enabled = false;
            this.tableEntryEditor.Location = new System.Drawing.Point(3, 3);
            this.tableEntryEditor.Name = "tableEntryEditor";
            this.tableEntryEditor.Size = new System.Drawing.Size(690, 474);
            this.tableEntryEditor.TabIndex = 0;
            // 
            // tpMessages
            // 
            this.tpMessages.Controls.Add(this.messageEditor);
            this.tpMessages.Location = new System.Drawing.Point(4, 22);
            this.tpMessages.Name = "tpMessages";
            this.tpMessages.Padding = new System.Windows.Forms.Padding(3);
            this.tpMessages.Size = new System.Drawing.Size(696, 480);
            this.tpMessages.TabIndex = 1;
            this.tpMessages.Text = "Messages";
            this.tpMessages.UseVisualStyleBackColor = true;
            // 
            // messageEditor
            // 
            this.messageEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageEditor.Enabled = false;
            this.messageEditor.Location = new System.Drawing.Point(3, 3);
            this.messageEditor.Name = "messageEditor";
            this.messageEditor.Size = new System.Drawing.Size(690, 474);
            this.messageEditor.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFolderToolStripButton,
            this.saveToolStripButton,
            this.toolStripSeparator1,
            this.unpackROMToolStripButton,
            this.repackROMToolStripButton,
            this.toolStripSeparator2,
            this.aboutToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(704, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // openFolderToolStripButton
            // 
            this.openFolderToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openFolderToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openFolderToolStripButton.Image")));
            this.openFolderToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openFolderToolStripButton.Name = "openFolderToolStripButton";
            this.openFolderToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openFolderToolStripButton.Text = "&Open";
            this.openFolderToolStripButton.Click += new System.EventHandler(this.openFolderToolStripButton_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Enabled = false;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // unpackROMToolStripButton
            // 
            this.unpackROMToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.unpackROMToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("unpackROMToolStripButton.Image")));
            this.unpackROMToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.unpackROMToolStripButton.Name = "unpackROMToolStripButton";
            this.unpackROMToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.unpackROMToolStripButton.Text = "&Unpack ROM";
            this.unpackROMToolStripButton.Click += new System.EventHandler(this.unpackROMToolStripButton_Click);
            // 
            // repackROMToolStripButton
            // 
            this.repackROMToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.repackROMToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("repackROMToolStripButton.Image")));
            this.repackROMToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.repackROMToolStripButton.Name = "repackROMToolStripButton";
            this.repackROMToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.repackROMToolStripButton.Text = "&Repack ROM";
            this.repackROMToolStripButton.Click += new System.EventHandler(this.repackROMToolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // aboutToolStripButton
            // 
            this.aboutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.aboutToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("aboutToolStripButton.Image")));
            this.aboutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.aboutToolStripButton.Name = "aboutToolStripButton";
            this.aboutToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.aboutToolStripButton.Text = "&About";
            this.aboutToolStripButton.Click += new System.EventHandler(this.aboutToolStripButton_Click);
            // 
            // tpFloorMaps
            // 
            this.tpFloorMaps.Controls.Add(this.floorMapEditor);
            this.tpFloorMaps.Location = new System.Drawing.Point(4, 22);
            this.tpFloorMaps.Name = "tpFloorMaps";
            this.tpFloorMaps.Padding = new System.Windows.Forms.Padding(3);
            this.tpFloorMaps.Size = new System.Drawing.Size(696, 480);
            this.tpFloorMaps.TabIndex = 2;
            this.tpFloorMaps.Text = "Floor Maps";
            this.tpFloorMaps.UseVisualStyleBackColor = true;
            // 
            // floorMapEditor1
            // 
            this.floorMapEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.floorMapEditor.Enabled = false;
            this.floorMapEditor.Location = new System.Drawing.Point(3, 3);
            this.floorMapEditor.Name = "floorMapEditor1";
            this.floorMapEditor.Size = new System.Drawing.Size(690, 474);
            this.floorMapEditor.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 577);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tpFloorMaps.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showMessageLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gameLanguageToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton openFolderToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton aboutToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem dumpMainFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dumpSmallFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unpackROMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem repackROMToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripButton unpackROMToolStripButton;
        private System.Windows.Forms.ToolStripButton repackROMToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TabPage tpFloorMaps;
        private Controls.FloorMapEditor floorMapEditor;
    }
}