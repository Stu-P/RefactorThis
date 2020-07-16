using System;
using System.Linq;
using System.Linq.Expressions;
using RefactorThis.Core.Interfaces;

namespace RefactorThis.Core.Models.Specs
{
    public class AndSpecification : CompositeSpecification
    {
        public AndSpecification(ISpecification<Product> left, ISpecification<Product> right) : base(left, right)
        {
        }

        public override Expression<Func<Product, bool>> Criteria
        {
            get
            {
                BinaryExpression andExpression = Expression.AndAlso(_leftSpec.Criteria.Body, _rightSpec.Criteria.Body);

                return Expression.Lambda<Func<Product, bool>>(andExpression, _leftSpec.Criteria.Parameters.Single());
            }
        }
    }
}
