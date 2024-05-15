using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ClickHouse.EntityFrameworkCore.Query.Internal
{
    public class ClickHouseStringTranslator : IMethodCallTranslator, IMemberTranslator
    {
        private static readonly MethodInfo ToUpper = typeof(string)
            .GetTypeInfo()
            .GetRuntimeMethod(nameof(string.ToUpper), Array.Empty<Type>());

        private static readonly MethodInfo ToLower = typeof(string)
            .GetTypeInfo()
            .GetRuntimeMethod(nameof(string.ToLower), Array.Empty<Type>());

        private static readonly MethodInfo IsNullOrEmpty = typeof(string)
            .GetTypeInfo()
            .GetRuntimeMethod(nameof(string.IsNullOrEmpty), new [] { typeof(string) });

        private static readonly PropertyInfo Length = typeof(string)
            .GetTypeInfo()
            .GetRuntimeProperty(nameof(string.Length));

        private static readonly MethodInfo TrimStart = typeof(string)
            .GetRuntimeMethod(nameof(string.TrimStart), Array.Empty<Type>());

        private static readonly MethodInfo TrimEnd = typeof(string)
            .GetRuntimeMethod(nameof(string.TrimEnd), Array.Empty<Type>());

        private static readonly MethodInfo Trim = typeof(string)
            .GetRuntimeMethod(nameof(string.Trim), Array.Empty<Type>());

        private static readonly MethodInfo StartsWith = typeof(string)
            .GetRuntimeMethod(nameof(string.StartsWith), new[] { typeof(string) });

        private static readonly MethodInfo EndsWith = typeof(string)
            .GetRuntimeMethod(nameof(string.EndsWith), new[] { typeof(string) });

        private static readonly MethodInfo Contains = typeof(string)
            .GetRuntimeMethod(nameof(string.Contains), new[] { typeof(string) });

        private readonly ISqlExpressionFactory _sqlExpressionFactory;
        
        public ClickHouseStringTranslator([NotNull]ISqlExpressionFactory sqlExpressionFactory)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        public SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
        {
            if (ToLower.Equals(method))
            {
                return _sqlExpressionFactory.Function(
                    "lowerUTF8",
                    new[] { instance },
                    true,
                    new[] { true },
                    method.ReturnType,
                    instance.TypeMapping);
            }

            if (ToUpper.Equals(method))
            {
                return _sqlExpressionFactory.Function(
                    "upperUTF8",
                    new[] { instance },
                    true,
                    new[] { true },
                    method.ReturnType,
                    instance.TypeMapping);
            }

            if (IsNullOrEmpty.Equals(method))
            {
                return _sqlExpressionFactory.Function(
                    "empty",
                    arguments.ToArray(),
                    false,
                    new [] { true },
                    method.ReturnType,
                    arguments[0].TypeMapping);
            }

            if (TrimStart.Equals(method))
            {
                return _sqlExpressionFactory.Function(
                    name: "trimLeft",
                    arguments: new [] { instance },
                    nullable: true,
                    argumentsPropagateNullability: new [] { true },
                    returnType: method.ReturnType);
            }

            if (TrimEnd.Equals(method))
            {
                return _sqlExpressionFactory.Function(
                    name: "trimRight",
                    arguments: new [] { instance },
                    nullable: true,
                    argumentsPropagateNullability: new [] { true },
                    returnType: method.ReturnType);
            }

            if (Trim.Equals(method))
            {
                return _sqlExpressionFactory.Function(
                    name: "trim",
                    arguments: new [] { instance },
                    nullable: true,
                    argumentsPropagateNullability: new [] { true },
                    returnType: method.ReturnType);
            }

            if (StartsWith.Equals(method))
            {
                return _sqlExpressionFactory.Function(
                    name: "startsWith",
                    arguments: arguments.Prepend(instance),
                    nullable: true,
                    argumentsPropagateNullability: new[] { true },
                    returnType: method.ReturnType);
            }

            if (EndsWith.Equals(method))
            {
                return _sqlExpressionFactory.Function(
                    name: "endsWith",
                    arguments: arguments.Prepend(instance),
                    nullable: true,
                    argumentsPropagateNullability: new[] { true },
                    returnType: method.ReturnType);
            }
            if (Contains.Equals(method))
            {
                return _sqlExpressionFactory.Function(
                    name: "endsWith",
                    arguments: arguments.Prepend(instance),
                    nullable: true,
                    argumentsPropagateNullability: new[] { true },
                    returnType: method.ReturnType);
            }
            return null;
        }

        public SqlExpression Translate(SqlExpression instance, MemberInfo member, Type returnType, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
        {
            if (member.Equals(Length))
            {
                return _sqlExpressionFactory.Function(
                    "char_length",
                    new[] { instance },
                    nullable: true,
                    argumentsPropagateNullability: new[] { true },
                    returnType);
            }

            return null;
        }
    }
}
