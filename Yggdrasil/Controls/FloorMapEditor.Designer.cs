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
            this.gridEditControl1 = new Yggdrasil.Controls.GridEditControl();
            this.SuspendLayout();
            // 
            // gridEditControl1
            // 
            this.gridEditControl1.Location = new System.Drawing.Point(0, 0);
            this.gridEditControl1.MinimumSize = new System.Drawing.Size(24, 24);
            this.gridEditControl1.Name = "gridEditControl1";
            this.gridEditControl1.ShowGrid = true;
            this.gridEditControl1.Size = new System.Drawing.Size(192, 192);
            this.gridEditControl1.TabIndex = 0;
            this.gridEditControl1.Zoom = 3;
            // 
            // FloorMapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridEditControl1);
            this.Name = "FloorMapEditor";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);

        }

        #endregion

        private GridEditControl gridEditControl1;
    }
}
