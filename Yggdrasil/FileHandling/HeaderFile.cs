using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.FileHandling
{
    public class HeaderFile : BaseFile
    {
        public HeaderFile(GameDataManager gameDataManager, string path) : base(gameDataManager, path) { }

        public string GameTitle { get; private set; }
        public string GameCode { get; private set; }
        public string MakerCode { get; private set; }
        public byte UnitCode { get; private set; }
        public byte EncryptionSeedSelect { get; private set; }
        public byte DeviceCapacity { get; private set; }
        //9 bytes reserved
        public byte ROMVersion { get; private set; }
        public byte Autostart { get; private set; }
        public uint ARM9_ROMOffset { get; private set; }
        public uint ARM9_EntryAddress { get; private set; }
        public uint ARM9_RAMAddress { get; private set; }
        public uint ARM9_Size { get; private set; }
        public uint ARM7_ROMOffset { get; private set; }
        public uint ARM7_EntryAddress { get; private set; }
        public uint ARM7_RAMAddress { get; private set; }
        public uint ARM7_Size { get; private set; }
        public uint FNT_Offset { get; private set; }
        public uint FNT_Size { get; private set; }
        public uint FAT_Offset { get; private set; }
        public uint FAT_Size { get; private set; }
        public uint ARM9_OverlayOffset { get; private set; }
        public uint ARM9_OverlaySize { get; private set; }
        public uint ARM7_OverlayOffset { get; private set; }
        public uint ARM7_OverlaySize { get; private set; }
        public uint Port40001A4_NormalCommands { get; private set; }
        public uint Port40001A4_KEY1Commands { get; private set; }
        public uint IconTitleOffset { get; private set; }
        public ushort SecureArea_CRC16 { get; private set; }
        public ushort SecureArea_LoadingTimeout { get; private set; }
        public uint ARM9_AutoLoadListRAMAddress { get; private set; }
        public uint ARM7_AutoLoadListRAMAddress { get; private set; }
        public ulong SecureArea_Disable { get; private set; }
        public uint TotalUsedROMSize { get; private set; }
        public uint ROMHeaderSize { get; private set; }
        //0x38 bytes reserved
        //0x9C bytes nintendo logo
        public ushort NintendoLogoCRC16 { get; private set; }
        public ushort HeaderCRC16 { get; private set; }
        public uint Debug_ROMOffset { get; private set; }
        public uint Debug_Size { get; private set; }
        public uint Debug_RAMAddress { get; private set; }
        //4 bytes reserved
        //0x90 bytes reserved

        public override void Parse()
        {
            this.GameTitle = Encoding.ASCII.GetString(Data, 0, 12).TrimEnd('\0');
            this.GameCode = Encoding.ASCII.GetString(Data, 12, 4).TrimEnd('\0');
            this.MakerCode = Encoding.ASCII.GetString(Data, 16, 2).TrimEnd('\0');
            this.UnitCode = Data[0x12];
            this.EncryptionSeedSelect = Data[0x13];
            this.DeviceCapacity = Data[0x14];
            this.ROMVersion = Data[0x1E];
            this.Autostart = Data[0x1F];
            this.ARM9_ROMOffset = BitConverter.ToUInt32(Data, 0x20);
            this.ARM9_EntryAddress = BitConverter.ToUInt32(Data, 0x24);
            this.ARM9_RAMAddress = BitConverter.ToUInt32(Data, 0x28);
            this.ARM9_Size = BitConverter.ToUInt32(Data, 0x2C);
            this.ARM7_ROMOffset = BitConverter.ToUInt32(Data, 0x30);
            this.ARM7_EntryAddress = BitConverter.ToUInt32(Data, 0x34);
            this.ARM7_RAMAddress = BitConverter.ToUInt32(Data, 0x38);
            this.ARM7_Size = BitConverter.ToUInt32(Data, 0x3C);
            this.FNT_Offset = BitConverter.ToUInt32(Data, 0x40);
            this.FNT_Size = BitConverter.ToUInt32(Data, 0x44);
            this.FAT_Offset = BitConverter.ToUInt32(Data, 0x48);
            this.FAT_Size = BitConverter.ToUInt32(Data, 0x4C);
            this.ARM9_OverlayOffset = BitConverter.ToUInt32(Data, 0x50);
            this.ARM9_OverlaySize = BitConverter.ToUInt32(Data, 0x54);
            this.ARM7_OverlayOffset = BitConverter.ToUInt32(Data, 0x58);
            this.ARM7_OverlaySize = BitConverter.ToUInt32(Data, 0x5C);
            this.Port40001A4_NormalCommands = BitConverter.ToUInt32(Data, 0x60);
            this.Port40001A4_KEY1Commands = BitConverter.ToUInt32(Data, 0x64);
            this.IconTitleOffset = BitConverter.ToUInt32(Data, 0x68);
            this.SecureArea_CRC16 = BitConverter.ToUInt16(Data, 0x6C);
            this.SecureArea_LoadingTimeout = BitConverter.ToUInt16(Data, 0x6E);
            this.ARM9_AutoLoadListRAMAddress = BitConverter.ToUInt32(Data, 0x70);
            this.ARM7_AutoLoadListRAMAddress = BitConverter.ToUInt32(Data, 0x74);
            this.SecureArea_Disable = BitConverter.ToUInt64(Data, 0x78);
            this.TotalUsedROMSize = BitConverter.ToUInt32(Data, 0x80);
            this.ROMHeaderSize = BitConverter.ToUInt32(Data, 0x84);
            this.NintendoLogoCRC16 = BitConverter.ToUInt16(Data, 0x15C);
            this.HeaderCRC16 = BitConverter.ToUInt16(Data, 0x15E);
            this.Debug_ROMOffset = BitConverter.ToUInt32(Data, 0x160);
            this.Debug_Size = BitConverter.ToUInt32(Data, 0x164);
            this.Debug_RAMAddress = BitConverter.ToUInt32(Data, 0x168);
        }
    }
}
