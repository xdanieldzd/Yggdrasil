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
	[PrioritizedCategory("Enemies", 1)]
	[ParserDescriptor("EnemyData.tbb", 0, "Enemy Data", 0)]
	public class EnemyDataParser : BaseParser
	{
		public enum EnemyTypes : uint
		{
			Unspecified, Normal, FOE, Boss
		};

		string name;
		[DisplayName("(Name)"), PrioritizedCategory("Information", byte.MaxValue)]
		[Description("In-game enemy name.")]
		[ReadOnly(true)]
		[CausesNodeUpdate(true)]
		public string Name
		{
			get
			{
				this.ChangeReadOnlyAttribute("Name", EnemyNumber <= 0);
				return name;
			}
			set { SetProperty(ref name, value, () => Name); }
		}
		public bool ShouldSerializeName() { return !(Name == (dynamic)originalValues["Name"]); }
		public void ResetName() { Name = (dynamic)originalValues["Name"]; }

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
			set { SetProperty(ref description, value, () => this.Description); }
		}
		public bool ShouldSerializeDescription() { return !(Description == (dynamic)originalValues["Description"]); }
		public void ResetDescription() { Description = (dynamic)originalValues["Description"]; }

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
			set { SetProperty(ref hitPoints, value, () => HitPoints); }
		}
		public bool ShouldSerializeHitPoints() { return !(HitPoints == (dynamic)originalValues["HitPoints"]); }
		public void ResetHitPoints() { HitPoints = (dynamic)originalValues["HitPoints"]; }

		EnemyTypes enemyType;
		[DisplayName("Enemy Type"), TypeConverter(typeof(EnumConverter)), PrioritizedCategory("Base Stats", 4)]
		[Description("Type of enemy; not sure what exactly this property influences.")]
		[CausesTreeRebuild(true)]
		public EnemyTypes EnemyType
		{
			get { return enemyType; }
			set { SetProperty(ref enemyType, value, () => EnemyType); }
		}
		public bool ShouldSerializeEnemyType() { return !(EnemyType == (dynamic)originalValues["EnemyType"]); }
		public void ResetEnemyType() { EnemyType = (dynamic)originalValues["EnemyType"]; }

		ushort strPoints;
		[DisplayName("STR Points"), PrioritizedCategory("Base Stats", 4)]
		[Description("Strength of enemy.")]
		public ushort StrPoints
		{
			get { return strPoints; }
			set { SetProperty(ref strPoints, value, () => StrPoints); }
		}
		public bool ShouldSerializeStrPoints() { return !(StrPoints == (dynamic)originalValues["StrPoints"]); }
		public void ResetStrPoints() { StrPoints = (dynamic)originalValues["StrPoints"]; }

		ushort vitPoints;
		[DisplayName("VIT Points"), PrioritizedCategory("Base Stats", 4)]
		[Description("Vitality of enemy.")]
		public ushort VitPoints
		{
			get { return vitPoints; }
			set { SetProperty(ref vitPoints, value, () => VitPoints); }
		}
		public bool ShouldSerializeVitPoints() { return !(VitPoints == (dynamic)originalValues["VitPoints"]); }
		public void ResetVitPoints() { VitPoints = (dynamic)originalValues["VitPoints"]; }

		ushort agiPoints;
		[DisplayName("AGI Points"), PrioritizedCategory("Base Stats", 4)]
		[Description("Agility of enemy.")]
		public ushort AgiPoints
		{
			get { return agiPoints; }
			set { SetProperty(ref agiPoints, value, () => AgiPoints); }
		}
		public bool ShouldSerializeAgiPoints() { return !(AgiPoints == (dynamic)originalValues["AgiPoints"]); }
		public void ResetAgiPoints() { AgiPoints = (dynamic)originalValues["AgiPoints"]; }

		ushort lucPoints;
		[DisplayName("LUC Points"), PrioritizedCategory("Base Stats", 4)]
		[Description("Luck of enemy.")]
		public ushort LucPoints
		{
			get { return lucPoints; }
			set { SetProperty(ref lucPoints, value, () => LucPoints); }
		}
		public bool ShouldSerializeLucPoints() { return !(LucPoints == (dynamic)originalValues["LucPoints"]); }
		public void ResetLucPoints() { LucPoints = (dynamic)originalValues["LucPoints"]; }

		ushort tecPoints;
		[DisplayName("TEC Points"), PrioritizedCategory("Base Stats", 4)]
		[Description("Technical points of enemy.")]
		public ushort TecPoints
		{
			get { return tecPoints; }
			set { SetProperty(ref tecPoints, value, () => TecPoints); }
		}
		public bool ShouldSerializeTecPoints() { return !(TecPoints == (dynamic)originalValues["TecPoints"]); }
		public void ResetTecPoints() { TecPoints = (dynamic)originalValues["TecPoints"]; }

		ushort resistSlash;
		[DisplayName("Slash Damage"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
		[Description("Effectiveness of slashing attacks on enemy.")]
		public ushort ResistSlash
		{
			get { return resistSlash; }
			set { SetProperty(ref resistSlash, value, () => ResistSlash); }
		}
		public bool ShouldSerializeResistSlash() { return !(ResistSlash == (dynamic)originalValues["ResistSlash"]); }
		public void ResetResistSlash() { ResistSlash = (dynamic)originalValues["ResistSlash"]; }

		ushort resistBlunt;
		[DisplayName("Blunt Damage"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
		[Description("Effectiveness of blunt attacks on enemy.")]
		public ushort ResistBlunt
		{
			get { return resistBlunt; }
			set { SetProperty(ref resistBlunt, value, () => ResistBlunt); }
		}
		public bool ShouldSerializeResistBlunt() { return !(ResistBlunt == (dynamic)originalValues["ResistBlunt"]); }
		public void ResetResistBlunt() { ResistBlunt = (dynamic)originalValues["ResistBlunt"]; }

		ushort resistPierce;
		[DisplayName("Pierce Damage"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
		[Description("Effectiveness of piercing attacks on enemy.")]
		public ushort ResistPierce
		{
			get { return resistPierce; }
			set { SetProperty(ref resistPierce, value, () => ResistPierce); }
		}
		public bool ShouldSerializeResistPierce() { return !(ResistPierce == (dynamic)originalValues["ResistPierce"]); }
		public void ResetResistPierce() { ResistPierce = (dynamic)originalValues["ResistPierce"]; }

		ushort resistFire;
		[DisplayName("Fire Damage"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
		[Description("Effectiveness of fire elemental attacks on enemy.")]
		public ushort ResistFire
		{
			get { return resistFire; }
			set { SetProperty(ref resistFire, value, () => ResistFire); }
		}
		public bool ShouldSerializeResistFire() { return !(ResistFire == (dynamic)originalValues["ResistFire"]); }
		public void ResetResistFire() { ResistFire = (dynamic)originalValues["ResistFire"]; }

		ushort resistIce;
		[DisplayName("Ice Damage"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
		[Description("Effectiveness of ice elemental attacks on enemy.")]
		public ushort ResistIce
		{
			get { return resistIce; }
			set { SetProperty(ref resistIce, value, () => ResistIce); }
		}
		public bool ShouldSerializeResistIce() { return !(ResistIce == (dynamic)originalValues["ResistIce"]); }
		public void ResetResistIce() { ResistIce = (dynamic)originalValues["ResistIce"]; }

		ushort resistVolt;
		[DisplayName("Volt Damage"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
		[Description("Effectiveness of electrical elemental attacks on enemy.")]
		public ushort ResistVolt
		{
			get { return resistVolt; }
			set { SetProperty(ref resistVolt, value, () => ResistVolt); }
		}
		public bool ShouldSerializeResistVolt() { return !(ResistVolt == (dynamic)originalValues["ResistVolt"]); }
		public void ResetResistVolt() { ResistVolt = (dynamic)originalValues["ResistVolt"]; }

		ushort resistDeath;
		[DisplayName("OHKOs"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
		[Description("Effectiveness of instant death attacks on enemy.")]
		public ushort ResistDeath
		{
			get { return resistDeath; }
			set { SetProperty(ref resistDeath, value, () => ResistDeath); }
		}
		public bool ShouldSerializeResistDeath() { return !(ResistDeath == (dynamic)originalValues["ResistDeath"]); }
		public void ResetResistDeath() { ResistDeath = (dynamic)originalValues["ResistDeath"]; }

		ushort resistAilment;
		[DisplayName("Ailments"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
		[Description("Effectiveness of ailment-inducing attacks on enemy.")]
		public ushort ResistAilment
		{
			get { return resistAilment; }
			set { SetProperty(ref resistAilment, value, () => ResistAilment); }
		}
		public bool ShouldSerializeResistAilment() { return !(ResistAilment == (dynamic)originalValues["ResistAilment"]); }
		public void ResetResistAilment() { ResistAilment = (dynamic)originalValues["ResistAilment"]; }

		ushort resistHeadBind;
		[DisplayName("Head Binds"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
		[Description("Effectiveness of head bind-inducing on enemy.")]
		public ushort ResistHeadBind
		{
			get { return resistHeadBind; }
			set { SetProperty(ref resistHeadBind, value, () => ResistHeadBind); }
		}
		public bool ShouldSerializeResistHeadBind() { return !(ResistHeadBind == (dynamic)originalValues["ResistHeadBind"]); }
		public void ResetResistHeadBind() { ResistHeadBind = (dynamic)originalValues["ResistHeadBind"]; }

		ushort resistArmBind;
		[DisplayName("Arm Binds"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
		[Description("Effectiveness of arm bind-inducing on enemy.")]
		public ushort ResistArmBind
		{
			get { return resistArmBind; }
			set { SetProperty(ref resistArmBind, value, () => ResistArmBind); }
		}
		public bool ShouldSerializeResistArmBind() { return !(ResistArmBind == (dynamic)originalValues["ResistArmBind"]); }
		public void ResetResistArmBind() { ResistArmBind = (dynamic)originalValues["ResistArmBind"]; }

		ushort resistLegBind;
		[DisplayName("Leg Binds"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("Damage Modifiers", 3)]
		[Description("Effectiveness of leg bind-inducing on enemy.")]
		public ushort ResistLegBind
		{
			get { return resistLegBind; }
			set { SetProperty(ref resistLegBind, value, () => ResistLegBind); }
		}
		public bool ShouldSerializeResistLegBind() { return !(ResistLegBind == (dynamic)originalValues["ResistLegBind"]); }
		public void ResetResistLegBind() { ResistLegBind = (dynamic)originalValues["ResistLegBind"]; }

		ushort itemDrop1;
		[DisplayName("1st Item Drop"), TypeConverter(typeof(TypeConverters.ItemNameConverter)), PrioritizedCategory("Drops", 2)]
		[Description("First possible item drop from enemy.")]
		public ushort ItemDrop1
		{
			get { return itemDrop1; }
			set { SetProperty(ref itemDrop1, value, () => ItemDrop1); }
		}
		public bool ShouldSerializeItemDrop1() { return !(ItemDrop1 == (dynamic)originalValues["ItemDrop1"]); }
		public void ResetItemDrop1() { ItemDrop1 = (dynamic)originalValues["ItemDrop1"]; }

		ushort itemDrop2;
		[DisplayName("2nd Item Drop"), TypeConverter(typeof(TypeConverters.ItemNameConverter)), PrioritizedCategory("Drops", 2)]
		[Description("Second possible item drop from enemy.")]
		public ushort ItemDrop2
		{
			get { return itemDrop2; }
			set { SetProperty(ref itemDrop2, value, () => ItemDrop2); }
		}
		public bool ShouldSerializeItemDrop2() { return !(ItemDrop2 == (dynamic)originalValues["ItemDrop2"]); }
		public void ResetItemDrop2() { ItemDrop2 = (dynamic)originalValues["ItemDrop2"]; }

		ushort itemDrop3;
		[DisplayName("3rd Item Drop"), TypeConverter(typeof(TypeConverters.ItemNameConverter)), PrioritizedCategory("Drops", 2)]
		[Description("Third possible item drop from enemy.")]
		public ushort ItemDrop3
		{
			get { return itemDrop3; }
			set { SetProperty(ref itemDrop3, value, () => ItemDrop3); }
		}
		public bool ShouldSerializeItemDrop3() { return !(ItemDrop3 == (dynamic)originalValues["ItemDrop3"]); }
		public void ResetItemDrop3() { ItemDrop3 = (dynamic)originalValues["ItemDrop3"]; }

		byte itemDropRate1;
		[DisplayName("1st Drop Rate"), TypeConverter(typeof(TypeConverters.BytePercentageConverter)), PrioritizedCategory("Drops", 2)]
		[Description("Drop rate of first possible item drop from enemy.")]
		public byte ItemDropRate1
		{
			get { return itemDropRate1; }
			set { SetProperty(ref itemDropRate1, value, () => ItemDropRate1); }
		}
		public bool ShouldSerializeItemDropRate1() { return !(ItemDropRate1 == (dynamic)originalValues["ItemDropRate1"]); }
		public void ResetItemDropRate1() { ItemDropRate1 = (dynamic)originalValues["ItemDropRate1"]; }

		byte itemDropRate2;
		[DisplayName("2nd Drop Rate"), TypeConverter(typeof(TypeConverters.BytePercentageConverter)), PrioritizedCategory("Drops", 2)]
		[Description("Drop rate of second possible item drop from enemy.")]
		public byte ItemDropRate2
		{
			get { return itemDropRate2; }
			set { SetProperty(ref itemDropRate2, value, () => ItemDropRate2); }
		}
		public bool ShouldSerializeItemDropRate2() { return !(ItemDropRate2 == (dynamic)originalValues["ItemDropRate2"]); }
		public void ResetItemDropRate2() { ItemDropRate2 = (dynamic)originalValues["ItemDropRate2"]; }

		byte itemDropRate3;
		[DisplayName("3rd Drop Rate"), TypeConverter(typeof(TypeConverters.BytePercentageConverter)), PrioritizedCategory("Drops", 2)]
		[Description("Drop rate of third possible item drop from enemy.")]
		public byte ItemDropRate3
		{
			get { return itemDropRate3; }
			set { SetProperty(ref itemDropRate3, value, () => ItemDropRate3); }
		}
		public bool ShouldSerializeItemDropRate3() { return !(ItemDropRate3 == (dynamic)originalValues["ItemDropRate3"]); }
		public void ResetItemDropRate3() { ItemDropRate3 = (dynamic)originalValues["ItemDropRate3"]; }

		byte unknown1;
		[DisplayName("Unknown 1"), TypeConverter(typeof(TypeConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
		public byte Unknown1
		{
			get { return unknown1; }
			set { SetProperty(ref unknown1, value, () => Unknown1); }
		}
		public bool ShouldSerializeUnknown1() { return !(Unknown1 == (dynamic)originalValues["Unknown1"]); }
		public void ResetUnknown1() { Unknown1 = (dynamic)originalValues["Unknown1"]; }

		byte unknown2;
		[DisplayName("Unknown 2"), TypeConverter(typeof(TypeConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
		public byte Unknown2
		{
			get { return unknown2; }
			set { SetProperty(ref unknown2, value, () => Unknown2); }
		}
		public bool ShouldSerializeUnknown2() { return !(Unknown2 == (dynamic)originalValues["Unknown2"]); }
		public void ResetUnknown2() { Unknown2 = (dynamic)originalValues["Unknown2"]; }

		byte unknown3;
		[DisplayName("Unknown 3"), TypeConverter(typeof(TypeConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
		public byte Unknown3
		{
			get { return unknown3; }
			set { SetProperty(ref unknown3, value, () => Unknown3); }
		}
		public bool ShouldSerializeUnknown3() { return !(Unknown3 == (dynamic)originalValues["Unknown3"]); }
		public void ResetUnknown3() { Unknown3 = (dynamic)originalValues["Unknown3"]; }

		uint experience;
		[DisplayName("Experience"), TypeConverter(typeof(TypeConverters.ExpConverter)), PrioritizedCategory("Drops", 2)]
		[Description("Base experience gained when defeating enemy.")]
		public uint Experience
		{
			get { return experience; }
			set { SetProperty(ref experience, value, () => Experience); }
		}
		public bool ShouldSerializeExperience() { return !(Experience == (dynamic)originalValues["Experience"]); }
		public void ResetExperience() { Experience = (dynamic)originalValues["Experience"]; }

		ushort unknown4;
		[DisplayName("Unknown 4"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
		public ushort Unknown4
		{
			get { return unknown4; }
			set { SetProperty(ref unknown4, value, () => Unknown4); }
		}
		public bool ShouldSerializeUnknown4() { return !(Unknown4 == (dynamic)originalValues["Unknown4"]); }
		public void ResetUnknown4() { Unknown4 = (dynamic)originalValues["Unknown4"]; }

		string aiName;
		[DisplayName("AI Name"), TypeConverter(typeof(TypeConverters.AINameListConverter)), PrioritizedCategory("External Data", 1)]
		[Description("Name of AI script used by enemy.")]
		public string AiName
		{
			get { return aiName; }
			set { SetProperty(ref aiName, value, () => AiName); }
		}
		public bool ShouldSerializeAiName() { return !(AiName == (dynamic)originalValues["AiName"]); }
		public void ResetAiName() { AiName = (dynamic)originalValues["AiName"]; }

		string spriteName;
		[DisplayName("Sprite Name"), TypeConverter(typeof(TypeConverters.SpriteNameListConverter)), PrioritizedCategory("External Data", 1)]
		[Description("Name of sprite image used by enemy.")]
		public string SpriteName
		{
			get { return spriteName; }
			set { SetProperty(ref spriteName, value, () => SpriteName); }
		}
		public bool ShouldSerializeSpriteName() { return !(SpriteName == (dynamic)originalValues["SpriteName"]); }
		public void ResetSpriteName() { SpriteName = (dynamic)originalValues["SpriteName"]; }

		ushort unknown5;
		[DisplayName("Unknown 5"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
		public ushort Unknown5
		{
			get { return unknown5; }
			set { SetProperty(ref unknown5, value, () => Unknown5); }
		}
		public bool ShouldSerializeUnknown5() { return !(Unknown5 == (dynamic)originalValues["Unknown5"]); }
		public void ResetUnknown5() { Unknown5 = (dynamic)originalValues["Unknown5"]; }

		ushort unknown6;
		[DisplayName("Unknown 6"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
		public ushort Unknown6
		{
			get { return unknown6; }
			set { SetProperty(ref unknown6, value, () => Unknown6); }
		}
		public bool ShouldSerializeUnknown6() { return !(Unknown6 == (dynamic)originalValues["Unknown6"]); }
		public void ResetUnknown6() { Unknown6 = (dynamic)originalValues["Unknown6"]; }

		ushort unknown7;
		[DisplayName("Unknown 7"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
		public ushort Unknown7
		{
			get { return unknown7; }
			set { SetProperty(ref unknown7, value, () => Unknown7); }
		}
		public bool ShouldSerializeUnknown7() { return !(Unknown7 == (dynamic)originalValues["Unknown7"]); }
		public void ResetUnknown7() { Unknown7 = (dynamic)originalValues["Unknown7"]; }

		ushort unknown8;
		[DisplayName("Unknown 8"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
		public ushort Unknown8
		{
			get { return unknown8; }
			set { SetProperty(ref unknown8, value, () => Unknown8); }
		}
		public bool ShouldSerializeUnknown8() { return !(Unknown8 == (dynamic)originalValues["Unknown8"]); }
		public void ResetUnknown8() { Unknown8 = (dynamic)originalValues["Unknown8"]; }

		public EnemyDataParser(GameDataManager gameDataManager, DataTable table, int entryNumber, PropertyChangedEventHandler propertyChanged = null) :
			base(gameDataManager, table, entryNumber, propertyChanged)
		{ Load(); }

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

		public static void GenerateEnemyNodes(TreeNode parserNode, List<BaseParser> currentParsers)
		{
			foreach (EnemyTypes type in (EnemyTypes[])Enum.GetValues(typeof(EnemyTypes)))
			{
				TreeNode typeNode = new TreeNode(type.ToString()) { Tag = type };

				foreach (BaseParser parsed in currentParsers.Where(x => (x as EnemyDataParser).EnemyType == type))
					typeNode.Nodes.Add(new TreeNode(parsed.EntryDescription) { Tag = parsed });

				parserNode.Nodes.Add(typeNode);
			}
		}
	}
}
