using System.ComponentModel.DataAnnotations;
using System.Globalization;
using EZMap.Configuration;
using EZMap.Utilities;

namespace EZMap.Mappers
{
    public class PrimitiveTypeMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget>
        where TSource : notnull
        where TTarget : notnull
    {
        private readonly MapperSettings mapperSettings;

        public PrimitiveTypeMapper(MapperSettings mapperSettings) 
        {
            this.mapperSettings = mapperSettings;
            AssertPrimitiveType(typeof(TSource), nameof(TSource));
            AssertPrimitiveType(typeof(TTarget), nameof(TTarget));
        }

        public Type SourceType => typeof(TSource);
        public Type TargetType => typeof(TTarget);

        public TTarget Map(TSource source)
        {
            var sourceTypeCode = Type.GetTypeCode(typeof(TSource));
            var targetTypeCode = Type.GetTypeCode(typeof(TTarget));

            if (sourceTypeCode == targetTypeCode)
            {
                return (TTarget)(object)source;
            }

            if (sourceTypeCode == TypeCode.String)
            {
                var parseFunc = GetParseMethod();
                if (parseFunc != null)
                {
                    var format = mapperSettings.NumberFormats[targetTypeCode];
                    return parseFunc((string)(object)source, format.NumberStyles, format.FormatProvider);
                }

                if (targetTypeCode == TypeCode.DateTime)
                {
                    return (TTarget)(object)DateTime.Parse((string)(object)source, mapperSettings.DateTimeFormatProvider, mapperSettings.DateTimeStyles);
                }
            }

            if (targetTypeCode == TypeCode.String)
            {
                var stringFunc = GetStringMethod();
                if (stringFunc != null)
                {
                    var format = mapperSettings.NumberFormats[targetTypeCode];
                    return (TTarget)(object)stringFunc(source, format.StringFormat, format.FormatProvider);
                }

                if (sourceTypeCode == TypeCode.DateTime)
                {
                    return (TTarget)(object)((DateTime)(object)source).ToString(mapperSettings.DateTimeFormat, mapperSettings.DateTimeFormatProvider);
                }
            }

            return (TTarget)Convert.ChangeType(source, TargetType, CultureInfo.CurrentCulture);
        }

        private static void AssertPrimitiveType(Type type, string parameterName)
        {
            if (!MapperUtilities.IsPrimitiveType(type))
            {
                throw new ArgumentException("Type is not primitive", parameterName);
            }
        }

        private Func<string, NumberStyles, IFormatProvider?, TTarget>? GetParseMethod()
        {
            var typeCode = Type.GetTypeCode(TargetType);
            if (typeCode is TypeCode.Single or 
                            TypeCode.Double or 
                            TypeCode.Decimal or 
                            TypeCode.Byte or 
                            TypeCode.SByte or 
                            TypeCode.UInt16 or 
                            TypeCode.Int16 or 
                            TypeCode.UInt32 or 
                            TypeCode.Int32 or
                            TypeCode.UInt64 or
                            TypeCode.Int64)
            {
                var parseMethod = TargetType.GetMethod("Parse", new Type[] { typeof(string), typeof(NumberStyles), typeof(IFormatProvider) });
                if (parseMethod != null)
                {
                    return (value, numberStyles, formatProvider) => (TTarget)parseMethod.Invoke(null, new object?[] { value, numberStyles, formatProvider })!;
                }
            }
            return null;
        }

        private Func<TSource, string?, IFormatProvider?, string>? GetStringMethod()
        {
            var typeCode = Type.GetTypeCode(SourceType);
            if (typeCode is TypeCode.Single or
                TypeCode.Double or
                TypeCode.Decimal or
                TypeCode.Byte or
                TypeCode.SByte or
                TypeCode.UInt16 or
                TypeCode.Int16 or
                TypeCode.UInt32 or
                TypeCode.Int32 or
                TypeCode.UInt64 or
                TypeCode.Int64)
            {
                var stringMethod = SourceType.GetMethod("ToString", new Type[] { typeof(string), typeof(IFormatProvider) });
                if (stringMethod != null)
                {
                    return (value, format, formatProvider) => (string)stringMethod.Invoke(value, new object?[] { format, formatProvider })!;
                }
            }
            return null;
        }
    }
}
