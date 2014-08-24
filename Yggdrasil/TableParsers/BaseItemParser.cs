using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Yggdrasil.FileTypes;

namespace Yggdrasil.TableParsers
{
    public abstract class BaseItemParser : BaseParser
    {
        [DisplayName("(Name)"), PrioritizedCategory("Information", byte.MaxValue)]
        [Description("In-game item name.")]
        public string Name { get { return GameDataManager.GetItemName(ItemNumber); } }

        [DisplayName("(Description)"), Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor)), PrioritizedCategory("Information", byte.MaxValue)]
        [Description("In-game item description.")]
        public string Description { get { return GameDataManager.GetItemDescription(ItemNumber); } }

        ushort itemNumber;
        [DisplayName("(ID)"), ReadOnly(true), PrioritizedCategory("Information", byte.MaxValue)]
        [Description("Internal ID number of item.")]
        public ushort ItemNumber
        {
            get { return itemNumber; }
            set { base.SetProperty(ref itemNumber, value, () => this.ItemNumber); }
        }

        public BaseItemParser(GameDataManager gameDataManager, TBB.TBL1 table, int entryNumber, PropertyChangedEventHandler propertyChanged = null) :
            base(gameDataManager, table, entryNumber, propertyChanged) { Load(); }

        protected override void Load()
        {
            itemNumber = BitConverter.ToUInt16(ParentTable.Data[EntryNumber], 0);

            base.Load();
        }

        public override void Save()
        {
            itemNumber.CopyTo(ParentTable.Data[EntryNumber], 0);

            base.Save();
        }
    }
}
