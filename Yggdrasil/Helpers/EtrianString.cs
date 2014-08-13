using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Yggdrasil.Helpers
{
    public class EtrianString
    {
        #region European/American character map

        static readonly Dictionary<ushort, char> characterMapEnglish = new Dictionary<ushort, char>()
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
        };

        #endregion

        static readonly Dictionary<ushort, char> characterMapJapanese = new Dictionary<ushort, char>()
        {
            { 0x0001, ' ' },
            { 0x0002, '、' },
            { 0x0003, '。' },
            { 0x0004, '，' },
            { 0x0005, '．' },
            { 0x0006, '・' },
            { 0x0007, '：' },
            { 0x0008, '；' },
            { 0x0009, '？' },
            { 0x000A, '！' },
            { 0x000B, '゛' },
            { 0x000C, '゜' },
            { 0x000D, '´' },
            { 0x000E, '｀' },
            { 0x000F, '¨' },
            { 0x0010, '＾' },
            { 0x0011, '￣' },
            { 0x0012, '＿' },
            { 0x0013, '々' },
            { 0x0014, 'ー' },
            { 0x0015, '―' },
            { 0x0016, '／' },
            { 0x0017, '\\' },
            { 0x0018, '～' },
            { 0x0019, '｜' },
            { 0x001A, '…' },
            { 0x001B, '‘' },
            { 0x001C, '’' },
            { 0x001D, '“' },
            { 0x001E, '”' },
            { 0x001F, '（' },
            { 0x0020, '）' },
            { 0x0021, '［' },
            { 0x0022, '］' },
            { 0x0023, '「' },
            { 0x0024, '」' },
            { 0x0025, '『' },
            { 0x0026, '』' },
            { 0x0027, '【' },
            { 0x0028, '】' },
            { 0x0029, '＋' },
            { 0x002A, '−' },
            { 0x002B, '±' },
            { 0x002C, '×' },
            { 0x002D, '÷' },
            { 0x002E, '＝' },
            { 0x002F, '＜' },
            { 0x0030, '＞' },
            { 0x0031, '≦' },
            { 0x0032, '≧' },
            { 0x0033, '￥' },
            { 0x0034, '＄' },
            { 0x0035, '％' },
            { 0x0036, '＃' },
            { 0x0037, '＆' },
            { 0x0038, '＊' },
            { 0x0039, '＠' },
            { 0x003A, '☆' },
            { 0x003B, '★' },
            { 0x003C, '○' },
            { 0x003D, '●' },
            { 0x003E, '◎' },
            { 0x003F, '◇' },
            { 0x0040, '◆' },
            { 0x0041, '□' },
            { 0x0042, '■' },
            { 0x0043, '△' },
            { 0x0044, '▲' },
            { 0x0045, '▽' },
            { 0x0046, '▼' },
            { 0x0047, '※' },
            { 0x0048, '→' },
            { 0x0049, '←' },
            { 0x004A, '↑' },
            { 0x004B, '↓' },
            { 0x004C, '０' },
            { 0x004D, '１' },
            { 0x004E, '２' },
            { 0x004F, '３' },
            { 0x0050, '４' },
            { 0x0051, '５' },
            { 0x0052, '６' },
            { 0x0053, '７' },
            { 0x0054, '８' },
            { 0x0055, '９' },
            { 0x0056, 'Ａ' },
            { 0x0057, 'Ｂ' },
            { 0x0058, 'Ｃ' },
            { 0x0059, 'Ｄ' },
            { 0x005A, 'Ｅ' },
            { 0x005B, 'Ｆ' },
            { 0x005C, 'Ｇ' },
            { 0x005D, 'Ｈ' },
            { 0x005E, 'Ｉ' },
            { 0x005F, 'Ｊ' },
            { 0x0060, 'Ｋ' },
            { 0x0061, 'Ｌ' },
            { 0x0062, 'Ｍ' },
            { 0x0063, 'Ｎ' },
            { 0x0064, 'Ｏ' },
            { 0x0065, 'Ｐ' },
            { 0x0066, 'Ｑ' },
            { 0x0067, 'Ｒ' },
            { 0x0068, 'Ｓ' },
            { 0x0069, 'Ｔ' },
            { 0x006A, 'Ｕ' },
            { 0x006B, 'Ｖ' },
            { 0x006C, 'Ｗ' },
            { 0x006D, 'Ｘ' },
            { 0x006E, 'Ｙ' },
            { 0x006F, 'Ｚ' },
            { 0x0070, 'ａ' },
            { 0x0071, 'ｂ' },
            { 0x0072, 'ｃ' },
            { 0x0073, 'ｄ' },
            { 0x0074, 'ｅ' },
            { 0x0075, 'ｆ' },
            { 0x0076, 'ｇ' },
            { 0x0077, 'ｈ' },
            { 0x0078, 'ｉ' },
            { 0x0079, 'ｊ' },
            { 0x007A, 'ｋ' },
            { 0x007B, 'ｌ' },
            { 0x007C, 'ｍ' },
            { 0x007D, 'ｎ' },
            { 0x007E, 'ｏ' },
            { 0x007F, 'ｐ' },
            { 0x0080, 'ｑ' },
            { 0x0081, 'ｒ' },
            { 0x0082, 'ｓ' },
            { 0x0083, 'ｔ' },
            { 0x0084, 'ｕ' },
            { 0x0085, 'ｖ' },
            { 0x0086, 'ｗ' },
            { 0x0087, 'ｘ' },
            { 0x0088, 'ｙ' },
            { 0x0089, 'ｚ' },
            { 0x008A, 'ぁ' },
            { 0x008B, 'あ' },
            { 0x008C, 'ぃ' },
            { 0x008D, 'い' },
            { 0x008E, 'ぅ' },
            { 0x008F, 'う' },
            { 0x0090, 'ぇ' },
            { 0x0091, 'え' },
            { 0x0092, 'ぉ' },
            { 0x0093, 'お' },
            { 0x0094, 'か' },
            { 0x0095, 'が' },
            { 0x0096, 'き' },
            { 0x0097, 'ぎ' },
            { 0x0098, 'く' },
            { 0x0099, 'ぐ' },
            { 0x009A, 'け' },
            { 0x009B, 'げ' },
            { 0x009C, 'こ' },
            { 0x009D, 'ご' },
            { 0x009E, 'さ' },
            { 0x009F, 'ざ' },
            { 0x00A0, 'し' },
            { 0x00A1, 'じ' },
            { 0x00A2, 'す' },
            { 0x00A3, 'ず' },
            { 0x00A4, 'せ' },
            { 0x00A5, 'ぜ' },
            { 0x00A6, 'そ' },
            { 0x00A7, 'ぞ' },
            { 0x00A8, 'た' },
            { 0x00A9, 'だ' },
            { 0x00AA, 'ち' },
            { 0x00AB, 'ぢ' },
            { 0x00AC, 'っ' },
            { 0x00AD, 'つ' },
            { 0x00AE, 'づ' },
            { 0x00AF, 'て' },
            { 0x00B0, 'で' },
            { 0x00B1, 'と' },
            { 0x00B2, 'ど' },
            { 0x00B3, 'な' },
            { 0x00B4, 'に' },
            { 0x00B5, 'ぬ' },
            { 0x00B6, 'ね' },
            { 0x00B7, 'の' },
            { 0x00B8, 'は' },
            { 0x00B9, 'ば' },
            { 0x00BA, 'ぱ' },
            { 0x00BB, 'ひ' },
            { 0x00BC, 'び' },
            { 0x00BD, 'ぴ' },
            { 0x00BE, 'ふ' },
            { 0x00BF, 'ぶ' },
            { 0x00C0, 'ぷ' },
            { 0x00C1, 'へ' },
            { 0x00C2, 'べ' },
            { 0x00C3, 'ぺ' },
            { 0x00C4, 'ほ' },
            { 0x00C5, 'ぼ' },
            { 0x00C6, 'ぽ' },
            { 0x00C7, 'ま' },
            { 0x00C8, 'み' },
            { 0x00C9, 'む' },
            { 0x00CA, 'め' },
            { 0x00CB, 'も' },
            { 0x00CC, 'ゃ' },
            { 0x00CD, 'や' },
            { 0x00CE, 'ゅ' },
            { 0x00CF, 'ゆ' },
            { 0x00D0, 'ょ' },
            { 0x00D1, 'よ' },
            { 0x00D2, 'ら' },
            { 0x00D3, 'り' },
            { 0x00D4, 'る' },
            { 0x00D5, 'れ' },
            { 0x00D6, 'ろ' },
            { 0x00D7, 'わ' },
            { 0x00D8, 'を' },
            { 0x00D9, 'ん' },
            { 0x00DA, 'ァ' },
            { 0x00DB, 'ア' },
            { 0x00DC, 'ィ' },
            { 0x00DD, 'イ' },
            { 0x00DE, 'ゥ' },
            { 0x00DF, 'ウ' },
            { 0x00E0, 'ェ' },
            { 0x00E1, 'エ' },
            { 0x00E2, 'ォ' },
            { 0x00E3, 'オ' },
            { 0x00E4, 'カ' },
            { 0x00E5, 'ガ' },
            { 0x00E6, 'キ' },
            { 0x00E7, 'ギ' },
            { 0x00E8, 'ク' },
            { 0x00E9, 'グ' },
            { 0x00EA, 'ケ' },
            { 0x00EB, 'ゲ' },
            { 0x00EC, 'コ' },
            { 0x00ED, 'ゴ' },
            { 0x00EE, 'サ' },
            { 0x00EF, 'ザ' },
            { 0x00F0, 'シ' },
            { 0x00F1, 'ジ' },
            { 0x00F2, 'ス' },
            { 0x00F3, 'ズ' },
            { 0x00F4, 'セ' },
            { 0x00F5, 'ゼ' },
            { 0x00F6, 'ソ' },
            { 0x00F7, 'ゾ' },
            { 0x00F8, 'タ' },
            { 0x00F9, 'ダ' },
            { 0x00FA, 'チ' },
            { 0x00FB, 'ヂ' },
            { 0x00FC, 'ッ' },
            { 0x00FD, 'ツ' },
            { 0x00FE, 'ヅ' },
            { 0x00FF, 'テ' },
            { 0x0100, 'デ' },
            { 0x0101, 'ト' },
            { 0x0102, 'ド' },
            { 0x0103, 'ナ' },
            { 0x0104, 'ニ' },
            { 0x0105, 'ヌ' },
            { 0x0106, 'ネ' },
            { 0x0107, 'ノ' },
            { 0x0108, 'ハ' },
            { 0x0109, 'バ' },
            { 0x010A, 'パ' },
            { 0x010B, 'ヒ' },
            { 0x010C, 'ビ' },
            { 0x010D, 'ピ' },
            { 0x010E, 'フ' },
            { 0x010F, 'ブ' },
            { 0x0110, 'プ' },
            { 0x0111, 'ヘ' },
            { 0x0112, 'ベ' },
            { 0x0113, 'ペ' },
            { 0x0114, 'ホ' },
            { 0x0115, 'ボ' },
            { 0x0116, 'ポ' },
            { 0x0117, 'マ' },
            { 0x0118, 'ミ' },
            { 0x0119, 'ム' },
            { 0x011A, 'メ' },
            { 0x011B, 'モ' },
            { 0x011C, 'ャ' },
            { 0x011D, 'ヤ' },
            { 0x011E, 'ュ' },
            { 0x011F, 'ユ' },
            { 0x0120, 'ョ' },
            { 0x0121, 'ヨ' },
            { 0x0122, 'ラ' },
            { 0x0123, 'リ' },
            { 0x0124, 'ル' },
            { 0x0125, 'レ' },
            { 0x0126, 'ロ' },
            { 0x0127, 'ワ' },
            { 0x0128, 'ヲ' },
            { 0x0129, 'ン' },
            { 0x012A, 'ヴ' },
            { 0x012B, 'Ⅱ' },
            { 0x012C, 'Ⅲ' },
        };

        static readonly Dictionary<ushort, char> characterMapCommon = new Dictionary<ushort, char>()
        {
            { 0x8001, '\n' },   //line feed / linebreak
            { 0x8002, '\f' },   //form feed / next page ??
        };

        public static Dictionary<ushort, char> CharacterMap { get; private set; }

        public ushort[] RawData { get; private set; }
        public string ConvertedString { get; private set; }

        GameDataManager.Versions gameVersion;
        public GameDataManager.Versions GameVersion
        {
            get { return gameVersion; }
            set
            {
                gameVersion = value;
                if (gameVersion != GameDataManager.Versions.Japanese)
                    CharacterMap = characterMapEnglish.Concat(characterMapCommon).ToDictionary(x => x.Key, x => x.Value);
                else
                    CharacterMap = characterMapJapanese.Concat(characterMapCommon).ToDictionary(x => x.Key, x => x.Value);
            }
        }

        public EtrianString(GameDataManager.Versions gameVersion, string textString)
        {
            ConvertedString = textString;

            this.GameVersion = gameVersion;

            RawData = new ushort[ConvertedString.Length];
            for (int i = 0; i < ConvertedString.Length; i++) RawData[i] = CharacterMap.GetByValue(ConvertedString[i]);
        }

        public EtrianString(GameDataManager.Versions gameVersion, ushort[] data)
        {
            this.GameVersion = gameVersion;

            ParseString(data);
        }

        public EtrianString(GameDataManager.Versions gameVersion, byte[] data, int offset)
        {
            this.GameVersion = gameVersion;

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
                    {
                        if (CharacterMap.ContainsKey(RawData[i]))
                            builder.Append(CharacterMap[RawData[i]]);
                        else
                            builder.AppendFormat("({0:X4})", RawData[i]);
                    }
                }
                ConvertedString = builder.ToString();
            }
            else
                ConvertedString = string.Empty;
        }

        public static implicit operator EtrianString(string textString)
        {
            if (textString == null) return null;

            return new EtrianString(GameDataManager.Versions.European, textString);
        }

        public static implicit operator EtrianString(ushort[] data)
        {
            if (data == null) return null;

            return new EtrianString(GameDataManager.Versions.European, data);
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
