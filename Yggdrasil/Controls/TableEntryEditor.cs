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
        public bool IsBusy() { return (treeViewWorker == null ? false : treeViewWorker.IsBusy); }

        GameDataManager gameDataManager;
        BackgroundWorker treeViewWorker;
        PropertyGridMessageFilter messageFilter;

        Dictionary<Type, Action<TreeNode, List<BaseParser>>> customChildCreators = new Dictionary<Type, Action<TreeNode, List<BaseParser>>>()
        {
            { typeof(EquipItemParser), EquipItemParser.GenerateEquipmentNodes },
            { typeof(GatherItemParser), GatherItemParser.GenerateGatheringNodes },
            { typeof(EnemyDataParser), EnemyDataParser.GenerateEnemyNodes }
        };

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
                return cp;
            }
        }

        public TableEntryEditor()
        {
            InitializeComponent();

            tvParsers.DoubleBuffered(true);
        }

        public override void Refresh()
        {
            pgData.Refresh();
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

                List<Type> typesWithAttrib = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(x => x.GetCustomAttributes(typeof(PrioritizedCategory), false).Length > 0 && x.GetCustomAttributes(typeof(ParserDescriptor), false).Length > 0)
                    .OrderBy(x => ((PrioritizedCategory)x.GetAttribute<PrioritizedCategory>()).Priority)
                    .ThenBy(x => ((ParserDescriptor)x.GetAttribute<ParserDescriptor>()).Priority)
                    .ToList();

                foreach (Type type in typesWithAttrib)
                {
                    PrioritizedCategory categoryAttrib = type.GetAttribute<PrioritizedCategory>();

                    List<TreeNode> dataNodes = gameDataManager.GenerateTreeNodes(type, customChildCreators.ContainsKey(type) ? customChildCreators[type] : null);

                    tvParsers.Invoke(new Action(() =>
                    {
                        if (tvParsers.Nodes[categoryAttrib.Category] == null)
                            tvParsers.Nodes.Add(new TreeNode(categoryAttrib.Category) { Name = categoryAttrib.Category });

                        tvParsers.Nodes[categoryAttrib.Category].Nodes.AddRange(dataNodes.ToArray());
                        tvParsers.Nodes[categoryAttrib.Category].Expand();
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

        public void Terminate()
        {
            Application.RemoveMessageFilter(messageFilter);

            this.gameDataManager = null;
            treeViewWorker = null;

            tvParsers.Nodes.Clear();
            pgData.SelectedObject = null;
        }

        public void SelectNodeByTag(object tag)
        {
            TreeNode node = tvParsers.FindNodeByTag(tag);
            if (node != null) tvParsers.SelectedNode = node;
        }

        private void tvParsers_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is BaseParser) pgData.SelectedObject = e.Node.Tag;
            else pgData.SelectedObject = null;

            pgData.Refresh();
        }

        private void resetPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pgData.SelectedGridItem != null && pgData.SelectedGridItem.PropertyDescriptor != null)
            {
                pgData.SelectedGridItem.PropertyDescriptor.ResetValue(pgData.SelectedObject);

                CheckNodeUpdateNeeded(pgData.SelectedObject as BaseParser, pgData.SelectedGridItem);
                CheckTreeRebuildNeeded(pgData.SelectedObject as BaseParser, pgData.SelectedGridItem);

                pgData.Refresh();
            }
        }

        private void pgData_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            BaseParser parser = ((sender as PropertyGrid).SelectedObject as BaseParser);

            CheckNodeUpdateNeeded(parser, e.ChangedItem);
            CheckTreeRebuildNeeded(parser, e.ChangedItem);
        }

        private void CheckNodeUpdateNeeded(BaseParser selectedObject, GridItem selectedProperty)
        {
            CausesNodeUpdate updateCheck = (selectedProperty.PropertyDescriptor.Attributes[typeof(CausesNodeUpdate)] as CausesNodeUpdate);
            if (updateCheck != null && updateCheck.Value) UpdateNodeText();
        }

        private void CheckTreeRebuildNeeded(BaseParser selectedObject, GridItem selectedProperty)
        {
            CausesTreeRebuild rebuildCheck = (selectedProperty.PropertyDescriptor.Attributes[typeof(CausesTreeRebuild)] as CausesTreeRebuild);
            if (rebuildCheck != null && rebuildCheck.Value) Rebuild();
        }

        private void tvParsers_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeView treeView = (sender as TreeView);
                treeView.SelectedNode = treeView.GetNodeAt(e.X, e.Y);

                if (treeView.SelectedNode != null)
                {
                    createHTMLDumpToolStripMenuItem.Enabled = (treeView.SelectedNode.Tag is List<BaseParser>);
                    cmsTreeView.Show(treeView, e.Location);
                }
            }
        }

        private void createHTMLDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Configuration.LastDataPath;
            sfd.Title = "Save HTML dump";
            sfd.Filter = "HTML Files (*.htm;*.html)|*.htm;*.html";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                DataDumpers.DumpParsers(gameDataManager, (tvParsers.SelectedNode.Tag as List<BaseParser>).FirstOrDefault().GetType(), sfd.FileName);
            }
        }
    }
}
