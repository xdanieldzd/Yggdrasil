using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.FileHandling.MapDataHandling
{
    [Flags]
    public enum UsableFromDirection : byte
    {
        North = 0x1,
        South = 0x2,
        West = 0x4,
        East = 0x8
    }

    public enum FacingAtDestination : byte
    {
        Invalid = 0x0,
        North = 0x1,
        South = 0x2,
        East = 0x3,
        West = 0x4
    }

    class StairTile : BaseTile
    {
        public StairTile(GameDataManager gameDataManager, MapDataFile mapDataFile, int offset, System.Drawing.Point coordinates) : base(gameDataManager, mapDataFile, offset, coordinates) { }

        public byte DestinationFloor { get; private set; }
        public byte DestinationX { get; private set; }
        public byte DestinationY { get; private set; }
        public UsableFromDirection UsableFromDirection { get; private set; }
        public FacingAtDestination FacingAtDestination { get; private set; }

        public bool StairsGoOutside { get { return (DestinationFloor == 0 && DestinationX == 0 && DestinationY == 0 && FacingAtDestination == 0); } }

        protected override void Load()
        {
            base.Load();

            DestinationFloor = this.Data[8];
            DestinationX = this.Data[9];
            DestinationY = this.Data[10];
            UsableFromDirection = (UsableFromDirection)(this.Data[11] >> 4);
            FacingAtDestination = (FacingAtDestination)(this.Data[11] & 0xF);
        }
    }
}
