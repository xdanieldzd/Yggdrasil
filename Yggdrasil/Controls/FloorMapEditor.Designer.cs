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
            this.gridEditControl = new Yggdrasil.Controls.GridEditControl();
            this.SuspendLayout();
            // 
            // gridEditControl
            // 
            this.gridEditControl.Location = new System.Drawing.Point(0, 0);
            this.gridEditControl.MinimumSize = new System.Drawing.Size(16, 16);
            this.gridEditControl.Name = "gridEditControl";
            this.gridEditControl.ShowGrid = true;
            this.gridEditControl.Size = new System.Drawing.Size(128, 128);
            this.gridEditControl.TabIndex = 0;
            this.gridEditControl.Zoom = 2;
            this.gridEditControl.TileClick += new System.EventHandler<Yggdrasil.Controls.TileClickEventArgs>(this.gridEditControl_TileClick);
            this.gridEditControl.Paint += new System.Windows.Forms.PaintEventHandler(this.gridEditControl_Paint);
            // 
            // FloorMapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridEditControl);
            this.Name = "FloorMapEditor";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);

        }

        #endregion

        private GridEditControl gridEditControl;
    }
}
