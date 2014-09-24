using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Yggdrasil.FileHandling.MapDataHandling
{
    public enum TreasureType : byte
    {
        Money = 0x2,
        Item = 0x3,
    };

    public enum TreasureItemCategory : byte
    {
        Equipment = 0x0,
        Consumables = 0x1,
    }

    class TreasureChestTile : BaseTile
    {
        public TreasureChestTile(GameDataManager gameDataManager, MapDataFile mapDataFile, int offset, System.Drawing.Point coordinates) : base(gameDataManager, mapDataFile, offset, coordinates) { }

        public TreasureType TreasureType { get; private set; }
        public byte TreasureChestID { get; private set; }
        public TreasureItemCategory TreasureItemCategory { get; private set; }

        [TypeConverter(typeof(TypeConverters.ItemNameConverter))]
        public ushort TreasureItemID { get; private set; }

        [TypeConverter(typeof(TypeConverters.UshortEtrianEnConverter))]
        public ushort TreasureMoney { get; private set; }

        protected override void Load()
        {
            base.Load();

            TreasureType = (TreasureType)(this.Data[15] >> 4);
            TreasureChestID = (byte)(this.Data[15] & 0xF);
            switch (TreasureType)
            {
                case TreasureType.Item:
                    TreasureItemCategory = (TreasureItemCategory)(this.Data[13] >> 4);
                    TreasureItemID = BitConverter.ToUInt16(this.Data, 12);
                    break;
                case TreasureType.Money:
                    TreasureMoney = BitConverter.ToUInt16(this.Data, 12);
                    break;
            }
        }
    }
}
