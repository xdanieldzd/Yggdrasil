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
using Yggdrasil.FileHandling.MapDataHandling;

namespace Yggdrasil.Controls
{
    public partial class FloorMapEditor : UserControl, IEditorControl
    {
        public bool IsInitialized() { return (gameDataManager != null); }

        GameDataManager gameDataManager;
        MapDataFile mapDataFile;

        SolidBrush brushEmpty, brushFloor, brushWall;

        public FloorMapEditor()
        {
            InitializeComponent();

            gridEditControl.Columns = 35;
            gridEditControl.Rows = 30;
        }

        public override void Refresh()
        {
            pgMapTile.Refresh();
        }

        public void Initialize(GameDataManager gameDataManager)
        {
            Program.Logger.LogMessage("Initializing {0}...", this.GetType().Name);

            brushEmpty = new SolidBrush(Color.FromArgb(0x01, 0x28, 0x47));
            brushFloor = new SolidBrush(Color.FromArgb(0x2F, 0x83, 0xB2));
            brushWall = new SolidBrush(Color.FromArgb(0x94, 0xFF, 0xEF));

            this.gameDataManager = gameDataManager;

            Rebuild();
        }

        public void Rebuild()
        {
            pgMapTile.SelectedObject = null;
            cbMaps.DataSource = gameDataManager.MapDataFiles;
            cbMaps.DisplayMember = "FloorName";
        }

        public void Terminate()
        {
            this.gameDataManager = null;

            cbMaps.DataSource = null;
            pgMapTile.SelectedObject = null;

            brushEmpty.Dispose();
            brushFloor.Dispose();
            brushWall.Dispose();
        }

        private void cbMaps_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (sender as ComboBox);
            if (comboBox.SelectedItem != null)
            {
                mapDataFile = (comboBox.SelectedItem as MapDataFile);
                pgMapTile.SelectedObject = mapDataFile.Tiles[0, 0];
            }

            gridEditControl.Invalidate();
            pgMapTile.Refresh();
        }

        private void gridEditControl_TileClick(object sender, TileClickEventArgs e)
        {
            if ((e.Button & System.Windows.Forms.MouseButtons.Left) != 0)
            {
                pgMapTile.SelectedObject = mapDataFile.Tiles[e.Coordinates.X, e.Coordinates.Y];
            }
        }

        private void gridEditControl_Paint(object sender, PaintEventArgs e)
        {
            if (!IsInitialized() || mapDataFile == null) return;

            GridEditControl gridEditControl = (sender as GridEditControl);
            for (int y = 0; y < MapDataFile.MapHeight; y++)
            {
                for (int x = 0; x < MapDataFile.MapWidth; x++)
                {
                    Rectangle tileRect = new Rectangle(gridEditControl.ZoomedTileSize.Width * x, gridEditControl.ZoomedTileSize.Height * y,
                        gridEditControl.ZoomedTileSize.Width, gridEditControl.ZoomedTileSize.Height);

                    BaseTile tile = mapDataFile.Tiles[x, y];
                    switch (tile.TileType)
                    {
                        case MapDataFile.TileTypes.Nothing:
                            e.Graphics.FillRectangle(Brushes.Black, tileRect);
                            break;
                        case MapDataFile.TileTypes.Wall:
                            DrawWallRectangle(e.Graphics, brushWall, brushEmpty, x, y);
                            break;
                        case MapDataFile.TileTypes.Floor:
                        case MapDataFile.TileTypes.FOEFloor:
                            e.Graphics.FillRectangle(brushFloor, tileRect);
                            break;
                        case MapDataFile.TileTypes.StairsUp:
                            e.Graphics.FillRectangle(Brushes.LightGreen, tileRect);
                            break;
                        case MapDataFile.TileTypes.StairsDown:
                            e.Graphics.FillRectangle(Brushes.GreenYellow, tileRect);
                            break;
                        case MapDataFile.TileTypes.OneWayShortcutN:
                        case MapDataFile.TileTypes.OneWayShortcutS:
                        case MapDataFile.TileTypes.OneWayShortcutW:
                        case MapDataFile.TileTypes.OneWayShortcutE:
                            e.Graphics.FillRectangle(Brushes.Gray, tileRect);
                            break;
                        case MapDataFile.TileTypes.DoorNS:
                        case MapDataFile.TileTypes.DoorWE:
                            e.Graphics.FillRectangle(Brushes.Blue, tileRect);
                            break;
                        case MapDataFile.TileTypes.TreasureChest:
                            e.Graphics.FillRectangle(Brushes.Gold, tileRect);
                            break;
                        case MapDataFile.TileTypes.GeomagneticField:
                            e.Graphics.FillRectangle(Brushes.DeepPink, tileRect);
                            break;
                        case MapDataFile.TileTypes.SandConveyorN:
                        case MapDataFile.TileTypes.SandConveyorS:
                        case MapDataFile.TileTypes.SandConveyorW:
                        case MapDataFile.TileTypes.SandConveyorE:
                            e.Graphics.FillRectangle(Brushes.SandyBrown, tileRect);
                            break;
                        case MapDataFile.TileTypes.CollapsingFloor:
                            e.Graphics.FillRectangle(Brushes.PowderBlue, tileRect);
                            break;
                        case MapDataFile.TileTypes.Water:
                            DrawWallRectangle(e.Graphics, brushWall, Brushes.DarkCyan, x, y);
                            break;
                        case MapDataFile.TileTypes.RefreshingWater:
                            e.Graphics.FillRectangle(Brushes.Cyan, tileRect);
                            break;
                        case MapDataFile.TileTypes.WarpEntrance:
                            e.Graphics.FillRectangle(Brushes.Purple, tileRect);
                            break;
                        case MapDataFile.TileTypes.WaterLily:
                            e.Graphics.FillRectangle(Brushes.Pink, tileRect);
                            break;
                        case MapDataFile.TileTypes.DamagingFloor:
                            e.Graphics.FillRectangle(Brushes.Red, tileRect);
                            break;
                        default:
                            e.Graphics.FillRectangle(brushEmpty, tileRect);
                            break;
                    }
                }
            }
        }

        private void DrawWallRectangle(Graphics g, Brush wallBrush, Brush emptyBrush, int x, int y)
        {
            int drawXCoord = (gridEditControl.ZoomedTileSize.Width * x);
            int drawYCoord = (gridEditControl.ZoomedTileSize.Height * y);

            bool left, right, top, bottom;
            bool topLeft, topRight, bottomLeft, bottomRight;

            g.FillRectangle(emptyBrush, new Rectangle(drawXCoord, drawYCoord, gridEditControl.ZoomedTileSize.Width, gridEditControl.ZoomedTileSize.Height));

            left = (x - 1 >= 0 && (bool)MapDataFile.IsTileWalkable[mapDataFile.Tiles[x - 1, y].TileType]);
            right = (x + 1 < MapDataFile.MapWidth && (bool)MapDataFile.IsTileWalkable[mapDataFile.Tiles[x + 1, y].TileType]);
            top = (y - 1 >= 0 && (bool)MapDataFile.IsTileWalkable[mapDataFile.Tiles[x, y - 1].TileType]);
            bottom = (y + 1 < MapDataFile.MapHeight && (bool)MapDataFile.IsTileWalkable[mapDataFile.Tiles[x, y + 1].TileType]);

            topLeft = (x - 1 >= 0 && y - 1 >= 0 && (bool)MapDataFile.IsTileWalkable[mapDataFile.Tiles[x - 1, y - 1].TileType]);
            topRight = (x + 1 < MapDataFile.MapWidth && y - 1 >= 0 && (bool)MapDataFile.IsTileWalkable[mapDataFile.Tiles[x + 1, y - 1].TileType]);
            bottomLeft = (x - 1 >= 0 && y + 1 < MapDataFile.MapHeight && (bool)MapDataFile.IsTileWalkable[mapDataFile.Tiles[x - 1, y + 1].TileType]);
            bottomRight = (x + 1 < MapDataFile.MapWidth && y + 1 < MapDataFile.MapHeight && (bool)MapDataFile.IsTileWalkable[mapDataFile.Tiles[x + 1, y + 1].TileType]);

            if (left) g.FillRectangle(wallBrush, new Rectangle(drawXCoord, drawYCoord, 2, gridEditControl.ZoomedTileSize.Height));
            if (right) g.FillRectangle(wallBrush, new Rectangle(drawXCoord + (gridEditControl.ZoomedTileSize.Width - 2), drawYCoord, 2, gridEditControl.ZoomedTileSize.Height));
            if (top) g.FillRectangle(wallBrush, new Rectangle(drawXCoord, drawYCoord, gridEditControl.ZoomedTileSize.Width, 2));
            if (bottom) g.FillRectangle(wallBrush, new Rectangle(drawXCoord, drawYCoord + (gridEditControl.ZoomedTileSize.Width - 2), gridEditControl.ZoomedTileSize.Width, 2));

            if (topLeft) g.FillRectangle(wallBrush, new Rectangle(drawXCoord, drawYCoord, 2, 2));
            if (topRight) g.FillRectangle(wallBrush, new Rectangle(drawXCoord + (gridEditControl.ZoomedTileSize.Width - 2), drawYCoord, 2, 2));
            if (bottomLeft) g.FillRectangle(wallBrush, new Rectangle(drawXCoord, drawYCoord + (gridEditControl.ZoomedTileSize.Height - 2), 2, 2));
            if (bottomRight) g.FillRectangle(wallBrush, new Rectangle(drawXCoord + (gridEditControl.ZoomedTileSize.Width - 2), drawYCoord + (gridEditControl.ZoomedTileSize.Height - 2), 2, 2));
        }
    }
}
