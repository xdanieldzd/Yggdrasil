using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

using Yggdrasil.FileTypes;

namespace Yggdrasil.TableParsers
{
    public abstract class BaseParser : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [Browsable(false)]
        public GameDataManager GameDataManager { get; private set; }
        [Browsable(false)]
        public TBB.TBL1 ParentTable { get; private set; }
        [Browsable(false)]
        public int EntryNumber { get; private set; }

        [Browsable(false)]
        public virtual string EntryDescription { get; private set; }

        protected Dictionary<string, object> originalValues;

        [Browsable(false)]
        public bool HasChanged { get { return this.GetType().GetProperties().Any(x => originalValues.ContainsKey(x.Name) && (dynamic)x.GetValue(this, null) != (dynamic)originalValues[x.Name]); } }

        public BaseParser(GameDataManager gameDataManager, TBB.TBL1 table, int entryNumber, PropertyChangedEventHandler propertyChanged = null)
        {
            GameDataManager = gameDataManager;
            ParentTable = table;
            EntryNumber = entryNumber;

            EntryDescription = string.Format("Entry #{0}", entryNumber);

            PropertyChanged = propertyChanged;
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
            GetOriginalValues();
        }
    }
}
