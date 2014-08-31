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

using Yggdrasil.FileHandling;
using Yggdrasil.Helpers;
using Yggdrasil.TableParsing;
using Yggdrasil.Attributes;

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
        PropertyGridMessageFilter messageFilter;

        Dictionary<Type, Action<TreeNode, List<BaseParser>>> customChildCreators = new Dictionary<Type, Action<TreeNode, List<BaseParser>>>()
        {
            { typeof(EquipItemParser), EquipItemParser.GenerateEquipmentNodes },
            { typeof(GatherItemParser), GatherItemParser.GenerateGatheringNodes }
        };

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
            Program.Logger.LogMessage("Initializing {0}...", this.GetType().Name);

            messageFilter = new PropertyGridMessageFilter(pgData.GetChildAtPoint(new Point(10, 10)), new MouseEventHandler((s, e) =>
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
            }));

            Application.AddMessageFilter(messageFilter);

            this.gameDataManager = gameDataManager;

            Rebuild();
        }

        public void Rebuild()
        {
            BaseParser lastObject = (BaseParser)pgData.SelectedObject;
            GridItem lastProperty = pgData.SelectedGridItem;
            pgData.SelectedObject = null;

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            Program.Logger.LogMessage("Rebuilding {0} nodes...", this.GetType().Name);

            treeViewWorker = new BackgroundWorker();
            treeViewWorker.DoWork += ((s, e) =>
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

                tvParsers.Invoke(new Action(() => { tvParsers.Nodes.Clear(); }));

                List<TreeNode> categories = new List<TreeNode>();

                List<Type> typesWithAttrib = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetCustomAttributes(typeof(ParserDescriptor), false).Length > 0).ToList();
                foreach (Type type in typesWithAttrib.OrderBy(x => ((ParserDescriptor)x.GetAttribute<ParserDescriptor>()).Priority))
                {
                    TreeNodeCategory categoryAttrib = type.GetAttribute<TreeNodeCategory>();

                    List<TreeNode> dataNodes = gameDataManager.GenerateTreeNodes(type, customChildCreators.ContainsKey(type) ? customChildCreators[type] : null);

                    tvParsers.Invoke(new Action(() =>
                    {
                        if (tvParsers.Nodes[categoryAttrib.CategoryName] == null)
                            tvParsers.Nodes.Add(new TreeNode(categoryAttrib.CategoryName) { Name = categoryAttrib.CategoryName });

                        tvParsers.Nodes[categoryAttrib.CategoryName].Nodes.AddRange(dataNodes.ToArray());
                        tvParsers.Nodes[categoryAttrib.CategoryName].Expand();
                    }));
                }

                tvParsers.Invoke(new Action(() =>
                {
                    tvParsers.SelectedNode = tvParsers.FindNodeByTag(lastObject);
                    if (lastProperty != null) pgData.SelectedGridItem = lastProperty;
                    tvParsers.Invalidate();
                }));
            });

            treeViewWorker.RunWorkerCompleted += ((s, e) =>
            {
                stopwatch.Stop();
                Program.Logger.LogMessage("Nodes rebuilt in {0:0.000} sec...", stopwatch.Elapsed.TotalSeconds);
            });

            treeViewWorker.RunWorkerAsync();
        }

        public void UpdateNodeText()
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            Program.Logger.LogMessage("Updating {0} node text...", this.GetType().Name);

            treeViewWorker = new BackgroundWorker();
            treeViewWorker.DoWork += ((s, e) =>
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

                List<TreeNode> nodes = null;
                tvParsers.Invoke(new Action(() => { nodes = tvParsers.FlattenTree().Where(x => x.Tag is BaseParser).ToList(); }));

                foreach (TreeNode node in nodes.Where(x => (x.Tag is BaseParser) && (x.Tag as BaseParser).EntryDescription != x.Text))
                    tvParsers.Invoke(new Action(() => { node.Text = (node.Tag as BaseParser).EntryDescription; }));
            });

            treeViewWorker.RunWorkerCompleted += ((s, e) =>
            {
                stopwatch.Stop();
                Program.Logger.LogMessage("Node text updated in {0:0.000} sec...", stopwatch.Elapsed.TotalSeconds);
            });

            treeViewWorker.RunWorkerAsync();
        }

        public void UpdateNodeText(object tag)
        {
            TreeNode node = tvParsers.FindNodeByTag(tag);
            if (node == null) throw new ArgumentException(string.Format("No node with specified tag (type {0}, hash 0x{1:X8}) found", tag.GetType().FullName, tag.GetHashCode()));

            if (tag is BaseParser)
            {
                node.Text = (tag as BaseParser).EntryDescription;
            }
        }

        public void Terminate()
        {
            Application.RemoveMessageFilter(messageFilter);

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
                pgData.SelectedGridItem.PropertyDescriptor.ResetValue(pgData.SelectedObject);
                CheckRebuildNeeded(pgData.SelectedObject as BaseParser, pgData.SelectedGridItem);
                pgData.Refresh();
            }
        }

        private void pgData_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            CheckRebuildNeeded((s as PropertyGrid).SelectedObject as BaseParser, e.ChangedItem);
        }

        private void CheckRebuildNeeded(BaseParser selectedObject, GridItem selectedProperty)
        {
            if (
                selectedObject is EquipItemParser && selectedProperty.Value is TableParsing.EquipItemParser.Groups ||
                selectedObject is GatherItemParser && selectedProperty.PropertyDescriptor.Converter.GetType() == typeof(TypeConverters.FloorNumberConverter)
                ) Rebuild();
        }
    }
}
