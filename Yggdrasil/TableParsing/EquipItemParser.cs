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
	[PrioritizedCategory("Items", 0)]
	[ParserDescriptor("Item.tbb", 0, "Equipment", 0)]
	public class EquipItemParser : BaseItemParser
	{
		public enum Groups : byte
		{
			None, Sword, Staff, Axe, Katana, Bow, Whip, Potion, Body, Shield, Headgear, Gloves, Boots, Accessory
		};

		enum UsabilityMask : ushort
		{
			Landsknecht = 0x0001,
			Survivalist = 0x0002,
			Protector = 0x0004,
			DarkHunter = 0x0008,
			Samurai = 0x0010,
			Medic = 0x0020,
			Alchemist = 0x0040,
			Bard = 0x0080,
			Hexcaster = 0x0100,
			All = 0x01FF,
		};

		[Browsable(false)]
		public override string EntryDescription { get { return (Name == string.Empty ? string.Format("(Equipment #{0})", ItemNumber) : Name.Truncate(20)); } }

		ushort itemEffects;
		[DisplayName("Item Effects"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("General", 6)]
		[Description("Appears to influence attack type and elemental affinity; undocumented.")]
		public ushort ItemEffects
		{
			get { return itemEffects; }
			set { SetProperty(ref itemEffects, value, () => ItemEffects); }
		}
		public bool ShouldSerializeItemEffects() { return !(ItemEffects == (dynamic)originalValues["ItemEffects"]); }
		public void ResetItemEffects() { ItemEffects = (dynamic)originalValues["ItemEffects"]; }

		ushort attack;
		[DisplayName("Attack"), PrioritizedCategory("Stats", 5)]
		[Description("Modifier for character's attack strength; set to zero if item is armor.")]
		public ushort Attack
		{
			get { return attack; }
			set { SetProperty(ref attack, value, () => Attack); }
		}
		public bool ShouldSerializeAttack() { return !(Attack == (dynamic)originalValues["Attack"]); }
		public void ResetAttack() { Attack = (dynamic)originalValues["Attack"]; }

		ushort attackAlt;
		[DisplayName("Attack (Alternative)"), PrioritizedCategory("Stats", 5)]
		[Description("Alternative modifier for character's attack strength; unknown if used. Should be set to same value as regular attack.")]
		public ushort AttackAlt
		{
			get { return attackAlt; }
			set { SetProperty(ref attackAlt, value, () => AttackAlt); }
		}
		public bool ShouldSerializeAttackAlt() { return !(AttackAlt == (dynamic)originalValues["AttackAlt"]); }
		public void ResetAttackAlt() { AttackAlt = (dynamic)originalValues["AttackAlt"]; }

		ushort defense;
		[DisplayName("Defense"), PrioritizedCategory("Stats", 5)]
		[Description("Modifier for character's defense stat; set to zero if item is weapon.")]
		public ushort Defense
		{
			get { return defense; }
			set { SetProperty(ref defense, value, () => Defense); }
		}
		public bool ShouldSerializeDefense() { return !(Defense == (dynamic)originalValues["Defense"]); }
		public void ResetDefense() { Defense = (dynamic)originalValues["Defense"]; }

		Groups group;
		[DisplayName("Group"), TypeConverter(typeof(EnumConverter)), PrioritizedCategory("General", 6)]
		[Description("Type of item, used ex. to determine if ATK or DEF should be applied, or for sorting when buying at Shilleka's Goods.")]
		[CausesTreeRebuild(true)]
		public Groups Group
		{
			get { return group; }
			set { SetProperty(ref group, value, () => Group); }
		}
		public bool ShouldSerializeGroup() { return !(Group == (dynamic)originalValues["Group"]); }
		public void ResetGroup() { Group = (dynamic)originalValues["Group"]; }

		sbyte unknown1;
		[DisplayName("Unknown 1"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Unknown", 0)]
		public sbyte Unknown1
		{
			get { return unknown1; }
			set { SetProperty(ref unknown1, value, () => Unknown1); }
		}
		public bool ShouldSerializeUnknown1() { return !(Unknown1 == (dynamic)originalValues["Unknown1"]); }
		public void ResetUnknown1() { Unknown1 = (dynamic)originalValues["Unknown1"]; }

		sbyte resistSlash;
		[DisplayName("Slash Resistance"), TypeConverter(typeof(TypeConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
		[Description("Modifier for resistance to slashing attacks in percent.")]
		public sbyte ResistSlash
		{
			get { return resistSlash; }
			set { SetProperty(ref resistSlash, value, () => ResistSlash); }
		}
		public bool ShouldSerializeResistSlash() { return !(ResistSlash == (dynamic)originalValues["ResistSlash"]); }
		public void ResetResistSlash() { ResistSlash = (dynamic)originalValues["ResistSlash"]; }

		sbyte resistBlunt;
		[DisplayName("Blunt Resistance"), TypeConverter(typeof(TypeConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
		[Description("Modifier for resistance to blunt attacks in percent.")]
		public sbyte ResistBlunt
		{
			get { return resistBlunt; }
			set { SetProperty(ref resistBlunt, value, () => ResistBlunt); }
		}
		public bool ShouldSerializeResistBlunt() { return !(ResistBlunt == (dynamic)originalValues["ResistBlunt"]); }
		public void ResetResistBlunt() { ResistBlunt = (dynamic)originalValues["ResistBlunt"]; }

		sbyte resistPierce;
		[DisplayName("Pierce Resistance"), TypeConverter(typeof(TypeConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
		[Description("Modifier for resistance to piercing attacks in percent.")]
		public sbyte ResistPierce
		{
			get { return resistPierce; }
			set { SetProperty(ref resistPierce, value, () => ResistPierce); }
		}
		public bool ShouldSerializeResistPierce() { return !(ResistPierce == (dynamic)originalValues["ResistPierce"]); }
		public void ResetResistPierce() { ResistPierce = (dynamic)originalValues["ResistPierce"]; }

		sbyte resistFire;
		[DisplayName("Fire Resistance"), TypeConverter(typeof(TypeConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
		[Description("Modifier for resistance to fire elemental attacks in percent.")]
		public sbyte ResistFire
		{
			get { return resistFire; }
			set { SetProperty(ref resistFire, value, () => ResistFire); }
		}
		public bool ShouldSerializeResistFire() { return !(ResistFire == (dynamic)originalValues["ResistFire"]); }
		public void ResetResistFire() { ResistFire = (dynamic)originalValues["ResistFire"]; }

		sbyte resistIce;
		[DisplayName("Ice Resistance"), TypeConverter(typeof(TypeConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
		[Description("Modifier for resistance to ice elemental attacks in percent.")]
		public sbyte ResistIce
		{
			get { return resistIce; }
			set { SetProperty(ref resistIce, value, () => ResistIce); }
		}
		public bool ShouldSerializeResistIce() { return !(ResistIce == (dynamic)originalValues["ResistIce"]); }
		public void ResetResistIce() { ResistIce = (dynamic)originalValues["ResistIce"]; }

		sbyte resistVolt;
		[DisplayName("Volt Resistance"), TypeConverter(typeof(TypeConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
		[Description("Modifier for resistance to electrical elemental attacks in percent.")]
		public sbyte ResistVolt
		{
			get { return resistVolt; }
			set { SetProperty(ref resistVolt, value, () => ResistVolt); }
		}
		public bool ShouldSerializeResistVolt() { return !(ResistVolt == (dynamic)originalValues["ResistVolt"]); }
		public void ResetResistVolt() { ResistVolt = (dynamic)originalValues["ResistVolt"]; }

		sbyte resistDeath;
		[DisplayName("OHKO Resistance"), TypeConverter(typeof(TypeConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
		[Description("Modifier for resistance to instant death attacks in percent.")]
		public sbyte ResistDeath
		{
			get { return resistDeath; }
			set { SetProperty(ref resistDeath, value, () => ResistDeath); }
		}
		public bool ShouldSerializeResistDeath() { return !(ResistDeath == (dynamic)originalValues["ResistDeath"]); }
		public void ResetResistDeath() { ResistDeath = (dynamic)originalValues["ResistDeath"]; }

		sbyte resistAilment;
		[DisplayName("Ailment Resistance"), TypeConverter(typeof(TypeConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
		[Description("Modifier for resistance to ailments in percent.")]
		public sbyte ResistAilment
		{
			get { return resistAilment; }
			set { SetProperty(ref resistAilment, value, () => ResistAilment); }
		}
		public bool ShouldSerializeResistAilment() { return !(ResistAilment == (dynamic)originalValues["ResistAilment"]); }
		public void ResetResistAilment() { ResistAilment = (dynamic)originalValues["ResistAilment"]; }

		sbyte resistHeadBind;
		[DisplayName("Head Bind Resistance"), TypeConverter(typeof(TypeConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
		[Description("Modifier for resistance to head binding attacks in percent.")]
		public sbyte ResistHeadBind
		{
			get { return resistHeadBind; }
			set { SetProperty(ref resistHeadBind, value, () => ResistHeadBind); }
		}
		public bool ShouldSerializeResistHeadBind() { return !(ResistHeadBind == (dynamic)originalValues["ResistHeadBind"]); }
		public void ResetResistHeadBind() { ResistHeadBind = (dynamic)originalValues["ResistHeadBind"]; }

		sbyte resistArmBind;
		[DisplayName("Arm Bind Resistance"), TypeConverter(typeof(TypeConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
		[Description("Modifier for resistance to arm binding attacks in percent.")]
		public sbyte ResistArmBind
		{
			get { return resistArmBind; }
			set { SetProperty(ref resistArmBind, value, () => ResistArmBind); }
		}
		public bool ShouldSerializeResistArmBind() { return !(ResistArmBind == (dynamic)originalValues["ResistArmBind"]); }
		public void ResetResistArmBind() { ResistArmBind = (dynamic)originalValues["ResistArmBind"]; }

		sbyte resistLegBind;
		[DisplayName("Leg Bind Resistance"), TypeConverter(typeof(TypeConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
		[Description("Modifier for resistance to leg binding attacks in percent.")]
		public sbyte ResistLegBind
		{
			get { return resistLegBind; }
			set { SetProperty(ref resistLegBind, value, () => ResistLegBind); }
		}
		public bool ShouldSerializeResistLegBind() { return !(ResistLegBind == (dynamic)originalValues["ResistLegBind"]); }
		public void ResetResistLegBind() { ResistLegBind = (dynamic)originalValues["ResistLegBind"]; }

		sbyte strModifier;
		[DisplayName("STR Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
		[Description("Modifier to character's strength stat.")]
		public sbyte StrModifier
		{
			get { return strModifier; }
			set { SetProperty(ref strModifier, value, () => StrModifier); }
		}
		public bool ShouldSerializeStrModifier() { return !(StrModifier == (dynamic)originalValues["StrModifier"]); }
		public void ResetStrModifier() { StrModifier = (dynamic)originalValues["StrModifier"]; }

		sbyte vitModifier;
		[DisplayName("VIT Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
		[Description("Modifier to character's vitality stat.")]
		public sbyte VitModifier
		{
			get { return vitModifier; }
			set { SetProperty(ref vitModifier, value, () => VitModifier); }
		}
		public bool ShouldSerializeVitModifier() { return !(VitModifier == (dynamic)originalValues["VitModifier"]); }
		public void ResetVitModifier() { VitModifier = (dynamic)originalValues["VitModifier"]; }

		sbyte agiModifier;
		[DisplayName("AGI Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
		[Description("Modifier to character's agility stat.")]
		public sbyte AgiModifier
		{
			get { return agiModifier; }
			set { SetProperty(ref agiModifier, value, () => AgiModifier); }
		}
		public bool ShouldSerializeAgiModifier() { return !(AgiModifier == (dynamic)originalValues["AgiModifier"]); }
		public void ResetAgiModifier() { AgiModifier = (dynamic)originalValues["AgiModifier"]; }

		sbyte lucModifier;
		[DisplayName("LUC Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
		[Description("Modifier to character's luck stat.")]
		public sbyte LucModifier
		{
			get { return lucModifier; }
			set { SetProperty(ref lucModifier, value, () => LucModifier); }
		}
		public bool ShouldSerializeLucModifier() { return !(LucModifier == (dynamic)originalValues["LucModifier"]); }
		public void ResetLucModifier() { LucModifier = (dynamic)originalValues["LucModifier"]; }

		sbyte tecModifier;
		[DisplayName("TEC Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
		[Description("Modifier to character's technical stat.")]
		public sbyte TecModifier
		{
			get { return tecModifier; }
			set { SetProperty(ref tecModifier, value, () => TecModifier); }
		}
		public bool ShouldSerializeTecModifier() { return !(TecModifier == (dynamic)originalValues["TecModifier"]); }
		public void ResetTecModifier() { TecModifier = (dynamic)originalValues["TecModifier"]; }

		sbyte hpModifier;
		[DisplayName("HP Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
		[Description("Modifier to character's hit points.")]
		public sbyte HPModifier
		{
			get { return hpModifier; }
			set { SetProperty(ref hpModifier, value, () => HPModifier); }
		}
		public bool ShouldSerializeHPModifier() { return !(HPModifier == (dynamic)originalValues["HPModifier"]); }
		public void ResetHPModifier() { HPModifier = (dynamic)originalValues["HPModifier"]; }

		sbyte tpModifier;
		[DisplayName("TP Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
		[Description("Modifier to character's technical points.")]
		public sbyte TPModifier
		{
			get { return tpModifier; }
			set { SetProperty(ref tpModifier, value, () => TPModifier); }
		}
		public bool ShouldSerializeTPModifier() { return !(TPModifier == (dynamic)originalValues["TPModifier"]); }
		public void ResetTPModifier() { TPModifier = (dynamic)originalValues["TPModifier"]; }

		sbyte boostModifier;
		[DisplayName("Boost Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
		[Description("Modifier to character's boost rate.")]
		public sbyte BoostModifier
		{
			get { return boostModifier; }
			set { SetProperty(ref boostModifier, value, () => BoostModifier); }
		}
		public bool ShouldSerializeBoostModifier() { return !(BoostModifier == (dynamic)originalValues["BoostModifier"]); }
		public void ResetBoostModifier() { BoostModifier = (dynamic)originalValues["BoostModifier"]; }

		byte unknown2;
		[DisplayName("Unknown 2"), TypeConverter(typeof(TypeConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
		public byte Unknown2
		{
			get { return unknown2; }
			set { SetProperty(ref unknown2, value, () => Unknown2); }
		}
		public bool ShouldSerializeUnknown2() { return !(Unknown2 == (dynamic)originalValues["Unknown2"]); }
		public void ResetUnknown2() { Unknown2 = (dynamic)originalValues["Unknown2"]; }

		uint buyPrice;
		[DisplayName("Buy Price"), TypeConverter(typeof(TypeConverters.UintEtrianEnConverter)), PrioritizedCategory("Cost", 4)]
		[Description("Price when buying item from Shilleka's Goods or Ceft Apothecary.")]
		public uint BuyPrice
		{
			get { return buyPrice; }
			set { SetProperty(ref buyPrice, value, () => BuyPrice); }
		}
		public bool ShouldSerializeBuyPrice() { return !(BuyPrice == (dynamic)originalValues["BuyPrice"]); }
		public void ResetBuyPrice() { BuyPrice = (dynamic)originalValues["BuyPrice"]; }

		uint sellPrice;
		[DisplayName("Sell Price"), TypeConverter(typeof(TypeConverters.UintEtrianEnConverter)), PrioritizedCategory("Cost", 4)]
		[Description("Return when selling item to Shilleka's Goods.")]
		public uint SellPrice
		{
			get { return sellPrice; }
			set { SetProperty(ref sellPrice, value, () => SellPrice); }
		}
		public bool ShouldSerializeSellPrice() { return !(SellPrice == (dynamic)originalValues["SellPrice"]); }
		public void ResetSellPrice() { SellPrice = (dynamic)originalValues["SellPrice"]; }

		ushort classUsability;
		[Browsable(false)]
		public ushort ClassUsability
		{
			get { return classUsability; }
			set { SetProperty(ref classUsability, value, () => ClassUsability); }
		}

		[DisplayName("Landsknecht"), PrioritizedCategory("Class Usability", 1)]
		[Description("Determines if item can be equipped by Landsknecht.")]
		public bool UseableByLandsknecht
		{
			get { return (classUsability & (ushort)UsabilityMask.Landsknecht) != 0; }
			set { SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Landsknecht) | (value ? (ushort)UsabilityMask.Landsknecht : 0)), () => ClassUsability); }
		}
		public bool ShouldSerializeUseableByLandsknecht() { return !(UseableByLandsknecht == (dynamic)originalValues["UseableByLandsknecht"]); }
		public void ResetUseableByLandsknecht() { UseableByLandsknecht = (dynamic)originalValues["UseableByLandsknecht"]; }

		[DisplayName("Survivalist"), PrioritizedCategory("Class Usability", 1)]
		[Description("Determines if item can be equipped by Survivalist.")]
		public bool UseableBySurvivalist
		{
			get { return (classUsability & (ushort)UsabilityMask.Survivalist) != 0; }
			set { SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Survivalist) | (value ? (ushort)UsabilityMask.Survivalist : 0)), () => ClassUsability); }
		}
		public bool ShouldSerializeUseableBySurvivalist() { return !(UseableBySurvivalist == (dynamic)originalValues["UseableBySurvivalist"]); }
		public void ResetUseableBySurvivalist() { UseableBySurvivalist = (dynamic)originalValues["UseableBySurvivalist"]; }

		[DisplayName("Protector"), PrioritizedCategory("Class Usability", 1)]
		[Description("Determines if item can be equipped by Protector.")]
		public bool UseableByProtector
		{
			get { return (classUsability & (ushort)UsabilityMask.Protector) != 0; }
			set { SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Protector) | (value ? (ushort)UsabilityMask.Protector : 0)), () => ClassUsability); }
		}
		public bool ShouldSerializeUseableByProtector() { return !(UseableByProtector == (dynamic)originalValues["UseableByProtector"]); }
		public void ResetUseableByProtector() { UseableByProtector = (dynamic)originalValues["UseableByProtector"]; }

		[DisplayName("Dark Hunter"), PrioritizedCategory("Class Usability", 1)]
		[Description("Determines if item can be equipped by Dark Hunter.")]
		public bool UseableByDarkHunter
		{
			get { return (classUsability & (ushort)UsabilityMask.DarkHunter) != 0; }
			set { SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.DarkHunter) | (value ? (ushort)UsabilityMask.DarkHunter : 0)), () => ClassUsability); }
		}
		public bool ShouldSerializeUseableByDarkHunter() { return !(UseableByDarkHunter == (dynamic)originalValues["UseableByDarkHunter"]); }
		public void ResetUseableByDarkHunter() { UseableByDarkHunter = (dynamic)originalValues["UseableByDarkHunter"]; }

		[DisplayName("Medic"), PrioritizedCategory("Class Usability", 1)]
		[Description("Determines if item can be equipped by Medic.")]
		public bool UseableByMedic
		{
			get { return (classUsability & (ushort)UsabilityMask.Medic) != 0; }
			set { SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Medic) | (value ? (ushort)UsabilityMask.Medic : 0)), () => ClassUsability); }
		}
		public bool ShouldSerializeUseableByMedic() { return !(UseableByMedic == (dynamic)originalValues["UseableByMedic"]); }
		public void ResetUseableByMedic() { UseableByMedic = (dynamic)originalValues["UseableByMedic"]; }

		[DisplayName("Alchemist"), PrioritizedCategory("Class Usability", 1)]
		[Description("Determines if item can be equipped by Alchemist.")]
		public bool UseableByAlchemist
		{
			get { return (classUsability & (ushort)UsabilityMask.Alchemist) != 0; }
			set { SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Alchemist) | (value ? (ushort)UsabilityMask.Alchemist : 0)), () => ClassUsability); }
		}
		public bool ShouldSerializeUseableByAlchemist() { return !(UseableByAlchemist == (dynamic)originalValues["UseableByAlchemist"]); }
		public void ResetUseableByAlchemist() { UseableByAlchemist = (dynamic)originalValues["UseableByAlchemist"]; }

		[DisplayName("Bard"), PrioritizedCategory("Class Usability", 1)]
		[Description("Determines if item can be equipped by Bard.")]
		public bool UseableByBard
		{
			get { return (classUsability & (ushort)UsabilityMask.Bard) != 0; }
			set { SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Bard) | (value ? (ushort)UsabilityMask.Bard : 0)), () => ClassUsability); }
		}
		public bool ShouldSerializeUseableByBard() { return !(UseableByBard == (dynamic)originalValues["UseableByBard"]); }
		public void ResetUseableByBard() { UseableByBard = (dynamic)originalValues["UseableByBard"]; }

		[DisplayName("Samurai"), PrioritizedCategory("Class Usability", 1)]
		[Description("Determines if item can be equipped by Samurai.")]
		public bool UseableBySamurai
		{
			get { return (classUsability & (ushort)UsabilityMask.Samurai) != 0; }
			set { SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Samurai) | (value ? (ushort)UsabilityMask.Samurai : 0)), () => ClassUsability); }
		}
		public bool ShouldSerializeUseableBySamurai() { return !(UseableBySamurai == (dynamic)originalValues["UseableBySamurai"]); }
		public void ResetUseableBySamurai() { UseableBySamurai = (dynamic)originalValues["UseableBySamurai"]; }

		[DisplayName("Hexcaster"), PrioritizedCategory("Class Usability", 1)]
		[Description("Determines if item can be equipped by Hexcaster.")]
		public bool UseableByHexcaster
		{
			get { return (classUsability & (ushort)UsabilityMask.Hexcaster) != 0; }
			set { SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Hexcaster) | (value ? (ushort)UsabilityMask.Hexcaster : 0)), () => ClassUsability); }
		}
		public bool ShouldSerializeUseableByHexcaster() { return !(UseableByHexcaster == (dynamic)originalValues["UseableByHexcaster"]); }
		public void ResetUseableByHexcaster() { UseableByHexcaster = (dynamic)originalValues["UseableByHexcaster"]; }

		ushort unknown3;
		[DisplayName("Unknown 3"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
		public ushort Unknown3
		{
			get { return unknown3; }
			set { SetProperty(ref unknown3, value, () => Unknown3); }
		}
		public bool ShouldSerializeUnknown3() { return !(Unknown3 == (dynamic)originalValues["Unknown3"]); }
		public void ResetUnknown3() { Unknown3 = (dynamic)originalValues["Unknown3"]; }

		[DisplayName("Unknown 4"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
		public ushort Unknown4
		{
			get { return (ushort)(classUsability & ~(ushort)UsabilityMask.All); }
			set { ClassUsability = (ushort)((ClassUsability & (ushort)UsabilityMask.All) | value); }
		}
		public bool ShouldSerializeUnknown4() { return !(Unknown4 == (dynamic)originalValues["Unknown4"]); }
		public void ResetUnknown4() { Unknown4 = (dynamic)originalValues["Unknown4"]; }

		public EquipItemParser(GameDataManager gameDataManager, DataTable table, int entryNumber, PropertyChangedEventHandler propertyChanged = null) :
			base(gameDataManager, table, entryNumber, propertyChanged)
		{ Load(); }

		protected override void Load()
		{
			itemEffects = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 2);
			attack = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 4);
			attackAlt = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 6);
			defense = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 8);
			group = (Groups)ParentTable.Data[EntryNumber][10];
			unknown1 = (sbyte)ParentTable.Data[EntryNumber][11];
			resistSlash = (sbyte)ParentTable.Data[EntryNumber][12];
			resistBlunt = (sbyte)ParentTable.Data[EntryNumber][13];
			resistPierce = (sbyte)ParentTable.Data[EntryNumber][14];
			resistFire = (sbyte)ParentTable.Data[EntryNumber][15];
			resistIce = (sbyte)ParentTable.Data[EntryNumber][16];
			resistVolt = (sbyte)ParentTable.Data[EntryNumber][17];
			resistDeath = (sbyte)ParentTable.Data[EntryNumber][18];
			resistAilment = (sbyte)ParentTable.Data[EntryNumber][19];
			resistHeadBind = (sbyte)ParentTable.Data[EntryNumber][20];
			resistArmBind = (sbyte)ParentTable.Data[EntryNumber][21];
			resistLegBind = (sbyte)ParentTable.Data[EntryNumber][22];
			strModifier = (sbyte)ParentTable.Data[EntryNumber][23];
			vitModifier = (sbyte)ParentTable.Data[EntryNumber][24];
			agiModifier = (sbyte)ParentTable.Data[EntryNumber][25];
			lucModifier = (sbyte)ParentTable.Data[EntryNumber][26];
			tecModifier = (sbyte)ParentTable.Data[EntryNumber][27];
			hpModifier = (sbyte)ParentTable.Data[EntryNumber][28];
			tpModifier = (sbyte)ParentTable.Data[EntryNumber][29];
			boostModifier = (sbyte)ParentTable.Data[EntryNumber][30];
			unknown2 = ParentTable.Data[EntryNumber][31];
			buyPrice = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 32);
			sellPrice = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 36);
			classUsability = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 40);
			unknown3 = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 42);

			base.Load();
		}

		public override void Save()
		{
			itemEffects.CopyTo(ParentTable.Data[EntryNumber], 2);
			attack.CopyTo(ParentTable.Data[EntryNumber], 4);
			attackAlt.CopyTo(ParentTable.Data[EntryNumber], 6);
			defense.CopyTo(ParentTable.Data[EntryNumber], 8);
			Convert.ToByte(group).CopyTo(ParentTable.Data[EntryNumber], 10);
			unknown1.CopyTo(ParentTable.Data[EntryNumber], 11);
			resistSlash.CopyTo(ParentTable.Data[EntryNumber], 12);
			resistBlunt.CopyTo(ParentTable.Data[EntryNumber], 13);
			resistPierce.CopyTo(ParentTable.Data[EntryNumber], 14);
			resistFire.CopyTo(ParentTable.Data[EntryNumber], 15);
			resistIce.CopyTo(ParentTable.Data[EntryNumber], 16);
			resistVolt.CopyTo(ParentTable.Data[EntryNumber], 17);
			resistDeath.CopyTo(ParentTable.Data[EntryNumber], 18);
			resistAilment.CopyTo(ParentTable.Data[EntryNumber], 19);
			resistHeadBind.CopyTo(ParentTable.Data[EntryNumber], 20);
			resistArmBind.CopyTo(ParentTable.Data[EntryNumber], 21);
			resistLegBind.CopyTo(ParentTable.Data[EntryNumber], 22);
			strModifier.CopyTo(ParentTable.Data[EntryNumber], 23);
			vitModifier.CopyTo(ParentTable.Data[EntryNumber], 24);
			agiModifier.CopyTo(ParentTable.Data[EntryNumber], 25);
			lucModifier.CopyTo(ParentTable.Data[EntryNumber], 26);
			tecModifier.CopyTo(ParentTable.Data[EntryNumber], 27);
			hpModifier.CopyTo(ParentTable.Data[EntryNumber], 28);
			tpModifier.CopyTo(ParentTable.Data[EntryNumber], 29);
			boostModifier.CopyTo(ParentTable.Data[EntryNumber], 30);
			unknown2.CopyTo(ParentTable.Data[EntryNumber], 31);
			buyPrice.CopyTo(ParentTable.Data[EntryNumber], 32);
			sellPrice.CopyTo(ParentTable.Data[EntryNumber], 36);
			classUsability.CopyTo(ParentTable.Data[EntryNumber], 40);
			unknown3.CopyTo(ParentTable.Data[EntryNumber], 42);

			base.Save();
		}

		public static void GenerateEquipmentNodes(TreeNode parserNode, List<BaseParser> currentParsers)
		{
			foreach (Groups group in (Groups[])Enum.GetValues(typeof(Groups)))
			{
				TreeNode groupNode = new TreeNode(group.ToString()) { Tag = group };

				foreach (BaseParser parsed in currentParsers.Where(x => (x as EquipItemParser).Group == group))
					groupNode.Nodes.Add(new TreeNode(parsed.EntryDescription) { Tag = parsed });

				parserNode.Nodes.Add(groupNode);
			}
		}
	}
}
