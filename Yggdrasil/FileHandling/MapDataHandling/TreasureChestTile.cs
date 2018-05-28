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
			base(gameDataManager, mapDataFile, offset, coordinates, propertyChanged)
		{ }

		public TreasureChestTile(BaseTile originalTile, MapDataFile.TileTypes newTileType) :
			base(originalTile, newTileType)
		{ }

		TreasureType treasureType;
		[DisplayName("Type"), PrioritizedCategory("General", 3)]
		[Description("")]
		public TreasureType TreasureType
		{
			get { return treasureType; }
			set { SetProperty(ref treasureType, value, () => TreasureType); }
		}
		public bool ShouldSerializeTreasureType() { return !(TreasureType == (dynamic)originalValues["TreasureType"]); }
		public void ResetTreasureType() { TreasureType = (dynamic)originalValues["TreasureType"]; }

		byte treasureChestID;
		[DisplayName("Chest ID"), TypeConverter(typeof(TypeConverters.HexByteConverter)), PrioritizedCategory("General", 3)]
		[Description("")]
		public byte TreasureChestID
		{
			get { return treasureChestID; }
			set { SetProperty(ref treasureChestID, value, () => TreasureChestID); }
		}
		public bool ShouldSerializeTreasureChestID() { return !(TreasureChestID == (dynamic)originalValues["TreasureChestID"]); }
		public void ResetTreasureChestID() { TreasureChestID = (dynamic)originalValues["TreasureChestID"]; }

		TreasureItemCategory treasureItemCategory;
		[DisplayName("Category"), PrioritizedCategory("Item Parameters", 2)]
		[Description("")]
		[Browsable(true)]
		public TreasureItemCategory TreasureItemCategory
		{
			get { return treasureItemCategory; }
			set { SetProperty(ref treasureItemCategory, value, () => TreasureItemCategory); }
		}
		public bool ShouldSerializeTreasureItemCategory() { return !(TreasureItemCategory == (dynamic)originalValues["TreasureItemCategory"]); }
		public void ResetTreasureItemCategory() { TreasureItemCategory = (dynamic)originalValues["TreasureItemCategory"]; }

		ushort treasureItemID;
		[DisplayName("Item"), TypeConverter(typeof(TypeConverters.ItemNameConverter)), PrioritizedCategory("Item Parameters", 2)]
		[Description("")]
		[Browsable(true)]
		public ushort TreasureItemID
		{
			get { return treasureItemID; }
			set { SetProperty(ref treasureItemID, value, () => TreasureItemID); }
		}
		public bool ShouldSerializeTreasureItemID() { return !(TreasureItemID == (dynamic)originalValues["TreasureItemID"]); }
		public void ResetTreasureItemID() { TreasureItemID = (dynamic)originalValues["TreasureItemID"]; }

		ushort treasureMoney;
		[DisplayName("Money"), TypeConverter(typeof(TypeConverters.UshortEtrianEnConverter)), PrioritizedCategory("Money Parameters", 1)]
		[Description("")]
		[Browsable(true)]
		public ushort TreasureMoney
		{
			get { return treasureMoney; }
			set { SetProperty(ref treasureMoney, value, () => TreasureMoney); }
		}
		public bool ShouldSerializeTreasureMoney() { return !(TreasureMoney == (dynamic)originalValues["TreasureMoney"]); }
		public void ResetTreasureMoney() { TreasureMoney = (dynamic)originalValues["TreasureMoney"]; }

		protected override void Load()
		{
			treasureType = (TreasureType)(Data[15] >> 4);
			treasureChestID = (byte)(Data[15] & 0xF);

			// TODO: ChangeBrowsableAttribute buggy? Always hides Money, never ItemCategory/-ID?
			switch (treasureType)
			{
				case TreasureType.Item:
					//this.ChangeBrowsableAttribute("TreasureItemCategory", true);
					//this.ChangeBrowsableAttribute("TreasureItemID", true);
					treasureItemCategory = (TreasureItemCategory)(Data[13] >> 4);
					treasureItemID = BitConverter.ToUInt16(Data, 12);

					//this.ChangeBrowsableAttribute("TreasureMoney", false);
					break;

				case TreasureType.Money:
					//this.ChangeBrowsableAttribute("TreasureItemCategory", false);
					//this.ChangeBrowsableAttribute("TreasureItemID", false);

					//this.ChangeBrowsableAttribute("TreasureMoney", true);
					treasureMoney = BitConverter.ToUInt16(Data, 12);
					break;
			}

			base.Load();
		}

		public override void Save()
		{
			byte tempData = (byte)((Convert.ToByte(treasureType) << 4) | (treasureChestID & 0xF));
			tempData.CopyTo(Data, 15);

			//

			base.Save();
		}
	}
}
