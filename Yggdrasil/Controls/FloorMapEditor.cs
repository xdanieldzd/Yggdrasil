using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Yggdrasil.Controls
{
    public partial class FloorMapEditor : UserControl, IEditorControl
    {
        public bool IsInitialized() { return (gameDataManager != null); }

        GameDataManager gameDataManager;

        public FloorMapEditor()
        {
            InitializeComponent();
        }

        public void Initialize(GameDataManager gameDataManager)
        {
            Program.Logger.LogMessage("Initializing {0}...", this.GetType().Name);

            this.gameDataManager = gameDataManager;

            Rebuild();
        }

        public void Rebuild()
        {
            gridEditControl.Columns = 35;
            gridEditControl.Rows = 30;
        }

        public void Terminate()
        {
            this.gameDataManager = null;
            //
        }

        private void gridEditControl_TileClick(object sender, TileClickEventArgs e)
        {
            if ((e.Button & System.Windows.Forms.MouseButtons.Left) != 0)
            {
                MessageBox.Show(string.Format("Clicked {0}", e.Coordinates));
            }
        }

        private void gridEditControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawIcon(SystemIcons.Question, 16, 16);
        }
    }
}
