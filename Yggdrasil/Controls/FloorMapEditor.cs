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
            gridEditControl1.Columns = 35;
            gridEditControl1.Rows = 30;
        }

        public void Terminate()
        {
            this.gameDataManager = null;
            //
        }
    }
}
