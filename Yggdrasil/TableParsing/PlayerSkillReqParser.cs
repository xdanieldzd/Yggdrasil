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
    [TreeNodeCategory("Player Skills")]
    [ParserUsage("Class2Skill.tbb", 0)]
    [PrioritizedDescription("Skill Requirements", 0)]
    public class PlayerSkillReqParser : BaseParser
    {
        string name;
        [DisplayName("(Name)"), PrioritizedCategory("Information", byte.MaxValue)]
        [Description("In-game skill name.")]
        public string Name
        {
            get { return name; }
            set { base.SetProperty(ref name, value, () => this.Name); }
        }
        public bool ShouldSerializeName() { return !(this.Name == (dynamic)base.originalValues["Name"]); }
        public void ResetName() { this.Name = (dynamic)base.originalValues["Name"]; }

        string shortDescription;
        [DisplayName("(Short Description)"), PrioritizedCategory("Information", byte.MaxValue)]
        [Description("In-game short skill description.")]
        public string ShortDescription
        {
            get { return shortDescription; }
            set { base.SetProperty(ref shortDescription, value, () => this.ShortDescription); }
        }
        public bool ShouldSerializeShortDescription() { return !(this.ShortDescription == (dynamic)base.originalValues["ShortDescription"]); }
        public void ResetShortDescription() { this.ShortDescription = (dynamic)base.originalValues["ShortDescription"]; }

        string description;
        [DisplayName("(Description)"), Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor)), PrioritizedCategory("Information", byte.MaxValue)]
        [Description("In-game skill description.")]
        public string Description
        {
            get { return description; }
            set { base.SetProperty(ref description, value, () => this.Description); }
        }
        public bool ShouldSerializeDescription() { return !(this.Description == (dynamic)base.originalValues["Description"]); }
        public void ResetDescription() { this.Description = (dynamic)base.originalValues["Description"]; }

        ushort skillNumber;
        [DisplayName("(ID)"), ReadOnly(true), PrioritizedCategory("Information", byte.MaxValue)]
        [Description("Internal ID number of skill.")]
        public ushort SkillNumber
        {
            get { return skillNumber; }
            set { base.SetProperty(ref skillNumber, value, () => this.SkillNumber); }
        }

        [Browsable(false)]
        public override string EntryDescription { get { return Name; } }

        ushort unknown1;
        [DisplayName("Unknown 1"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
        public ushort Unknown1
        {
            get { return unknown1; }
            set { base.SetProperty(ref unknown1, value, () => this.Unknown1); }
        }
        public bool ShouldSerializeUnknown1() { return !(this.Unknown1 == (dynamic)base.originalValues["Unknown1"]); }
        public void ResetUnknown1() { this.Unknown1 = (dynamic)base.originalValues["Unknown1"]; }

        ushort requiredSkill1;
        [DisplayName("Required Skill"), TypeConverter(typeof(TypeConverters.PlayerSkillNameConverter)), PrioritizedCategory("1st Requirement", 2)]
        public ushort RequiredSkill1
        {
            get { return requiredSkill1; }
            set { base.SetProperty(ref requiredSkill1, value, () => this.RequiredSkill1); }
        }
        public bool ShouldSerializeRequiredSkill1() { return !(this.RequiredSkill1 == (dynamic)base.originalValues["RequiredSkill1"]); }
        public void ResetRequiredSkill1() { this.RequiredSkill1 = (dynamic)base.originalValues["RequiredSkill1"]; }

        ushort requiredSkillLevel1;
        [DisplayName("Skill Level"), PrioritizedCategory("1st Requirement", 2)]
        public ushort RequiredSkillLevel1
        {
            get { return requiredSkillLevel1; }
            set { base.SetProperty(ref requiredSkillLevel1, value, () => this.RequiredSkillLevel1); }
        }
        public bool ShouldSerializeRequiredSkillLevel1() { return !(this.RequiredSkillLevel1 == (dynamic)base.originalValues["RequiredSkillLevel1"]); }
        public void ResetRequiredSkillLevel1() { this.RequiredSkillLevel1 = (dynamic)base.originalValues["RequiredSkillLevel1"]; }

        ushort requiredSkill2;
        [DisplayName("Required Skill"), TypeConverter(typeof(TypeConverters.PlayerSkillNameConverter)), PrioritizedCategory("2nd Requirement", 1)]
        public ushort RequiredSkill2
        {
            get { return requiredSkill2; }
            set { base.SetProperty(ref requiredSkill2, value, () => this.RequiredSkill2); }
        }
        public bool ShouldSerializeRequiredSkill2() { return !(this.RequiredSkill2 == (dynamic)base.originalValues["RequiredSkill2"]); }
        public void ResetRequiredSkill2() { this.RequiredSkill2 = (dynamic)base.originalValues["RequiredSkill2"]; }

        ushort requiredSkillLevel2;
        [DisplayName("Skill Level"), PrioritizedCategory("2nd Requirement", 1)]
        public ushort RequiredSkillLevel2
        {
            get { return requiredSkillLevel2; }
            set { base.SetProperty(ref requiredSkillLevel2, value, () => this.RequiredSkillLevel2); }
        }
        public bool ShouldSerializeRequiredSkillLevel2() { return !(this.RequiredSkillLevel2 == (dynamic)base.originalValues["RequiredSkillLevel2"]); }
        public void ResetRequiredSkillLevel2() { this.RequiredSkillLevel2 = (dynamic)base.originalValues["RequiredSkillLevel2"]; }

        public PlayerSkillReqParser(GameDataManager gameDataManager, DataTable table, int entryNumber, PropertyChangedEventHandler propertyChanged = null) :
            base(gameDataManager, table, entryNumber, propertyChanged) { Load(); }

        protected override void Load()
        {
            skillNumber = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 0);

            name = GameDataManager.PlayerSkillNames[SkillNumber];
            shortDescription = GameDataManager.PlayerSkillShortDescriptions[SkillNumber];
            description = GameDataManager.PlayerSkillDescriptions[SkillNumber];

            unknown1 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 2);
            requiredSkill1 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 4);
            requiredSkillLevel1 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 6);
            requiredSkill2 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 8);
            requiredSkillLevel2 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 10);

            base.Load();
        }

        public override void Save()
        {
            skillNumber.CopyTo(ParentTable.Data[EntryNumber], 0);

            unknown1.CopyTo(ParentTable.Data[EntryNumber], 2);
            requiredSkill1.CopyTo(ParentTable.Data[EntryNumber], 4);
            requiredSkillLevel1.CopyTo(ParentTable.Data[EntryNumber], 6);
            requiredSkill2.CopyTo(ParentTable.Data[EntryNumber], 8);
            requiredSkillLevel2.CopyTo(ParentTable.Data[EntryNumber], 10);

            base.Save();
        }
    }
}
