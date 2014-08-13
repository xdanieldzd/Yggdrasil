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

            if (renderedString != null) renderedString.Dispose();

            renderedString = game.FontRenderer.RenderString(this.etrianString, 256, 1);
            txtString.Text = this.etrianString.ConvertedString.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
        }

        private void pbPreview_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            if (renderedString != null) e.Graphics.DrawImageUnscaled(renderedString, Point.Empty);
        }
    }
}
