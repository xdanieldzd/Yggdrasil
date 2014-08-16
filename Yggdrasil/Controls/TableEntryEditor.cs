using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Yggdrasil.FileTypes;
using Yggdrasil.Helpers;
using Yggdrasil.TableParsers;

namespace Yggdrasil.Controls
{
    public partial class TableEntryEditor : UserControl, IEditorControl
    {
        public bool IsInitialized() { return (game != null); }

        public int SplitterPosition
        {
            get { return splitContainer1.SplitterDistance; }
            set { splitContainer1.SplitterDistance = value; }
        }

        GameDataManager game;
        BackgroundWorker treeViewWorker;

        public TableEntryEditor()
        {
            InitializeComponent();

            tvParsers.DoubleBuffered(true);
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (IsInitialized()) Configuration.TableEntryEditorSplitter = e.SplitX;
        }

        public void Initialize(GameDataManager game)
        {
            this.game = game;
            this.Font = GUIHelpers.GetSuggestedGUIFont(game.Version);

            treeViewWorker = new BackgroundWorker();
            treeViewWorker.DoWork += ((s, e) =>
            {
                tvParsers.Invoke(new Action(() => { tvParsers.Nodes.Clear(); }));
                foreach (Tuple<Type, IList<BaseParser>> parsedTuple in this.game.GetAllParsedData(true))
                {
                    System.Reflection.MethodInfo mi = parsedTuple.Item1.GetMethod("GenerateTreeNode", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                    if (mi != null)
                    {
                        TreeNode dataNode = (TreeNode)mi.Invoke(null, new object[] { game, parsedTuple.Item2 });
                        tvParsers.Invoke(new Action(() => { tvParsers.Nodes.Add(dataNode); }));
                    }
                }
                tvParsers.Invoke(new Action(() => { tvParsers.Invalidate(); }));
            });

            treeViewWorker.RunWorkerAsync();
        }

        public void Terminate()
        {
            this.game = null;
            treeViewWorker = null;

            tvParsers.Nodes.Clear();
            pgData.SelectedObject = null;
        }

        private void tvParsers_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is BaseParser)
                pgData.SelectedObject = e.Node.Tag;
            else
                pgData.SelectedObject = null;
        }
    }
}
