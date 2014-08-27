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
    [TreeNodeCategory("Enemies")]
    [ParserUsage("EnemyData.tbb", 0)]
    [PrioritizedDescription("Enemy Data", 0)]
    public class EnemyDataParser : BaseParser
    {
        public enum EnemyTypes : uint
        {
            None, Normal, FOE, Boss
        };

        string name;
        [DisplayName("(Name)"), PrioritizedCategory("Information", byte.MaxValue)]
        [Description("In-game enemy name.")]
        [ReadOnly(true)]
        public string Name
        {
            get
            {
                this.ChangeReadOnlyAttribute("Name", EnemyNumber <= 0);
                return name;
            }
            set { base.SetProperty(ref name, value, () => this.Name); }
        }
        public bool ShouldSerializeName() { return !(this.Name == (dynamic)base.originalValues["Name"]); }
        public void ResetName() { this.Name = (dynamic)base.originalValues["Name"]; }

        string description;
        [DisplayName("(Description)"), Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor)), PrioritizedCategory("Information", byte.MaxValue)]
        [Description("In-game enemy description.")]
        [ReadOnly(true)]
        public string Description
        {
            get
            {
                this.ChangeReadOnlyAttribute("Description", EnemyNumber <= 0);
                return description;
            }
            set { base.SetProperty(ref description, value, () => this.Description); }
        }
        public bool ShouldSerializeDescription() { return !(this.Description == (dynamic)base.originalValues["Description"]); }
        public void ResetDescription() { this.Description = (dynamic)base.originalValues["Description"]; }

        [DisplayName("(ID)"), PrioritizedCategory("Information", byte.MaxValue)]
        [Description("Internal ID number of enemy.")]
        public ushort EnemyNumber { get { return (ushort)EntryNumber; } }

        [Browsable(false)]
        public override string EntryDescription { get { return Name; } }

        uint hitPoints;
        [DisplayName("Hit Points"), PrioritizedCategory("Base Stats", 4)]
        [Description("Total hit points of enemy.")]
        public uint HitPoints
        {
            get { return hitPoints; }
            set { base.SetProperty(ref hitPoints, value, () => this.HitPoints); }
        }
        public bool ShouldSerializeHitPoints() { return !(this.HitPoints == (dynamic)base.originalValues["HitPoints"]); }
        public void ResetHitPoints() { this.HitPoints = (dynamic)base.originalValues["HitPoints"]; }

        EnemyTypes enemyType;
        [DisplayName("Enemy Type"), TypeConverter(typeof(EnumConverter)), PrioritizedCategory("Base Stats", 4)]
        [Description("Type of enemy; not sure what exactly this property influences.")]
        public EnemyTypes EnemyType
        {
            get { return enemyType; }
            set { base.SetProperty(ref enemyType, value, () => this.EnemyType); }
        }
        public bool ShouldSerializeEnemyType() { return !(this.EnemyType == (dynamic)base.originalValues["EnemyType"]); }
        public void ResetEnemyType() { this.EnemyType = (dynamic)base.originalValues["EnemyType"]; }

        ushort strPoints;
        [DisplayName("STR Points"), PrioritizedCategory("Base Stats", 4)]
        [Description("Strength of enemy.")]
        public ushort StrPoints
        {
            get { return strPoints; }
            set { base.SetProperty(ref strPoints, value, () => this.StrPoints); }
        }
        public bool ShouldSerializeStrPoints() { return !(this.StrPoints == (dynamic)base.originalValues["StrPoints"]); }
        public void ResetStrPoints() { this.StrPoints = (dynamic)base.originalValues["StrPoints"]; }

        ushort vitPoints;
        [DisplayName("VIT Points"), PrioritizedCategory("Base Stats", 4)]
        [Description("Vitality of enemy.")]
        public ushort VitPoints
        {
            get { return vitPoints; }
            set { base.SetProperty(ref vitPoints, value, () => this.VitPoints); }
        }
        public bool ShouldSerializeVitPoints() { return !(this.VitPoints == (dynamic)base.originalValues["VitPoints"]); }
        public void ResetVitPoints() { this.VitPoints = (dynamic)base.originalValues["VitPoints"]; }

        ushort agiPoints;
        [DisplayName("AGI Points"), PrioritizedCategory("Base Stats", 4)]
        [Description("Agility of enemy.")]
        public ushort AgiPoints
        {
            get { return agiPoints; }
            set { base.SetProperty(ref agiPoints, value, () => this.AgiPoints); }
        }
        public bool ShouldSerializeAgiPoints() { return !(this.AgiPoints == (dynamic)base.originalValues["AgiPoints"]); }
        public void ResetAgiPoints() { this.AgiPoints = (dynamic)base.originalValues["AgiPoints"]; }

        ushort lucPoints;
        [DisplayName("LUC Points"), PrioritizedCategory("Base Stats", 4)]
        [Description("Luck of enemy.")]
        public ushort LucPoints
        {
            get { return lucPoints; }
            set { base.SetProperty(ref lucPoints, value, () => this.LucPoints); }
        }
        public bool ShouldSerializeLucPoints() { return !(this.LucPoints == (dynamic)base.originalValues["LucPoints"]); }
        public void ResetLucPoints() { this.LucPoints = (dynamic)base.originalValues["LucPoints"]; }

        ushort tecPoints;
        [DisplayName("TEC Points"), PrioritizedCategory("Base Stats", 4)]
        [Description("Technical points of enemy.")]
        public ushort TecPoints
        {
            get { return tecPoints; }
            set { base.SetProperty(ref tecPoints, value, () => this.TecPoints); }
        }
        public bool ShouldSerializeTecPoints() { return !(this.TecPoints == (dynamic)base.originalValues["TecPoints"]); }
        public void ResetTecPoints() { this.TecPoints = (dynamic)base.originalValues["TecPoints"]; }

        ushort resistSlash;
        [DisplayName("Slash Damage"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
        [Description("Effectiveness of slashing attacks on enemy.")]
        public ushort ResistSlash
        {
            get { return resistSlash; }
            set { base.SetProperty(ref resistSlash, value, () => this.ResistSlash); }
        }
        public bool ShouldSerializeResistSlash() { return !(this.ResistSlash == (dynamic)base.originalValues["ResistSlash"]); }
        public void ResetResistSlash() { this.ResistSlash = (dynamic)base.originalValues["ResistSlash"]; }

        ushort resistBlunt;
        [DisplayName("Blunt Damage"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
        [Description("Effectiveness of blunt attacks on enemy.")]
        public ushort ResistBlunt
        {
            get { return resistBlunt; }
            set { base.SetProperty(ref resistBlunt, value, () => this.ResistBlunt); }
        }
        public bool ShouldSerializeResistBlunt() { return !(this.ResistBlunt == (dynamic)base.originalValues["ResistBlunt"]); }
        public void ResetResistBlunt() { this.ResistBlunt = (dynamic)base.originalValues["ResistBlunt"]; }

        ushort resistPierce;
        [DisplayName("Pierce Damage"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
        [Description("Effectiveness of piercing attacks on enemy.")]
        public ushort ResistPierce
        {
            get { return resistPierce; }
            set { base.SetProperty(ref resistPierce, value, () => this.ResistPierce); }
        }
        public bool ShouldSerializeResistPierce() { return !(this.ResistPierce == (dynamic)base.originalValues["ResistPierce"]); }
        public void ResetResistPierce() { this.ResistPierce = (dynamic)base.originalValues["ResistPierce"]; }

        ushort resistFire;
        [DisplayName("Fire Damage"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
        [Description("Effectiveness of fire elemental attacks on enemy.")]
        public ushort ResistFire
        {
            get { return resistFire; }
            set { base.SetProperty(ref resistFire, value, () => this.ResistFire); }
        }
        public bool ShouldSerializeResistFire() { return !(this.ResistFire == (dynamic)base.originalValues["ResistFire"]); }
        public void ResetResistFire() { this.ResistFire = (dynamic)base.originalValues["ResistFire"]; }

        ushort resistIce;
        [DisplayName("Ice Damage"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
        [Description("Effectiveness of ice elemental attacks on enemy.")]
        public ushort ResistIce
        {
            get { return resistIce; }
            set { base.SetProperty(ref resistIce, value, () => this.ResistIce); }
        }
        public bool ShouldSerializeResistIce() { return !(this.ResistIce == (dynamic)base.originalValues["ResistIce"]); }
        public void ResetResistIce() { this.ResistIce = (dynamic)base.originalValues["ResistIce"]; }

        ushort resistVolt;
        [DisplayName("Volt Damage"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
        [Description("Effectiveness of electrical elemental attacks on enemy.")]
        public ushort ResistVolt
        {
            get { return resistVolt; }
            set { base.SetProperty(ref resistVolt, value, () => this.ResistVolt); }
        }
        public bool ShouldSerializeResistVolt() { return !(this.ResistVolt == (dynamic)base.originalValues["ResistVolt"]); }
        public void ResetResistVolt() { this.ResistVolt = (dynamic)base.originalValues["ResistVolt"]; }

        ushort resistDeath;
        [DisplayName("OHKOs"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
        [Description("Effectiveness of instant death attacks on enemy.")]
        public ushort ResistDeath
        {
            get { return resistDeath; }
            set { base.SetProperty(ref resistDeath, value, () => this.ResistDeath); }
        }
        public bool ShouldSerializeResistDeath() { return !(this.ResistDeath == (dynamic)base.originalValues["ResistDeath"]); }
        public void ResetResistDeath() { this.ResistDeath = (dynamic)base.originalValues["ResistDeath"]; }

        ushort resistAilment;
        [DisplayName("Ailments"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
        [Description("Effectiveness of ailment-inducing attacks on enemy.")]
        public ushort ResistAilment
        {
            get { return resistAilment; }
            set { base.SetProperty(ref resistAilment, value, () => this.ResistAilment); }
        }
        public bool ShouldSerializeResistAilment() { return !(this.ResistAilment == (dynamic)base.originalValues["ResistAilment"]); }
        public void ResetResistAilment() { this.ResistAilment = (dynamic)base.originalValues["ResistAilment"]; }

        ushort resistHeadBind;
        [DisplayName("Head Binds"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
        [Description("Effectiveness of head bind-inducing on enemy.")]
        public ushort ResistHeadBind
        {
            get { return resistHeadBind; }
            set { base.SetProperty(ref resistHeadBind, value, () => this.ResistHeadBind); }
        }
        public bool ShouldSerializeResistHeadBind() { return !(this.ResistHeadBind == (dynamic)base.originalValues["ResistHeadBind"]); }
        public void ResetResistHeadBind() { this.ResistHeadBind = (dynamic)base.originalValues["ResistHeadBind"]; }

        ushort resistArmBind;
        [DisplayName("Arm Binds"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
        [Description("Effectiveness of arm bind-inducing on enemy.")]
        public ushort ResistArmBind
        {
            get { return resistArmBind; }
            set { base.SetProperty(ref resistArmBind, value, () => this.ResistArmBind); }
        }
        public bool ShouldSerializeResistArmBind() { return !(this.ResistArmBind == (dynamic)base.originalValues["ResistArmBind"]); }
        public void ResetResistArmBind() { this.ResistArmBind = (dynamic)base.originalValues["ResistArmBind"]; }

        ushort resistLegBind;
        [DisplayName("Leg Binds"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
        [Description("Effectiveness of leg bind-inducing on enemy.")]
        public ushort ResistLegBind
        {
            get { return resistLegBind; }
            set { base.SetProperty(ref resistLegBind, value, () => this.ResistLegBind); }
        }
        public bool ShouldSerializeResistLegBind() { return !(this.ResistLegBind == (dynamic)base.originalValues["ResistLegBind"]); }
        public void ResetResistLegBind() { this.ResistLegBind = (dynamic)base.originalValues["ResistLegBind"]; }

        ushort itemDrop1;
        [DisplayName("1st Item Drop"), TypeConverter(typeof(TypeConverters.ItemNameConverter)), PrioritizedCategory("Drops", 2)]
        [Description("First possible item drop from enemy.")]
        public ushort ItemDrop1
        {
            get { return itemDrop1; }
            set { base.SetProperty(ref itemDrop1, value, () => this.ItemDrop1); }
        }
        public bool ShouldSerializeItemDrop1() { return !(this.ItemDrop1 == (dynamic)base.originalValues["ItemDrop1"]); }
        public void ResetItemDrop1() { this.ItemDrop1 = (dynamic)base.originalValues["ItemDrop1"]; }

        ushort itemDrop2;
        [DisplayName("2nd Item Drop"), TypeConverter(typeof(TypeConverters.ItemNameConverter)), PrioritizedCategory("Drops", 2)]
        [Description("Second possible item drop from enemy.")]
        public ushort ItemDrop2
        {
            get { return itemDrop2; }
            set { base.SetProperty(ref itemDrop2, value, () => this.ItemDrop2); }
        }
        public bool ShouldSerializeItemDrop2() { return !(this.ItemDrop2 == (dynamic)base.originalValues["ItemDrop2"]); }
        public void ResetItemDrop2() { this.ItemDrop2 = (dynamic)base.originalValues["ItemDrop2"]; }

        ushort itemDrop3;
        [DisplayName("3rd Item Drop"), TypeConverter(typeof(TypeConverters.ItemNameConverter)), PrioritizedCategory("Drops", 2)]
        [Description("Third possible item drop from enemy.")]
        public ushort ItemDrop3
        {
            get { return itemDrop3; }
            set { base.SetProperty(ref itemDrop3, value, () => this.ItemDrop3); }
        }
        public bool ShouldSerializeItemDrop3() { return !(this.ItemDrop3 == (dynamic)base.originalValues["ItemDrop3"]); }
        public void ResetItemDrop3() { this.ItemDrop3 = (dynamic)base.originalValues["ItemDrop3"]; }

        byte itemDropRate1;
        [DisplayName("1st Drop Rate"), TypeConverter(typeof(TypeConverters.BytePercentageConverter)), PrioritizedCategory("Drops", 2)]
        [Description("Drop rate of first possible item drop from enemy.")]
        public byte ItemDropRate1
        {
            get { return itemDropRate1; }
            set { base.SetProperty(ref itemDropRate1, value, () => this.ItemDropRate1); }
        }
        public bool ShouldSerializeItemDropRate1() { return !(this.ItemDropRate1 == (dynamic)base.originalValues["ItemDropRate1"]); }
        public void ResetItemDropRate1() { this.ItemDropRate1 = (dynamic)base.originalValues["ItemDropRate1"]; }

        byte itemDropRate2;
        [DisplayName("2nd Drop Rate"), TypeConverter(typeof(TypeConverters.BytePercentageConverter)), PrioritizedCategory("Drops", 2)]
        [Description("Drop rate of second possible item drop from enemy.")]
        public byte ItemDropRate2
        {
            get { return itemDropRate2; }
            set { base.SetProperty(ref itemDropRate2, value, () => this.ItemDropRate2); }
        }
        public bool ShouldSerializeItemDropRate2() { return !(this.ItemDropRate2 == (dynamic)base.originalValues["ItemDropRate2"]); }
        public void ResetItemDropRate2() { this.ItemDropRate2 = (dynamic)base.originalValues["ItemDropRate2"]; }

        byte itemDropRate3;
        [DisplayName("3rd Drop Rate"), TypeConverter(typeof(TypeConverters.BytePercentageConverter)), PrioritizedCategory("Drops", 2)]
        [Description("Drop rate of third possible item drop from enemy.")]
        public byte ItemDropRate3
        {
            get { return itemDropRate3; }
            set { base.SetProperty(ref itemDropRate3, value, () => this.ItemDropRate3); }
        }
        public bool ShouldSerializeItemDropRate3() { return !(this.ItemDropRate3 == (dynamic)base.originalValues["ItemDropRate3"]); }
        public void ResetItemDropRate3() { this.ItemDropRate3 = (dynamic)base.originalValues["ItemDropRate3"]; }

        byte unknown1;
        [DisplayName("Unknown 1"), TypeConverter(typeof(TypeConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
        public byte Unknown1
        {
            get { return unknown1; }
            set { base.SetProperty(ref unknown1, value, () => this.Unknown1); }
        }
        public bool ShouldSerializeUnknown1() { return !(this.Unknown1 == (dynamic)base.originalValues["Unknown1"]); }
        public void ResetUnknown1() { this.Unknown1 = (dynamic)base.originalValues["Unknown1"]; }

        byte unknown2;
        [DisplayName("Unknown 2"), TypeConverter(typeof(TypeConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
        public byte Unknown2
        {
            get { return unknown2; }
            set { base.SetProperty(ref unknown2, value, () => this.Unknown2); }
        }
        public bool ShouldSerializeUnknown2() { return !(this.Unknown2 == (dynamic)base.originalValues["Unknown2"]); }
        public void ResetUnknown2() { this.Unknown2 = (dynamic)base.originalValues["Unknown2"]; }

        byte unknown3;
        [DisplayName("Unknown 3"), TypeConverter(typeof(TypeConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
        public byte Unknown3
        {
            get { return unknown3; }
            set { base.SetProperty(ref unknown3, value, () => this.Unknown3); }
        }
        public bool ShouldSerializeUnknown3() { return !(this.Unknown3 == (dynamic)base.originalValues["Unknown3"]); }
        public void ResetUnknown3() { this.Unknown3 = (dynamic)base.originalValues["Unknown3"]; }

        uint experience;
        [DisplayName("Experience"), TypeConverter(typeof(TypeConverters.ExpConverter)), PrioritizedCategory("Drops", 2)]
        [Description("Base experience gained when defeating enemy.")]
        public uint Experience
        {
            get { return experience; }
            set { base.SetProperty(ref experience, value, () => this.Experience); }
        }
        public bool ShouldSerializeExperience() { return !(this.Experience == (dynamic)base.originalValues["Experience"]); }
        public void ResetExperience() { this.Experience = (dynamic)base.originalValues["Experience"]; }

        ushort unknown4;
        [DisplayName("Unknown 4"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
        public ushort Unknown4
        {
            get { return unknown4; }
            set { base.SetProperty(ref unknown4, value, () => this.Unknown4); }
        }
        public bool ShouldSerializeUnknown4() { return !(this.Unknown4 == (dynamic)base.originalValues["Unknown4"]); }
        public void ResetUnknown4() { this.Unknown4 = (dynamic)base.originalValues["Unknown4"]; }

        string aiName;
        [DisplayName("AI Name"), TypeConverter(typeof(TypeConverters.AINameListConverter)), PrioritizedCategory("External Data", 1)]
        [Description("Name of AI script used by enemy.")]
        public string AiName
        {
            get { return aiName; }
            set { base.SetProperty(ref aiName, value, () => this.AiName); }
        }
        public bool ShouldSerializeAiName() { return !(this.AiName == (dynamic)base.originalValues["AiName"]); }
        public void ResetAiName() { this.AiName = (dynamic)base.originalValues["AiName"]; }

        string spriteName;
        [DisplayName("Sprite Name"), TypeConverter(typeof(TypeConverters.SpriteNameListConverter)), PrioritizedCategory("External Data", 1)]
        [Description("Name of sprite image used by enemy.")]
        public string SpriteName
        {
            get { return spriteName; }
            set { base.SetProperty(ref spriteName, value, () => this.SpriteName); }
        }
        public bool ShouldSerializeSpriteName() { return !(this.SpriteName == (dynamic)base.originalValues["SpriteName"]); }
        public void ResetSpriteName() { this.SpriteName = (dynamic)base.originalValues["SpriteName"]; }

        ushort unknown5;
        [DisplayName("Unknown 5"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
        public ushort Unknown5
        {
            get { return unknown5; }
            set { base.SetProperty(ref unknown5, value, () => this.Unknown5); }
        }
        public bool ShouldSerializeUnknown5() { return !(this.Unknown5 == (dynamic)base.originalValues["Unknown5"]); }
        public void ResetUnknown5() { this.Unknown5 = (dynamic)base.originalValues["Unknown5"]; }

        ushort unknown6;
        [DisplayName("Unknown 6"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
        public ushort Unknown6
        {
            get { return unknown6; }
            set { base.SetProperty(ref unknown6, value, () => this.Unknown6); }
        }
        public bool ShouldSerializeUnknown6() { return !(this.Unknown6 == (dynamic)base.originalValues["Unknown6"]); }
        public void ResetUnknown6() { this.Unknown6 = (dynamic)base.originalValues["Unknown6"]; }

        ushort unknown7;
        [DisplayName("Unknown 7"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
        public ushort Unknown7
        {
            get { return unknown7; }
            set { base.SetProperty(ref unknown7, value, () => this.Unknown7); }
        }
        public bool ShouldSerializeUnknown7() { return !(this.Unknown7 == (dynamic)base.originalValues["Unknown7"]); }
        public void ResetUnknown7() { this.Unknown7 = (dynamic)base.originalValues["Unknown7"]; }

        ushort unknown8;
        [DisplayName("Unknown 8"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
        public ushort Unknown8
        {
            get { return unknown8; }
            set { base.SetProperty(ref unknown8, value, () => this.Unknown8); }
        }
        public bool ShouldSerializeUnknown8() { return !(this.Unknown8 == (dynamic)base.originalValues["Unknown8"]); }
        public void ResetUnknown8() { this.Unknown8 = (dynamic)base.originalValues["Unknown8"]; }

        public EnemyDataParser(GameDataManager gameDataManager, DataTable table, int entryNumber, PropertyChangedEventHandler propertyChanged = null) :
            base(gameDataManager, table, entryNumber, propertyChanged) { Load(); }

        protected override void Load()
        {
            name = GameDataManager.EnemyNames[EnemyNumber];
            description = GameDataManager.EnemyDescriptions[EnemyNumber];

            hitPoints = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 0);
            enemyType = (EnemyTypes)BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 4);
            strPoints = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 8);
            vitPoints = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 10);
            agiPoints = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 12);
            lucPoints = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 14);
            tecPoints = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 16);
            resistSlash = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 18);
            resistBlunt = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 20);
            resistPierce = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 22);
            resistFire = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 24);
            resistIce = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 26);
            resistVolt = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 28);
            resistDeath = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 30);
            resistAilment = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 32);
            resistHeadBind = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 34);
            resistArmBind = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 36);
            resistLegBind = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 38);
            itemDrop1 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 40);
            itemDrop2 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 42);
            itemDrop3 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 44);
            itemDropRate1 = ParentTable.Data[EntryNumber][46];
            itemDropRate2 = ParentTable.Data[EntryNumber][47];
            itemDropRate3 = ParentTable.Data[EntryNumber][48];
            unknown1 = ParentTable.Data[EntryNumber][49];
            unknown2 = ParentTable.Data[EntryNumber][50];
            unknown3 = ParentTable.Data[EntryNumber][51];
            experience = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 52);
            unknown4 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 56);
            aiName = Encoding.ASCII.GetString(ParentTable.Data[EntryNumber], 58, 16).TrimEnd('\0');
            spriteName = Encoding.ASCII.GetString(ParentTable.Data[EntryNumber], 74, 14).TrimEnd('\0');
            unknown5 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 88);
            unknown6 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 90);
            unknown7 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 92);
            unknown8 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 94);

            base.Load();
        }

        public override void Save()
        {
            hitPoints.CopyTo(ParentTable.Data[EntryNumber], 0);
            Convert.ToUInt32(enemyType).CopyTo(ParentTable.Data[EntryNumber], 4);
            strPoints.CopyTo(ParentTable.Data[EntryNumber], 8);
            vitPoints.CopyTo(ParentTable.Data[EntryNumber], 10);
            agiPoints.CopyTo(ParentTable.Data[EntryNumber], 12);
            lucPoints.CopyTo(ParentTable.Data[EntryNumber], 14);
            tecPoints.CopyTo(ParentTable.Data[EntryNumber], 16);
            resistSlash.CopyTo(ParentTable.Data[EntryNumber], 18);
            resistBlunt.CopyTo(ParentTable.Data[EntryNumber], 20);
            resistPierce.CopyTo(ParentTable.Data[EntryNumber], 22);
            resistFire.CopyTo(ParentTable.Data[EntryNumber], 24);
            resistIce.CopyTo(ParentTable.Data[EntryNumber], 26);
            resistVolt.CopyTo(ParentTable.Data[EntryNumber], 28);
            resistDeath.CopyTo(ParentTable.Data[EntryNumber], 30);
            resistAilment.CopyTo(ParentTable.Data[EntryNumber], 32);
            resistHeadBind.CopyTo(ParentTable.Data[EntryNumber], 34);
            resistArmBind.CopyTo(ParentTable.Data[EntryNumber], 36);
            resistLegBind.CopyTo(ParentTable.Data[EntryNumber], 38);
            itemDrop1.CopyTo(ParentTable.Data[EntryNumber], 40);
            itemDrop2.CopyTo(ParentTable.Data[EntryNumber], 42);
            itemDrop3.CopyTo(ParentTable.Data[EntryNumber], 44);
            itemDropRate1.CopyTo(ParentTable.Data[EntryNumber], 46);
            itemDropRate2.CopyTo(ParentTable.Data[EntryNumber], 47);
            itemDropRate3.CopyTo(ParentTable.Data[EntryNumber], 48);
            unknown1.CopyTo(ParentTable.Data[EntryNumber], 49);
            unknown2.CopyTo(ParentTable.Data[EntryNumber], 50);
            unknown3.CopyTo(ParentTable.Data[EntryNumber], 51);
            experience.CopyTo(ParentTable.Data[EntryNumber], 52);
            unknown4.CopyTo(ParentTable.Data[EntryNumber], 56);
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(aiName.PadRight(16, '\0')), 0, ParentTable.Data[EntryNumber], 58, 16);
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(spriteName.PadRight(14, '\0')), 0, ParentTable.Data[EntryNumber], 74, 14);
            unknown5.CopyTo(ParentTable.Data[EntryNumber], 88);
            unknown6.CopyTo(ParentTable.Data[EntryNumber], 90);
            unknown7.CopyTo(ParentTable.Data[EntryNumber], 92);
            unknown8.CopyTo(ParentTable.Data[EntryNumber], 94);

            base.Save();
        }

        public static TreeNode GenerateTreeNode(GameDataManager gameDataManager, IList<BaseParser> parsedData)
        {
            string description = (typeof(EnemyDataParser).GetCustomAttributes(false).FirstOrDefault(x => x is DescriptionAttribute) as DescriptionAttribute).Description;
            TreeNode node = new TreeNode(description) { Tag = parsedData };

            foreach (BaseParser parsed in parsedData)
                node.Nodes.Add(new TreeNode(parsed.EntryDescription) { Tag = parsed });

            return node;
        }
    }
}
