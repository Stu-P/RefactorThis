using System;
using System.Linq.Expressions;
using RefactorThis.Core.Interfaces;

namespace RefactorThis.Core.Models.Specs
{
    public class ProductDescriptionSpecification : ISpecification<Product>
    {
        private string _descFilter { get; set; }


        public ProductDescriptionSpecification(string descFilter)
        {
            _descFilter = descFilter;
        }

        public Expression<Func<Product, bool>> Criteria => p => p.Description.Contains(_descFilter);
    }
}
