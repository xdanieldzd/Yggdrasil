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

        ushort unknown1;
        [DisplayName("Unknown 1"), TypeConverter(typeof(CustomConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
        public ushort Unknown1
        {
            get { return unknown1; }
            set { base.SetProperty(ref unknown1, value, () => this.Unknown1); }
        }

        ushort unknown2;
        [DisplayName("Unknown 2"), TypeConverter(typeof(CustomConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
        public ushort Unknown2
        {
            get { return unknown2; }
            set { base.SetProperty(ref unknown2, value, () => this.Unknown2); }
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

        byte unknown3;
        [DisplayName("Unknown 3"), TypeConverter(typeof(CustomConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
        public byte Unknown3
        {
            get { return unknown3; }
            set { base.SetProperty(ref unknown3, value, () => this.Unknown3); }
        }

        byte unknown4;
        [DisplayName("Unknown 4"), TypeConverter(typeof(CustomConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
        public byte Unknown4
        {
            get { return unknown4; }
            set { base.SetProperty(ref unknown4, value, () => this.Unknown4); }
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
            unknown1 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 8);
            unknown2 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 10);
            itemCount1 = ParentTable.Data[EntryNumber][12];
            itemCount2 = ParentTable.Data[EntryNumber][13];
            itemCount3 = ParentTable.Data[EntryNumber][14];
            unknown3 = ParentTable.Data[EntryNumber][15];
            unknown4 = ParentTable.Data[EntryNumber][16];
            Unknown4 = ParentTable.Data[EntryNumber][17];

            base.Load();
        }

        public override void Save()
        {
            itemCompound1.CopyTo(ParentTable.Data[EntryNumber], 2);
            itemCompound2.CopyTo(ParentTable.Data[EntryNumber], 4);
            itemCompound3.CopyTo(ParentTable.Data[EntryNumber], 6);
            unknown1.CopyTo(ParentTable.Data[EntryNumber], 8);
            unknown2.CopyTo(ParentTable.Data[EntryNumber], 10);
            itemCount1.CopyTo(ParentTable.Data[EntryNumber], 12);
            itemCount2.CopyTo(ParentTable.Data[EntryNumber], 13);
            itemCount3.CopyTo(ParentTable.Data[EntryNumber], 14);
            unknown3.CopyTo(ParentTable.Data[EntryNumber], 15);
            unknown4.CopyTo(ParentTable.Data[EntryNumber], 16);
            Unknown4.CopyTo(ParentTable.Data[EntryNumber], 17);

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
