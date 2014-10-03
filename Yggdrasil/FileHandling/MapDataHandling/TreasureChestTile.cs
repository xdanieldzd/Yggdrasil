using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Yggdrasil.Attributes;

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
        public TreasureChestTile(GameDataManager gameDataManager, MapDataFile mapDataFile, int offset, System.Drawing.Point coordinates, PropertyChangedEventHandler propertyChanged = null) :
            base(gameDataManager, mapDataFile, offset, coordinates, propertyChanged) { }

        public TreasureChestTile(BaseTile originalTile, MapDataFile.TileTypes newTileType) :
            base(originalTile, newTileType) { }

        TreasureType treasureType;
        [DisplayName("Type"), PrioritizedCategory("General", 3)]
        [Description("")]
        public TreasureType TreasureType
        {
            get { return treasureType; }
            set { base.SetProperty(ref treasureType, value, () => this.TreasureType); }
        }
        public bool ShouldSerializeTreasureType() { return !(this.TreasureType == (dynamic)base.originalValues["TreasureType"]); }
        public void ResetTreasureType() { this.TreasureType = (dynamic)base.originalValues["TreasureType"]; }

        byte treasureChestID;
        [DisplayName("Chest ID"), TypeConverter(typeof(TypeConverters.HexByteConverter)), PrioritizedCategory("General", 3)]
        [Description("")]
        public byte TreasureChestID
        {
            get { return treasureChestID; }
            set { base.SetProperty(ref treasureChestID, value, () => this.TreasureChestID); }
        }
        public bool ShouldSerializeTreasureChestID() { return !(this.TreasureChestID == (dynamic)base.originalValues["TreasureChestID"]); }
        public void ResetTreasureChestID() { this.TreasureChestID = (dynamic)base.originalValues["TreasureChestID"]; }

        TreasureItemCategory treasureItemCategory;
        [DisplayName("Category"), PrioritizedCategory("Item Parameters", 2)]
        [Description("")]
        [Browsable(true)]
        public TreasureItemCategory TreasureItemCategory
        {
            get { return treasureItemCategory; }
            set { base.SetProperty(ref treasureItemCategory, value, () => this.TreasureItemCategory); }
        }
        public bool ShouldSerializeTreasureItemCategory() { return !(this.TreasureItemCategory == (dynamic)base.originalValues["TreasureItemCategory"]); }
        public void ResetTreasureItemCategory() { this.TreasureItemCategory = (dynamic)base.originalValues["TreasureItemCategory"]; }

        ushort treasureItemID;
        [DisplayName("Item"), TypeConverter(typeof(TypeConverters.ItemNameConverter)), PrioritizedCategory("Item Parameters", 2)]
        [Description("")]
        [Browsable(true)]
        public ushort TreasureItemID
        {
            get { return treasureItemID; }
            set { base.SetProperty(ref treasureItemID, value, () => this.TreasureItemID); }
        }
        public bool ShouldSerializeTreasureItemID() { return !(this.TreasureItemID == (dynamic)base.originalValues["TreasureItemID"]); }
        public void ResetTreasureItemID() { this.TreasureItemID = (dynamic)base.originalValues["TreasureItemID"]; }

        ushort treasureMoney;
        [DisplayName("Money"), TypeConverter(typeof(TypeConverters.UshortEtrianEnConverter)), PrioritizedCategory("Money Parameters", 1)]
        [Description("")]
        [Browsable(true)]
        public ushort TreasureMoney
        {
            get { return treasureMoney; }
            set { base.SetProperty(ref treasureMoney, value, () => this.TreasureMoney); }
        }
        public bool ShouldSerializeTreasureMoney() { return !(this.TreasureMoney == (dynamic)base.originalValues["TreasureMoney"]); }
        public void ResetTreasureMoney() { this.TreasureMoney = (dynamic)base.originalValues["TreasureMoney"]; }

        protected override void Load()
        {
            treasureType = (TreasureType)(this.Data[15] >> 4);
            treasureChestID = (byte)(this.Data[15] & 0xF);

            // TODO: ChangeBrowsableAttribute buggy? Always hides Money, never ItemCategory/-ID?
            switch (treasureType)
            {
                case TreasureType.Item:
                    //this.ChangeBrowsableAttribute("TreasureItemCategory", true);
                    //this.ChangeBrowsableAttribute("TreasureItemID", true);
                    treasureItemCategory = (TreasureItemCategory)(this.Data[13] >> 4);
                    treasureItemID = BitConverter.ToUInt16(this.Data, 12);

                    //this.ChangeBrowsableAttribute("TreasureMoney", false);
                    break;

                case TreasureType.Money:
                    //this.ChangeBrowsableAttribute("TreasureItemCategory", false);
                    //this.ChangeBrowsableAttribute("TreasureItemID", false);

                    //this.ChangeBrowsableAttribute("TreasureMoney", true);
                    treasureMoney = BitConverter.ToUInt16(this.Data, 12);
                    break;
            }

            base.Load();
        }

        public override void Save()
        {
            byte tempData = (byte)((Convert.ToByte(treasureType) << 4) | (treasureChestID & 0xF));
            tempData.CopyTo(this.Data, 15);

            //

            base.Save();
        }
    }
}
