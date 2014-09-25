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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pgMapTile = new Yggdrasil.Controls.PropertyGridEx();
            this.chkEventOverlay = new System.Windows.Forms.CheckBox();
            this.chkGatherOverlay = new System.Windows.Forms.CheckBox();
            this.pnlMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMap
            // 
            this.pnlMap.AutoScroll = true;
            this.pnlMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMap.Controls.Add(this.gridEditControl);
            this.pnlMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMap.Location = new System.Drawing.Point(0, 0);
            this.pnlMap.Name = "pnlMap";
            this.pnlMap.Size = new System.Drawing.Size(250, 400);
            this.pnlMap.TabIndex = 1;
            // 
            // gridEditControl
            // 
            this.gridEditControl.Location = new System.Drawing.Point(0, 0);
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
            this.cbMaps.Size = new System.Drawing.Size(246, 21);
            this.cbMaps.TabIndex = 4;
            this.cbMaps.SelectedIndexChanged += new System.EventHandler(this.cbMaps_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pnlMap);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pgMapTile);
            this.splitContainer1.Panel2.Controls.Add(this.chkEventOverlay);
            this.splitContainer1.Panel2.Controls.Add(this.chkGatherOverlay);
            this.splitContainer1.Panel2.Controls.Add(this.cbMaps);
            this.splitContainer1.Size = new System.Drawing.Size(500, 400);
            this.splitContainer1.SplitterDistance = 250;
            this.splitContainer1.TabIndex = 5;
            // 
            // pgMapTile
            // 
            this.pgMapTile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgMapTile.Location = new System.Drawing.Point(0, 73);
            this.pgMapTile.Name = "pgMapTile";
            this.pgMapTile.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgMapTile.Size = new System.Drawing.Size(246, 327);
            this.pgMapTile.TabIndex = 1;
            this.pgMapTile.ToolbarVisible = false;
            this.pgMapTile.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgMapTile_PropertyValueChanged);
            // 
            // chkEventOverlay
            // 
            this.chkEventOverlay.AutoSize = true;
            this.chkEventOverlay.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkEventOverlay.Enabled = false;
            this.chkEventOverlay.Location = new System.Drawing.Point(0, 47);
            this.chkEventOverlay.Name = "chkEventOverlay";
            this.chkEventOverlay.Padding = new System.Windows.Forms.Padding(0, 3, 0, 6);
            this.chkEventOverlay.Size = new System.Drawing.Size(246, 26);
            this.chkEventOverlay.TabIndex = 6;
            this.chkEventOverlay.Text = "Overlay Event Locations";
            this.chkEventOverlay.UseVisualStyleBackColor = true;
            this.chkEventOverlay.CheckedChanged += new System.EventHandler(this.chkEventOverlay_CheckedChanged);
            // 
            // chkGatherOverlay
            // 
            this.chkGatherOverlay.AutoSize = true;
            this.chkGatherOverlay.Checked = true;
            this.chkGatherOverlay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGatherOverlay.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkGatherOverlay.Location = new System.Drawing.Point(0, 21);
            this.chkGatherOverlay.Name = "chkGatherOverlay";
            this.chkGatherOverlay.Padding = new System.Windows.Forms.Padding(0, 6, 0, 3);
            this.chkGatherOverlay.Size = new System.Drawing.Size(246, 26);
            this.chkGatherOverlay.TabIndex = 5;
            this.chkGatherOverlay.Text = "Overlay Gathering Points";
            this.chkGatherOverlay.UseVisualStyleBackColor = true;
            this.chkGatherOverlay.CheckedChanged += new System.EventHandler(this.chkGatherOverlay_CheckedChanged);
            // 
            // FloorMapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "FloorMapEditor";
            this.Size = new System.Drawing.Size(500, 400);
            this.pnlMap.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GridEditControl gridEditControl;
        private System.Windows.Forms.Panel pnlMap;
        private PropertyGridEx pgMapTile;
        private System.Windows.Forms.ComboBox cbMaps;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox chkGatherOverlay;
        private System.Windows.Forms.CheckBox chkEventOverlay;
    }
}
