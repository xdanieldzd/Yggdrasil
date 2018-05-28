using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Yggdrasil.FileHandling;
using Yggdrasil.FileHandling.TableHandling;
using Yggdrasil.Attributes;

namespace Yggdrasil.TableParsing
{
	[PrioritizedCategory("Items", 0)]
	[ParserDescriptor("Item.tbb", 1, "General Items", 1)]
	public class MiscItemParser : BaseItemParser
	{
		[Browsable(false)]
		public override string EntryDescription { get { return (Name == string.Empty ? string.Format("(Item #{0})", ItemNumber) : Name.Truncate(20)); } }

		// "special item" ??
		// 0009 + unk2 0005 -> in battle, STR UP, DEF DOWN
		ushort unknown1;
		[DisplayName("Unknown 1"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
		public ushort Unknown1
		{
			get { return unknown1; }
			set { SetProperty(ref unknown1, value, () => Unknown1); }
		}
		public bool ShouldSerializeUnknown1() { return !(Unknown1 == (dynamic)originalValues["Unknown1"]); }
		public void ResetUnknown1() { Unknown1 = (dynamic)originalValues["Unknown1"]; }

		// "special item variable" ??
		ushort unknown2;
		[DisplayName("Unknown 2"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
		public ushort Unknown2
		{
			get { return unknown2; }
			set { SetProperty(ref unknown2, value, () => Unknown2); }
		}
		public bool ShouldSerializeUnknown2() { return !(Unknown2 == (dynamic)originalValues["Unknown2"]); }
		public void ResetUnknown2() { Unknown2 = (dynamic)originalValues["Unknown2"]; }

		ushort recoveredHP;
		[DisplayName("Recovered HP"), PrioritizedCategory("Modifiers", 2)]
		[Description("When regular item, HP recovered on use.")]
		public ushort RecoveredHP
		{
			get { return recoveredHP; }
			set { SetProperty(ref recoveredHP, value, () => RecoveredHP); }
		}
		public bool ShouldSerializeRecoveredHP() { return !(RecoveredHP == (dynamic)originalValues["RecoveredHP"]); }
		public void ResetRecoveredHP() { RecoveredHP = (dynamic)originalValues["RecoveredHP"]; }

		ushort recoveredTP;
		[DisplayName("Recovered TP"), PrioritizedCategory("Modifiers", 2)]
		[Description("When regular item, TP recovered on use.")]
		public ushort RecoveredTP
		{
			get { return recoveredTP; }
			set { SetProperty(ref recoveredTP, value, () => RecoveredTP); }
		}
		public bool ShouldSerializeRecoveredTP() { return !(RecoveredTP == (dynamic)originalValues["RecoveredTP"]); }
		public void ResetRecoveredTP() { RecoveredTP = (dynamic)originalValues["RecoveredTP"]; }

		ushort recoveredBoost;
		[DisplayName("Boost Modifier"), PrioritizedCategory("Modifiers", 2)]
		[Description("When regular item, Boost points added on use.")]
		public ushort RecoveredBoost
		{
			get { return recoveredBoost; }
			set { SetProperty(ref recoveredBoost, value, () => RecoveredBoost); }
		}
		public bool ShouldSerializeRecoveredBoost() { return !(RecoveredBoost == (dynamic)originalValues["RecoveredBoost"]); }
		public void ResetRecoveredBoost() { RecoveredBoost = (dynamic)originalValues["RecoveredBoost"]; }

		// 0004 -> can USE
		ushort unknown3Flags;
		[DisplayName("Unknown 3 (Flags)"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
		public ushort Unknown3Flags
		{
			get { return unknown3Flags; }
			set { SetProperty(ref unknown3Flags, value, () => Unknown3Flags); }
		}
		public bool ShouldSerializeUnknown3Flags() { return !(Unknown3Flags == (dynamic)originalValues["Unknown3Flags"]); }
		public void ResetUnknown3Flags() { Unknown3Flags = (dynamic)originalValues["Unknown3Flags"]; }

		byte unknown4;
		[DisplayName("Unknown 4"), TypeConverter(typeof(TypeConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
		public byte Unknown4
		{
			get { return unknown4; }
			set { SetProperty(ref unknown4, value, () => Unknown4); }
		}
		public bool ShouldSerializeUnknown4() { return !(Unknown4 == (dynamic)originalValues["Unknown4"]); }
		public void ResetUnknown4() { Unknown4 = (dynamic)originalValues["Unknown4"]; }

		// 01 -> BUY: if unlocked, sold out?!
		// 08 -> can't DISCARD nor SELL
		// 20 -> USE: target whole group
		byte unknown5Flags;
		[DisplayName("Unknown 5 (Flags)"), TypeConverter(typeof(TypeConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
		public byte Unknown5Flags
		{
			get { return unknown5Flags; }
			set { SetProperty(ref unknown5Flags, value, () => Unknown5Flags); }
		}
		public bool ShouldSerializeUnknown5Flags() { return !(Unknown5Flags == (dynamic)originalValues["Unknown5Flags"]); }
		public void ResetUnknown5Flags() { Unknown5Flags = (dynamic)originalValues["Unknown5Flags"]; }

		uint buyPrice;
		[DisplayName("Buy Price"), TypeConverter(typeof(TypeConverters.UintEtrianEnConverter)), PrioritizedCategory("Cost", 1)]
		[Description("Price when buying item from Shilleka's Goods or Ceft Apothecary.")]
		public uint BuyPrice
		{
			get { return buyPrice; }
			set { SetProperty(ref buyPrice, value, () => BuyPrice); }
		}
		public bool ShouldSerializeBuyPrice() { return !(BuyPrice == (dynamic)originalValues["BuyPrice"]); }
		public void ResetBuyPrice() { BuyPrice = (dynamic)originalValues["BuyPrice"]; }

		uint sellPrice;
		[DisplayName("Sell Price"), TypeConverter(typeof(TypeConverters.UintEtrianEnConverter)), PrioritizedCategory("Cost", 1)]
		[Description("Return when selling item to Shilleka's Goods.")]
		public uint SellPrice
		{
			get { return sellPrice; }
			set { SetProperty(ref sellPrice, value, () => SellPrice); }
		}
		public bool ShouldSerializeSellPrice() { return !(SellPrice == (dynamic)originalValues["SellPrice"]); }
		public void ResetSellPrice() { SellPrice = (dynamic)originalValues["SellPrice"]; }

		public MiscItemParser(GameDataManager gameDataManager, DataTable table, int entryNumber, PropertyChangedEventHandler propertyChanged = null) :
			base(gameDataManager, table, entryNumber, propertyChanged)
		{ Load(); }

		protected override void Load()
		{
			unknown1 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 2);
			unknown2 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 4);
			recoveredHP = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 6);
			recoveredTP = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 8);
			recoveredBoost = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 10);
			unknown3Flags = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 12);
			unknown4 = ParentTable.Data[EntryNumber][14];
			unknown5Flags = ParentTable.Data[EntryNumber][15];
			buyPrice = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 16);
			sellPrice = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 20);

			base.Load();
		}

		public override void Save()
		{
			unknown1.CopyTo(ParentTable.Data[EntryNumber], 2);
			unknown2.CopyTo(ParentTable.Data[EntryNumber], 4);
			recoveredHP.CopyTo(ParentTable.Data[EntryNumber], 6);
			recoveredTP.CopyTo(ParentTable.Data[EntryNumber], 8);
			recoveredBoost.CopyTo(ParentTable.Data[EntryNumber], 10);
			unknown3Flags.CopyTo(ParentTable.Data[EntryNumber], 12);
			unknown4.CopyTo(ParentTable.Data[EntryNumber], 14);
			unknown5Flags.CopyTo(ParentTable.Data[EntryNumber], 15);
			buyPrice.CopyTo(ParentTable.Data[EntryNumber], 16);
			sellPrice.CopyTo(ParentTable.Data[EntryNumber], 20);

			base.Save();
		}
	}
}
