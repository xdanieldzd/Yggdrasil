using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

using Yggdrasil.FileTypes;

namespace Yggdrasil.TableParsers
{
    [ParserUsage("Item.tbb", 1)]
    [Description("General Items")]
    public class MiscItemParser : BaseItemParser
    {
        [Browsable(false)]
        public override string EntryDescription { get { return Name; } }

        // "special item" ??
        // 0009 + unk2 0005 -> in battle, STR UP, DEF DOWN
        ushort unknown1;
        [DisplayName("Unknown 1"), TypeConverter(typeof(CustomConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
        public ushort Unknown1
        {
            get { return unknown1; }
            set { base.SetProperty(ref unknown1, value, () => this.Unknown1); }
        }
        public bool ShouldSerializeUnknown1() { return !(this.Unknown1 == (dynamic)base.originalValues["Unknown1"]); }
        public void ResetUnknown1() { this.Unknown1 = (dynamic)base.originalValues["Unknown1"]; }

        // "special item variable" ??
        ushort unknown2;
        [DisplayName("Unknown 2"), TypeConverter(typeof(CustomConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
        public ushort Unknown2
        {
            get { return unknown2; }
            set { base.SetProperty(ref unknown2, value, () => this.Unknown2); }
        }
        public bool ShouldSerializeUnknown2() { return !(this.Unknown2 == (dynamic)base.originalValues["Unknown2"]); }
        public void ResetUnknown2() { this.Unknown2 = (dynamic)base.originalValues["Unknown2"]; }

        ushort recoveredHP;
        [DisplayName("Recovered HP"), PrioritizedCategory("Modifiers", 2)]
        [Description("When regular item, HP recovered on use.")]
        public ushort RecoveredHP
        {
            get { return recoveredHP; }
            set { base.SetProperty(ref recoveredHP, value, () => this.RecoveredHP); }
        }
        public bool ShouldSerializeRecoveredHP() { return !(this.RecoveredHP == (dynamic)base.originalValues["RecoveredHP"]); }
        public void ResetRecoveredHP() { this.RecoveredHP = (dynamic)base.originalValues["RecoveredHP"]; }

        ushort recoveredTP;
        [DisplayName("Recovered TP"), PrioritizedCategory("Modifiers", 2)]
        [Description("When regular item, TP recovered on use.")]
        public ushort RecoveredTP
        {
            get { return recoveredTP; }
            set { base.SetProperty(ref recoveredTP, value, () => this.RecoveredTP); }
        }
        public bool ShouldSerializeRecoveredTP() { return !(this.RecoveredTP == (dynamic)base.originalValues["RecoveredTP"]); }
        public void ResetRecoveredTP() { this.RecoveredTP = (dynamic)base.originalValues["RecoveredTP"]; }

        ushort recoveredBoost;
        [DisplayName("Boost Modifier"), PrioritizedCategory("Modifiers", 2)]
        [Description("When regular item, Boost points added on use.")]
        public ushort RecoveredBoost
        {
            get { return recoveredBoost; }
            set { base.SetProperty(ref recoveredBoost, value, () => this.RecoveredBoost); }
        }
        public bool ShouldSerializeRecoveredBoost() { return !(this.RecoveredBoost == (dynamic)base.originalValues["RecoveredBoost"]); }
        public void ResetRecoveredBoost() { this.RecoveredBoost = (dynamic)base.originalValues["RecoveredBoost"]; }

        // 0004 -> can USE
        ushort unknown3;
        [DisplayName("Unknown 3"), TypeConverter(typeof(CustomConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
        public ushort Unknown3
        {
            get { return unknown3; }
            set { base.SetProperty(ref unknown3, value, () => this.Unknown3); }
        }
        public bool ShouldSerializeUnknown3() { return !(this.Unknown3 == (dynamic)base.originalValues["Unknown3"]); }
        public void ResetUnknown3() { this.Unknown3 = (dynamic)base.originalValues["Unknown3"]; }

        byte unknown4;
        [DisplayName("Unknown 4"), TypeConverter(typeof(CustomConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
        public byte Unknown4
        {
            get { return unknown4; }
            set { base.SetProperty(ref unknown4, value, () => this.Unknown4); }
        }
        public bool ShouldSerializeUnknown4() { return !(this.Unknown4 == (dynamic)base.originalValues["Unknown4"]); }
        public void ResetUnknown4() { this.Unknown4 = (dynamic)base.originalValues["Unknown4"]; }

        // 01 -> BUY: if unlocked, sold out?!
        // 08 -> can't DISCARD nor SELL
        // 20 -> USE: target whole group
        byte unknown5;
        [DisplayName("Unknown 5"), TypeConverter(typeof(CustomConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
        public byte Unknown5
        {
            get { return unknown5; }
            set { base.SetProperty(ref unknown5, value, () => this.Unknown5); }
        }
        public bool ShouldSerializeUnknown5() { return !(this.Unknown5 == (dynamic)base.originalValues["Unknown5"]); }
        public void ResetUnknown5() { this.Unknown5 = (dynamic)base.originalValues["Unknown5"]; }

        uint buyPrice;
        [DisplayName("Buy Price"), TypeConverter(typeof(CustomConverters.EtrianEnConverter)), PrioritizedCategory("Cost", 1)]
        [Description("Price when buying item from Shilleka's Goods or Ceft Apothecary.")]
        public uint BuyPrice
        {
            get { return buyPrice; }
            set { base.SetProperty(ref buyPrice, value, () => this.BuyPrice); }
        }
        public bool ShouldSerializeBuyPrice() { return !(this.BuyPrice == (dynamic)base.originalValues["BuyPrice"]); }
        public void ResetBuyPrice() { this.BuyPrice = (dynamic)base.originalValues["BuyPrice"]; }

        uint sellPrice;
        [DisplayName("Sell Price"), TypeConverter(typeof(CustomConverters.EtrianEnConverter)), PrioritizedCategory("Cost", 1)]
        [Description("Return when selling item to Shilleka's Goods.")]
        public uint SellPrice
        {
            get { return sellPrice; }
            set { base.SetProperty(ref sellPrice, value, () => this.SellPrice); }
        }
        public bool ShouldSerializeSellPrice() { return !(this.SellPrice == (dynamic)base.originalValues["SellPrice"]); }
        public void ResetSellPrice() { this.SellPrice = (dynamic)base.originalValues["SellPrice"]; }

        public MiscItemParser(GameDataManager gameDataManager, TBB.TBL1 table, int entryNumber, PropertyChangedEventHandler propertyChanged = null) :
            base(gameDataManager, table, entryNumber, propertyChanged) { Load(); }

        protected override void Load()
        {
            unknown1 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 2);
            unknown2 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 4);
            recoveredHP = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 6);
            recoveredTP = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 8);
            recoveredBoost = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 10);
            unknown3 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 12);
            unknown4 = ParentTable.Data[EntryNumber][14];
            unknown5 = ParentTable.Data[EntryNumber][15];
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
            unknown3.CopyTo(ParentTable.Data[EntryNumber], 12);
            unknown4.CopyTo(ParentTable.Data[EntryNumber], 14);
            unknown5.CopyTo(ParentTable.Data[EntryNumber], 15);
            buyPrice.CopyTo(ParentTable.Data[EntryNumber], 16);
            sellPrice.CopyTo(ParentTable.Data[EntryNumber], 20);

            base.Save();
        }

        public static TreeNode GenerateTreeNode(GameDataManager gameDataManager, IList<BaseParser> parsedData)
        {
            string description = (typeof(MiscItemParser).GetCustomAttributes(false).FirstOrDefault(x => x is DescriptionAttribute) as DescriptionAttribute).Description;
            TreeNode node = new TreeNode(description) { Tag = parsedData };

            foreach (BaseParser parsed in parsedData)
                node.Nodes.Add(new TreeNode(parsed.EntryDescription) { Tag = parsed });

            return node;
        }
    }
}
