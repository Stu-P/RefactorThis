using System;
using System.Linq.Expressions;

namespace RefactorThis.Core.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
    }
}
