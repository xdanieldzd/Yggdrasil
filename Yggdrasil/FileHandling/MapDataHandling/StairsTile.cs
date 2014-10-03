using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

using Yggdrasil.Attributes;

namespace Yggdrasil.FileHandling.MapDataHandling
{
    [Flags]
    public enum UseableFromDirection : byte
    {
        North = 0x1,
        South = 0x2,
        West = 0x4,
        East = 0x8,
        All = 0xF
    }

    public enum FacingAtDestination : byte
    {
        Invalid = 0x0,
        North = 0x1,
        South = 0x2,
        East = 0x3,
        West = 0x4
    }

    class StairsTile : BaseTile
    {
        public StairsTile(GameDataManager gameDataManager, MapDataFile mapDataFile, int offset, System.Drawing.Point coordinates, PropertyChangedEventHandler propertyChanged = null) :
            base(gameDataManager, mapDataFile, offset, coordinates, propertyChanged) { }

        public StairsTile(BaseTile originalTile, MapDataFile.TileTypes newTileType) :
            base(originalTile, newTileType) { }

        byte destinationFloor;
        [DisplayName("Floor Number"), TypeConverter(typeof(TypeConverters.FloorNumberConverter)), PrioritizedCategory("Destination Parameters", 1)]
        [Description("")]
        public byte DestinationFloor
        {
            get { return destinationFloor; }
            set { base.SetProperty(ref destinationFloor, value, () => this.DestinationFloor); }
        }
        public bool ShouldSerializeDestinationFloor() { return !(this.DestinationFloor == (dynamic)base.originalValues["DestinationFloor"]); }
        public void ResetDestinationFloor() { this.DestinationFloor = (dynamic)base.originalValues["DestinationFloor"]; }

        Point destinationCoords;
        [DisplayName("Coordinates"), PrioritizedCategory("Destination Parameters", 1)]
        [Description("")]
        public Point DestinationCoords
        {
            get { return destinationCoords; }
            set { base.SetProperty(ref destinationCoords, value, () => this.DestinationCoords); }
        }
        public bool ShouldSerializeDestinationCoords() { return !(this.DestinationCoords == (dynamic)base.originalValues["DestinationCoords"]); }
        public void ResetDestinationCoords() { this.DestinationCoords = (dynamic)base.originalValues["DestinationCoords"]; }

        UseableFromDirection useableFromDirection;
        [DisplayName("Usable From Directions"), TypeConverter(typeof(TypeConverters.FlagsEnumConverter)), PrioritizedCategory("Destination Parameters", 1)]
        [DefaultValue(typeof(UseableFromDirection), "All")]
        [Description("")]
        public UseableFromDirection UseableFromDirection
        {
            get { return useableFromDirection; }
            set { base.SetProperty(ref useableFromDirection, value, () => this.UseableFromDirection); }
        }
        public bool ShouldSerializeUseableFromDirection() { return !(this.UseableFromDirection == (dynamic)base.originalValues["UseableFromDirection"]); }
        public void ResetUseableFromDirection() { this.UseableFromDirection = (dynamic)base.originalValues["UseableFromDirection"]; }

        FacingAtDestination facingAtDestination;
        [DisplayName("Facing at Destination"), PrioritizedCategory("Destination Parameters", 1)]
        [Description("")]
        public FacingAtDestination FacingAtDestination
        {
            get { return facingAtDestination; }
            set { base.SetProperty(ref facingAtDestination, value, () => this.FacingAtDestination); }
        }
        public bool ShouldSerializeFacingAtDestination() { return !(this.FacingAtDestination == (dynamic)base.originalValues["FacingAtDestination"]); }
        public void ResetFacingAtDestination() { this.FacingAtDestination = (dynamic)base.originalValues["FacingAtDestination"]; }

        [DisplayName("(Stairs Lead Outside?)"), PrioritizedCategory("Miscellaneous", byte.MaxValue - 1)]
        [Description("")]
        public bool StairsGoOutside { get { return (destinationFloor == 0 && destinationCoords.X == 0 && destinationCoords.Y == 0 && facingAtDestination == 0); } }

        protected override void Load()
        {
            destinationFloor = this.Data[8];
            destinationCoords = new Point(this.Data[9], this.Data[10]);
            useableFromDirection = (UseableFromDirection)(this.Data[11] >> 4);
            facingAtDestination = (FacingAtDestination)(this.Data[11] & 0xF);

            this.ChangeDefaultValueAttribute("UseableFromDirection", useableFromDirection);

            base.Load();
        }

        public override void Save()
        {
            destinationFloor.CopyTo(this.Data, 8);
            destinationCoords.X.CopyTo(this.Data, 9);
            destinationCoords.Y.CopyTo(this.Data, 10);
            byte tempData = (byte)((Convert.ToByte(useableFromDirection) << 4) | (Convert.ToByte(facingAtDestination) & 0xF));
            tempData.CopyTo(this.Data, 11);

            this.ChangeDefaultValueAttribute("UseableFromDirection", useableFromDirection);

            base.Save();
        }
    }
}
