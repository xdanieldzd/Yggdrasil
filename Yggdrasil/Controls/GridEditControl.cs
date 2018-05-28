using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace Yggdrasil.Controls
{
	class GridEditControl : Panel
	{
		public event EventHandler<TileClickEventArgs> TileClick;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
				return cp;
			}
		}

		[DefaultValue(typeof(Color), "Black")]
		public override Color BackColor
		{
			get { return base.BackColor; }
			set { base.BackColor = value; }
		}

		int columns;
		[DefaultValue(8)]
		public int Columns { get { return columns; } set { columns = value; UpdateDisplay(); } }

		int rows;
		[DefaultValue(8)]
		public int Rows { get { return rows; } set { rows = value; UpdateDisplay(); } }

		Size tileSize;
		[DefaultValue(typeof(Size), "8, 8")]
		public Size TileSize { get { return tileSize; } set { tileSize = value; UpdateDisplay(); } }

		int tileGap;
		[DefaultValue(2)]
		public int TileGap { get { return tileGap; } set { tileGap = value; UpdateDisplay(); } }

		int zoom;
		[DefaultValue(1), NotifyParentProperty(true), RefreshProperties(RefreshProperties.Repaint)]
		public int Zoom { get { return zoom; } set { zoom = value; UpdateDisplay(); } }

		bool showGrid;
		[DefaultValue(false)]
		public bool ShowGrid { get { return showGrid; } set { showGrid = value; UpdateDisplay(); } }

		Color gridColor;
		[DefaultValue(typeof(Color), "128, 128, 128, 128")]
		public Color GridColor { get { return gridColor; } set { gridColor = value; UpdateDisplay(); } }

		protected override Size DefaultSize
		{
			get { return new Size(zoom * tileSize.Width, zoom * tileSize.Height); }
		}

		public override Size MinimumSize
		{
			get { return DefaultSize; }
		}

		[DefaultValue(true), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool AutoSize
		{
			get { return true; }
		}

		[DefaultValue(typeof(System.Windows.Forms.AutoSizeMode), "GrowAndShrink"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new AutoSizeMode AutoSizeMode
		{
			get { return System.Windows.Forms.AutoSizeMode.GrowAndShrink; }
		}

		Size zoomedTileSize;
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public Size ZoomedTileSize { get { return zoomedTileSize; } }

		int zoomedTileGap;
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public int ZoomedTileGap { get { return zoomedTileGap; } }

		bool debug;
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public bool Debug { get { return debug; } set { debug = value; UpdateDisplay(); } }

		Point hoverTileCoords, selectedTileCoords;
		Rectangle hoverTileRect, selectedTileRect;

		public GridEditControl()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);

			BackColor = Color.Black;

			columns = 8;
			rows = 8;
			tileSize = new Size(8, 8);
			tileGap = 2;
			zoom = 1;
			showGrid = false;
			gridColor = Color.FromArgb(128, Color.Gray);

			zoomedTileSize = Size.Empty;
			zoomedTileGap = -1;
			hoverTileCoords = selectedTileCoords = Point.Empty;
			hoverTileRect = selectedTileRect = Rectangle.Empty;

			debug = false;
		}

		private void UpdateDisplay()
		{
			zoomedTileSize = new Size(tileSize.Width * zoom, tileSize.Height * zoom);
			zoomedTileGap = tileGap * zoom;
			Invalidate();
			OnResize(EventArgs.Empty);
		}

		protected override void OnResize(EventArgs eventargs)
		{
			Size newSize = new Size(Columns * (zoomedTileSize.Width + zoomedTileGap), Rows * (zoomedTileSize.Height + zoomedTileGap));
			if (Size != newSize)
			{
				Size = newSize;
				base.OnResize(eventargs);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			hoverTileCoords = new Point((e.X / (zoomedTileSize.Width + zoomedTileGap)), (e.Y / (zoomedTileSize.Height + zoomedTileGap)));
			hoverTileRect = new Rectangle(
				hoverTileCoords.X * (zoomedTileSize.Width + zoomedTileGap), hoverTileCoords.Y * (zoomedTileSize.Height + zoomedTileGap),
				zoomedTileSize.Width - 1, zoomedTileSize.Height - 1);

			Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			selectedTileCoords = new Point((e.X / (zoomedTileSize.Width + zoomedTileGap)), (e.Y / (zoomedTileSize.Height + zoomedTileGap)));
			selectedTileRect = new Rectangle(
				selectedTileCoords.X * (zoomedTileSize.Width + zoomedTileGap), selectedTileCoords.Y * (zoomedTileSize.Height + zoomedTileGap),
				zoomedTileSize.Width - 1, zoomedTileSize.Height - 1);

			Select();

			TileClick?.Invoke(this, new TileClickEventArgs(e.Button, selectedTileCoords));
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

			if (showGrid)
			{
				using (SolidBrush brush = new SolidBrush(gridColor))
				{
					for (int x = 0; x < columns; x++)
					{
						for (int y = 0; y < rows; y++)
						{
							/* Right */
							e.Graphics.FillRectangle(brush, new Rectangle(
								x * (zoomedTileSize.Width + zoomedTileGap) + zoomedTileSize.Width, y * (zoomedTileSize.Height + zoomedTileGap),
								zoomedTileGap, zoomedTileSize.Height));

							/* Bottom */
							e.Graphics.FillRectangle(brush, new Rectangle(
								x * (zoomedTileSize.Width + zoomedTileGap), y * (zoomedTileSize.Height + zoomedTileGap) + zoomedTileSize.Height,
								zoomedTileSize.Width, zoomedTileGap));

							/* Corner */
							e.Graphics.FillRectangle(brush, new Rectangle(
								x * (zoomedTileSize.Width + zoomedTileGap) + zoomedTileSize.Width, y * (zoomedTileSize.Height + zoomedTileGap) + zoomedTileSize.Height,
								zoomedTileGap, zoomedTileGap));
						}
					}
				}
			}

			using (Pen pen = new Pen(Color.FromArgb(160, Color.Orange), 3.0f))
			{
				e.Graphics.DrawRectangle(pen, hoverTileRect);
				if (debug) e.Graphics.DrawLine(pen, Point.Empty, hoverTileRect.Location);
			}

			using (Pen pen = new Pen(Color.FromArgb(160, Color.Red), 3.0f))
			{
				e.Graphics.DrawRectangle(pen, selectedTileRect);
				if (debug) e.Graphics.DrawLine(pen, new Point(ClientRectangle.Right, ClientRectangle.Bottom), new Point(selectedTileRect.Right, selectedTileRect.Bottom));
			}

			if (debug) e.Graphics.DrawString(string.Format("Hover:{0}, Selected:{1}", hoverTileCoords, selectedTileCoords), SystemFonts.MessageBoxFont, Brushes.Yellow, Point.Empty);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(BackColor);
		}
	}

	class TileClickEventArgs : EventArgs
	{
		public MouseButtons Button { get; private set; }
		public Point Coordinates { get; private set; }

		public TileClickEventArgs(MouseButtons button, Point coords)
		{
			Button = button;
			Coordinates = coords;
		}
	}
}
