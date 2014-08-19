using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

using Yggdrasil.FileTypes;
using Yggdrasil.Helpers;
using Yggdrasil.TableParsers;

namespace Yggdrasil.Controls
{
    public partial class TableEntryEditor : UserControl, IEditorControl
    {
        public bool IsInitialized() { return (gameDataManager != null); }

        public int SplitterPosition
        {
            get { return splitContainer1.SplitterDistance; }
            set { splitContainer1.SplitterDistance = value; }
        }

        GameDataManager gameDataManager;
        BackgroundWorker treeViewWorker;

        public TableEntryEditor()
        {
            InitializeComponent();

            tvParsers.DoubleBuffered(true);
        }

        public override void Refresh()
        {
            pgData.Refresh();
            base.Refresh();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (IsInitialized()) Configuration.TableEntryEditorSplitter = e.SplitX;
        }

        public void Initialize(GameDataManager gameDataManager)
        {
            Application.AddMessageFilter(new PropertyGridMessageFilter(pgData.GetChildAtPoint(new Point(10, 10)), new MouseEventHandler((s, e) =>
            {
                if (e.Button == MouseButtons.Right && pgData.SelectedGridItem != null &&
                    pgData.SelectedGridItem.PropertyDescriptor != null && !pgData.SelectedGridItem.PropertyDescriptor.IsReadOnly)
                {
                    PropertyDescriptor descriptor = pgData.SelectedGridItem.PropertyDescriptor;
                    FieldInfo field = descriptor.ComponentType.UnderlyingSystemType.GetField("originalValues", BindingFlags.NonPublic | BindingFlags.Instance);
                    Dictionary<string, object> valueDict = (Dictionary<string, object>)field.GetValue(pgData.SelectedObject);

                    if (valueDict != null)
                    {
                        var defaultValue = valueDict[descriptor.Name];

                        resetPropertyToolStripMenuItem.Enabled = (descriptor.CanResetValue(pgData.SelectedObject) && (descriptor.GetValue(pgData.SelectedObject) != defaultValue));
                        resetPropertyToolStripMenuItem.Text = string.Format("{0} ({1})", (resetPropertyToolStripMenuItem.Tag as string), descriptor.Converter.ConvertTo(defaultValue, typeof(string)));
                        cmsDataGrid.Show(this, PointToClient(MousePosition));
                    }
                }
            })));

            this.gameDataManager = gameDataManager;
            this.Font = GUIHelpers.GetSuggestedGUIFont(gameDataManager.Version);

            treeViewWorker = new BackgroundWorker();
            treeViewWorker.DoWork += ((s, e) =>
            {
                tvParsers.Invoke(new Action(() => { tvParsers.Nodes.Clear(); }));
                foreach (Tuple<Type, IList<BaseParser>> parsedTuple in this.gameDataManager.GetAllParsedData(true))
                {
                    MethodInfo mi = parsedTuple.Item1.GetMethod("GenerateTreeNode", BindingFlags.Static | BindingFlags.Public);
                    if (mi != null)
                    {
                        TreeNode dataNode = (TreeNode)mi.Invoke(null, new object[] { gameDataManager, parsedTuple.Item2 });
                        tvParsers.Invoke(new Action(() => { tvParsers.Nodes.Add(dataNode); }));
                    }
                }
                tvParsers.Invoke(new Action(() => { tvParsers.Invalidate(); }));
            });

            treeViewWorker.RunWorkerAsync();
        }

        public void Terminate()
        {
            this.gameDataManager = null;
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

        private void resetPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pgData.SelectedGridItem != null && pgData.SelectedGridItem.PropertyDescriptor != null)
            {
                PropertyDescriptor descriptor = pgData.SelectedGridItem.PropertyDescriptor;
                descriptor.ResetValue(pgData.SelectedObject);
                pgData.Refresh();
            }
        }
    }
}
