using System.Globalization;

namespace EZMap.Configuration
{
    public record MapperNumberFormat(NumberStyles NumberStyles, string? StringFormat, IFormatProvider? FormatProvider);
}
