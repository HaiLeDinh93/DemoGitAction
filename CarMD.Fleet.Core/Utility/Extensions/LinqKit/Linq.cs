using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace CarMD.Fleet.Core.Utility.Extensions.LinqKit
{
    /// <summary>
    /// Another good idea by Tomas Petricek.
    /// See http://tomasp.net/blog/dynamic-linq-queries.aspx for information on how it's used.
    /// </summary>
    public static class Linq
    {
        // Returns the given anonymous method as a lambda expression
        public static Expression<Func<T, TResult>> Expr<T, TResult>(Expression<Func<T, TResult>> expr)
        {
            return expr;
        }

        // Returns the given anonymous function as a Func delegate
        public static Func<T, TResult> Func<T, TResult>(Func<T, TResult> expr)
        {
            return expr;
        }

        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            var enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
            var relationalCommandCache = enumerator.Private("_relationalCommandCache");
            var selectExpression = relationalCommandCache.Private<SelectExpression>("_selectExpression");
            var factory = relationalCommandCache.Private<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory");

            var sqlGenerator = factory.Create();
            var command = sqlGenerator.GetCommand(selectExpression);

            string sql = command.CommandText;
            return sql;
        }

        private static object Private(this object obj, string privateField) => obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        private static T Private<T>(this object obj, string privateField) => (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
    }
}