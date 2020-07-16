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
            if (!string.IsNullOrWhiteSpace(searchCritera.Name))
            {
                return new ProductNameSpecification(searchCritera.Name);
            }

            return new NoFilterSpecification();

        }
    }
}
