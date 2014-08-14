﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

using Yggdrasil.FileTypes;

namespace Yggdrasil.TableParsers
{
    [ParserUsage("Item.tbb", 0)]
    [Description("Equipment")]
    [ItemNameDescriptionFiles("ItemName", 0, "ItemInfo", 0)]
    public class EquipItemData : BaseParser
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

        public override string EntryDescription { get { return (Name != string.Empty ? Name : "(Unnamed)"); } }

        ushort itemNumber;
        [Browsable(false)]
        public ushort ItemNumber
        {
            get { return itemNumber; }
            set { base.SetProperty(ref itemNumber, value, () => this.ItemNumber); }
        }

        [DisplayName("(Name)"), PrioritizedCategory("Information", 7)]
        [Description("In-game item name.")]
        public string Name { get; private set; }

        [DisplayName("(Description)"), Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor)), PrioritizedCategory("Information", 7)]
        [Description("In-game item description.")]
        public string Description { get; private set; }

        ushort itemType;
        [DisplayName("Item Type"), TypeConverter(typeof(CustomConverters.HexUshortConverter)), PrioritizedCategory("General", 6)]
        public ushort ItemType
        {
            get { return itemType; }
            set { base.SetProperty(ref itemType, value, () => this.ItemType); }
        }

        ushort attack;
        [DisplayName("Attack"), PrioritizedCategory("Stats", 5)]
        [Description("Modifier for character's attack strength.")]
        public ushort Attack
        {
            get { return attack; }
            set { base.SetProperty(ref attack, value, () => this.Attack); }
        }

        ushort attackAlt;
        [DisplayName("Attack (Alternative)"), PrioritizedCategory("Stats", 5)]
        [Description("Alternative modifier for character's attack strength; unknown if used. Should be set to same value as regular attack.")]
        public ushort AttackAlt
        {
            get { return attackAlt; }
            set { base.SetProperty(ref attackAlt, value, () => this.AttackAlt); }
        }

        ushort defense;
        [DisplayName("Defense"), PrioritizedCategory("Stats", 5)]
        [Description("Modifier for character's defense stat.")]
        public ushort Defense
        {
            get { return defense; }
            set { base.SetProperty(ref defense, value, () => this.Defense); }
        }

        Groups group;
        [DisplayName("Group"), TypeConverter(typeof(EnumConverter)), PrioritizedCategory("General", 6)]
        [Description("Type of item used for sorting, ex. when buying at Shilleka's Goods")]
        public Groups Group
        {
            get { return group; }
            set { base.SetProperty(ref group, value, () => this.Group); }
        }

        sbyte unknown1;
        [DisplayName("Unknown 1"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Unknown", 0)]
        public sbyte Unknown1
        {
            get { return unknown1; }
            set { base.SetProperty(ref unknown1, value, () => this.Unknown1); }
        }

        sbyte resistSlash;
        [DisplayName("Slash Resistance"), TypeConverter(typeof(CustomConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
        [Description("Modifier for resistance to slashing attacks in percent.")]
        public sbyte ResistSlash
        {
            get { return resistSlash; }
            set { base.SetProperty(ref resistSlash, value, () => this.ResistSlash); }
        }

        sbyte resistBlunt;
        [DisplayName("Blunt Resistance"), TypeConverter(typeof(CustomConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
        [Description("Modifier for resistance to blunt attacks in percent.")]
        public sbyte ResistBlunt
        {
            get { return resistBlunt; }
            set { base.SetProperty(ref resistBlunt, value, () => this.ResistBlunt); }
        }

        sbyte resistPierce;
        [DisplayName("Pierce Resistance"), TypeConverter(typeof(CustomConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
        [Description("Modifier for resistance to piercing attacks in percent.")]
        public sbyte ResistPierce
        {
            get { return resistPierce; }
            set { base.SetProperty(ref resistPierce, value, () => this.ResistPierce); }
        }

        sbyte resistFire;
        [DisplayName("Fire Resistance"), TypeConverter(typeof(CustomConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
        [Description("Modifier for resistance to fire elemental attacks in percent.")]
        public sbyte ResistFire
        {
            get { return resistFire; }
            set { base.SetProperty(ref resistFire, value, () => this.ResistFire); }
        }

        sbyte resistIce;
        [DisplayName("Ice Resistance"), TypeConverter(typeof(CustomConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
        [Description("Modifier for resistance to ice elemental attacks in percent.")]
        public sbyte ResistIce
        {
            get { return resistIce; }
            set { base.SetProperty(ref resistIce, value, () => this.ResistIce); }
        }

        sbyte resistVolt;
        [DisplayName("Volt Resistance"), TypeConverter(typeof(CustomConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
        [Description("Modifier for resistance to electrical elemental attacks in percent.")]
        public sbyte ResistVolt
        {
            get { return resistVolt; }
            set { base.SetProperty(ref resistVolt, value, () => this.ResistVolt); }
        }

        sbyte resistDeath;
        [DisplayName("OHKO Resistance"), TypeConverter(typeof(CustomConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
        [Description("Modifier for resistance to instant death attacks in percent.")]
        public sbyte ResistDeath
        {
            get { return resistDeath; }
            set { base.SetProperty(ref resistDeath, value, () => this.ResistDeath); }
        }

        sbyte resistAilment;
        [DisplayName("Ailment Resistance"), TypeConverter(typeof(CustomConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
        [Description("Modifier for resistance to ailments in percent.")]
        public sbyte ResistAilment
        {
            get { return resistAilment; }
            set { base.SetProperty(ref resistAilment, value, () => this.ResistAilment); }
        }

        sbyte resistHeadBind;
        [DisplayName("Head Bind Resistance"), TypeConverter(typeof(CustomConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
        [Description("Modifier for resistance to head binding attacks in percent.")]
        public sbyte ResistHeadBind
        {
            get { return resistHeadBind; }
            set { base.SetProperty(ref resistHeadBind, value, () => this.ResistHeadBind); }
        }

        sbyte resistArmBind;
        [DisplayName("Arm Bind Resistance"), TypeConverter(typeof(CustomConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
        [Description("Modifier for resistance to arm binding attacks in percent.")]
        public sbyte ResistArmBind
        {
            get { return resistArmBind; }
            set { base.SetProperty(ref resistArmBind, value, () => this.ResistArmBind); }
        }

        sbyte resistLegBind;
        [DisplayName("Leg Bind Resistance"), TypeConverter(typeof(CustomConverters.SbytePercentageConverter)), PrioritizedCategory("Resistances", 2)]
        [Description("Modifier for resistance to leg binding attacks in percent.")]
        public sbyte ResistLegBind
        {
            get { return resistLegBind; }
            set { base.SetProperty(ref resistLegBind, value, () => this.ResistLegBind); }
        }

        sbyte strModifier;
        [DisplayName("STR Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
        [Description("Modifier to character's strength stat.")]
        public sbyte StrModifier
        {
            get { return strModifier; }
            set { base.SetProperty(ref strModifier, value, () => this.StrModifier); }
        }

        sbyte vitModifier;
        [DisplayName("VIT Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
        [Description("Modifier to character's vitality stat.")]
        public sbyte VitModifier
        {
            get { return vitModifier; }
            set { base.SetProperty(ref vitModifier, value, () => this.VitModifier); }
        }

        sbyte agiModifier;
        [DisplayName("AGI Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
        [Description("Modifier to character's agility stat.")]
        public sbyte AgiModifier
        {
            get { return agiModifier; }
            set { base.SetProperty(ref agiModifier, value, () => this.AgiModifier); }
        }

        sbyte lucModifier;
        [DisplayName("LUC Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
        [Description("Modifier to character's luck stat.")]
        public sbyte LucModifier
        {
            get { return lucModifier; }
            set { base.SetProperty(ref lucModifier, value, () => this.LucModifier); }
        }

        sbyte tecModifier;
        [DisplayName("TEC Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
        [Description("Modifier to character's technical stat.")]
        public sbyte TecModifier
        {
            get { return tecModifier; }
            set { base.SetProperty(ref tecModifier, value, () => this.TecModifier); }
        }

        sbyte hpModifier;
        [DisplayName("HP Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
        [Description("Modifier to character's hit points.")]
        public sbyte HPModifier
        {
            get { return hpModifier; }
            set { base.SetProperty(ref hpModifier, value, () => this.HPModifier); }
        }

        sbyte tpModifier;
        [DisplayName("TP Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
        [Description("Modifier to character's technical points.")]
        public sbyte TPModifier
        {
            get { return tpModifier; }
            set { base.SetProperty(ref tpModifier, value, () => this.TPModifier); }
        }

        sbyte boostModifier;
        [DisplayName("Boost Modifier"), TypeConverter(typeof(SByteConverter)), PrioritizedCategory("Modifiers", 3)]
        [Description("Modifier to character's boost rate.")]
        public sbyte BoostModifier
        {
            get { return boostModifier; }
            set { base.SetProperty(ref boostModifier, value, () => this.BoostModifier); }
        }

        byte unknown2;
        [DisplayName("Unknown 2"), TypeConverter(typeof(CustomConverters.HexByteConverter)), PrioritizedCategory("Unknown", 0)]
        public byte Unknown2
        {
            get { return unknown2; }
            set { base.SetProperty(ref unknown2, value, () => this.Unknown2); }
        }

        uint buyPrice;
        [DisplayName("Buy Price"), TypeConverter(typeof(CustomConverters.EtrianEnConverter)), PrioritizedCategory("Cost", 4)]
        [Description("Price when buying item from Shilleka's Goods or Ceft Apothecary.")]
        public uint BuyPrice
        {
            get { return buyPrice; }
            set { base.SetProperty(ref buyPrice, value, () => this.BuyPrice); }
        }

        uint sellPrice;
        [DisplayName("Sell Price"), TypeConverter(typeof(CustomConverters.EtrianEnConverter)), PrioritizedCategory("Cost", 4)]
        [Description("Return when selling item to Shilleka's Goods.")]
        public uint SellPrice
        {
            get { return sellPrice; }
            set { base.SetProperty(ref sellPrice, value, () => this.SellPrice); }
        }

        ushort classUsability;
        [Browsable(false)]
        public ushort ClassUsability
        {
            get { return classUsability; }
            set { base.SetProperty(ref classUsability, value, () => this.ClassUsability); }
        }

        [DisplayName("Landsknecht"), PrioritizedCategory("Class Usability", 1)]
        [Description("Determines if item can be equipped by Landsknecht.")]
        public bool UseableByLandsknecht
        {
            get { return (classUsability & (ushort)UsabilityMask.Landsknecht) != 0; }
            set { base.SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Landsknecht) | (value ? (ushort)UsabilityMask.Landsknecht : 0)), () => this.ClassUsability); }
        }

        [DisplayName("Survivalist"), PrioritizedCategory("Class Usability", 1)]
        [Description("Determines if item can be equipped by Survivalist.")]
        public bool UseableBySurvivalist
        {
            get { return (classUsability & (ushort)UsabilityMask.Survivalist) != 0; }
            set { base.SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Survivalist) | (value ? (ushort)UsabilityMask.Survivalist : 0)), () => this.ClassUsability); }
        }

        [DisplayName("Protector"), PrioritizedCategory("Class Usability", 1)]
        [Description("Determines if item can be equipped by Protector.")]
        public bool UseableByProtector
        {
            get { return (classUsability & (ushort)UsabilityMask.Protector) != 0; }
            set { base.SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Protector) | (value ? (ushort)UsabilityMask.Protector : 0)), () => this.ClassUsability); }
        }

        [DisplayName("Dark Hunter"), PrioritizedCategory("Class Usability", 1)]
        [Description("Determines if item can be equipped by Dark Hunter.")]
        public bool UseableByDarkHunter
        {
            get { return (classUsability & (ushort)UsabilityMask.DarkHunter) != 0; }
            set { base.SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.DarkHunter) | (value ? (ushort)UsabilityMask.DarkHunter : 0)), () => this.ClassUsability); }
        }

        [DisplayName("Medic"), PrioritizedCategory("Class Usability", 1)]
        [Description("Determines if item can be equipped by Medic.")]
        public bool UseableByMedic
        {
            get { return (classUsability & (ushort)UsabilityMask.Medic) != 0; }
            set { base.SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Medic) | (value ? (ushort)UsabilityMask.Medic : 0)), () => this.ClassUsability); }
        }

        [DisplayName("Alchemist"), PrioritizedCategory("Class Usability", 1)]
        [Description("Determines if item can be equipped by Alchemist.")]
        public bool UseableByAlchemist
        {
            get { return (classUsability & (ushort)UsabilityMask.Alchemist) != 0; }
            set { base.SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Alchemist) | (value ? (ushort)UsabilityMask.Alchemist : 0)), () => this.ClassUsability); }
        }

        [DisplayName("Bard"), PrioritizedCategory("Class Usability", 1)]
        [Description("Determines if item can be equipped by Bard.")]
        public bool UseableByBard
        {
            get { return (classUsability & (ushort)UsabilityMask.Bard) != 0; }
            set { base.SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Bard) | (value ? (ushort)UsabilityMask.Bard : 0)), () => this.ClassUsability); }
        }

        [DisplayName("Samurai"), PrioritizedCategory("Class Usability", 1)]
        [Description("Determines if item can be equipped by Samurai.")]
        public bool UseableBySamurai
        {
            get { return (classUsability & (ushort)UsabilityMask.Samurai) != 0; }
            set { base.SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Samurai) | (value ? (ushort)UsabilityMask.Samurai : 0)), () => this.ClassUsability); }
        }

        [DisplayName("Hexcaster"), PrioritizedCategory("Class Usability", 1)]
        [Description("Determines if item can be equipped by Hexcaster.")]
        public bool UseableByHexcaster
        {
            get { return (classUsability & (ushort)UsabilityMask.Hexcaster) != 0; }
            set { base.SetProperty(ref classUsability, (ushort)((ClassUsability & ~(ushort)UsabilityMask.Hexcaster) | (value ? (ushort)UsabilityMask.Hexcaster : 0)), () => this.ClassUsability); }
        }

        ushort unknown3;
        [DisplayName("Unknown 3"), TypeConverter(typeof(CustomConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
        public ushort Unknown3
        {
            get { return unknown3; }
            set { base.SetProperty(ref unknown3, value, () => this.Unknown3); }
        }

        [DisplayName("Unknown 4"), TypeConverter(typeof(CustomConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 0)]
        public ushort Unknown4
        {
            get { return (ushort)(classUsability & ~(ushort)UsabilityMask.All); }
            set { ClassUsability = (ushort)((ClassUsability & (ushort)UsabilityMask.All) | value); }
        }

        public EquipItemData(GameDataManager game, TBB.TBL1 table, int entryNumber, PropertyChangedEventHandler propertyChanged = null) : base(game, table, entryNumber, propertyChanged) { Load(); }

        protected override void Load()
        {
            itemNumber = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 0);
            itemType = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 2);
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

            Name = Game.GetMessageString(this.GetAttribute<ItemNameDescriptionFiles>().NameFile, this.GetAttribute<ItemNameDescriptionFiles>().NameTableNo, itemNumber - 1);
            Description = Game.GetMessageString(this.GetAttribute<ItemNameDescriptionFiles>().DescriptionFile, this.GetAttribute<ItemNameDescriptionFiles>().DescriptionTableNo, itemNumber - 1);

            base.Load();
        }

        public override void Save()
        {
            itemNumber.CopyTo(ParentTable.Data[EntryNumber], 0);
            itemType.CopyTo(ParentTable.Data[EntryNumber], 2);
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

        public static TreeNode GenerateTreeNode(IList<BaseParser> parsedData)
        {
            string description = (typeof(EquipItemData).GetCustomAttributes(false).FirstOrDefault(x => x is DescriptionAttribute) as DescriptionAttribute).Description;
            TreeNode node = new TreeNode(description) { Tag = parsedData };

            foreach (Groups group in (Groups[])Enum.GetValues(typeof(Groups)))
            {
                TreeNode groupNode = new TreeNode(group.ToString()) { Tag = group };
                foreach (BaseParser parsed in parsedData.Where(x => (x as EquipItemData).Group == group))
                    groupNode.Nodes.Add(new TreeNode(parsed.EntryDescription.Truncate(20)) { Tag = parsed });
                node.Nodes.Add(groupNode);
            }

            return node;
        }
    }
}
