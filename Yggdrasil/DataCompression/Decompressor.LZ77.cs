using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.DataCompression
{
    public static partial class Decompressor
    {
        public static byte[] ProcessLZ77(byte[] compData, int compOffset)
        {
            int dataLen = (BitConverter.ToInt32(compData, compOffset + 1) & 0xFFFFFF);
            byte[] decData = new byte[dataLen];

            int i, j, xIn, xOut;
            xIn = compOffset + 4;
            xOut = 0;
            int length, offset, windowOffset, data;
            byte d;

            while (dataLen > 0)
            {
                d = compData[xIn++];
                if (d != 0)
                {
                    for (i = 0; i < 8; i++)
                    {
                        if ((d & 0x80) != 0)
                        {
                            data = ((compData[xIn] << 8) | compData[xIn + 1]);
                            xIn += 2;
                            length = (data >> 12) + 3;
                            offset = data & 0xFFF;
                            windowOffset = xOut - offset - 1;
                            for (j = 0; j < length; j++)
                            {
                                decData[xOut++] = decData[windowOffset++];
                                dataLen--;
                                if (dataLen == 0) return decData;
                            }
                        }
                        else
                        {
                            decData[xOut++] = compData[xIn++];
                            dataLen--;
                            if (dataLen == 0) return decData;
                        }
                        d <<= 1;
                    }
                }
                else
                {
                    for (i = 0; i < 8; i++)
                    {
                        decData[xOut++] = compData[xIn++];
                        dataLen--;
                        if (dataLen == 0) return decData;
                    }
                }
            }

            return decData;
        }
    }
}