using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Yggdrasil.FileHandling;
using Yggdrasil.FileHandling.TableHandling;
using Yggdrasil.TextHandling;

namespace Yggdrasil.Controls
{
    public partial class MessageEditor : UserControl, IEditorControl
    {
        public bool IsInitialized() { return (gameDataManager != null); }

        GameDataManager gameDataManager;
        BackgroundWorker treeViewWorker;

        public MessageEditor()
        {
            InitializeComponent();

            tvMessageFiles.DoubleBuffered(true);
        }

        public void Initialize(GameDataManager gameDataManager)
        {
            Program.Logger.LogMessage("Initializing {0}...", this.GetType().Name);

            this.gameDataManager = gameDataManager;

            Rebuild();
        }

        public void Rebuild()
        {
            if (this.gameDataManager.MessageFiles != null)
            {
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                Program.Logger.LogMessage("Rebuilding {0} nodes...", this.GetType().Name);

                treeViewWorker = new BackgroundWorker();
                treeViewWorker.DoWork += ((s, e) =>
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

                    tvMessageFiles.Invoke(new Action(() => { tvMessageFiles.Nodes.Clear(); }));
                    foreach (TableFile messageFile in this.gameDataManager.MessageFiles.OrderBy(x => Path.GetFileName(x.Filename)))
                    {
                        if (messageFile.InArchive)
                        {
                            Program.Logger.LogMessage(true, "Generating tree nodes for {0}, file #{1}...", Path.GetFileName(messageFile.Filename), messageFile.FileNumber);

                            TreeNode prevNode = null;
                            tvMessageFiles.Invoke(new Action(() => { prevNode = tvMessageFiles.FindNodeByTag(messageFile.ArchiveFile); }));

                            if (prevNode == null)
                            {
                                prevNode = new TreeNode(Path.GetFileName(messageFile.ArchiveFile.Filename)) { Tag = messageFile.ArchiveFile };
                                tvMessageFiles.Invoke(new Action(() => { tvMessageFiles.Nodes.Add(prevNode); }));
                            }

                            TreeNode fileNode = new TreeNode(string.Format("File #{0}", messageFile.FileNumber)) { Tag = messageFile };
                            fileNode.Nodes.AddRange(GenerateNodes(messageFile).ToArray());

                            tvMessageFiles.Invoke(new Action(() => { prevNode.Nodes.Add(fileNode); }));
                        }
                        else
                        {
                            Program.Logger.LogMessage(true, "Generating tree nodes for {0}...", Path.GetFileName(messageFile.Filename));
                            TreeNode fileNode = new TreeNode(Path.GetFileName(messageFile.Filename)) { Tag = messageFile };

                            fileNode.Nodes.AddRange(GenerateNodes(messageFile).ToArray());
                            tvMessageFiles.Invoke(new Action(() => { tvMessageFiles.Nodes.Add(fileNode); }));
                        }
                    }
                });

                treeViewWorker.RunWorkerCompleted += ((s, e) =>
                {
                    tvMessageFiles.Invalidate();

                    Program.MainForm.StatusText = "Ready";
                    stopwatch.Stop();
                    Program.Logger.LogMessage("Nodes rebuilt in {0:0.000} sec...", stopwatch.Elapsed.TotalSeconds);
                });

                treeViewWorker.RunWorkerAsync();
            }
        }

        private List<TreeNode> GenerateNodes(TableFile messageFile)
        {
            List<TreeNode> nodes = new List<TreeNode>();

            for (int i = 0; i < messageFile.Tables.Length; i++)
            {
                MessageTable messageTable = (messageFile.Tables[i] as MessageTable);

                if (messageTable == null) continue;

                TreeNode tableNode = new TreeNode(string.Format("Table #{0}", i)) { Tag = messageTable };
                nodes.Add(tableNode);

                for (int j = 0; j < messageTable.NumMessages; j++)
                {
                    if (messageTable.MessageOffsets[j] == 0) continue;

                    int truncatePosition = messageTable.Messages[j].ConvertedString.IndexOf(Environment.NewLine);
                    if (truncatePosition == -1) truncatePosition = (gameDataManager.Version == GameDataManager.Versions.Japanese ? 12 : 24);
                    TreeNode messageNode = new TreeNode(messageTable.Messages[j].ConvertedString.Truncate(truncatePosition)) { Tag = messageTable.Messages[j] };
                    tableNode.Nodes.Add(messageNode);
                }
            }

            return nodes;
        }

        public void Terminate()
        {
            this.gameDataManager = null;
            treeViewWorker = null;

            tvMessageFiles.Nodes.Clear();
            stringPreviewControl.Terminate();
        }

        private void tvMessageFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is EtrianString)
                stringPreviewControl.Initialize(gameDataManager, e.Node.Tag as EtrianString);
            else
                stringPreviewControl.Terminate();
        }
    }
}
