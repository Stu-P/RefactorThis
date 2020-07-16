using System;
using System.Linq.Expressions;
using RefactorThis.Core.Interfaces;

namespace RefactorThis.Core.Models.Specs
{
    public class MinPriceSpecification : ISpecification<Product>
    {
        private decimal _minPrice;
        public MinPriceSpecification(decimal minPrice)
        {
            _minPrice = minPrice;
        }

        public Expression<Func<Product, bool>> Criteria => p => p.Price > _minPrice;
    }
}
