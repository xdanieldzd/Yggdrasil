using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

using Yggdrasil.FileHandling;
using Yggdrasil.FileHandling.TableHandling;
using Yggdrasil.Attributes;

namespace Yggdrasil.TableParsing
{
    [TreeNodeCategory("Items")]
    [ParserDescriptor("SkillItemCutData.tbb", 0, "Gathering Points\\Chop Skill", 3)]
    [ParserDescriptor("SkillItemMiningData.tbb", 0, "Gathering Points\\Mine Skill", 3)]
    [ParserDescriptor("SkillItemPickData.tbb", 0, "Gathering Points\\Take Skill", 3)]
    public class GatherItemParser : BaseParser
    {
        ushort gatherNumber;
        [DisplayName("(ID)"), ReadOnly(true), PrioritizedCategory("Information", byte.MaxValue)]
        [Description("Internal ID number of gathering point data.")]
        public ushort GatherNumber
        {
            get { return gatherNumber; }
            set { base.SetProperty(ref gatherNumber, value, () => this.GatherNumber); }
        }
        public bool ShouldSerializeGatherNumber() { return !(this.GatherNumber == (dynamic)base.originalValues["GatherNumber"]); }
        public void ResetGatherNumber() { this.GatherNumber = (dynamic)base.originalValues["GatherNumber"]; }

        [Browsable(false)]
        public override string EntryDescription { get { return string.Format("{0} ({1}{2})", TypeConverters.FloorNumberConverter.Dictionary[FloorNumber], (char)('A' + (YCoord / 5)), (XCoord / 5) + 1); } }

        ushort unknown1;
        [DisplayName("Unknown 1"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
        [Description("Unknown; always zero?")]
        public ushort Unknown1
        {
            get { return unknown1; }
            set { base.SetProperty(ref unknown1, value, () => this.Unknown1); }
        }
        public bool ShouldSerializeUnknown1() { return !(this.Unknown1 == (dynamic)base.originalValues["Unknown1"]); }
        public void ResetUnknown1() { this.Unknown1 = (dynamic)base.originalValues["Unknown1"]; }

        byte floorNumber;
        [DisplayName("Floor"), TypeConverter(typeof(TypeConverters.FloorNumberConverter)), PrioritizedCategory("Location", 4)]
        [Description("Labyrinth floor where the gathering point is located.")]
        public byte FloorNumber
        {
            get { return floorNumber; }
            set { base.SetProperty(ref floorNumber, value, () => this.FloorNumber); }
        }
        public bool ShouldSerializeFloorNumber() { return !(this.FloorNumber == (dynamic)base.originalValues["FloorNumber"]); }
        public void ResetFloorNumber() { this.FloorNumber = (dynamic)base.originalValues["FloorNumber"]; }

        byte xCoord;
        [DisplayName("X Coordinate"), PrioritizedCategory("Location", 4)]
        [Description("X coordinate in tiles where the gathering point is located.")]
        public byte XCoord
        {
            get { return xCoord; }
            set { base.SetProperty(ref xCoord, value, () => this.XCoord); }
        }
        public bool ShouldSerializeXCoord() { return !(this.XCoord == (dynamic)base.originalValues["XCoord"]); }
        public void ResetXCoord() { this.XCoord = (dynamic)base.originalValues["XCoord"]; }

        byte yCoord;
        [DisplayName("Y Coordinate"), PrioritizedCategory("Location", 4)]
        [Description("Y coordinate in tiles where the gathering point is located.")]
        public byte YCoord
        {
            get { return yCoord; }
            set { base.SetProperty(ref yCoord, value, () => this.YCoord); }
        }
        public bool ShouldSerializeYCoord() { return !(this.YCoord == (dynamic)base.originalValues["YCoord"]); }
        public void ResetYCoord() { this.YCoord = (dynamic)base.originalValues["YCoord"]; }

        byte unknown2;
        [DisplayName("Unknown 2"), TypeConverter(typeof(TypeConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
        [Description("Unknown; always zero?")]
        public byte Unknown2
        {
            get { return unknown2; }
            set { base.SetProperty(ref unknown2, value, () => this.Unknown2); }
        }
        public bool ShouldSerializeUnknown2() { return !(this.Unknown2 == (dynamic)base.originalValues["Unknown2"]); }
        public void ResetUnknown2() { this.Unknown2 = (dynamic)base.originalValues["Unknown2"]; }

        ushort itemID1;
        [DisplayName("Item Drop"), TypeConverter(typeof(TypeConverters.ItemNameConverter)), PrioritizedCategory("1st Item", 3)]
        [Description("First possible item to be found at this point.")]
        public ushort ItemID1
        {
            get { return itemID1; }
            set { base.SetProperty(ref itemID1, value, () => this.ItemID1); }
        }
        public bool ShouldSerializeItemID1() { return !(this.ItemID1 == (dynamic)base.originalValues["ItemID1"]); }
        public void ResetItemID1() { this.ItemID1 = (dynamic)base.originalValues["ItemID1"]; }

        ushort itemID2;
        [DisplayName("Item Drop"), TypeConverter(typeof(TypeConverters.ItemNameConverter)), PrioritizedCategory("2nd Item", 2)]
        [Description("Second possible item to be found at this point.")]
        public ushort ItemID2
        {
            get { return itemID2; }
            set { base.SetProperty(ref itemID2, value, () => this.ItemID2); }
        }
        public bool ShouldSerializeItemID2() { return !(this.ItemID2 == (dynamic)base.originalValues["ItemID2"]); }
        public void ResetItemID2() { this.ItemID2 = (dynamic)base.originalValues["ItemID2"]; }

        ushort itemID3;
        [DisplayName("Item Drop"), TypeConverter(typeof(TypeConverters.ItemNameConverter)), PrioritizedCategory("3rd Item", 1)]
        [Description("Third possible item to be found at this point.")]
        public ushort ItemID3
        {
            get { return itemID3; }
            set { base.SetProperty(ref itemID3, value, () => this.ItemID3); }
        }
        public bool ShouldSerializeItemID3() { return !(this.ItemID3 == (dynamic)base.originalValues["ItemID3"]); }
        public void ResetItemID3() { this.ItemID3 = (dynamic)base.originalValues["ItemID3"]; }

        ushort itemProbability1;
        [DisplayName("Drop Rate"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("1st Item", 3)]
        [Description("Drop rate of first possible item at this point.")]
        public ushort ItemProbability1
        {
            get { return itemProbability1; }
            set { base.SetProperty(ref itemProbability1, value, () => this.ItemProbability1); }
        }
        public bool ShouldSerializeItemProbability1() { return !(this.ItemProbability1 == (dynamic)base.originalValues["ItemProbability1"]); }
        public void ResetItemProbability1() { this.ItemProbability1 = (dynamic)base.originalValues["ItemProbability1"]; }

        ushort itemProbability2;
        [DisplayName("Drop Rate"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("2nd Item", 2)]
        [Description("Drop rate of second possible item at this point.")]
        public ushort ItemProbability2
        {
            get { return itemProbability2; }
            set { base.SetProperty(ref itemProbability2, value, () => this.ItemProbability2); }
        }
        public bool ShouldSerializeItemProbability2() { return !(this.ItemProbability2 == (dynamic)base.originalValues["ItemProbability2"]); }
        public void ResetItemProbability2() { this.ItemProbability2 = (dynamic)base.originalValues["ItemProbability2"]; }

        ushort itemProbability3;
        [DisplayName("Drop Rate"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("3rd Item", 1)]
        [Description("Drop rate of third possible item at this point.")]
        public ushort ItemProbability3
        {
            get { return itemProbability3; }
            set { base.SetProperty(ref itemProbability3, value, () => this.ItemProbability3); }
        }
        public bool ShouldSerializeItemProbability3() { return !(this.ItemProbability3 == (dynamic)base.originalValues["ItemProbability3"]); }
        public void ResetItemProbability3() { this.ItemProbability3 = (dynamic)base.originalValues["ItemProbability3"]; }

        uint unknown3;
        [DisplayName("Unknown 3"), TypeConverter(typeof(TypeConverters.HexUintConverter)), PrioritizedCategory("Unknown", 0)]
        [Description("Unknown; always 0xFFFFFFFF?")]
        public uint Unknown3
        {
            get { return unknown3; }
            set { base.SetProperty(ref unknown3, value, () => this.Unknown3); }
        }
        public bool ShouldSerializeUnknown3() { return !(this.Unknown3 == (dynamic)base.originalValues["Unknown3"]); }
        public void ResetUnknown3() { this.Unknown3 = (dynamic)base.originalValues["Unknown3"]; }

        uint unknown4;
        [DisplayName("Unknown 4"), TypeConverter(typeof(TypeConverters.HexUintConverter)), PrioritizedCategory("Unknown", 0)]
        [Description("Unknown; always 0xFFFFFFFF?")]
        public uint Unknown4
        {
            get { return unknown4; }
            set { base.SetProperty(ref unknown4, value, () => this.Unknown4); }
        }
        public bool ShouldSerializeUnknown4() { return !(this.Unknown4 == (dynamic)base.originalValues["Unknown4"]); }
        public void ResetUnknown4() { this.Unknown4 = (dynamic)base.originalValues["Unknown4"]; }

        uint unknown5;
        [DisplayName("Unknown 5"), TypeConverter(typeof(TypeConverters.HexUintConverter)), PrioritizedCategory("Unknown", 0)]
        [Description("Unknown; always 0xFFFFFFFF?")]
        public uint Unknown5
        {
            get { return unknown5; }
            set { base.SetProperty(ref unknown5, value, () => this.Unknown5); }
        }
        public bool ShouldSerializeUnknown5() { return !(this.Unknown5 == (dynamic)base.originalValues["Unknown5"]); }
        public void ResetUnknown5() { this.Unknown5 = (dynamic)base.originalValues["Unknown5"]; }

        public GatherItemParser(GameDataManager gameDataManager, DataTable table, int entryNumber, PropertyChangedEventHandler propertyChanged = null) :
            base(gameDataManager, table, entryNumber, propertyChanged) { Load(); }

        protected override void Load()
        {
            gatherNumber = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 0);
            unknown1 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 2);
            floorNumber = ParentTable.Data[EntryNumber][4];
            xCoord = ParentTable.Data[EntryNumber][5];
            yCoord = ParentTable.Data[EntryNumber][6];
            unknown2 = ParentTable.Data[EntryNumber][7];
            itemID1 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 8);
            itemID2 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 10);
            itemID3 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 12);
            itemProbability1 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 14);
            itemProbability2 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 16);
            itemProbability3 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 18);
            unknown3 = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 20);
            unknown4 = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 24);
            unknown5 = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 28);

            base.Load();
        }

        public override void Save()
        {
            gatherNumber.CopyTo(ParentTable.Data[EntryNumber], 0);
            unknown1.CopyTo(ParentTable.Data[EntryNumber], 2);
            floorNumber.CopyTo(ParentTable.Data[EntryNumber], 4);
            xCoord.CopyTo(ParentTable.Data[EntryNumber], 5);
            yCoord.CopyTo(ParentTable.Data[EntryNumber], 6);
            unknown2.CopyTo(ParentTable.Data[EntryNumber], 7);
            itemID1.CopyTo(ParentTable.Data[EntryNumber], 8);
            itemID2.CopyTo(ParentTable.Data[EntryNumber], 10);
            itemID3.CopyTo(ParentTable.Data[EntryNumber], 12);
            itemProbability1.CopyTo(ParentTable.Data[EntryNumber], 14);
            itemProbability2.CopyTo(ParentTable.Data[EntryNumber], 16);
            itemProbability3.CopyTo(ParentTable.Data[EntryNumber], 18);
            unknown3.CopyTo(ParentTable.Data[EntryNumber], 20);
            unknown4.CopyTo(ParentTable.Data[EntryNumber], 24);
            unknown5.CopyTo(ParentTable.Data[EntryNumber], 28);

            base.Save();
        }

        public static void GenerateGatheringNodes(TreeNode parserNode, List<BaseParser> currentParsers)
        {
            foreach (BaseParser parser in currentParsers.OrderBy(x => (x as GatherItemParser).XCoord).OrderBy(x => (x as GatherItemParser).FloorNumber))
                parserNode.Nodes.Add(new TreeNode(parser.EntryDescription) { Tag = parser });
        }
    }
}
