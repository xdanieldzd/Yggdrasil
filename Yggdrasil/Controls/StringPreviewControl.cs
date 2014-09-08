using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Yggdrasil.TextHandling;
using Yggdrasil.FileHandling.TableHandling;

namespace Yggdrasil.Controls
{
    public partial class StringPreviewControl : UserControl
    {
        GameDataManager gameDataManager;
        MessageTable messageTable;
        int messageNo;

        EtrianString etrianString;

        bool isInitializing;
        Bitmap renderedString;

        public StringPreviewControl()
        {
            InitializeComponent();

            Application.Idle += ((s, e) => { pbPreview.Invalidate(); });

            this.txtString.TextChanged += new EventHandler(txtString_TextChanged);
        }

        public void Initialize(GameDataManager gameDataManager, MessageTable messageTable, int messageNo)
        {
            this.gameDataManager = gameDataManager;
            this.messageTable = messageTable;
            this.messageNo = messageNo;

            this.etrianString = this.messageTable.Messages[this.messageNo];

            isInitializing = true;
            UpdateString();
            isInitializing = false;
        }

        public void Terminate()
        {
            etrianString = null;
            if (renderedString != null) renderedString.Dispose();
            txtString.Text = string.Empty;
        }

        private void UpdateString()
        {
            if (renderedString != null) renderedString.Dispose();

            renderedString = gameDataManager.FontRenderer.RenderString(this.etrianString, 256, 1);
            txtString.Text = this.etrianString.ConvertedString;
        }

        private void pbPreview_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            if (etrianString != null && renderedString != null) e.Graphics.DrawImageUnscaled(renderedString, 1, 1);
        }

        private void txtString_TextChanged(object sender, EventArgs e)
        {
            if (isInitializing || this.etrianString == null) return;

            this.gameDataManager.SetMessageString(messageTable, messageNo, txtString.Text);
            UpdateString();
        }
    }
}
