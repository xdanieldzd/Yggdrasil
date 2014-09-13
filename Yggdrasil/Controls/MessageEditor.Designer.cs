namespace Yggdrasil.Controls
{
    partial class MessageEditor
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.stringPreviewControl = new Yggdrasil.Controls.StringPreviewControl();
            this.tvMessageFiles = new Yggdrasil.Controls.TreeViewEx();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cmsTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createHTMLDumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.cmsTreeView.SuspendLayout();
            this.SuspendLayout();
            // 
            // stringPreviewControl
            // 
            this.stringPreviewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stringPreviewControl.Location = new System.Drawing.Point(0, 0);
            this.stringPreviewControl.Name = "stringPreviewControl";
            this.stringPreviewControl.Size = new System.Drawing.Size(296, 400);
            this.stringPreviewControl.TabIndex = 0;
            // 
            // tvMessageFiles
            // 
            this.tvMessageFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvMessageFiles.HideSelection = false;
            this.tvMessageFiles.Location = new System.Drawing.Point(0, 0);
            this.tvMessageFiles.Name = "tvMessageFiles";
            this.tvMessageFiles.Size = new System.Drawing.Size(200, 400);
            this.tvMessageFiles.TabIndex = 0;
            this.tvMessageFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvMessageFiles_AfterSelect);
            this.tvMessageFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvMessageFiles_MouseUp);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvMessageFiles);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.stringPreviewControl);
            this.splitContainer1.Size = new System.Drawing.Size(500, 400);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 0;
            // 
            // cmsTreeView
            // 
            this.cmsTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createHTMLDumpToolStripMenuItem});
            this.cmsTreeView.Name = "cmsTreeView";
            this.cmsTreeView.Size = new System.Drawing.Size(190, 26);
            // 
            // createHTMLDumpToolStripMenuItem
            // 
            this.createHTMLDumpToolStripMenuItem.Name = "createHTMLDumpToolStripMenuItem";
            this.createHTMLDumpToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.createHTMLDumpToolStripMenuItem.Text = "&Create HTML Dump...";
            this.createHTMLDumpToolStripMenuItem.Click += new System.EventHandler(this.createHTMLDumpToolStripMenuItem_Click);
            // 
            // MessageEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "MessageEditor";
            this.Size = new System.Drawing.Size(500, 400);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.cmsTreeView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TreeViewEx tvMessageFiles;
        private StringPreviewControl stringPreviewControl;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip cmsTreeView;
        private System.Windows.Forms.ToolStripMenuItem createHTMLDumpToolStripMenuItem;
    }
}
