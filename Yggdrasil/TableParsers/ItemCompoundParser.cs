using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

using Yggdrasil.FileTypes;

namespace Yggdrasil.TableParsers
{
    [ParserUsage("ItemCompound.tbb", 0)]
    [PrioritizedDescription("Item Requirements", 2)]
    public class ItemCompoundParser : BaseItemParser
    {
        [Browsable(false)]
        public override string EntryDescription { get { return Name; } }

        ushort itemCompound1;
        [DisplayName("1st Item"), TypeConverter(typeof(CustomConverters.ItemNameConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("First item required to be sold before it becomes available for purchase.")]
        public ushort ItemCompound1
        {
            get { return itemCompound1; }
            set { base.SetProperty(ref itemCompound1, value, () => this.ItemCompound1); }
        }
        public bool ShouldSerializeItemCompound1() { return !(this.ItemCompound1 == (dynamic)base.originalValues["ItemCompound1"]); }
        public void ResetItemCompound1() { this.ItemCompound1 = (dynamic)base.originalValues["ItemCompound1"]; }

        ushort itemCompound2;
        [DisplayName("2nd Item"), TypeConverter(typeof(CustomConverters.ItemNameConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Second item required to be sold before it becomes available for purchase.")]
        public ushort ItemCompound2
        {
            get { return itemCompound2; }
            set { base.SetProperty(ref itemCompound2, value, () => this.ItemCompound2); }
        }
        public bool ShouldSerializeItemCompound2() { return !(this.ItemCompound2 == (dynamic)base.originalValues["ItemCompound2"]); }
        public void ResetItemCompound2() { this.ItemCompound2 = (dynamic)base.originalValues["ItemCompound2"]; }

        ushort itemCompound3;
        [DisplayName("3rd Item"), TypeConverter(typeof(CustomConverters.ItemNameConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Third item required to be sold before it becomes available for purchase.")]
        public ushort ItemCompound3
        {
            get { return itemCompound3; }
            set { base.SetProperty(ref itemCompound3, value, () => this.ItemCompound3); }
        }
        public bool ShouldSerializeItemCompound3() { return !(this.ItemCompound3 == (dynamic)base.originalValues["ItemCompound3"]); }
        public void ResetItemCompound3() { this.ItemCompound3 = (dynamic)base.originalValues["ItemCompound3"]; }

        ushort itemCompound4;
        [DisplayName("4th Item"), TypeConverter(typeof(CustomConverters.ItemNameConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Fourth item required to be sold before it becomes available for purchase.")]
        public ushort ItemCompound4
        {
            get { return itemCompound4; }
            set { base.SetProperty(ref itemCompound4, value, () => this.ItemCompound4); }
        }
        public bool ShouldSerializeItemCompound4() { return !(this.ItemCompound4 == (dynamic)base.originalValues["ItemCompound4"]); }
        public void ResetItemCompound4() { this.ItemCompound4 = (dynamic)base.originalValues["ItemCompound4"]; }

        ushort itemCompound5;
        [DisplayName("5th Item"), TypeConverter(typeof(CustomConverters.ItemNameConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Fifth item required to be sold before it becomes available for purchase.")]
        public ushort ItemCompound5
        {
            get { return itemCompound5; }
            set { base.SetProperty(ref itemCompound5, value, () => this.ItemCompound5); }
        }
        public bool ShouldSerializeItemCompound5() { return !(this.ItemCompound5 == (dynamic)base.originalValues["ItemCompound5"]); }
        public void ResetItemCompound5() { this.ItemCompound5 = (dynamic)base.originalValues["ItemCompound5"]; }

        byte itemCount1;
        [DisplayName("Amount of 1st Item"), TypeConverter(typeof(CustomConverters.ByteItemCountConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Amount of first item required to be sold before it becomes available for purchase.")]
        public byte ItemCount1
        {
            get { return itemCount1; }
            set { base.SetProperty(ref itemCount1, value, () => this.ItemCount1); }
        }
        public bool ShouldSerializeItemCount1() { return !(this.ItemCount1 == (dynamic)base.originalValues["ItemCount1"]); }
        public void ResetItemCount1() { this.ItemCount1 = (dynamic)base.originalValues["ItemCount1"]; }

        byte itemCount2;
        [DisplayName("Amount of 2nd Item"), TypeConverter(typeof(CustomConverters.ByteItemCountConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Amount of second item required to be sold before it becomes available for purchase.")]
        public byte ItemCount2
        {
            get { return itemCount2; }
            set { base.SetProperty(ref itemCount2, value, () => this.ItemCount2); }
        }
        public bool ShouldSerializeItemCount2() { return !(this.ItemCount2 == (dynamic)base.originalValues["ItemCount2"]); }
        public void ResetItemCount2() { this.ItemCount2 = (dynamic)base.originalValues["ItemCount2"]; }

        byte itemCount3;
        [DisplayName("Amount of 3rd Item"), TypeConverter(typeof(CustomConverters.ByteItemCountConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Amount of third item required to be sold before it becomes available for purchase.")]
        public byte ItemCount3
        {
            get { return itemCount3; }
            set { base.SetProperty(ref itemCount3, value, () => this.ItemCount3); }
        }
        public bool ShouldSerializeItemCount3() { return !(this.ItemCount3 == (dynamic)base.originalValues["ItemCount3"]); }
        public void ResetItemCount3() { this.ItemCount3 = (dynamic)base.originalValues["ItemCount3"]; }

        byte itemCount4;
        [DisplayName("Amount of 4th Item"), TypeConverter(typeof(CustomConverters.ByteItemCountConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Amount of fourth item required to be sold before it becomes available for purchase.")]
        public byte ItemCount4
        {
            get { return itemCount4; }
            set { base.SetProperty(ref itemCount4, value, () => this.ItemCount4); }
        }
        public bool ShouldSerializeItemCount4() { return !(this.ItemCount4 == (dynamic)base.originalValues["ItemCount4"]); }
        public void ResetItemCount4() { this.ItemCount4 = (dynamic)base.originalValues["ItemCount4"]; }

        byte itemCount5;
        [DisplayName("Amount of 5th Item"), TypeConverter(typeof(CustomConverters.ByteItemCountConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Amount of fifth item required to be sold before it becomes available for purchase.")]
        public byte ItemCount5
        {
            get { return itemCount5; }
            set { base.SetProperty(ref itemCount5, value, () => this.ItemCount5); }
        }
        public bool ShouldSerializeItemCount5() { return !(this.ItemCount5 == (dynamic)base.originalValues["ItemCount5"]); }
        public void ResetItemCount5() { this.ItemCount5 = (dynamic)base.originalValues["ItemCount5"]; }

        byte unknownPadding;
        [DisplayName("Padding?"), TypeConverter(typeof(CustomConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
        [Description("Unknown, possibly padding.")]
        public byte UnknownPadding
        {
            get { return unknownPadding; }
            set { base.SetProperty(ref unknownPadding, value, () => this.UnknownPadding); }
        }
        public bool ShouldSerializeUnknownPadding() { return !(this.UnknownPadding == (dynamic)base.originalValues["UnknownPadding"]); }
        public void ResetUnknownPadding() { this.UnknownPadding = (dynamic)base.originalValues["UnknownPadding"]; }

        public ItemCompoundParser(GameDataManager gameDataManager, TBB.TBL1 table, int entryNumber, PropertyChangedEventHandler propertyChanged = null) :
            base(gameDataManager, table, entryNumber, propertyChanged) { Load(); }

        protected override void Load()
        {
            itemCompound1 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 2);
            itemCompound2 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 4);
            itemCompound3 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 6);
            itemCompound4 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 8);
            itemCompound5 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 10);
            itemCount1 = ParentTable.Data[EntryNumber][12];
            itemCount2 = ParentTable.Data[EntryNumber][13];
            itemCount3 = ParentTable.Data[EntryNumber][14];
            itemCount4 = ParentTable.Data[EntryNumber][15];
            itemCount5 = ParentTable.Data[EntryNumber][16];
            unknownPadding = ParentTable.Data[EntryNumber][17];

            base.Load();
        }

        public override void Save()
        {
            itemCompound1.CopyTo(ParentTable.Data[EntryNumber], 2);
            itemCompound2.CopyTo(ParentTable.Data[EntryNumber], 4);
            itemCompound3.CopyTo(ParentTable.Data[EntryNumber], 6);
            itemCompound4.CopyTo(ParentTable.Data[EntryNumber], 8);
            itemCompound5.CopyTo(ParentTable.Data[EntryNumber], 10);
            itemCount1.CopyTo(ParentTable.Data[EntryNumber], 12);
            itemCount2.CopyTo(ParentTable.Data[EntryNumber], 13);
            itemCount3.CopyTo(ParentTable.Data[EntryNumber], 14);
            itemCount4.CopyTo(ParentTable.Data[EntryNumber], 15);
            itemCount5.CopyTo(ParentTable.Data[EntryNumber], 16);
            unknownPadding.CopyTo(ParentTable.Data[EntryNumber], 17);

            base.Save();
        }

        public static TreeNode GenerateTreeNode(GameDataManager gameDataManager, IList<BaseParser> parsedData)
        {
            string description = (typeof(ItemCompoundParser).GetCustomAttributes(false).FirstOrDefault(x => x is DescriptionAttribute) as DescriptionAttribute).Description;
            TreeNode node = new TreeNode(description) { Tag = parsedData };

            foreach (BaseParser parsed in parsedData)
                node.Nodes.Add(new TreeNode(parsed.EntryDescription) { Tag = parsed });

            return node;
        }
    }
}
