using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
            this.GameTitle = Encoding.ASCII.GetString(Stream.ToArray(), 0, 12).TrimEnd('\0');
            this.GameCode = Encoding.ASCII.GetString(Stream.ToArray(), 12, 4).TrimEnd('\0');
            this.MakerCode = Encoding.ASCII.GetString(Stream.ToArray(), 16, 2).TrimEnd('\0');

            BinaryReader reader = new BinaryReader(Stream);

            reader.BaseStream.Seek(0x12, SeekOrigin.Begin);
            this.UnitCode = reader.ReadByte();
            this.EncryptionSeedSelect = reader.ReadByte();
            this.DeviceCapacity = reader.ReadByte();

            reader.BaseStream.Seek(0x1E, SeekOrigin.Begin);
            this.ROMVersion = reader.ReadByte();
            this.Autostart = reader.ReadByte();
            this.ARM9_ROMOffset = reader.ReadUInt32();
            this.ARM9_EntryAddress = reader.ReadUInt32();
            this.ARM9_RAMAddress = reader.ReadUInt32();
            this.ARM9_Size = reader.ReadUInt32();
            this.ARM7_ROMOffset = reader.ReadUInt32();
            this.ARM7_EntryAddress = reader.ReadUInt32();
            this.ARM7_RAMAddress = reader.ReadUInt32();
            this.ARM7_Size = reader.ReadUInt32();
            this.FNT_Offset = reader.ReadUInt32();
            this.FNT_Size = reader.ReadUInt32();
            this.FAT_Offset = reader.ReadUInt32();
            this.FAT_Size = reader.ReadUInt32();
            this.ARM9_OverlayOffset = reader.ReadUInt32();
            this.ARM9_OverlaySize = reader.ReadUInt32();
            this.ARM7_OverlayOffset = reader.ReadUInt32();
            this.ARM7_OverlaySize = reader.ReadUInt32();
            this.Port40001A4_NormalCommands = reader.ReadUInt32();
            this.Port40001A4_KEY1Commands = reader.ReadUInt32();
            this.IconTitleOffset = reader.ReadUInt32();
            this.SecureArea_CRC16 = reader.ReadUInt16();
            this.SecureArea_LoadingTimeout = reader.ReadUInt16();
            this.ARM9_AutoLoadListRAMAddress = reader.ReadUInt32();
            this.ARM7_AutoLoadListRAMAddress = reader.ReadUInt32();
            this.SecureArea_Disable = reader.ReadUInt64();
            this.TotalUsedROMSize = reader.ReadUInt32();
            this.ROMHeaderSize = reader.ReadUInt32();

            reader.BaseStream.Seek(0x15C, SeekOrigin.Begin);
            this.NintendoLogoCRC16 = reader.ReadUInt16();
            this.HeaderCRC16 = reader.ReadUInt16();
            this.Debug_ROMOffset = reader.ReadUInt32();
            this.Debug_Size = reader.ReadUInt32();
            this.Debug_RAMAddress = reader.ReadUInt32();
        }
    }
}
