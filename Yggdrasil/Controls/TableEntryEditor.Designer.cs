namespace Yggdrasil.Controls
{
    partial class TableEntryEditor
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
            this.pgData = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvParsers = new Yggdrasil.Controls.TreeViewEx();
            this.cmsDataGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetPropertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.cmsDataGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // pgData
            // 
            this.pgData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgData.Location = new System.Drawing.Point(0, 0);
            this.pgData.Name = "pgData";
            this.pgData.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgData.Size = new System.Drawing.Size(330, 400);
            this.pgData.TabIndex = 0;
            this.pgData.ToolbarVisible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvParsers);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pgData);
            this.splitContainer1.Size = new System.Drawing.Size(500, 400);
            this.splitContainer1.SplitterDistance = 166;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // tvParsers
            // 
            this.tvParsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvParsers.HideSelection = false;
            this.tvParsers.Location = new System.Drawing.Point(0, 0);
            this.tvParsers.Name = "tvParsers";
            this.tvParsers.Size = new System.Drawing.Size(166, 400);
            this.tvParsers.TabIndex = 0;
            this.tvParsers.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvParsers_AfterSelect);
            // 
            // cmsDataGrid
            // 
            this.cmsDataGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetPropertyToolStripMenuItem});
            this.cmsDataGrid.Name = "cmsDataGrid";
            this.cmsDataGrid.Size = new System.Drawing.Size(153, 48);
            // 
            // resetPropertyToolStripMenuItem
            // 
            this.resetPropertyToolStripMenuItem.Name = "resetPropertyToolStripMenuItem";
            this.resetPropertyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.resetPropertyToolStripMenuItem.Tag = "&Reset Property";
            this.resetPropertyToolStripMenuItem.Text = "-----";
            this.resetPropertyToolStripMenuItem.Click += new System.EventHandler(this.resetPropertyToolStripMenuItem_Click);
            // 
            // TableEntryEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "TableEntryEditor";
            this.Size = new System.Drawing.Size(500, 400);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.cmsDataGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TreeViewEx tvParsers;
        private System.Windows.Forms.PropertyGrid pgData;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip cmsDataGrid;
        private System.Windows.Forms.ToolStripMenuItem resetPropertyToolStripMenuItem;
    }
}
