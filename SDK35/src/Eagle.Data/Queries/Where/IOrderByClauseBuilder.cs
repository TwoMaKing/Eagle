using System;
using System.Linq.Expressions;

namespace Eagle.Data.Queries
{
    public interface IOrderByClauseBuilder<T> where T : class, new()
    {
        string BuildOrderByClause(Expression<Func<T, object>> expression);
    }
}
