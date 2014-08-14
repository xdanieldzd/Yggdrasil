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
            this.pgData = new System.Windows.Forms.PropertyGrid();
            this.tvParsers = new Yggdrasil.Controls.TreeViewEx();
            this.SuspendLayout();
            // 
            // pgData
            // 
            this.pgData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgData.Location = new System.Drawing.Point(165, 0);
            this.pgData.Name = "pgData";
            this.pgData.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgData.Size = new System.Drawing.Size(335, 400);
            this.pgData.TabIndex = 5;
            this.pgData.ToolbarVisible = false;
            // 
            // tvParsers
            // 
            this.tvParsers.Dock = System.Windows.Forms.DockStyle.Left;
            this.tvParsers.HideSelection = false;
            this.tvParsers.Location = new System.Drawing.Point(0, 0);
            this.tvParsers.Name = "tvParsers";
            this.tvParsers.Size = new System.Drawing.Size(165, 400);
            this.tvParsers.TabIndex = 2;
            this.tvParsers.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvParsers_AfterSelect);
            // 
            // TableEntryEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pgData);
            this.Controls.Add(this.tvParsers);
            this.Name = "TableEntryEditor";
            this.Size = new System.Drawing.Size(500, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private TreeViewEx tvParsers;
        private System.Windows.Forms.PropertyGrid pgData;
    }
}
