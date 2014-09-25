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
        [ReadOnly(true)]
        [DisplayName("(Tile Type)"), TypeConverter(typeof(TypeConverters.FriendlyEnumConverter)), PrioritizedCategory("Information", byte.MaxValue)]
        [Description("Type of selected map tile.")]
        public MapDataFile.TileTypes TileType
        {
            get { return tileType; }
            set { this.SetProperty(ref tileType, value, () => this.TileType); }
        }
        public bool ShouldSerializeTileType() { return !(this.TileType == (dynamic)this.originalValues["TileType"]); }
        public void ResetTileType() { this.TileType = (dynamic)this.originalValues["TileType"]; }

        protected Dictionary<string, object> originalValues;

        [Browsable(false)]
        public bool HasChanged { get { return this.GetType().GetProperties().Any(x => originalValues.ContainsKey(x.Name) && (dynamic)x.GetValue(this, null) != (dynamic)originalValues[x.Name]); } }

        public BaseTile(GameDataManager gameDataManager, MapDataFile mapDataFile, int offset, Point coordinates, PropertyChangedEventHandler propertyChanged = null)
        {
            this.GameDataManager = gameDataManager;
            this.MapDataFile = mapDataFile;
            this.Offset = offset;
            this.Coordinates = coordinates;

            this.PropertyChanged = propertyChanged;

            this.MapDataFile.Stream.Seek(this.Offset, SeekOrigin.Begin);
            this.Data = new byte[16];
            this.MapDataFile.Stream.Read(this.Data, 0, this.Data.Length);

            this.tileType = (MapDataFile.TileTypes)this.Data[0];

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

                var handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
            }
        }

        private void GetOriginalValues()
        {
            originalValues = new Dictionary<string, object>();
            foreach (PropertyInfo prop in this.GetType().GetProperties().Where(x => x.CanWrite)) originalValues.Add(prop.Name, prop.GetValue(this, null));
        }

        protected virtual void Load()
        {
            GetOriginalValues();
        }

        public virtual void Save()
        {
            Convert.ToByte(tileType).CopyTo(this.Data, 0);

            GetOriginalValues();
        }
    }
}
