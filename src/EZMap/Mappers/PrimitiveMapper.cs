using System.Globalization;
using EZMap.Configuration;
using EZMap.Utilities;

namespace EZMap.Mappers
{
    public class PrimitiveMapper : IMapper
    {
        private readonly MapperSettings mapperSettings;

        public PrimitiveMapper(MapperSettings mapperSettings) 
        {
            this.mapperSettings = mapperSettings;
        }

        public object Map(object source, Type sourceType, Type targetType)
        {
            AssertPrimitiveType(sourceType, nameof(sourceType));
            AssertPrimitiveType(targetType, nameof(targetType));

            var sourceTypeCode = Type.GetTypeCode(sourceType);
            var targetTypeCode = Type.GetTypeCode(targetType);

            if (sourceTypeCode == targetTypeCode)
            {
                return source;
            }

            if (sourceTypeCode == TypeCode.String)
            {
                var parseFunc = GetParseMethod(targetTypeCode);
                if (parseFunc != null)
                {
                    var format = mapperSettings.NumberFormats[targetTypeCode];
                    return parseFunc((string)source, format.NumberStyles, format.FormatProvider);
                }

                if (targetTypeCode == TypeCode.DateTime)
                {
                    return DateTime.Parse((string)source, mapperSettings.DateTimeFormatProvider, mapperSettings.DateTimeStyles);
                }
            }

            if (targetTypeCode == TypeCode.String)
            {
                var stringFunc = GetStringMethod(sourceTypeCode);
                if (stringFunc != null)
                {
                    var format = mapperSettings.NumberFormats[targetTypeCode];
                    return stringFunc(source, format.StringFormat, format.FormatProvider);
                }

                if (sourceTypeCode == TypeCode.DateTime)
                {
                    return ((DateTime)source).ToString(mapperSettings.DateTimeFormat, mapperSettings.DateTimeFormatProvider);
                }
            }

            return Convert.ChangeType(source, targetType, CultureInfo.CurrentCulture);
        }

        private static void AssertPrimitiveType(Type type, string parameterName)
        {
            if (!MapperUtilities.IsPrimitiveType(type))
            {
                throw new ArgumentException("Type is not primitive", parameterName);
            }
        }

        private static Func<string, NumberStyles, IFormatProvider?, object>? GetParseMethod(TypeCode typeCode)
        {
            return typeCode switch
            {
                TypeCode.Single => (value, numberStyles, formatProvider) => float.Parse(value, numberStyles, formatProvider),
                TypeCode.Double => (value, numberStyles, formatProvider) => double.Parse(value, numberStyles, formatProvider),
                TypeCode.Decimal => (value, numberStyles, formatProvider) => decimal.Parse(value, numberStyles, formatProvider),
                TypeCode.Byte => (value, numberStyles, formatProvider) => byte.Parse(value, numberStyles, formatProvider),
                TypeCode.SByte => (value, numberStyles, formatProvider) => sbyte.Parse(value, numberStyles, formatProvider),
                TypeCode.UInt16 => (value, numberStyles, formatProvider) => ushort.Parse(value, numberStyles, formatProvider),
                TypeCode.Int16 => (value, numberStyles, formatProvider) => short.Parse(value, numberStyles, formatProvider),
                TypeCode.UInt32 => (value, numberStyles, formatProvider) => uint.Parse(value, numberStyles, formatProvider),
                TypeCode.Int32 => (value, numberStyles, formatProvider) => int.Parse(value, numberStyles, formatProvider),
                TypeCode.UInt64 => (value, numberStyles, formatProvider) => ulong.Parse(value, numberStyles, formatProvider),
                TypeCode.Int64 => (value, numberStyles, formatProvider) => long.Parse(value, numberStyles, formatProvider),
                _ => null,
            };
        }

        private static Func<object, string?, IFormatProvider?, string>? GetStringMethod(TypeCode typeCode)
        {
            return typeCode switch
            {
                TypeCode.Single => (value, format, formatProvider) => ((float)value).ToString(format, formatProvider),
                TypeCode.Double => (value, format, formatProvider) => ((double)value).ToString(format, formatProvider),
                TypeCode.Decimal => (value, format, formatProvider) => ((decimal)value).ToString(format, formatProvider),
                TypeCode.Byte => (value, format, formatProvider) => ((byte)value).ToString(format, formatProvider),
                TypeCode.SByte => (value, format, formatProvider) => ((sbyte)value).ToString(format, formatProvider),
                TypeCode.UInt16 => (value, format, formatProvider) => ((ushort)value).ToString(format, formatProvider),
                TypeCode.Int16 => (value, format, formatProvider) => ((short)value).ToString(format, formatProvider),
                TypeCode.UInt32 => (value, format, formatProvider) => ((uint)value).ToString(format, formatProvider),
                TypeCode.Int32 => (value, format, formatProvider) => ((int)value).ToString(format, formatProvider),
                TypeCode.UInt64 => (value, format, formatProvider) => ((ulong)value).ToString(format, formatProvider),
                TypeCode.Int64 => (value, format, formatProvider) => ((long)value).ToString(format, formatProvider),
                _ => null,
            };
        }
    }
}
