﻿using System;
using System.Linq.Expressions;

namespace Csla.Core
{
  internal class InnermostWhereFinder : ExpressionVisitor
  {
    private MethodCallExpression innermostWhereExpression;

    public MethodCallExpression GetInnermostWhere(Expression expression)
    {
      Visit(expression);
      return innermostWhereExpression;
    }

    protected override Expression VisitMethodCall(MethodCallExpression expression)
    {
      if (expression.Method.Name == "Where")
        innermostWhereExpression = expression;
      //{
      //  innermostWhereExpression = expression;
      //  return expression;
      //}

      Visit(expression.Arguments[0]);

      return expression;
    }
  }

  internal class InnermostOrderByFinder : ExpressionVisitor
  {
    private MethodCallExpression innermostOrderByExpression;

    public MethodCallExpression GetInnermostOrderBy(Expression expression)
    {
      Visit(expression);
      return innermostOrderByExpression;
    }

    protected override Expression VisitMethodCall(MethodCallExpression expression)
    {
      if (expression.Method.Name.StartsWith("OrderBy"))
        innermostOrderByExpression = expression;
      //{
      //  innermostWhereExpression = expression;
      //  return expression;
      //}

      Visit(expression.Arguments[0]);

      return expression;
    }
  }
}
