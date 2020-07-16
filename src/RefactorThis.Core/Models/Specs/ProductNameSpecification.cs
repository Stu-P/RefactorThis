using System;
using System.Linq.Expressions;
using RefactorThis.Core.Interfaces;

namespace RefactorThis.Core.Models.Specs
{
    public class ProductNameSpecification : ISpecification<Product>
    {
        private string _nameFilter { get; set; }


        public ProductNameSpecification(string nameFilter)
        {
            _nameFilter = nameFilter;
        }

        public Expression<Func<Product, bool>> Criteria => p => p.Name.Contains(_nameFilter);
    }
}
