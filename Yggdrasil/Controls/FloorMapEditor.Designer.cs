namespace Yggdrasil.Controls
{
    partial class FloorMapEditor
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
            this.pnlMap = new System.Windows.Forms.Panel();
            this.gridEditControl = new Yggdrasil.Controls.GridEditControl();
            this.cbMaps = new System.Windows.Forms.ComboBox();
            this.pgMapTile = new Yggdrasil.Controls.PropertyGridEx();
            this.pnlMap.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMap
            // 
            this.pnlMap.AutoScroll = true;
            this.pnlMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMap.Controls.Add(this.gridEditControl);
            this.pnlMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMap.Location = new System.Drawing.Point(0, 21);
            this.pnlMap.Name = "pnlMap";
            this.pnlMap.Size = new System.Drawing.Size(300, 279);
            this.pnlMap.TabIndex = 1;
            // 
            // gridEditControl
            // 
            this.gridEditControl.Location = new System.Drawing.Point(3, 3);
            this.gridEditControl.MinimumSize = new System.Drawing.Size(16, 16);
            this.gridEditControl.Name = "gridEditControl";
            this.gridEditControl.Size = new System.Drawing.Size(144, 144);
            this.gridEditControl.TabIndex = 0;
            this.gridEditControl.TileSize = new System.Drawing.Size(16, 16);
            this.gridEditControl.TileClick += new System.EventHandler<Yggdrasil.Controls.TileClickEventArgs>(this.gridEditControl_TileClick);
            this.gridEditControl.Paint += new System.Windows.Forms.PaintEventHandler(this.gridEditControl_Paint);
            // 
            // cbMaps
            // 
            this.cbMaps.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbMaps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMaps.FormattingEnabled = true;
            this.cbMaps.Location = new System.Drawing.Point(0, 0);
            this.cbMaps.Name = "cbMaps";
            this.cbMaps.Size = new System.Drawing.Size(500, 21);
            this.cbMaps.TabIndex = 4;
            this.cbMaps.SelectedIndexChanged += new System.EventHandler(this.cbMaps_SelectedIndexChanged);
            // 
            // pgMapTile
            // 
            this.pgMapTile.Dock = System.Windows.Forms.DockStyle.Right;
            this.pgMapTile.Location = new System.Drawing.Point(300, 21);
            this.pgMapTile.Name = "pgMapTile";
            this.pgMapTile.Size = new System.Drawing.Size(200, 279);
            this.pgMapTile.TabIndex = 1;
            // 
            // FloorMapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMap);
            this.Controls.Add(this.pgMapTile);
            this.Controls.Add(this.cbMaps);
            this.Name = "FloorMapEditor";
            this.Size = new System.Drawing.Size(500, 300);
            this.pnlMap.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GridEditControl gridEditControl;
        private System.Windows.Forms.Panel pnlMap;
        private PropertyGridEx pgMapTile;
        private System.Windows.Forms.ComboBox cbMaps;
    }
}
