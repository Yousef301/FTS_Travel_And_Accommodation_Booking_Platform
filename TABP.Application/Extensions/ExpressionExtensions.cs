using System.Linq.Expressions;

namespace TABP.Application.Extensions;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftBody = ReplaceParameter(left.Body, left.Parameters[0], parameter);
        var rightBody = ReplaceParameter(right.Body, right.Parameters[0], parameter);

        var body = Expression.AndAlso(leftBody, rightBody);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private static Expression ReplaceParameter(Expression expression, ParameterExpression sourceParameter,
        ParameterExpression targetParameter)
    {
        return new ParameterRebinder(sourceParameter, targetParameter).Visit(expression);
    }

    private class ParameterRebinder : ExpressionVisitor
    {
        private readonly ParameterExpression _sourceParameter;
        private readonly ParameterExpression _targetParameter;

        public ParameterRebinder(ParameterExpression sourceParameter, ParameterExpression targetParameter)
        {
            _sourceParameter = sourceParameter;
            _targetParameter = targetParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _sourceParameter ? _targetParameter : base.VisitParameter(node);
        }
    }
}