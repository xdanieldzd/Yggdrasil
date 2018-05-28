using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

using Yggdrasil.Attributes;

namespace Yggdrasil.FileHandling.MapDataHandling
{
	public class BaseTile : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[Browsable(false)]
		public GameDataManager GameDataManager { get; private set; }
		[Browsable(false)]
		public MapDataFile MapDataFile { get; private set; }

		[Browsable(false)]
		public int Offset { get; private set; }
		[Browsable(false)]
		public Point Coordinates { get; private set; }

		[Browsable(false)]
		public byte[] Data { get; private set; }

		MapDataFile.TileTypes tileType;
		//[ReadOnly(true)]
		[DisplayName("(Tile Type)"), TypeConverter(typeof(TypeConverters.FriendlyEnumConverter)), PrioritizedCategory("Information", byte.MaxValue)]
		[Description("Type of selected map tile.")]
		public MapDataFile.TileTypes TileType
		{
			get { return tileType; }
			set { SetProperty(ref tileType, value, () => TileType); }
		}
		public bool ShouldSerializeTileType() { return !(TileType == (dynamic)originalValues["TileType"]); }
		public void ResetTileType() { TileType = (dynamic)originalValues["TileType"]; }

		protected Dictionary<string, object> originalValues;

		[Browsable(false)]
		public bool HasChanged { get { return GetType().GetProperties().Any(x => originalValues.ContainsKey(x.Name) && (dynamic)x.GetValue(this, null) != (dynamic)originalValues[x.Name]); } }

		public BaseTile(GameDataManager gameDataManager, MapDataFile mapDataFile, int offset, Point coordinates, PropertyChangedEventHandler propertyChanged = null)
		{
			GameDataManager = gameDataManager;
			MapDataFile = mapDataFile;
			Offset = offset;
			Coordinates = coordinates;

			PropertyChanged = propertyChanged;

			MapDataFile.Stream.Seek(Offset, SeekOrigin.Begin);
			Data = new byte[16];
			MapDataFile.Stream.Read(Data, 0, Data.Length);

			tileType = (MapDataFile.TileTypes)Data[0];

			Load();
		}

		public BaseTile(BaseTile originalTile, MapDataFile.TileTypes newTileType)
		{
			GameDataManager = originalTile.GameDataManager;
			MapDataFile = originalTile.MapDataFile;
			Offset = originalTile.Offset;
			Coordinates = originalTile.Coordinates;

			PropertyChanged = originalTile.PropertyChanged;

			Data = originalTile.Data;
			Data[0] = (byte)(tileType = newTileType);

			Load();
		}

		public void SetProperty<T>(ref T field, T value, Expression<Func<T>> member)
		{
			if (member == null) throw new NullReferenceException("member must not be null.");

			MemberExpression memberExpression = (member.Body as MemberExpression);
			if (memberExpression == null) throw new InvalidOperationException("member.Body must be a MemberExpression");

			if (!EqualityComparer<T>.Default.Equals(field, value))
			{
				field = value;

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
			}
		}

		private void GetOriginalValues()
		{
			originalValues = new Dictionary<string, object>();
			foreach (PropertyInfo prop in GetType().GetProperties().Where(x => x.CanWrite)) originalValues.Add(prop.Name, prop.GetValue(this, null));
		}

		protected virtual void Load()
		{
			GetOriginalValues();
		}

		public virtual void Save()
		{
			Convert.ToByte(tileType).CopyTo(Data, 0);

			GetOriginalValues();
		}
	}
}
