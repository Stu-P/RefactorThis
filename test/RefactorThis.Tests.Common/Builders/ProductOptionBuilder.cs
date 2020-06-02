using System;
using RefactorThis.Core.Models;

namespace RefactorThis.Tests.Common.Builders
{
    public class ProductOptionBuilder : ProductOption
    {
        public ProductOptionBuilder WithId(Guid value)
        {
            Id = value;
            return this;
        }

        public ProductOptionBuilder WithProductId(Guid value)
        {
            ProductId = value;
            return this;
        }

        public ProductOptionBuilder WithName(string value)
        {
            Name = value;
            return this;
        }

        public ProductOptionBuilder WithDescription(string value)
        {
            Description = value;
            return this;
        }
    }
}