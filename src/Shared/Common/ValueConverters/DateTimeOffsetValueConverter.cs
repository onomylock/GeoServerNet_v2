using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shared.Common.ValueConverters;

public class DateTimeOffsetValueConverter()
    : ValueConverter<DateTimeOffset, DateTimeOffset>(d => d.ToUniversalTime(), d => d.ToUniversalTime());