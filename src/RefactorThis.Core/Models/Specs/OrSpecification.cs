using System;
using System.Linq;
using System.Linq.Expressions;
using RefactorThis.Core.Interfaces;

namespace RefactorThis.Core.Models.Specs
{
    public class OrSpecification : CompositeSpecification
    {
        public OrSpecification(ISpecification<Product> left, ISpecification<Product> right) : base(left, right) { }


        public override Expression<Func<Product, bool>> Criteria
        {
            get
            {
                var orSpecification = Expression.OrElse(_leftSpec.Criteria.Body, _rightSpec.Criteria.Body);
                return Expression.Lambda<Func<Product, bool>>(orSpecification, _leftSpec.Criteria.Parameters.Single());
            }

        }
    }
}
