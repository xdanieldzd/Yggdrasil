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
            base(gameDataManager, mapDataFile, offset, coordinates, propertyChanged) { }

        public FloorTile(BaseTile originalTile, MapDataFile.TileTypes newTileType) :
            base(originalTile, newTileType) { }

        byte dangerIncrement;
        [DisplayName("Danger Increment"), PrioritizedCategory("Encounter Parameters", 2)]
        [Description("")]
        public byte DangerIncrement
        {
            get { return dangerIncrement; }
            set { base.SetProperty(ref dangerIncrement, value, () => this.DangerIncrement); }
        }
        public bool ShouldSerializeDangerIncrement() { return !(this.DangerIncrement == (dynamic)base.originalValues["DangerIncrement"]); }
        public void ResetDangerIncrement() { this.DangerIncrement = (dynamic)base.originalValues["DangerIncrement"]; }

        ushort unknown1;
        [DisplayName("Unknown 1"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 1)]
        [Description("")]
        public ushort Unknown1
        {
            get { return unknown1; }
            set { base.SetProperty(ref unknown1, value, () => this.Unknown1); }
        }
        public bool ShouldSerializeUnknown1() { return !(this.Unknown1 == (dynamic)base.originalValues["Unknown1"]); }
        public void ResetUnknown1() { this.Unknown1 = (dynamic)base.originalValues["Unknown1"]; }

        ushort unknown2;
        [DisplayName("Unknown 2"), TypeConverter(typeof(TypeConverters.HexUshortConverter)), PrioritizedCategory("Unknown", 1)]
        [Description("")]
        public ushort Unknown2
        {
            get { return unknown2; }
            set { base.SetProperty(ref unknown2, value, () => this.Unknown2); }
        }
        public bool ShouldSerializeUnknown2() { return !(this.Unknown2 == (dynamic)base.originalValues["Unknown2"]); }
        public void ResetUnknown2() { this.Unknown2 = (dynamic)base.originalValues["Unknown2"]; }

        ushort encounterGroup;
        [DisplayName("Encounter Group"), PrioritizedCategory("Encounter Parameters", 2)]
        [Description("")]
        public ushort EncounterGroup
        {
            get { return encounterGroup; }
            set { base.SetProperty(ref encounterGroup, value, () => this.EncounterGroup); }
        }
        public bool ShouldSerializeEncounterGroup() { return !(this.EncounterGroup == (dynamic)base.originalValues["EncounterGroup"]); }
        public void ResetEncounterGroup() { this.EncounterGroup = (dynamic)base.originalValues["EncounterGroup"]; }

        uint unknown3;
        [DisplayName("Unknown 3"), TypeConverter(typeof(TypeConverters.HexUintConverter)), PrioritizedCategory("Unknown", 1)]
        [Description("")]
        public uint Unknown3
        {
            get { return unknown3; }
            set { base.SetProperty(ref unknown3, value, () => this.Unknown3); }
        }
        public bool ShouldSerializeUnknown3() { return !(this.Unknown3 == (dynamic)base.originalValues["Unknown3"]); }
        public void ResetUnknown3() { this.Unknown3 = (dynamic)base.originalValues["Unknown3"]; }

        uint unknown4;
        [DisplayName("Unknown 4"), TypeConverter(typeof(TypeConverters.HexUintConverter)), PrioritizedCategory("Unknown", 1)]
        [Description("")]
        public uint Unknown4
        {
            get { return unknown4; }
            set { base.SetProperty(ref unknown4, value, () => this.Unknown4); }
        }
        public bool ShouldSerializeUnknown4() { return !(this.Unknown4 == (dynamic)base.originalValues["Unknown4"]); }
        public void ResetUnknown4() { this.Unknown4 = (dynamic)base.originalValues["Unknown4"]; }

        protected override void Load()
        {
            dangerIncrement = this.Data[1];
            unknown1 = BitConverter.ToUInt16(this.Data, 2);
            unknown2 = BitConverter.ToUInt16(this.Data, 4);
            encounterGroup = BitConverter.ToUInt16(this.Data, 6);
            unknown3 = BitConverter.ToUInt32(this.Data, 8);
            unknown4 = BitConverter.ToUInt32(this.Data, 12);

            base.Load();
        }

        public override void Save()
        {
            dangerIncrement.CopyTo(this.Data, 1);
            unknown1.CopyTo(this.Data, 2);
            unknown2.CopyTo(this.Data, 4);
            Convert.ToUInt16(encounterGroup).CopyTo(this.Data, 6);
            unknown3.CopyTo(this.Data, 8);
            unknown4.CopyTo(this.Data, 12);

            base.Save();
        }
    }
}
