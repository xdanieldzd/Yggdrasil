using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Yggdrasil.Helpers
{
    public class EtrianString
    {
        #region Character map

        public static readonly Dictionary<ushort, char> CharacterMap = new Dictionary<ushort, char>()
        {
            { 0x0001, ' ' },
            { 0x0002, '!' },
            { 0x0003, '"' },
            { 0x0004, '#' },
            { 0x0005, '$' },
            { 0x0006, '%' },
            { 0x0007, '&' },
            { 0x0008, '\'' },
            { 0x0009, '(' },
            { 0x000A, ')' },
            { 0x000B, '*' },
            { 0x000C, '+' },
            { 0x000D, ',' },
            { 0x000E, '-' },
            { 0x000F, '.' },
            { 0x0010, '/' },
            { 0x0011, '0' },
            { 0x0012, '1' },
            { 0x0013, '2' },
            { 0x0014, '3' },
            { 0x0015, '4' },
            { 0x0016, '5' },
            { 0x0017, '6' },
            { 0x0018, '7' },
            { 0x0019, '8' },
            { 0x001A, '9' },
            { 0x001B, ':' },
            { 0x001C, ';' },
            { 0x001D, '<' },
            { 0x001E, '=' },
            { 0x001F, '>' },
            { 0x0020, '?' },
            { 0x0021, '@' },
            { 0x0022, 'A' },
            { 0x0023, 'B' },
            { 0x0024, 'C' },
            { 0x0025, 'D' },
            { 0x0026, 'E' },
            { 0x0027, 'F' },
            { 0x0028, 'G' },
            { 0x0029, 'H' },
            { 0x002A, 'I' },
            { 0x002B, 'J' },
            { 0x002C, 'K' },
            { 0x002D, 'L' },
            { 0x002E, 'M' },
            { 0x002F, 'N' },
            { 0x0030, 'O' },
            { 0x0031, 'P' },
            { 0x0032, 'Q' },
            { 0x0033, 'R' },
            { 0x0034, 'S' },
            { 0x0035, 'T' },
            { 0x0036, 'U' },
            { 0x0037, 'V' },
            { 0x0038, 'W' },
            { 0x0039, 'X' },
            { 0x003A, 'Y' },
            { 0x003B, 'Z' },
            { 0x003C, '[' },
            { 0x003D, '\\' },
            { 0x003E, ']' },
            { 0x003F, '^' },
            { 0x0040, '_' },
            { 0x0041, '´' },
            { 0x0042, 'a' },
            { 0x0043, 'b' },
            { 0x0044, 'c' },
            { 0x0045, 'd' },
            { 0x0046, 'e' },
            { 0x0047, 'f' },
            { 0x0048, 'g' },
            { 0x0049, 'h' },
            { 0x004A, 'i' },
            { 0x004B, 'j' },
            { 0x004C, 'k' },
            { 0x004D, 'l' },
            { 0x004E, 'm' },
            { 0x004F, 'n' },
            { 0x0050, 'o' },
            { 0x0051, 'p' },
            { 0x0052, 'q' },
            { 0x0053, 'r' },
            { 0x0054, 's' },
            { 0x0055, 't' },
            { 0x0056, 'u' },
            { 0x0057, 'v' },
            { 0x0058, 'w' },
            { 0x0059, 'x' },
            { 0x005A, 'y' },
            { 0x005B, 'z' },
            { 0x005C, '{' },
            { 0x005D, '|' },
            { 0x005E, '}' },
            { 0x005F, '~' },
            { 0x0060, '€' },
            { 0x0061, '․' },
            { 0x0062, '‥' },
            { 0x0063, '…' },
            { 0x0064, '‸' },
            { 0x0065, 'Œ' },
            { 0x0066, '′' },
            { 0x0067, '‵' },
            { 0x0068, '″' },
            { 0x0069, '‶' },
            { 0x006A, '•' },
            { 0x006B, '‴' },
            { 0x006C, '™' },
            { 0x006D, '›' },
            { 0x006E, 'œ' },
            { 0x006F, '¡' },
            { 0x0070, '¢' },
            { 0x0071, '£' },
            { 0x0072, '¨' },
            { 0x0073, '©' },
            { 0x0074, '®' },
            { 0x0075, '°' },
            { 0x0076, '±' },
            { 0x0077, '´' },
            { 0x0078, '·' },
            { 0x0079, '¿' },
            { 0x007A, 'À' },
            { 0x007B, 'Á' },
            { 0x007C, 'Â' },
            { 0x007D, 'Ã' },
            { 0x007E, 'Ä' },
            { 0x007F, 'Å' },
            { 0x0080, 'Æ' },
            { 0x0081, 'Ç' },
            { 0x0082, 'È' },
            { 0x0083, 'É' },
            { 0x0084, 'Ê' },
            { 0x0085, 'Ë' },
            { 0x0086, 'Ì' },
            { 0x0087, 'Í' },
            { 0x0088, 'Î' },
            { 0x0089, 'Ï' },
            { 0x008A, 'Ð' },
            { 0x008B, 'Ñ' },
            { 0x008C, 'Ò' },
            { 0x008D, 'Ó' },
            { 0x008E, 'Ô' },
            { 0x008F, 'Õ' },
            { 0x0090, 'Ö' },
            { 0x0091, '×' },
            { 0x0092, 'Ø' },
            { 0x0093, 'Ù' },
            { 0x0094, 'Ú' },
            { 0x0095, 'Û' },
            { 0x0096, 'Ü' },
            { 0x0097, 'Ý' },
            { 0x0098, 'ß' },
            { 0x0099, 'à' },
            { 0x009A, 'á' },
            { 0x009B, 'â' },
            { 0x009C, 'ã' },
            { 0x009D, 'ä' },
            { 0x009E, 'å' },
            { 0x009F, 'æ' },
            { 0x00A0, 'ç' },
            { 0x00A1, 'è' },
            { 0x00A2, 'é' },
            { 0x00A3, 'ê' },
            { 0x00A4, 'ë' },
            { 0x00A5, 'ì' },
            { 0x00A6, 'í' },
            { 0x00A7, 'î' },
            { 0x00A8, 'ï' },
            { 0x00A9, 'ð' },
            { 0x00AA, 'ñ' },
            { 0x00AB, 'ò' },
            { 0x00AC, 'ó' },
            { 0x00AD, 'ô' },
            { 0x00AE, 'õ' },
            { 0x00AF, 'ö' },
            { 0x00B0, '÷' },
            { 0x00B1, 'ø' },
            { 0x00B2, 'ù' },
            { 0x00B3, 'ú' },
            { 0x00B4, 'û' },
            { 0x00B5, 'ü' },
            { 0x00B6, 'ý' },
            { 0x00B7, '→' },
            { 0x00B8, '←' },
            { 0x00B9, '↑' },
            { 0x00BA, '↓' },
            { 0x00BB, '«' },
            { 0x00BC, '»' },
            { 0x00BD, 'ª' },
            { 0x00BE, 'º' },
            { 0x00BF, 'ͤ' },
            { 0x00C0, 'ͬ' },

            { 0x8001, '\n' },   //line feed / linebreak
            { 0x8002, '\f' },   //form feed / next page ??
        };

        #endregion

        public ushort[] RawData { get; private set; }
        public string ConvertedString { get; private set; }

        public EtrianString(string textString)
        {
            ConvertedString = textString;

            RawData = new ushort[ConvertedString.Length];
            for (int i = 0; i < ConvertedString.Length; i++) RawData[i] = CharacterMap.GetByValue(ConvertedString[i]);
        }

        public EtrianString(ushort[] data)
        {
            ParseString(data);
        }

        public EtrianString(byte[] data, int offset)
        {
            int stringLength = -1;
            for (int i = 0; i < 0x2000; i += 2)
            {
                if (BitConverter.ToUInt16(data, offset + i) == 0x0000) { stringLength = i / 2; break; }
            }

            ushort[] strData = new ushort[stringLength];
            Buffer.BlockCopy(data, offset, strData, 0, strData.Length * 2);
            ParseString(strData);
        }

        private void ParseString(ushort[] data)
        {
            RawData = data;

            if (RawData.Length != 0)
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < RawData.Length; i++)
                {
                    if ((RawData[i] & 0x8000) == 0x8000 && !CharacterMap.ContainsKey(RawData[i]))
                    {
                        /* TODO  control codes go HERE */
                        switch (RawData[i] & 0xFF)
                        {
                            case 0x04:
                                // color?
                                i++;
                                break;

                            default:
                                builder.AppendFormat("({0:X4})", RawData[i]);
                                break;
                        }
                    }
                    else
                        builder.Append(CharacterMap[RawData[i]]);
                }
                ConvertedString = builder.ToString();
            }
            else
                ConvertedString = string.Empty;
        }

        public static implicit operator EtrianString(string textString)
        {
            if (textString == null) return null;

            return new EtrianString(textString);
        }

        public static implicit operator EtrianString(ushort[] data)
        {
            if (data == null) return null;

            return new EtrianString(data);
        }

        public static implicit operator string(EtrianString etrianString)
        {
            if (etrianString == null) return null;

            return etrianString.ConvertedString;
        }

        public override string ToString()
        {
            return ConvertedString;
        }
    }

    public static class Extensions
    {
        public static T1 GetByValue<T1, T2>(this Dictionary<T1, T2> dict, T2 val)
        {
            if (!dict.ContainsValue(val)) throw new Exception("Value not found");
            return dict.FirstOrDefault(x => x.Value.Equals(val)).Key;
        }
    }
}
