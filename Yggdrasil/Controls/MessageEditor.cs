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

namespace Yggdrasil.Controls
{
    public partial class MessageEditor : UserControl
    {
        public bool IsInitialized { get { return (game != null); } }

        GameDataManager game;
        BackgroundWorker treeViewWorker;

        public MessageEditor()
        {
            InitializeComponent();

            tvMessageFiles.DoubleBuffered(true);
        }

        public void Initialize(GameDataManager game)
        {
            this.game = game;

            if (this.game.MessageFiles != null)
            {
                treeViewWorker = new BackgroundWorker();
                treeViewWorker.DoWork += ((s, e) =>
                {
                    foreach (TBB messageFile in this.game.MessageFiles)
                    {
                        TreeNode fileNode = new TreeNode(Path.GetFileNameWithoutExtension(messageFile.Filename)) { Tag = messageFile };

                        for (int i = 0; i < messageFile.Tables.Length; i++)
                        {
                            TBB.MTBL messageTable = (messageFile.Tables[i] as TBB.MTBL);

                            if (messageTable == null) continue;

                            TreeNode tableNode = new TreeNode(string.Format("Table #{0}", i)) { Tag = messageTable };
                            fileNode.Nodes.Add(tableNode);

                            for (int j = 0; j < messageTable.NumMessages; j++)
                            {
                                if (messageTable.MessageOffsets[j] == 0) continue;

                                TreeNode messageNode = new TreeNode(messageTable.Messages[j].ConvertedString.Truncate(16)) { Tag = messageTable.Messages[j] };
                                tableNode.Nodes.Add(messageNode);
                            }
                        }

                        tvMessageFiles.Invoke(new Action(() => { tvMessageFiles.Nodes.Add(fileNode); }));
                    }
                });

                treeViewWorker.RunWorkerAsync();
            }
        }

        private void tvMessageFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is EtrianString) stringPreviewControl.Initialize(game, e.Node.Tag as EtrianString);
        }
    }
}
