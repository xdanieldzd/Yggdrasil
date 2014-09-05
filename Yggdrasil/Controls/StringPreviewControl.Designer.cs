namespace Yggdrasil.Controls
{
    partial class StringPreviewControl
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
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.txtString = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // pbPreview
            // 
            this.pbPreview.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbPreview.Location = new System.Drawing.Point(0, 0);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(400, 200);
            this.pbPreview.TabIndex = 0;
            this.pbPreview.TabStop = false;
            this.pbPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pbPreview_Paint);
            // 
            // txtString
            // 
            this.txtString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtString.Location = new System.Drawing.Point(0, 200);
            this.txtString.Multiline = true;
            this.txtString.Name = "txtString";
            this.txtString.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtString.Size = new System.Drawing.Size(400, 200);
            this.txtString.TabIndex = 0;
            // 
            // StringPreviewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtString);
            this.Controls.Add(this.pbPreview);
            this.Name = "StringPreviewControl";
            this.Size = new System.Drawing.Size(400, 400);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.TextBox txtString;
    }
}
