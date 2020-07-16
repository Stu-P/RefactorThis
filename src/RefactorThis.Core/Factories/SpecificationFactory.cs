using System;
using RefactorThis.Core.Interfaces;
using RefactorThis.Core.Models;
using RefactorThis.Core.Models.Specs;

namespace RefactorThis.Core.Factories
{
    public class SpecificationFactory : ISpecificationFactory<Product>
    {
        public SpecificationFactory()
        {
        }

        public ISpecification<Product> Create(SearchCritera searchCritera)
        {
            ISpecification<Product> spec = new NoFilterSpecification();

            if (!string.IsNullOrWhiteSpace(searchCritera.Name))
            {
                spec = new AndSpecification(spec, new ProductNameSpecification(searchCritera.Name));
            }
            if (searchCritera.MinPrice.HasValue)
            {
                spec = new AndSpecification(spec, new MinPriceSpecification(searchCritera.MinPrice.Value));
            }

            return spec;

        }
    }
}
