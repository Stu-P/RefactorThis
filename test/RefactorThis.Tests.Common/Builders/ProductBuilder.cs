using System;
using RefactorThis.Core.Models;

namespace RefactorThis.Tests.Common.Builders
{
    public class ProductBuilder : Product
    {
        public ProductBuilder WithId(Guid value)
        {
            Id = value;
            return this;
        }

        public ProductBuilder WithName(string value)
        {
            Name = value;
            return this;
        }

        public ProductBuilder WithDescription(string value)
        {
            Description = value;
            return this;
        }

        public ProductBuilder WithPrice(decimal value)
        {
            Price = value;
            return this;
        }

        public ProductBuilder WithDeliveryPrice(decimal value)
        {
            DeliveryPrice = value;
            return this;
        }

        public ProductBuilder WithOption(ProductOptionBuilder value)
        {
            Options.Add(value);
            return this;
        }
    }
}