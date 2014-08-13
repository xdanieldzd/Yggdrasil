using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Yggdrasil.Helpers
{
    public static partial class Decompressor
    {
        public static byte[] Decompress(string path, out bool isCompressed)
        {
            BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            byte[] data = new byte[reader.BaseStream.Length];
            if (reader.Read(data, 0, data.Length) != data.Length)
            {
                throw new Exception("BinaryReader Read length mismatch");
            }
            reader.Close();

            return Decompress(data, out isCompressed);
        }

        public static byte[] Decompress(byte[] data, out bool isCompressed)
        {
            isCompressed = true;

            switch (data[0])
            {
                case 0x10: return ProcessLZ77(data);

                case 0x24:
                case 0x28: return ProcessHuffman(data);

                case 0x30: return ProcessRLE(data);

                default:
                    isCompressed = false;
                    return data;
            }
        }
    }
}
