using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

using Yggdrasil.FileTypes;

namespace Yggdrasil.TableParsers
{
    [TreeNodeCategory("Enemies")]
    [ParserUsage("EncountData.tbb", 1)]
    [PrioritizedDescription("Encounters Groups", 1)]
    public class EncounterGroupParser : BaseParser
    {
        [DisplayName("(ID)"), PrioritizedCategory("Information", byte.MaxValue)]
        [Description("Internal ID number of encounter group.")]
        public ushort GroupNumber { get { return (ushort)EntryNumber; } }

        uint stratumNumber;
        [DisplayName("Battle Background"), PrioritizedCategory("General", 1)]
        [Description("Background used by this group in battle; usually the zero-based number of stratum this group appears in.")]
        public uint StratumNumber
        {
            get { return stratumNumber; }
            set { base.SetProperty(ref stratumNumber, value, () => this.StratumNumber); }
        }
        public bool ShouldSerializeStratumNumber() { return !(this.StratumNumber == (dynamic)base.originalValues["StratumNumber"]); }
        public void ResetStratumNumber() { this.StratumNumber = (dynamic)base.originalValues["StratumNumber"]; }

        uint unknown1;
        [DisplayName("Unknown 1"), TypeConverter(typeof(CustomConverters.HexUintConverter)), PrioritizedCategory("Unknown", 0)]
        public uint Unknown1
        {
            get { return unknown1; }
            set { base.SetProperty(ref unknown1, value, () => this.Unknown1); }
        }
        public bool ShouldSerializeUnknown1() { return !(this.Unknown1 == (dynamic)base.originalValues["Unknown1"]); }
        public void ResetUnknown1() { this.Unknown1 = (dynamic)base.originalValues["Unknown1"]; }

        ushort encounterNumber1;
        [DisplayName("Encounter Type"), TypeConverter(typeof(CustomConverters.EncounterConverter)), PrioritizedCategory("1st Encounter", 4)]
        [Description("First possible encounter type in this group.")]
        public ushort EncounterNumber1
        {
            get { return encounterNumber1; }
            set { base.SetProperty(ref encounterNumber1, value, () => this.EncounterNumber1); }
        }
        public bool ShouldSerializeEncounterNumber1() { return !(this.EncounterNumber1 == (dynamic)base.originalValues["EncounterNumber1"]); }
        public void ResetEncounterNumber1() { this.EncounterNumber1 = (dynamic)base.originalValues["EncounterNumber1"]; }

        ushort encounterProbability1;
        [DisplayName("Frequency"), TypeConverter(typeof(CustomConverters.UshortPercentageConverter)), PrioritizedCategory("1st Encounter", 4)]
        [Description("Appearance frequency of first encounter type in this group.")]
        public ushort EncounterProbability1
        {
            get { return encounterProbability1; }
            set { base.SetProperty(ref encounterProbability1, value, () => this.EncounterProbability1); }
        }
        public bool ShouldSerializeEncounterProbability1() { return !(this.EncounterProbability1 == (dynamic)base.originalValues["EncounterProbability1"]); }
        public void ResetEncounterProbability1() { this.EncounterProbability1 = (dynamic)base.originalValues["EncounterProbability1"]; }

        ushort encounterNumber2;
        [DisplayName("Encounter Type"), TypeConverter(typeof(CustomConverters.EncounterConverter)), PrioritizedCategory("2nd Encounter", 3)]
        [Description("Second possible encounter type in this group.")]
        public ushort EncounterNumber2
        {
            get { return encounterNumber2; }
            set { base.SetProperty(ref encounterNumber2, value, () => this.EncounterNumber2); }
        }
        public bool ShouldSerializeEncounterNumber2() { return !(this.EncounterNumber2 == (dynamic)base.originalValues["EncounterNumber2"]); }
        public void ResetEncounterNumber2() { this.EncounterNumber2 = (dynamic)base.originalValues["EncounterNumber2"]; }

        ushort encounterProbability2;
        [DisplayName("Frequency"), TypeConverter(typeof(CustomConverters.UshortPercentageConverter)), PrioritizedCategory("2nd Encounter", 3)]
        [Description("Appearance frequency of second encounter type in this group.")]
        public ushort EncounterProbability2
        {
            get { return encounterProbability2; }
            set { base.SetProperty(ref encounterProbability2, value, () => this.EncounterProbability2); }
        }
        public bool ShouldSerializeEncounterProbability2() { return !(this.EncounterProbability2 == (dynamic)base.originalValues["EncounterProbability2"]); }
        public void ResetEncounterProbability2() { this.EncounterProbability2 = (dynamic)base.originalValues["EncounterProbability2"]; }

        ushort encounterNumber3;
        [DisplayName("Encounter Type"), TypeConverter(typeof(CustomConverters.EncounterConverter)), PrioritizedCategory("3rd Encounter", 2)]
        [Description("Third possible encounter type in this group.")]
        public ushort EncounterNumber3
        {
            get { return encounterNumber3; }
            set { base.SetProperty(ref encounterNumber3, value, () => this.EncounterNumber3); }
        }
        public bool ShouldSerializeEncounterNumber3() { return !(this.EncounterNumber3 == (dynamic)base.originalValues["EncounterNumber3"]); }
        public void ResetEncounterNumber3() { this.EncounterNumber3 = (dynamic)base.originalValues["EncounterNumber3"]; }

        ushort encounterProbability3;
        [DisplayName("Frequency"), TypeConverter(typeof(CustomConverters.UshortPercentageConverter)), PrioritizedCategory("3rd Encounter", 2)]
        [Description("Appearance frequency of third encounter type in this group.")]
        public ushort EncounterProbability3
        {
            get { return encounterProbability3; }
            set { base.SetProperty(ref encounterProbability3, value, () => this.EncounterProbability3); }
        }
        public bool ShouldSerializeEncounterProbability3() { return !(this.EncounterProbability3 == (dynamic)base.originalValues["EncounterProbability3"]); }
        public void ResetEncounterProbability3() { this.EncounterProbability3 = (dynamic)base.originalValues["EncounterProbability3"]; }

        public EncounterGroupParser(GameDataManager gameDataManager, TBB.TBL1 table, int entryNumber, PropertyChangedEventHandler propertyChanged = null) :
            base(gameDataManager, table, entryNumber, propertyChanged) { Load(); }

        protected override void Load()
        {
            stratumNumber = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 0);
            unknown1 = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 4);
            encounterNumber1 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 8);
            encounterProbability1 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 10);
            encounterNumber2 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 12);
            encounterProbability2 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 14);
            encounterNumber3 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 16);
            encounterProbability3 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 18);

            base.Load();
        }

        public override void Save()
        {
            stratumNumber.CopyTo(ParentTable.Data[EntryNumber], 0);
            unknown1.CopyTo(ParentTable.Data[EntryNumber], 4);
            encounterNumber1.CopyTo(ParentTable.Data[EntryNumber], 8);
            encounterProbability1.CopyTo(ParentTable.Data[EntryNumber], 10);
            encounterNumber2.CopyTo(ParentTable.Data[EntryNumber], 12);
            encounterProbability2.CopyTo(ParentTable.Data[EntryNumber], 14);
            encounterNumber3.CopyTo(ParentTable.Data[EntryNumber], 16);
            encounterProbability3.CopyTo(ParentTable.Data[EntryNumber], 18);

            base.Save();
        }

        public static TreeNode GenerateTreeNode(GameDataManager gameDataManager, IList<BaseParser> parsedData)
        {
            string description = (typeof(EncounterGroupParser).GetCustomAttributes(false).FirstOrDefault(x => x is DescriptionAttribute) as DescriptionAttribute).Description;
            TreeNode node = new TreeNode(description) { Tag = parsedData };

            foreach (BaseParser parsed in parsedData)
                node.Nodes.Add(new TreeNode(parsed.EntryDescription) { Tag = parsed });

            return node;
        }
    }
}
