using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Away.App.Core.Utils;

/// <summary> The Predicate Operator </summary>
public enum PredicateOperator
{
    /// <summary> The "Or" </summary>
    Or,

    /// <summary> The "And" </summary>
    And
}

/// <summary>
/// See http://www.albahari.com/expressions for information and examples.
/// </summary>
public static class CondBuilder
{
    private class RebindParameterVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter;
        private readonly ParameterExpression _newParameter;

        public RebindParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node == _oldParameter)
            {
                return _newParameter;
            }

            return base.VisitParameter(node);
        }
    }

    /// <summary> Start an expression </summary>
    [Pure]
    public static ExpressionStarter<T> New<T>()
    { return new ExpressionStarter<T>(); }

    /// <summary> Start an expression </summary>
    /// <param name="expression">Expression to be used when starting the builder.</param>
    [Pure]
    public static ExpressionStarter<T> New<T>(Expression<Func<T, bool>> expression)
    { return new ExpressionStarter<T>(expression); }

    /// <summary> Create an expression with a stub expression true or false to use when the expression is not yet started. </summary>
    [Pure]
    public static ExpressionStarter<T> New<T>(bool defaultExpression)
    { return new ExpressionStarter<T>(defaultExpression); }

    /// <summary> OR </summary>
    [Pure]
    public static Expression<Func<T, bool>> Or<T>([NotNull] this Expression<Func<T, bool>> expr1, [NotNull] Expression<Func<T, bool>> expr2)
    {
        var expr2Body = new RebindParameterVisitor(expr2.Parameters[0], expr1.Parameters[0]).Visit(expr2.Body);
        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, expr2Body), expr1.Parameters);
    }

    /// <summary> AND </summary>
    [Pure]
    public static Expression<Func<T, bool>> And<T>([NotNull] this Expression<Func<T, bool>> expr1, [NotNull] Expression<Func<T, bool>> expr2)
    {
        var expr2Body = new RebindParameterVisitor(expr2.Parameters[0], expr1.Parameters[0]).Visit(expr2.Body);
        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, expr2Body), expr1.Parameters);
    }

    /// <summary> NOT </summary>
    [Pure]
    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
        => Expression.Lambda<Func<T, bool>>(Expression.Not(expr.Body), expr.Parameters);

    /// <summary>
    /// Extends the specified source Predicate with another Predicate and the specified PredicateOperator.
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    /// <param name="first">The source Predicate.</param>
    /// <param name="second">The second Predicate.</param>
    /// <param name="operator">The Operator (can be "And" or "Or").</param>
    /// <returns>Expression{Func{T, bool}}</returns>
    [Pure]
    public static Expression<Func<T, bool>> Extend<T>([NotNull] this Expression<Func<T, bool>> first, [NotNull] Expression<Func<T, bool>> second, PredicateOperator @operator = PredicateOperator.Or)
    {
        return @operator == PredicateOperator.Or ? first.Or(second) : first.And(second);
    }

    /// <summary>
    /// Extends the specified source Predicate with another Predicate and the specified PredicateOperator.
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    /// <param name="first">The source Predicate.</param>
    /// <param name="second">The second Predicate.</param>
    /// <param name="operator">The Operator (can be "And" or "Or").</param>
    /// <returns>Expression{Func{T, bool}}</returns>
    [Pure]
    public static Expression<Func<T, bool>> Extend<T>([NotNull] this ExpressionStarter<T> first, [NotNull] Expression<Func<T, bool>> second, PredicateOperator @operator = PredicateOperator.Or)
    {
        return @operator == PredicateOperator.Or ? first.Or(second) : first.And(second);
    }

    public static Expression<Func<T, bool>> And<T>([NotNull] this Expression<Func<T, bool>> expr1, [NotNull] bool isAdd, [NotNull] Expression<Func<T, bool>> expr2)
    {
        return isAdd ? expr1.And(expr2) : expr1;
    }
}

public class ExpressionStarter<T>
{
    internal ExpressionStarter() : this(false)
    {
    }

    internal ExpressionStarter(bool defaultExpression)
    {
        if (defaultExpression)
            DefaultExpression = f => true;
        else
            DefaultExpression = f => false;
    }

    internal ExpressionStarter(Expression<Func<T, bool>> exp) : this(false)
    {
        _predicate = exp;
    }

    /// <summary>The actual Predicate. It can only be set by calling Start.</summary>
    private Expression<Func<T, bool>> Predicate => IsStarted || !UseDefaultExpression ? _predicate : DefaultExpression;

    private Expression<Func<T, bool>> _predicate = null!;

    /// <summary>Determines if the predicate is started.</summary>
    public bool IsStarted => _predicate != null;

    /// <summary> A default expression to use only when the expression is null </summary>
    public bool UseDefaultExpression => DefaultExpression != null;

    /// <summary>The default expression</summary>
    public Expression<Func<T, bool>> DefaultExpression { get; set; }

    /// <summary>Set the Expression predicate</summary>
    /// <param name="exp">The first expression</param>
    public Expression<Func<T, bool>> Start(Expression<Func<T, bool>> exp)
    {
        if (IsStarted)
            throw new Exception("Predicate cannot be started again.");

        return _predicate = exp;
    }

    /// <summary>Or</summary>
    [Pure]
    public Expression<Func<T, bool>> Or([NotNull] Expression<Func<T, bool>> expr2)
    {
        return IsStarted ? _predicate = Predicate.Or(expr2) : Start(expr2);
    }

    /// <summary>And</summary>
    [Pure]
    public Expression<Func<T, bool>> And([NotNull] Expression<Func<T, bool>> expr2)
    {
        return IsStarted ? _predicate = Predicate.And(expr2) : Start(expr2);
    }

    public Expression<Func<T, bool>> And([NotNull] bool isAdd, [NotNull] Expression<Func<T, bool>> expr2)
    {
        if (!isAdd)
        {
            return _predicate = Predicate;
        }
        return IsStarted ? _predicate = Predicate.And(expr2) : Start(expr2);
    }
}