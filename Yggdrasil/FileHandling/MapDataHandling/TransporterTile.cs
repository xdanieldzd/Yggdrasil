using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

using Yggdrasil.Attributes;

namespace Yggdrasil.FileHandling.MapDataHandling
{
	class TransporterTile : BaseTile
	{
		public TransporterTile(GameDataManager gameDataManager, MapDataFile mapDataFile, int offset, System.Drawing.Point coordinates, PropertyChangedEventHandler propertyChanged = null) :
			base(gameDataManager, mapDataFile, offset, coordinates, propertyChanged)
		{ }

		public TransporterTile(BaseTile originalTile, MapDataFile.TileTypes newTileType) :
			base(originalTile, newTileType)
		{ }

		byte destinationFloor;
		[DisplayName("Floor Number"), TypeConverter(typeof(TypeConverters.FloorNumberConverter)), PrioritizedCategory("Destination Parameters", 1)]
		[Description("")]
		public byte DestinationFloor
		{
			get { return destinationFloor; }
			set { SetProperty(ref destinationFloor, value, () => DestinationFloor); }
		}
		public bool ShouldSerializeDestinationFloor() { return !(DestinationFloor == (dynamic)originalValues["DestinationFloor"]); }
		public void ResetDestinationFloor() { DestinationFloor = (dynamic)originalValues["DestinationFloor"]; }

		Point destinationCoords;
		[DisplayName("Coordinates"), PrioritizedCategory("Destination Parameters", 1)]
		[Description("")]
		public Point DestinationCoords
		{
			get { return destinationCoords; }
			set { SetProperty(ref destinationCoords, value, () => DestinationCoords); }
		}
		public bool ShouldSerializeDestinationCoords() { return !(DestinationCoords == (dynamic)originalValues["DestinationCoords"]); }
		public void ResetDestinationCoords() { DestinationCoords = (dynamic)originalValues["DestinationCoords"]; }

		protected override void Load()
		{
			destinationFloor = Data[8];
			destinationCoords = new Point(Data[9], Data[10]);

			base.Load();
		}

		public override void Save()
		{
			destinationFloor.CopyTo(Data, 8);
			destinationCoords.X.CopyTo(Data, 9);
			destinationCoords.Y.CopyTo(Data, 10);

			base.Save();
		}
	}
}
