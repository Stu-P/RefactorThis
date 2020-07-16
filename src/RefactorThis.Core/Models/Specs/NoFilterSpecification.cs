using System;
using System.Linq.Expressions;
using RefactorThis.Core.Interfaces;

namespace RefactorThis.Core.Models.Specs
{
    public class NoFilterSpecification : ISpecification<Product>
    {
        public NoFilterSpecification()
        {
        }

        public Expression<Func<Product, bool>> Criteria => p => true;
    }
}
