using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace ClickHouse.EntityFrameworkCore.Storage.ValueConversation;

public class ClickHouseCharValueConverter : ValueConverter<char, string>
{
    public ClickHouseCharValueConverter() : 
        base(
            c => Convert.ToString(c),
            s => Convert.ToChar(s),
            new ConverterMappingHints(1, 0, 0, true))
    {
    }
}
