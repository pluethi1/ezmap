using System.Globalization;

namespace EZMap.Configuration
{
    public class MapperSettings
    {
        public string? DateTimeFormat { get; set; }
        public IFormatProvider? DateTimeFormatProvider { get; set; }
        public DateTimeStyles DateTimeStyles { get; set; } = DateTimeStyles.None;

        public AutoMapperExceptionBehavior AutoMapperExceptionBehavior { get; set; } = AutoMapperExceptionBehavior.Propagate;

        public Dictionary<TypeCode, MapperNumberFormat> NumberFormats { get; } = new()
        {
            [TypeCode.Single] = new MapperNumberFormat(NumberStyles.Number, null, CultureInfo.CurrentCulture.NumberFormat),
            [TypeCode.Double] = new MapperNumberFormat(NumberStyles.Number, null, CultureInfo.CurrentCulture.NumberFormat),
            [TypeCode.Decimal] = new MapperNumberFormat(NumberStyles.Number, null, CultureInfo.CurrentCulture.NumberFormat),
            [TypeCode.Byte] = new MapperNumberFormat(NumberStyles.Number, null, CultureInfo.CurrentCulture.NumberFormat),
            [TypeCode.SByte] = new MapperNumberFormat(NumberStyles.Number, null, CultureInfo.CurrentCulture.NumberFormat),
            [TypeCode.UInt16] = new MapperNumberFormat(NumberStyles.Number, null, CultureInfo.CurrentCulture.NumberFormat),
            [TypeCode.Int16] = new MapperNumberFormat(NumberStyles.Number, null, CultureInfo.CurrentCulture.NumberFormat),
            [TypeCode.UInt32] = new MapperNumberFormat(NumberStyles.Number, null, CultureInfo.InvariantCulture.NumberFormat),
            [TypeCode.Int32] = new MapperNumberFormat(NumberStyles.Number, null, CultureInfo.InvariantCulture.NumberFormat),
            [TypeCode.UInt64] = new MapperNumberFormat(NumberStyles.Number, null, CultureInfo.InvariantCulture.NumberFormat),
            [TypeCode.Int64] = new MapperNumberFormat(NumberStyles.Number, null, CultureInfo.InvariantCulture.NumberFormat),
        };
    }
}