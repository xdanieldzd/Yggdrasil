using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Yggdrasil.Attributes;

namespace Yggdrasil.FileHandling.MapDataHandling
{
	class FloorTile : BaseTile
	{
		public FloorTile(GameDataManager gameDataManager, MapDataFile mapDataFile, int offset, System.Drawing.Point coordinates, PropertyChangedEventHandler propertyChanged = null) :
			base(gameDataManager, mapDataFile, offset, coordinates, propertyChanged)
		{ }

		public FloorTile(BaseTile originalTile, MapDataFile.TileTypes newTileType) :
			base(originalTile, newTileType)
		{ }

		byte dangerIncrement;
		[DisplayName("Danger Increment"), PrioritizedCategory("Encounter Parameters", 2)]
		[Description("")]
		public byte DangerIncrement
		{
			get { return dangerIncrement; }
			set { SetProperty(ref dangerIncrement, value, () => DangerIncrement); }
		}
		public bool ShouldSerializeDangerIncrement() { return !(DangerIncrement == (dynamic)originalValues["DangerIncrement"]); }
		public void ResetDangerIncrement() { DangerIncrement = (dynamic)originalValues["DangerIncrement"]; }

		ushort unknown1;
		[DisplayName("Unknown 1"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 1)]
		[Description("")]
		public ushort Unknown1
		{
			get { return unknown1; }
			set { SetProperty(ref unknown1, value, () => Unknown1); }
		}
		public bool ShouldSerializeUnknown1() { return !(Unknown1 == (dynamic)originalValues["Unknown1"]); }
		public void ResetUnknown1() { Unknown1 = (dynamic)originalValues["Unknown1"]; }

		ushort unknown2;
		[DisplayName("Unknown 2"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 1)]
		[Description("")]
		public ushort Unknown2
		{
			get { return unknown2; }
			set { SetProperty(ref unknown2, value, () => Unknown2); }
		}
		public bool ShouldSerializeUnknown2() { return !(Unknown2 == (dynamic)originalValues["Unknown2"]); }
		public void ResetUnknown2() { Unknown2 = (dynamic)originalValues["Unknown2"]; }

		ushort encounterGroup;
		[DisplayName("Encounter Group"), PrioritizedCategory("Encounter Parameters", 2)]
		[Description("")]
		public ushort EncounterGroup
		{
			get { return encounterGroup; }
			set { SetProperty(ref encounterGroup, value, () => EncounterGroup); }
		}
		public bool ShouldSerializeEncounterGroup() { return !(EncounterGroup == (dynamic)originalValues["EncounterGroup"]); }
		public void ResetEncounterGroup() { EncounterGroup = (dynamic)originalValues["EncounterGroup"]; }

		uint unknown3;
		[DisplayName("Unknown 3"), TypeConverter(typeof(TypeConverters.HexUintConverter)), PrioritizedCategory("Unknown", 1)]
		[Description("")]
		public uint Unknown3
		{
			get { return unknown3; }
			set { SetProperty(ref unknown3, value, () => Unknown3); }
		}
		public bool ShouldSerializeUnknown3() { return !(Unknown3 == (dynamic)originalValues["Unknown3"]); }
		public void ResetUnknown3() { Unknown3 = (dynamic)originalValues["Unknown3"]; }

		uint unknown4;
		[DisplayName("Unknown 4"), TypeConverter(typeof(TypeConverters.HexUintConverter)), PrioritizedCategory("Unknown", 1)]
		[Description("")]
		public uint Unknown4
		{
			get { return unknown4; }
			set { SetProperty(ref unknown4, value, () => Unknown4); }
		}
		public bool ShouldSerializeUnknown4() { return !(Unknown4 == (dynamic)originalValues["Unknown4"]); }
		public void ResetUnknown4() { Unknown4 = (dynamic)originalValues["Unknown4"]; }

		protected override void Load()
		{
			dangerIncrement = Data[1];
			unknown1 = BitConverter.ToUInt16(Data, 2);
			unknown2 = BitConverter.ToUInt16(Data, 4);
			encounterGroup = BitConverter.ToUInt16(Data, 6);
			unknown3 = BitConverter.ToUInt32(Data, 8);
			unknown4 = BitConverter.ToUInt32(Data, 12);

			base.Load();
		}

		public override void Save()
		{
			dangerIncrement.CopyTo(Data, 1);
			unknown1.CopyTo(Data, 2);
			unknown2.CopyTo(Data, 4);
			Convert.ToUInt16(encounterGroup).CopyTo(Data, 6);
			unknown3.CopyTo(Data, 8);
			unknown4.CopyTo(Data, 12);

			base.Save();
		}
	}
}
