using ClickHouse.EntityFrameworkCore.Storage.Engines;
using ClickHouse.EntityFrameworkCore.Storage.Internal.Mapping;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace ClickHouse.EntityFrameworkCore.Storage.Internal;

public class ClickHouseTypeMappingSource : RelationalTypeMappingSource
{
    private static readonly Dictionary<Type, RelationalTypeMapping> ClrTypeMappings = new()
    {
        { typeof(string), new StringTypeMapping("String", DbType.String) },
        { typeof(bool), new ClickHouseBoolTypeMapping() },
        { typeof(byte), new ByteTypeMapping("UInt8") },
        { typeof(char), new ClickHouseCharTypeMapping() },
        { typeof(int), new IntTypeMapping("Int32") },
        { typeof(ulong), new ULongTypeMapping("UInt64") },
        { typeof(long), new LongTypeMapping("Int64") },
        { typeof(sbyte), new SByteTypeMapping("Int8") },
        { typeof(short), new ShortTypeMapping("Int16") },
        { typeof(uint), new UIntTypeMapping("UInt32") },
        { typeof(ushort), new UShortTypeMapping("UInt16") },
        { typeof(DateTime), new DateTimeTypeMapping("DateTime") },
        { typeof(double), new DoubleTypeMapping("Float64") },
        { typeof(float), new FloatTypeMapping("Float32") },
        { typeof(Guid), new GuidTypeMapping("UUID") }
    };

    public ClickHouseTypeMappingSource(TypeMappingSourceDependencies dependencies, RelationalTypeMappingSourceDependencies relationalDependencies)
        : base(dependencies, relationalDependencies)
    {
    }
    public override RelationalTypeMapping? FindMapping(Type type)
    {
        return base.FindMapping(type);
    }

    protected override RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo) =>
        FindExistingMapping(mappingInfo) ??
        FindArrayMapping(mappingInfo) ??
        GetDecimalMapping(mappingInfo) ??
        base.FindMapping(in mappingInfo);

    private RelationalTypeMapping FindExistingMapping(in RelationalTypeMappingInfo mappingInfo)
    {
        if (mappingInfo.ClrType != null && ClrTypeMappings.TryGetValue(mappingInfo.ClrType, out var map))
        {
            return map;
        }

        return null;
    }

    private RelationalTypeMapping FindArrayMapping(in RelationalTypeMappingInfo mappingInfo)
    {
        //Debugger.Launch();
        var isEnumerable= typeof(IEnumerable).IsAssignableFrom(mappingInfo.ClrType);
        if ((mappingInfo.ClrType == null || !mappingInfo.ClrType.IsArray) && !isEnumerable)
            return null;

        if(isEnumerable)
        {
            var enumerableType = mappingInfo.ClrType.GenericTypeArguments[0];
            if (enumerableType.IsEnum)
            {
                var enumType = enumerableType.GetEnumUnderlyingType();
                if (ClrTypeMappings.TryGetValue(enumType, out var enumTypeMapping))
                    return new ClickHouseArrayTypeMapping($"Array({enumTypeMapping.StoreType})", enumTypeMapping);
            }
            if (ClrTypeMappings.TryGetValue(enumerableType, out var enumerableTypeMapping))
                return new ClickHouseArrayTypeMapping($"Array({enumerableTypeMapping.StoreType})", enumerableTypeMapping);
        }

        var elementType = mappingInfo.ClrType.GetElementType();
        if (elementType.IsEnum)
        {
            var enumType = elementType.GetEnumUnderlyingType();
            if (ClrTypeMappings.TryGetValue(enumType, out var enumTypeMapping))
                return new ClickHouseArrayTypeMapping($"Array({enumTypeMapping.StoreType})", enumTypeMapping);
        }
        if (ClrTypeMappings.TryGetValue(elementType, out var elementTypeMapping))
            return new ClickHouseArrayTypeMapping($"Array({elementTypeMapping.StoreType})", elementTypeMapping);
        return null;
    }

    private RelationalTypeMapping GetDecimalMapping(in RelationalTypeMappingInfo mappingInfo)
    {
        if (mappingInfo.ClrType == typeof(decimal))
        {
            return new ClickHouseDecimalTypeMapping(mappingInfo.Precision, mappingInfo.Scale, mappingInfo.Size);
        }

        return null;
    }
}
