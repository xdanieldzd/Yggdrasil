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
            this.chkSpacing = new System.Windows.Forms.CheckBox();
            this.pnlPreview = new System.Windows.Forms.Panel();
            this.chkSmallFont = new System.Windows.Forms.CheckBox();
            this.chkZoom = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.pnlPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = System.Drawing.Color.Black;
            this.pbPreview.Location = new System.Drawing.Point(0, 0);
            this.pbPreview.Margin = new System.Windows.Forms.Padding(0);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(100, 100);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbPreview.TabIndex = 0;
            this.pbPreview.TabStop = false;
            // 
            // txtString
            // 
            this.txtString.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtString.Location = new System.Drawing.Point(0, 260);
            this.txtString.Margin = new System.Windows.Forms.Padding(0);
            this.txtString.Multiline = true;
            this.txtString.Name = "txtString";
            this.txtString.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtString.Size = new System.Drawing.Size(400, 140);
            this.txtString.TabIndex = 0;
            // 
            // chkSpacing
            // 
            this.chkSpacing.AutoSize = true;
            this.chkSpacing.Location = new System.Drawing.Point(0, 240);
            this.chkSpacing.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.chkSpacing.Name = "chkSpacing";
            this.chkSpacing.Size = new System.Drawing.Size(132, 17);
            this.chkSpacing.TabIndex = 1;
            this.chkSpacing.Text = "Use Alternate Spacing";
            this.chkSpacing.UseVisualStyleBackColor = true;
            this.chkSpacing.CheckedChanged += new System.EventHandler(this.chkSpacing_CheckedChanged);
            // 
            // pnlPreview
            // 
            this.pnlPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPreview.AutoScroll = true;
            this.pnlPreview.BackColor = System.Drawing.Color.Black;
            this.pnlPreview.Controls.Add(this.pbPreview);
            this.pnlPreview.Location = new System.Drawing.Point(0, 0);
            this.pnlPreview.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPreview.Name = "pnlPreview";
            this.pnlPreview.Size = new System.Drawing.Size(400, 237);
            this.pnlPreview.TabIndex = 2;
            // 
            // chkSmallFont
            // 
            this.chkSmallFont.AutoSize = true;
            this.chkSmallFont.Location = new System.Drawing.Point(132, 240);
            this.chkSmallFont.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.chkSmallFont.Name = "chkSmallFont";
            this.chkSmallFont.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.chkSmallFont.Size = new System.Drawing.Size(103, 17);
            this.chkSmallFont.TabIndex = 3;
            this.chkSmallFont.Text = "Use Small Font";
            this.chkSmallFont.UseVisualStyleBackColor = true;
            this.chkSmallFont.CheckedChanged += new System.EventHandler(this.chkSmallFont_CheckedChanged);
            // 
            // chkZoom
            // 
            this.chkZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkZoom.AutoSize = true;
            this.chkZoom.Checked = true;
            this.chkZoom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkZoom.Location = new System.Drawing.Point(335, 240);
            this.chkZoom.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.chkZoom.Name = "chkZoom";
            this.chkZoom.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.chkZoom.Size = new System.Drawing.Size(65, 17);
            this.chkZoom.TabIndex = 4;
            this.chkZoom.Text = "Zoom";
            this.chkZoom.UseVisualStyleBackColor = true;
            this.chkZoom.CheckedChanged += new System.EventHandler(this.chkZoom_CheckedChanged);
            // 
            // StringPreviewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkZoom);
            this.Controls.Add(this.chkSmallFont);
            this.Controls.Add(this.pnlPreview);
            this.Controls.Add(this.txtString);
            this.Controls.Add(this.chkSpacing);
            this.DoubleBuffered = true;
            this.Name = "StringPreviewControl";
            this.Size = new System.Drawing.Size(400, 400);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.pnlPreview.ResumeLayout(false);
            this.pnlPreview.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.TextBox txtString;
        private System.Windows.Forms.CheckBox chkSpacing;
        private System.Windows.Forms.Panel pnlPreview;
        private System.Windows.Forms.CheckBox chkSmallFont;
        private System.Windows.Forms.CheckBox chkZoom;
    }
}
