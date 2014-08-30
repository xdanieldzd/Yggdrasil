namespace Yggdrasil.Forms
{
    partial class ProgressForm
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblInstructionText = new System.Windows.Forms.Label();
            this.lblDetailText = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.progressBar.Location = new System.Drawing.Point(12, 49);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.progressBar.MarqueeAnimationSpeed = 10;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(320, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 0;
            this.progressBar.UseWaitCursor = true;
            // 
            // lblInstructionText
            // 
            this.lblInstructionText.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblInstructionText.Location = new System.Drawing.Point(12, 9);
            this.lblInstructionText.Name = "lblInstructionText";
            this.lblInstructionText.Size = new System.Drawing.Size(320, 35);
            this.lblInstructionText.TabIndex = 1;
            this.lblInstructionText.Text = "InstructionText";
            this.lblInstructionText.UseWaitCursor = true;
            // 
            // lblDetailText
            // 
            this.lblDetailText.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDetailText.Location = new System.Drawing.Point(12, 77);
            this.lblDetailText.Name = "lblDetailText";
            this.lblDetailText.Size = new System.Drawing.Size(320, 15);
            this.lblDetailText.TabIndex = 2;
            this.lblDetailText.Text = "DetailText";
            this.lblDetailText.UseWaitCursor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCancel.Location = new System.Drawing.Point(257, 97);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.UseWaitCursor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 132);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblDetailText);
            this.Controls.Add(this.lblInstructionText);
            this.Controls.Add(this.progressBar);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Title";
            this.UseWaitCursor = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.ProgressBar progressBar;
        public System.Windows.Forms.Label lblInstructionText;
        public System.Windows.Forms.Label lblDetailText;
    }
}