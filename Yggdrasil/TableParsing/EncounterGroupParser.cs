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
	[ParserDescriptor("EncountData.tbb", 1, "Encounters Groups", 2)]
	public class EncounterGroupParser : BaseParser
	{
		[Browsable(false)]
		public override string EntryDescription { get { return string.Format("Group #{0}", GroupNumber); } }

		[DisplayName("(ID)"), PrioritizedCategory("Information", byte.MaxValue)]
		[Description("Internal ID number of encounter group.")]
		public ushort GroupNumber { get { return (ushort)EntryNumber; } }

		uint stratumNumber;
		[DisplayName("Background"), PrioritizedCategory("Battle Scene", 1)]
		[Description("Background graphics used by this group in battle; usually the zero-based number of stratum this group appears in.")]
		public uint StratumNumber
		{
			get { return stratumNumber; }
			set { SetProperty(ref stratumNumber, value, () => StratumNumber); }
		}
		public bool ShouldSerializeStratumNumber() { return !(StratumNumber == (dynamic)originalValues["StratumNumber"]); }
		public void ResetStratumNumber() { StratumNumber = (dynamic)originalValues["StratumNumber"]; }

		uint battleBGM;
		[DisplayName("BGM"), PrioritizedCategory("Battle Scene", 1)]
		[Description("Background music theme used by this group in battle.")]
		public uint BattleBGM
		{
			get { return battleBGM; }
			set { SetProperty(ref battleBGM, value, () => BattleBGM); }
		}
		public bool ShouldSerializeBattleBGM() { return !(BattleBGM == (dynamic)originalValues["BattleBGM"]); }
		public void ResetBattleBGM() { BattleBGM = (dynamic)originalValues["BattleBGM"]; }

		ushort encounterNumber1;
		[DisplayName("Encounter Type"), TypeConverter(typeof(TypeConverters.EncounterConverter)), PrioritizedCategory("1st Encounter", 4)]
		[Description("First possible encounter type in this group.")]
		public ushort EncounterNumber1
		{
			get { return encounterNumber1; }
			set { SetProperty(ref encounterNumber1, value, () => EncounterNumber1); }
		}
		public bool ShouldSerializeEncounterNumber1() { return !(EncounterNumber1 == (dynamic)originalValues["EncounterNumber1"]); }
		public void ResetEncounterNumber1() { EncounterNumber1 = (dynamic)originalValues["EncounterNumber1"]; }

		ushort encounterProbability1;
		[DisplayName("Frequency"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("1st Encounter", 4)]
		[Description("Appearance frequency of first encounter type in this group.")]
		public ushort EncounterProbability1
		{
			get { return encounterProbability1; }
			set { SetProperty(ref encounterProbability1, value, () => EncounterProbability1); }
		}
		public bool ShouldSerializeEncounterProbability1() { return !(EncounterProbability1 == (dynamic)originalValues["EncounterProbability1"]); }
		public void ResetEncounterProbability1() { EncounterProbability1 = (dynamic)originalValues["EncounterProbability1"]; }

		ushort encounterNumber2;
		[DisplayName("Encounter Type"), TypeConverter(typeof(TypeConverters.EncounterConverter)), PrioritizedCategory("2nd Encounter", 3)]
		[Description("Second possible encounter type in this group.")]
		public ushort EncounterNumber2
		{
			get { return encounterNumber2; }
			set { SetProperty(ref encounterNumber2, value, () => EncounterNumber2); }
		}
		public bool ShouldSerializeEncounterNumber2() { return !(EncounterNumber2 == (dynamic)originalValues["EncounterNumber2"]); }
		public void ResetEncounterNumber2() { EncounterNumber2 = (dynamic)originalValues["EncounterNumber2"]; }

		ushort encounterProbability2;
		[DisplayName("Frequency"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("2nd Encounter", 3)]
		[Description("Appearance frequency of second encounter type in this group.")]
		public ushort EncounterProbability2
		{
			get { return encounterProbability2; }
			set { SetProperty(ref encounterProbability2, value, () => EncounterProbability2); }
		}
		public bool ShouldSerializeEncounterProbability2() { return !(EncounterProbability2 == (dynamic)originalValues["EncounterProbability2"]); }
		public void ResetEncounterProbability2() { EncounterProbability2 = (dynamic)originalValues["EncounterProbability2"]; }

		ushort encounterNumber3;
		[DisplayName("Encounter Type"), TypeConverter(typeof(TypeConverters.EncounterConverter)), PrioritizedCategory("3rd Encounter", 2)]
		[Description("Third possible encounter type in this group.")]
		public ushort EncounterNumber3
		{
			get { return encounterNumber3; }
			set { SetProperty(ref encounterNumber3, value, () => EncounterNumber3); }
		}
		public bool ShouldSerializeEncounterNumber3() { return !(EncounterNumber3 == (dynamic)originalValues["EncounterNumber3"]); }
		public void ResetEncounterNumber3() { EncounterNumber3 = (dynamic)originalValues["EncounterNumber3"]; }

		ushort encounterProbability3;
		[DisplayName("Frequency"), TypeConverter(typeof(TypeConverters.UshortPercentageConverter)), PrioritizedCategory("3rd Encounter", 2)]
		[Description("Appearance frequency of third encounter type in this group.")]
		public ushort EncounterProbability3
		{
			get { return encounterProbability3; }
			set { SetProperty(ref encounterProbability3, value, () => EncounterProbability3); }
		}
		public bool ShouldSerializeEncounterProbability3() { return !(EncounterProbability3 == (dynamic)originalValues["EncounterProbability3"]); }
		public void ResetEncounterProbability3() { EncounterProbability3 = (dynamic)originalValues["EncounterProbability3"]; }

		public EncounterGroupParser(GameDataManager gameDataManager, DataTable table, int entryNumber, PropertyChangedEventHandler propertyChanged = null) :
			base(gameDataManager, table, entryNumber, propertyChanged)
		{ Load(); }

		protected override void Load()
		{
			stratumNumber = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 0);
			battleBGM = BitConverter.ToUInt32(ParentTable.Data[EntryNumber], 4);
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
			battleBGM.CopyTo(ParentTable.Data[EntryNumber], 4);
			encounterNumber1.CopyTo(ParentTable.Data[EntryNumber], 8);
			encounterProbability1.CopyTo(ParentTable.Data[EntryNumber], 10);
			encounterNumber2.CopyTo(ParentTable.Data[EntryNumber], 12);
			encounterProbability2.CopyTo(ParentTable.Data[EntryNumber], 14);
			encounterNumber3.CopyTo(ParentTable.Data[EntryNumber], 16);
			encounterProbability3.CopyTo(ParentTable.Data[EntryNumber], 18);

			base.Save();
		}
	}
}
