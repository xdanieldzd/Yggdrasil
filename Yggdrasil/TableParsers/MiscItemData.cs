using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Yggdrasil.FileTypes;

namespace Yggdrasil.TableParsers
{
    [ParserUsage("Item.tbb", 1)]
    public class MiscItemData : BaseParser
    {
        ushort itemNumber;
        public ushort ItemNumber
        {
            get { return itemNumber; }
            set { base.SetProperty(ref itemNumber, value, () => this.ItemNumber); }
        }

        ushort unknown1;
        public ushort Unknown1
        {
            get { return unknown1; }
            set { base.SetProperty(ref unknown1, value, () => this.Unknown1); }
        }

        ushort unknown2;
        public ushort Unknown2
        {
            get { return unknown2; }
            set { base.SetProperty(ref unknown2, value, () => this.Unknown2); }
        }

        ushort recoveredHP;
        public ushort RecoveredHP
        {
            get { return recoveredHP; }
            set { base.SetProperty(ref recoveredHP, value, () => this.RecoveredHP); }
        }

        ushort recoveredTP;
        public ushort RecoveredTP
        {
            get { return recoveredTP; }
            set { base.SetProperty(ref recoveredTP, value, () => this.RecoveredTP); }
        }

        ushort unknown3;
        public ushort Unknown3
        {
            get { return unknown3; }
            set { base.SetProperty(ref unknown3, value, () => this.Unknown3); }
        }

        ushort unknown4;
        public ushort Unknown4
        {
            get { return unknown4; }
            set { base.SetProperty(ref unknown4, value, () => this.Unknown4); }
        }

        ushort unknown5;
        public ushort Unknown5
        {
            get { return unknown5; }
            set { base.SetProperty(ref unknown5, value, () => this.Unknown5); }
        }

        uint buyPrice;
        public uint BuyPrice
        {
            get { return buyPrice; }
            set { base.SetProperty(ref buyPrice, value, () => this.BuyPrice); }
        }

        uint sellPrice;
        public uint SellPrice
        {
            get { return sellPrice; }
            set { base.SetProperty(ref sellPrice, value, () => this.SellPrice); }
        }

        public MiscItemData(GameDataManager game, TBB.TBL1 table, byte[] data, PropertyChangedEventHandler propertyChanged = null) : base(game, table, data, propertyChanged) { OnLoad(); }

        protected override void OnLoad()
        {
            itemNumber = BitConverter.ToUInt16(RawData, 0);
            unknown1 = BitConverter.ToUInt16(RawData, 2);
            unknown2 = BitConverter.ToUInt16(RawData, 4);
            recoveredHP = BitConverter.ToUInt16(RawData, 6);
            recoveredTP = BitConverter.ToUInt16(RawData, 8);
            unknown3 = BitConverter.ToUInt16(RawData, 10);
            unknown4 = BitConverter.ToUInt16(RawData, 12);
            unknown5 = BitConverter.ToUInt16(RawData, 14);
            buyPrice = BitConverter.ToUInt32(RawData, 16);
            sellPrice = BitConverter.ToUInt32(RawData, 20);

            base.OnLoad();
        }
    }
}
