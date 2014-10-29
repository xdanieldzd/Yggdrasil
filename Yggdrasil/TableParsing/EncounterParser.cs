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
    [PrioritizedCategory("Enemies", 1)]
    [ParserDescriptor("EncountData.tbb", 0, "Enemy Encounters", 1)]
    public class EncounterParser : BaseParser
    {
        [DisplayName("(ID)"), PrioritizedCategory("Information", byte.MaxValue)]
        [Description("Internal ID number of encounter.")]
        public ushort EncounterNumber { get { return (ushort)EntryNumber; } }

        [Browsable(false)]
        public override string EntryDescription
        {
            get
            {
                var enemies = new[]
                {
                    new { Number = enemyNumber1, Name = GameDataManager.EnemyNames[enemyNumber1] },
                    new { Number = enemyNumber2, Name = GameDataManager.EnemyNames[enemyNumber2] },
                    new { Number = enemyNumber3, Name = GameDataManager.EnemyNames[enemyNumber3] },
                    new { Number = enemyNumber4, Name = GameDataManager.EnemyNames[enemyNumber4] },
                    new { Number = enemyNumber5, Name = GameDataManager.EnemyNames[enemyNumber5] },
                };
                var query = enemies.Where(x => x.Number != 0).GroupBy(y => y).Select(z => new { Name = z.Key.Name, Count = z.Count() });

                if (query.Count() == 0) return "(Empty)";

                List<string> strings = new List<string>();
                foreach (var result in query) strings.Add(string.Format("{0}x {1}", result.Count, result.Name));
                return string.Join(", ", strings);
            }
        }

        uint unknown1;
        [DisplayName("Unknown 1"), TypeConverter(typeof(TypeConverters.HexUintConverter)), PrioritizedCategory("Unknown", 0)]
        [Description("Unknown, never higher than 5; possibly related to number of stratum encounter appears in?")]
        public uint Unknown1
        {
            get { return unknown1; }
            set { base.SetProperty(ref unknown1, value, () => this.Unknown1); }
        }
        public bool ShouldSerializeUnknown1() { return !(this.Unknown1 == (dynamic)base.originalValues["Unknown1"]); }
        public void ResetUnknown1() { this.Unknown1 = (dynamic)base.originalValues["Unknown1"]; }

        uint unknown2;
        [DisplayName("Unknown 2"), TypeConverter(typeof(TypeConverters.HexUintConverter)), PrioritizedCategory("Unknown", 0)]
        public uint Unknown2
        {
            get { return unknown2; }
            set { base.SetProperty(ref unknown2, value, () => this.Unknown2); }
        }
        public bool ShouldSerializeUnknown2() { return !(this.Unknown2 == (dynamic)base.originalValues["Unknown2"]); }
        public void ResetUnknown2() { this.Unknown2 = (dynamic)base.originalValues["Unknown2"]; }

        ushort enemyNumber1;
        [DisplayName("1st Enemy"), TypeConverter(typeof(TypeConverters.EnemyNameConverter)), PrioritizedCategory("Enemies", 1)]
        [Description("First enemy in encounter formation.")]
        public ushort EnemyNumber1
        {
            get { return enemyNumber1; }
            set { base.SetProperty(ref enemyNumber1, value, () => this.EnemyNumber1); }
        }
        public bool ShouldSerializeEnemyNumber1() { return !(this.EnemyNumber1 == (dynamic)base.originalValues["EnemyNumber1"]); }
        public void ResetEnemyNumber1() { this.EnemyNumber1 = (dynamic)base.originalValues["EnemyNumber1"]; }

        ushort enemyNumber2;
        [DisplayName("2nd Enemy"), TypeConverter(typeof(TypeConverters.EnemyNameConverter)), PrioritizedCategory("Enemies", 1)]
        [Description("Second enemy in encounter formation.")]
        public ushort EnemyNumber2
        {
            get { return enemyNumber2; }
            set { base.SetProperty(ref enemyNumber2, value, () => this.EnemyNumber2); }
        }
        public bool ShouldSerializeEnemyNumber2() { return !(this.EnemyNumber2 == (dynamic)base.originalValues["EnemyNumber2"]); }
        public void ResetEnemyNumber2() { this.EnemyNumber2 = (dynamic)base.originalValues["EnemyNumber2"]; }

        ushort enemyNumber3;
        [DisplayName("3rd Enemy"), TypeConverter(typeof(TypeConverters.EnemyNameConverter)), PrioritizedCategory("Enemies", 1)]
        [Description("Third enemy in encounter formation.")]
        public ushort EnemyNumber3
        {
            get { return enemyNumber3; }
            set { base.SetProperty(ref enemyNumber3, value, () => this.EnemyNumber3); }
        }
        public bool ShouldSerializeEnemyNumber3() { return !(this.EnemyNumber3 == (dynamic)base.originalValues["EnemyNumber3"]); }
        public void ResetEnemyNumber3() { this.EnemyNumber3 = (dynamic)base.originalValues["EnemyNumber3"]; }

        ushort enemyNumber4;
        [DisplayName("4th Enemy"), TypeConverter(typeof(TypeConverters.EnemyNameConverter)), PrioritizedCategory("Enemies", 1)]
        [Description("Fourth enemy in encounter formation.")]
        public ushort EnemyNumber4
        {
            get { return enemyNumber4; }
            set { base.SetProperty(ref enemyNumber4, value, () => this.EnemyNumber4); }
        }
        public bool ShouldSerializeEnemyNumber4() { return !(this.EnemyNumber4 == (dynamic)base.originalValues["EnemyNumber4"]); }
        public void ResetEnemyNumber4() { this.EnemyNumber4 = (dynamic)base.originalValues["EnemyNumber4"]; }

        ushort enemyNumber5;
        [DisplayName("5th Enemy"), TypeConverter(typeof(TypeConverters.EnemyNameConverter)), PrioritizedCategory("Enemies", 1)]
        [Description("Fifth enemy in encounter formation.")]
        public ushort EnemyNumber5
        {
            get { return enemyNumber5; }
            set { base.SetProperty(ref enemyNumber5, value, () => this.EnemyNumber5); }
        }
        public bool ShouldSerializeEnemyNumber5() { return !(this.EnemyNumber5 == (dynamic)base.originalValues["EnemyNumber5"]); }
        public void ResetEnemyNumber5() { this.EnemyNumber5 = (dynamic)base.originalValues["EnemyNumber5"]; }

        ushort unknown3;
        [DisplayName("Unknown 3"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
        public ushort Unknown3
        {
            get { return unknown3; }
            set { base.SetProperty(ref unknown3, value, () => this.Unknown3); }
        }
        public bool ShouldSerializeUnknown3() { return !(this.Unknown3 == (dynamic)base.originalValues["Unknown3"]); }
        public void ResetUnknown3() { this.Unknown3 = (dynamic)base.originalValues["Unknown3"]; }

        uint unknown4;
        [DisplayName("Unknown 4"), TypeConverter(typeof(TypeConverters.HexUintConverter)), PrioritizedCategory("Unknown", 0)]
        public uint Unknown4
        {
            get { return unknown4; }
            set { base.SetProperty(ref unknown4, value, () => this.Unknown4); }
        }
        public bool ShouldSerializeUnknown4() { return !(this.Unknown4 == (dynamic)base.originalValues["Unknown4"]); }
        public void ResetUnknown4() { this.Unknown4 = (dynamic)base.originalValues["Unknown4"]; }

        public EncounterParser(GameDataManager gameDataManager, DataTable table, int entryNumber, PropertyChangedEventHandler propertyChanged = null) :
            base(gameDataManager, table, entryNumber, propertyChanged) { Load(); }

        protected override void Load()
        {
            unknown1 = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 0);
            unknown2 = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 4);
            enemyNumber1 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 8);
            enemyNumber2 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 10);
            enemyNumber3 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 12);
            enemyNumber4 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 14);
            enemyNumber5 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 16);
            unknown3 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 18);
            unknown4 = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 20);

            base.Load();
        }

        public override void Save()
        {
            unknown1.CopyTo(ParentTable.Data[EntryNumber], 0);
            unknown2.CopyTo(ParentTable.Data[EntryNumber], 4);
            enemyNumber1.CopyTo(ParentTable.Data[EntryNumber], 8);
            enemyNumber2.CopyTo(ParentTable.Data[EntryNumber], 10);
            enemyNumber3.CopyTo(ParentTable.Data[EntryNumber], 12);
            enemyNumber4.CopyTo(ParentTable.Data[EntryNumber], 14);
            enemyNumber5.CopyTo(ParentTable.Data[EntryNumber], 16);
            unknown3.CopyTo(ParentTable.Data[EntryNumber], 18);
            unknown4.CopyTo(ParentTable.Data[EntryNumber], 20);

            base.Save();
        }
    }
}
