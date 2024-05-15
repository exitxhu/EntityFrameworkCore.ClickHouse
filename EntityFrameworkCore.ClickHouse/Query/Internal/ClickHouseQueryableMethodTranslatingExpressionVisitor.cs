using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ClickHouse.EntityFrameworkCore.Query.Internal;

public class ClickHouseQueryableMethodTranslatingExpressionVisitor
    : RelationalQueryableMethodTranslatingExpressionVisitor
{
    private readonly RelationalSqlTranslatingExpressionVisitor _sqlTranslator;

    public ClickHouseQueryableMethodTranslatingExpressionVisitor(
        QueryableMethodTranslatingExpressionVisitorDependencies dependencies, 
        RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies,
        QueryCompilationContext queryCompilationContext) : base(dependencies, relationalDependencies, queryCompilationContext)
    {
        _sqlTranslator =
            relationalDependencies.RelationalSqlTranslatingExpressionVisitorFactory.Create(queryCompilationContext,
                this);
    }

    protected override Expression VisitConstant(ConstantExpression constantExpression)
    {
        return base.VisitConstant(constantExpression);
    }

    protected override ShapedQueryExpression TranslateTake(ShapedQueryExpression source, Expression count)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (count == null)
        {
            throw new ArgumentNullException(nameof(count));
        }

        var selectExpression = (SelectExpression)source.QueryExpression;
        var translation = _sqlTranslator.Translate(count);

        if (translation != null)
        {
            selectExpression.ApplyLimit(translation);

            return source;
        }

        return null;
    }
}