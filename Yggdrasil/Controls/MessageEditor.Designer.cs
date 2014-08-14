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
            this.stringPreviewControl = new Yggdrasil.Controls.StringPreviewControl();
            this.tvMessageFiles = new Yggdrasil.Controls.TreeViewEx();
            this.SuspendLayout();
            // 
            // stringPreviewControl
            // 
            this.stringPreviewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stringPreviewControl.Location = new System.Drawing.Point(200, 0);
            this.stringPreviewControl.Name = "stringPreviewControl";
            this.stringPreviewControl.Size = new System.Drawing.Size(300, 400);
            this.stringPreviewControl.TabIndex = 2;
            // 
            // tvMessageFiles
            // 
            this.tvMessageFiles.Dock = System.Windows.Forms.DockStyle.Left;
            this.tvMessageFiles.HideSelection = false;
            this.tvMessageFiles.Location = new System.Drawing.Point(0, 0);
            this.tvMessageFiles.Name = "tvMessageFiles";
            this.tvMessageFiles.Size = new System.Drawing.Size(200, 400);
            this.tvMessageFiles.TabIndex = 1;
            this.tvMessageFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvMessageFiles_AfterSelect);
            // 
            // MessageEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stringPreviewControl);
            this.Controls.Add(this.tvMessageFiles);
            this.Name = "MessageEditor";
            this.Size = new System.Drawing.Size(500, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private TreeViewEx tvMessageFiles;
        private StringPreviewControl stringPreviewControl;
    }
}
