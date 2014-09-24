using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Yggdrasil.FileHandling.MapDataHandling
{
    class WaterLilyTile : BaseTile
    {
        public WaterLilyTile(GameDataManager gameDataManager, MapDataFile mapDataFile, int offset, System.Drawing.Point coordinates) : base(gameDataManager, mapDataFile, offset, coordinates) { }

        [TypeConverter(typeof(TypeConverters.FloorNumberConverter))]
        public byte DestinationFloor { get; private set; }
        public byte DestinationX { get; private set; }
        public byte DestinationY { get; private set; }

        protected override void Load()
        {
            base.Load();

            DestinationFloor = this.Data[8];
            DestinationX = this.Data[9];
            DestinationY = this.Data[10];
        }
    }
}
