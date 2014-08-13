using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Helpers
{
    public static partial class Decompressor
    {
        public static byte[] ProcessRLE(byte[] compData)
        {
            int dataLen = (BitConverter.ToInt32(compData, 1) & 0xFFFFFF);
            byte[] decData = new byte[dataLen];

            int inOffset = 4, outOffset = 0;

            while (outOffset < dataLen)
            {
                byte data = compData[inOffset++];
                bool compFlag = (data & 0x80) != 0;
                byte count = (byte)(data & 0x7F);

                if (compFlag)
                {
                    for (int i = 0; i < count + 3; i++) decData[outOffset++] = compData[inOffset];
                    inOffset++;
                }
                else
                    for (int i = 0; i < count + 1; i++) decData[outOffset++] = compData[inOffset++];
            }

            return decData;
        }
    }
}
