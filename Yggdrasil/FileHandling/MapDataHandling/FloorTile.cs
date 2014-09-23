using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.FileHandling.MapDataHandling
{
    class FloorTile : BaseTile
    {
        public FloorTile(GameDataManager gameDataManager, MapDataFile mapDataFile, int offset, System.Drawing.Point coordinates) : base(gameDataManager, mapDataFile, offset, coordinates) { }

        public byte DangerIncrement { get; private set; }
        public ushort Unknown1 { get; private set; }
        public ushort Unknown2 { get; private set; }
        public ushort EncounterGroup { get; private set; }
        public uint Unknown3 { get; private set; }
        public uint Unknown4 { get; private set; }

        protected override void Load()
        {
            base.Load();

            DangerIncrement = this.Data[1];
            Unknown1 = BitConverter.ToUInt16(this.Data, 2);
            Unknown2 = BitConverter.ToUInt16(this.Data, 4);
            EncounterGroup = BitConverter.ToUInt16(this.Data, 6);
            Unknown3 = BitConverter.ToUInt32(this.Data, 8);
            Unknown4 = BitConverter.ToUInt32(this.Data, 12);
        }
    }
}
