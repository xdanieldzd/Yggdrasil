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
using Yggdrasil.Helpers;

namespace Yggdrasil.Controls
{
    public partial class StringPreviewControl : UserControl
    {
        GameDataManager gameDataManager;
        MessageTable messageTable;
        int messageNo;

        EtrianString etrianString;

        bool isBusy;
        Bitmap renderedString;

        public StringPreviewControl()
        {
            InitializeComponent();

            Application.Idle += ((s, e) => { pbPreview.Invalidate(); });

            this.txtString.TextChanged += new EventHandler(txtString_TextChanged);
        }

        public void Initialize(GameDataManager gameDataManager, MessageTable messageTable, int messageNo)
        {
            isBusy = true;

            this.gameDataManager = gameDataManager;
            this.messageTable = messageTable;
            this.messageNo = messageNo;

            this.etrianString = this.messageTable.Messages[this.messageNo];
            txtString.Text = this.etrianString.ConvertedString;

            RenderString();

            isBusy = false;
        }

        public void Terminate()
        {
            isBusy = true;

            etrianString = null;
            pbPreview.Image = null;
            if (renderedString != null) renderedString.Dispose();
            txtString.Text = string.Empty;

            isBusy = false;
        }

        private void RenderString()
        {
            if (renderedString != null) renderedString.Dispose();

            renderedString =
                (chkSmallFont.Checked ? this.gameDataManager.SmallFontRenderer : this.gameDataManager.MainFontRenderer)
                .RenderString(this.etrianString, 256, (chkSpacing.Checked ? 1 : 0), (chkZoom.Checked ? 2 : 1));

            pbPreview.Image = renderedString;
        }

        private void txtString_TextChanged(object sender, EventArgs e)
        {
            if (isBusy || this.etrianString == null) return;

            this.gameDataManager.SetMessageString(messageTable, messageNo, txtString.Text);
            RenderString();
        }

        private void chkSpacing_CheckedChanged(object sender, EventArgs e)
        {
            RenderString();
        }

        private void chkSmallFont_CheckedChanged(object sender, EventArgs e)
        {
            RenderString();
        }

        private void chkZoom_CheckedChanged(object sender, EventArgs e)
        {
            RenderString();
        }
    }
}
