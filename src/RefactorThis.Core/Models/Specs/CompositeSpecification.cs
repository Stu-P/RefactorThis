using System;
using System.Linq;
using System.Linq.Expressions;
using RefactorThis.Core.Interfaces;

namespace RefactorThis.Core.Models.Specs
{
    public abstract class CompositeSpecification : ISpecification<Product>
    {
        protected ISpecification<Product> _leftSpec;
        protected ISpecification<Product> _rightSpec;


        public CompositeSpecification(ISpecification<Product> left, ISpecification<Product> right)
        {
            _leftSpec = left;
            _rightSpec = right;
        }

        public abstract Expression<Func<Product, bool>> Criteria { get; }
    }
}
