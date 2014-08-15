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
    [Description("Item Compounds")]
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

        ushort itemCompound2;
        [DisplayName("2nd Item"), TypeConverter(typeof(CustomConverters.ItemNameConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Second item required to be sold before it becomes available for purchase.")]
        public ushort ItemCompound2
        {
            get { return itemCompound2; }
            set { base.SetProperty(ref itemCompound2, value, () => this.ItemCompound2); }
        }

        ushort itemCompound3;
        [DisplayName("3rd Item"), TypeConverter(typeof(CustomConverters.ItemNameConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Third item required to be sold before it becomes available for purchase.")]
        public ushort ItemCompound3
        {
            get { return itemCompound3; }
            set { base.SetProperty(ref itemCompound3, value, () => this.ItemCompound3); }
        }

        ushort itemCompound4;
        [DisplayName("4th Item"), TypeConverter(typeof(CustomConverters.ItemNameConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Fourth item required to be sold before it becomes available for purchase.")]
        public ushort ItemCompound4
        {
            get { return itemCompound4; }
            set { base.SetProperty(ref itemCompound4, value, () => this.ItemCompound4); }
        }

        ushort itemCompound5;
        [DisplayName("5th Item"), TypeConverter(typeof(CustomConverters.ItemNameConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Fifth item required to be sold before it becomes available for purchase.")]
        public ushort ItemCompound5
        {
            get { return itemCompound5; }
            set { base.SetProperty(ref itemCompound5, value, () => this.ItemCompound5); }
        }

        byte itemCount1;
        [DisplayName("Amount of 1st Item"), TypeConverter(typeof(CustomConverters.ByteItemCountConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Amount of first item required to be sold before it becomes available for purchase.")]
        public byte ItemCount1
        {
            get { return itemCount1; }
            set { base.SetProperty(ref itemCount1, value, () => this.ItemCount1); }
        }

        byte itemCount2;
        [DisplayName("Amount of 2nd Item"), TypeConverter(typeof(CustomConverters.ByteItemCountConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Amount of second item required to be sold before it becomes available for purchase.")]
        public byte ItemCount2
        {
            get { return itemCount2; }
            set { base.SetProperty(ref itemCount2, value, () => this.ItemCount2); }
        }

        byte itemCount3;
        [DisplayName("Amount of 3rd Item"), TypeConverter(typeof(CustomConverters.ByteItemCountConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Amount of third item required to be sold before it becomes available for purchase.")]
        public byte ItemCount3
        {
            get { return itemCount3; }
            set { base.SetProperty(ref itemCount3, value, () => this.ItemCount3); }
        }

        byte itemCount4;
        [DisplayName("Amount of 4th Item"), TypeConverter(typeof(CustomConverters.ByteItemCountConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Amount of fourth item required to be sold before it becomes available for purchase.")]
        public byte ItemCount4
        {
            get { return itemCount4; }
            set { base.SetProperty(ref itemCount4, value, () => this.ItemCount4); }
        }

        byte itemCount5;
        [DisplayName("Amount of 5th Item"), TypeConverter(typeof(CustomConverters.ByteItemCountConverter)), PrioritizedCategory("Requirements", 0)]
        [Description("Amount of fifth item required to be sold before it becomes available for purchase.")]
        public byte ItemCount5
        {
            get { return itemCount5; }
            set { base.SetProperty(ref itemCount5, value, () => this.ItemCount5); }
        }

        byte unknown5;
        [DisplayName("Unknown 5"), TypeConverter(typeof(CustomConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
        public byte Unknown5
        {
            get { return unknown5; }
            set { base.SetProperty(ref unknown5, value, () => this.Unknown5); }
        }

        public ItemCompoundParser(GameDataManager game, TBB.TBL1 table, int entryNumber, PropertyChangedEventHandler propertyChanged = null) : base(game, table, entryNumber, propertyChanged) { Load(); }

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
            unknown5 = ParentTable.Data[EntryNumber][17];

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
            unknown5.CopyTo(ParentTable.Data[EntryNumber], 17);

            base.Save();
        }

        public static TreeNode GenerateTreeNode(GameDataManager game, IList<BaseParser> parsedData)
        {
            string description = (typeof(ItemCompoundParser).GetCustomAttributes(false).FirstOrDefault(x => x is DescriptionAttribute) as DescriptionAttribute).Description;
            TreeNode node = new TreeNode(description) { Tag = parsedData };

            foreach (BaseParser parsed in parsedData)
                node.Nodes.Add(new TreeNode(parsed.EntryDescription) { Tag = parsed });

            return node;
        }
    }
}
