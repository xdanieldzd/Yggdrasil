using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Yggdrasil
{
    class CustomConverters
    {
        public class HexByteConverter : TypeConverter
        {
            public virtual Type DataType { get { return typeof(byte); } }
            public virtual string FormatString { get { return "0x{0:X2}"; } }

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string)) return true;
                else return base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(string)) return true;
                else return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string) && value.GetType() == DataType)
                    return string.Format(FormatString, value);
                else
                    return base.ConvertTo(context, culture, value, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value.GetType() == typeof(string))
                {
                    string input = (string)value;
                    if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) input = input.Substring(2);

                    try
                    {
                        return DataType.InvokeMember("Parse", BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static, null, null,
                            new object[] { input, System.Globalization.NumberStyles.HexNumber, culture });
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                    return base.ConvertFrom(context, culture, value);
            }
        }

        public class HexSbyteConverter : HexByteConverter
        {
            public override Type DataType { get { return typeof(sbyte); } }
        }

        public class HexUshortConverter : HexByteConverter
        {
            public override Type DataType { get { return typeof(ushort); } }
            public override string FormatString { get { return "0x{0:X4}"; } }
        }

        public class HexShortConverter : HexByteConverter
        {
            public override Type DataType { get { return typeof(short); } }
        }

        public class HexUintConverter : HexByteConverter
        {
            public override Type DataType { get { return typeof(uint); } }
            public override string FormatString { get { return "0x{0:X8}"; } }
        }

        public class HexIntConverter : HexByteConverter
        {
            public override Type DataType { get { return typeof(int); } }
        }

        public class SbytePercentageConverter : SByteConverter
        {
            const string suffix = "%";

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string) && value.GetType() == typeof(sbyte)) return string.Format("{0}{1}", value, suffix);
                else return base.ConvertTo(context, culture, value, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value.GetType() == typeof(string))
                {
                    try
                    {
                        string input = Regex.Replace((string)value, @"[^-?\d]", "");
                        return sbyte.Parse(input, System.Globalization.NumberStyles.Integer, culture);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                    return base.ConvertFrom(context, culture, value);
            }
        }

        public class EtrianEnConverter : UInt32Converter
        {
            const string suffix = " en";

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string) && value.GetType() == typeof(uint)) return string.Format("{0}{1}", value, suffix);
                else return base.ConvertTo(context, culture, value, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value.GetType() == typeof(string))
                {
                    try
                    {
                        string input = Regex.Replace((string)value, @"[^-?\d]", "");
                        return uint.Parse(input, System.Globalization.NumberStyles.Integer, culture);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                    return base.ConvertFrom(context, culture, value);
            }
        }
    }
}
