using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.ComponentModel;

using Yggdrasil.Attributes;
using Yggdrasil.FileHandling.MapDataHandling;

namespace Yggdrasil.FileHandling
{
    [MagicNumber("YGMD")]
    public class MapDataFile : BaseFile
    {
        public MapDataFile(GameDataManager gameDataManager, string path) : base(gameDataManager, path) { }

        public const int MapWidth = 35;
        public const int MapHeight = 30;
        public const int MapDataOffset = 0x10;

        public enum TileTypes : byte
        {
            Nothing = 0x0,
            Floor = 0x1,
            Wall = 0x2,
            [Description("Stairs (Up)")]
            StairsUp = 0x3,
            [Description("Stairs (Down)")]
            StairsDown = 0x4,
            [Description("One-way Shortcut (N)")]
            OneWayShortcutN = 0x5,
            [Description("One-way Shortcut (S)")]
            OneWayShortcutS = 0x6,
            [Description("One-way Shortcut (W)")]
            OneWayShortcutW = 0x7,
            [Description("One-way Shortcut (E)")]
            OneWayShortcutE = 0x8,
            [Description("Door (N-S)")]
            DoorNS = 0x9,
            [Description("Door (W-E)")]
            DoorWE = 0xA,
            [Description("Treasure Chest")]
            TreasureChest = 0xB,
            [Description("Geomagnetic Field")]
            GeomagneticField = 0xC,
            [Description("Conveyor (N)")]
            SandConveyorN = 0xD,
            [Description("Conveyor (S)")]
            SandConveyorS = 0xE,
            [Description("Conveyor (W)")]
            SandConveyorW = 0xF,
            [Description("Conveyor (E)")]
            SandConveyorE = 0x10,
            [Description("F.O.E. Floor")]
            FOEFloor = 0x11, /* not sure, but seems FOE-related */
            [Description("Collapsing Floor")]
            CollapsingFloor = 0x12,
            Water = 0x13,
            Elevator = 0x14,
            [Description("Refreshing Water")]
            RefreshingWater = 0x15,
            [Description("Warp Entrance")]
            WarpEntrance = 0x16,
            Transporter = 0x17,
            [Description("Damaging Floor")]
            DamagingFloor = 0x18,
            [Description("Unknown (0x19)")]
            Unknown0x19 = 0x19,
        };

        public static Dictionary<TileTypes, Type> TileTypeClassMapping = new Dictionary<TileTypes, Type>()
        {
            { TileTypes.Floor, typeof(FloorTile) },
            { TileTypes.FOEFloor, typeof(FloorTile) },
            { TileTypes.DamagingFloor, typeof(FloorTile) },
            { TileTypes.CollapsingFloor, typeof(FloorTile) },
            { TileTypes.StairsUp, typeof(StairsTile) },
            { TileTypes.StairsDown, typeof(StairsTile) },
            { TileTypes.TreasureChest, typeof(TreasureChestTile) },
            { TileTypes.Transporter, typeof(TransporterTile) },
        };

        public static Dictionary<TileTypes, bool> IsTileWalkable = new Dictionary<TileTypes, bool>()
        {
            /* Maybe not "walkable" as such - treasure chest, etc. -, but at least for the sake of wall drawing... */
            { TileTypes.Nothing, false },
            { TileTypes.Floor, true },
            { TileTypes.Wall, false },
            { TileTypes.StairsUp, true },
            { TileTypes.StairsDown, true },
            { TileTypes.OneWayShortcutN, false },
            { TileTypes.OneWayShortcutS, false },
            { TileTypes.OneWayShortcutW, false },
            { TileTypes.OneWayShortcutE, false },
            { TileTypes.DoorNS, true },
            { TileTypes.DoorWE, true },
            { TileTypes.TreasureChest, true },
            { TileTypes.GeomagneticField, true },
            { TileTypes.SandConveyorN, true },
            { TileTypes.SandConveyorS, true },
            { TileTypes.SandConveyorW, true },
            { TileTypes.SandConveyorE, true },
            { TileTypes.FOEFloor, true },  
            { TileTypes.CollapsingFloor, true },
            { TileTypes.Water, false },
            { TileTypes.Elevator, true },
            { TileTypes.RefreshingWater, true },
            { TileTypes.WarpEntrance, true },
            { TileTypes.Transporter, false },     
            { TileTypes.DamagingFloor, true },
            { TileTypes.Unknown0x19, false },
        };

        public static Dictionary<TileTypes, Point> TileImageCoords = new Dictionary<TileTypes, Point>()
        {
            { TileTypes.StairsUp, new Point(112, 90) },
            { TileTypes.StairsDown, new Point(128, 90) },
            { TileTypes.OneWayShortcutN, new Point(96, 90) },
            { TileTypes.OneWayShortcutS, new Point(96, 90) },
            { TileTypes.OneWayShortcutW, new Point(96, 90) },
            { TileTypes.OneWayShortcutE, new Point(96, 90) },
            { TileTypes.DoorNS, new Point(64, 90) },
            { TileTypes.DoorWE, new Point(64, 90) },
            { TileTypes.TreasureChest, new Point(16, 90) },
            { TileTypes.GeomagneticField, new Point(0, 0) },
            { TileTypes.CollapsingFloor, new Point(80, 90) },
            { TileTypes.RefreshingWater, new Point(0, 0) },
        };

        public int FloorNumber { get { return int.Parse(System.Text.RegularExpressions.Regex.Match(Path.GetFileNameWithoutExtension(this.Filename), @"\d+").Value); } }
        public string FloorName { get { return string.Format("B{0}F", FloorNumber); } }

        public string FileSignature { get; private set; }
        public uint Unknown1 { get; private set; }
        public uint Unknown2 { get; private set; }
        public uint Unknown3 { get; private set; }

        public byte[] UnknownBlock { get; private set; }

        public override void Parse()
        {
            FileSignature = Encoding.ASCII.GetString(Stream.ToArray(), 0, 4);

            string magic = this.GetAttribute<MagicNumber>().Magic;
            if (FileSignature != magic) throw new Exception(string.Format("Invalid file signature, got '{0}' expected '{1}'", FileSignature, magic));

            BinaryReader reader = new BinaryReader(Stream);

            reader.BaseStream.Seek(4, SeekOrigin.Begin);
            Unknown1 = reader.ReadUInt32();
            Unknown2 = reader.ReadUInt32();
            Unknown3 = reader.ReadUInt32();

            int unknownBlockOffset = MapDataOffset + (MapWidth * MapHeight * 0x10);
            if (unknownBlockOffset < reader.BaseStream.Length)
            {
                reader.BaseStream.Seek(unknownBlockOffset, SeekOrigin.Begin);
                UnknownBlock = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
            }
        }

        public override void Save()
        {
            List<byte> rebuilt = new List<byte>();

            rebuilt.AddRange(Encoding.ASCII.GetBytes(FileSignature));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown1));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown2));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown3));

            for (int y = 0; y < MapDataFile.MapHeight; y++)
                for (int x = 0; x < MapDataFile.MapWidth; x++)
                    rebuilt.AddRange(GameDataManager.MapTileData[this][x, y].Data);

            rebuilt.AddRange(UnknownBlock);

            Stream = new MemoryStream(rebuilt.ToArray());
            if (!base.InArchive && base.FileNumber == -1)
            {
                using (FileStream fileStream = new FileStream(Filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    Stream.CopyTo(fileStream);
                }
            }
        }
    }
}
