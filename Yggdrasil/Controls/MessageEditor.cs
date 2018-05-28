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
using Yggdrasil.Helpers;

namespace Yggdrasil.Controls
{
	public partial class MessageEditor : UserControl, IEditorControl
	{
		public bool IsInitialized() { return (gameDataManager != null); }
		public bool IsBusy() { return (treeViewWorker == null ? false : treeViewWorker.IsBusy); }

		GameDataManager gameDataManager;
		BackgroundWorker treeViewWorker;

		public MessageEditor()
		{
			InitializeComponent();

			tvMessageFiles.DoubleBuffered(true);
		}

		public void Initialize(GameDataManager gameDataManager)
		{
			Program.Logger.LogMessage("Initializing {0}...", GetType().Name);

			this.gameDataManager = gameDataManager;

			Rebuild();
		}

		public void Rebuild()
		{
			if (gameDataManager.MessageFiles != null)
			{
				System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
				stopwatch.Start();

				Program.Logger.LogMessage("Rebuilding {0} nodes...", GetType().Name);

				treeViewWorker = new BackgroundWorker();
				treeViewWorker.DoWork += ((s, e) =>
				{
					System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

					tvMessageFiles.Invoke(new Action(() => { tvMessageFiles.Nodes.Clear(); }));
					foreach (TableFile messageFile in gameDataManager.MessageFiles.OrderBy(x => Path.GetFileName(x.Filename)))
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
					TreeNode messageNode = new TreeNode(messageTable.Messages[j].ConvertedString.Truncate(truncatePosition)) { Tag = new Tuple<MessageTable, int>(messageTable, j) };
					tableNode.Nodes.Add(messageNode);
				}
			}

			return nodes;
		}

		public void Terminate()
		{
			gameDataManager = null;
			treeViewWorker = null;

			tvMessageFiles.Nodes.Clear();
			stringPreviewControl.Terminate();
		}

		private void tvMessageFiles_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Tag is Tuple<MessageTable, int>)
			{
				Tuple<MessageTable, int> data = (e.Node.Tag as Tuple<MessageTable, int>);
				stringPreviewControl.Initialize(gameDataManager, data.Item1, data.Item2);
			}
			else
				stringPreviewControl.Terminate();
		}

		private void tvMessageFiles_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				TreeView treeView = (sender as TreeView);
				treeView.SelectedNode = treeView.GetNodeAt(e.X, e.Y);

				if (treeView.SelectedNode != null)
				{
					createHTMLDumpToolStripMenuItem.Enabled = (treeView.SelectedNode.Tag is TableFile);
					cmsTreeView.Show(treeView, e.Location);
				}
			}
		}

		private void createHTMLDumpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TableFile file = (tvMessageFiles.SelectedNode.Tag as TableFile);

			SaveFileDialog sfd = new SaveFileDialog
			{
				InitialDirectory = Configuration.LastDataPath,
				Title = "Save HTML dump",
				Filter = "HTML Files (*.htm;*.html)|*.htm;*.html"
			};
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				List<TableFile> files = new List<TableFile>();
				files.Add(file);

				if (gameDataManager.Version == GameDataManager.Versions.European &&
					MessageBox.Show("Fetch every language version of this file to dump?", "Language Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					string strippedName = Path.GetFileNameWithoutExtension(file.Filename);
					foreach (KeyValuePair<GameDataManager.Languages, string> pair in gameDataManager.LanguageSuffixes) strippedName = strippedName.Replace(pair.Value, "");

					files.AddRange(gameDataManager.MessageFiles
						.Where(x => Path.GetFileNameWithoutExtension(x.Filename).StartsWith(strippedName) && x.Filename != file.Filename && x.FileNumber == file.FileNumber));
				}

				DataDumpers.DumpMessages(gameDataManager, files, sfd.FileName);
			}
		}
	}
}
