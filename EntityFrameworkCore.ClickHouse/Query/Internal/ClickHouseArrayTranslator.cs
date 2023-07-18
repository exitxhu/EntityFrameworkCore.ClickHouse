using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ClickHouse.EntityFrameworkCore.Query.Internal;

public class ClickHouseArrayTranslator : IMethodCallTranslator, IMemberTranslator
{
    private static readonly MethodInfo EmptyArrayUInt8 = typeof(Array)
        .GetRuntimeMethod(nameof(Array.Empty), Array.Empty<Type>())
        !.MakeGenericMethod(typeof(byte));

    private static readonly MethodInfo EmptyArrayUInt16 = typeof(Array)
        .GetRuntimeMethod(nameof(Array.Empty), Array.Empty<Type>())
        !.MakeGenericMethod(typeof(ushort));

    private static readonly MethodInfo EmptyArrayUInt32 = typeof(Array)
        .GetRuntimeMethod(nameof(Array.Empty), Array.Empty<Type>())
        !.MakeGenericMethod(typeof(uint));

    private static readonly MethodInfo EmptyArrayUInt64 = typeof(Array)
        .GetRuntimeMethod(nameof(Array.Empty), Array.Empty<Type>())
        !.MakeGenericMethod(typeof(ulong));

    private static readonly MethodInfo EmptyArrayInt8 = typeof(Array)
        .GetRuntimeMethod(nameof(Array.Empty), Array.Empty<Type>())
        !.MakeGenericMethod(typeof(sbyte));

    private static readonly MethodInfo EmptyArrayInt16 = typeof(Array)
        .GetRuntimeMethod(nameof(Array.Empty), Array.Empty<Type>())
        !.MakeGenericMethod(typeof(short));

    private static readonly MethodInfo EmptyArrayInt32 = typeof(Array)
        .GetRuntimeMethod(nameof(Array.Empty), Array.Empty<Type>())
        !.MakeGenericMethod(typeof(int));

    private static readonly MethodInfo EmptyArrayInt64 = typeof(Array)
        .GetRuntimeMethod(nameof(Array.Empty), Array.Empty<Type>())
        !.MakeGenericMethod(typeof(long));

    private static readonly MethodInfo EmptyArrayFloat32 = typeof(Array)
        .GetRuntimeMethod(nameof(Array.Empty), Array.Empty<Type>())
        !.MakeGenericMethod(typeof(float));

    private static readonly MethodInfo EmptyArrayFloat64 = typeof(Array)
        .GetRuntimeMethod(nameof(Array.Empty), Array.Empty<Type>())
        !.MakeGenericMethod(typeof(double));

    private static readonly MethodInfo EmptyArrayDateTime = typeof(Array)
        .GetRuntimeMethod(nameof(Array.Empty), Array.Empty<Type>())
        !.MakeGenericMethod(typeof(DateTime));

    private static readonly MethodInfo EmptyArrayString = typeof(Array)
        .GetRuntimeMethod(nameof(Array.Empty), Array.Empty<Type>())
        !.MakeGenericMethod(typeof(String));

    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public ClickHouseArrayTranslator(ISqlExpressionFactory sqlExpressionFactory)
    {
        _sqlExpressionFactory = sqlExpressionFactory;
    }

    public SqlExpression Translate(SqlExpression instance, MemberInfo member, Type returnType, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        return instance != null && instance.Type.IsArray &&
               member.Name == nameof(Array.Length)
            ? _sqlExpressionFactory.Function(
                name: "length",
                arguments: new[] { instance },
                nullable: true,
                argumentsPropagateNullability: new[] { true },
                returnType: returnType)
            : null;
    }

    public SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (method.Equals(EmptyArrayUInt8))
        {
            return _sqlExpressionFactory.Function(
                name: "emptyArrayUInt8",
                arguments: new SqlExpression[0],
                nullable: false,
                argumentsPropagateNullability: new[] { true },
                returnType: typeof(byte[]));
        }

        if (method.Equals(EmptyArrayUInt16))
        {
            return _sqlExpressionFactory.Function(
                name: "emptyArrayUInt16",
                arguments: new SqlExpression[0],
                nullable: false,
                argumentsPropagateNullability: new[] { true },
                returnType: typeof(ushort[]));
        }
        
        if (method.Equals(EmptyArrayUInt32))
        {
            return _sqlExpressionFactory.Function(
                name: "emptyArrayUInt32",
                arguments: new SqlExpression[0],
                nullable: false,
                argumentsPropagateNullability: new[] { true },
                returnType: typeof(uint[]));
        }
        
        if (method.Equals(EmptyArrayUInt64))
        {
            return _sqlExpressionFactory.Function(
                name: "emptyArrayUInt64",
                arguments: new SqlExpression[0],
                nullable: false,
                argumentsPropagateNullability: new[] { true },
                returnType: typeof(ulong[]));
        }
        
        if (method.Equals(EmptyArrayInt8))
        {
            return _sqlExpressionFactory.Function(
                name: "emptyArrayInt8",
                arguments: new SqlExpression[0],
                nullable: false,
                argumentsPropagateNullability: new[] { true },
                returnType: typeof(sbyte[]));
        }
        
        if (method.Equals(EmptyArrayInt16))
        {
            return _sqlExpressionFactory.Function(
                name: "emptyArrayInt16",
                arguments: new SqlExpression[0],
                nullable: false,
                argumentsPropagateNullability: new[] { true },
                returnType: typeof(short[]));
        }
        
        if (method.Equals(EmptyArrayInt32))
        {
            return _sqlExpressionFactory.Function(
                name: "emptyArrayInt32",
                arguments: new SqlExpression[0],
                nullable: false,
                argumentsPropagateNullability: new[] { true },
                returnType: typeof(int[]));
        }
        
        if (method.Equals(EmptyArrayInt64))
        {
            return _sqlExpressionFactory.Function(
                name: "emptyArrayInt64",
                arguments: new SqlExpression[0],
                nullable: false,
                argumentsPropagateNullability: new[] { true },
                returnType: typeof(long[]));
        }
        
        if (method.Equals(EmptyArrayFloat32))
        {
            return _sqlExpressionFactory.Function(
                name: "emptyArrayFloat32",
                arguments: new SqlExpression[0],
                nullable: false,
                argumentsPropagateNullability: new[] { true },
                returnType: typeof(float[]));
        }
        
        if (method.Equals(EmptyArrayFloat64))
        {
            return _sqlExpressionFactory.Function(
                name: "emptyArrayFloat64",
                arguments: new SqlExpression[0],
                nullable: false,
                argumentsPropagateNullability: new[] { true },
                returnType: typeof(double[]));
        }
        
        if (method.Equals(EmptyArrayDateTime))
        {
            return _sqlExpressionFactory.Function(
                name: "emptyArrayDateTime",
                arguments: new SqlExpression[0],
                nullable: false,
                argumentsPropagateNullability: new[] { true },
                returnType: typeof(DateTime[]));
        }
        
        if (method.Equals(EmptyArrayString))
        {
            return _sqlExpressionFactory.Function(
                name: "emptyArrayString",
                arguments: new SqlExpression[0],
                nullable: false,
                argumentsPropagateNullability: new[] { true },
                returnType: typeof(string[]));
        }
        
        return null;
    }
}
