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
using Yggdrasil.Helpers;

namespace Yggdrasil.Controls
{
    public partial class MessageEditor : UserControl, IEditorControl
    {
        public bool IsInitialized() { return (gameDataManager != null); }

        public int SplitterPosition
        {
            get { return splitContainer1.SplitterDistance; }
            set { splitContainer1.SplitterDistance = value; }
        }

        GameDataManager gameDataManager;
        BackgroundWorker treeViewWorker;

        public MessageEditor()
        {
            InitializeComponent();

            tvMessageFiles.DoubleBuffered(true);
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (IsInitialized()) Configuration.MessageEditorSplitter = e.SplitX;
        }

        public void Initialize(GameDataManager gameDataManager)
        {
            Program.Logger.LogMessage("Initializing {0}...", this.GetType().Name);

            this.gameDataManager = gameDataManager;
            this.Font = GUIHelpers.GetSuggestedGUIFont(gameDataManager.Version);

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
                    foreach (TableFile messageFile in this.gameDataManager.MessageFiles)
                    {
                        TreeNode fileNode = new TreeNode(Path.GetFileNameWithoutExtension(messageFile.Filename)) { Tag = messageFile };

                        Program.Logger.LogMessage("Generating tree nodes for {0}...", fileNode.Text);

                        for (int i = 0; i < messageFile.Tables.Length; i++)
                        {
                            MessageTable messageTable = (messageFile.Tables[i] as MessageTable);

                            if (messageTable == null) continue;

                            TreeNode tableNode = new TreeNode(string.Format("Table #{0}", i)) { Tag = messageTable };
                            fileNode.Nodes.Add(tableNode);

                            for (int j = 0; j < messageTable.NumMessages; j++)
                            {
                                if (messageTable.MessageOffsets[j] == 0) continue;

                                int truncatePosition = messageTable.Messages[j].ConvertedString.IndexOf(Environment.NewLine);
                                if (truncatePosition == -1) truncatePosition = (gameDataManager.Version == GameDataManager.Versions.Japanese ? 12 : 24);
                                TreeNode messageNode = new TreeNode(messageTable.Messages[j].ConvertedString.Truncate(truncatePosition)) { Tag = messageTable.Messages[j] };
                                tableNode.Nodes.Add(messageNode);
                            }
                        }
                        tvMessageFiles.Invoke(new Action(() => { tvMessageFiles.Nodes.Add(fileNode); }));
                    }
                    tvMessageFiles.Invoke(new Action(() => { tvMessageFiles.Invalidate(); }));
                });

                treeViewWorker.RunWorkerCompleted += ((s, e) =>
                {
                    stopwatch.Stop();
                    Program.Logger.LogMessage("Nodes rebuilt in {0:0.000} sec...", stopwatch.Elapsed.TotalSeconds);
                });

                treeViewWorker.RunWorkerAsync();
            }
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
