using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Yggdrasil.Helpers;

namespace Yggdrasil.Controls
{
    public partial class StringPreviewControl : UserControl
    {
        GameDataManager game;
        EtrianString etrianString;

        Bitmap renderedString;

        public StringPreviewControl()
        {
            InitializeComponent();

            Application.Idle += ((s, e) => { pbPreview.Invalidate(); });
        }

        public void Initialize(GameDataManager game, EtrianString etrianString)
        {
            this.game = game;
            this.etrianString = etrianString;

            UpdateString();
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

            renderedString = game.FontRenderer.RenderString(this.etrianString, 256, 1);
            txtString.Text = this.etrianString.ConvertedString;
        }

        private void pbPreview_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            if (etrianString != null && renderedString != null) e.Graphics.DrawImageUnscaled(renderedString, Point.Empty);
        }

        private void txtString_TextChanged(object sender, EventArgs e)
        {
            this.etrianString = txtString.Text;
            UpdateString();
        }
    }
}
