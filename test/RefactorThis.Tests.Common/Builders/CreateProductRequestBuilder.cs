using RefactorThis.Core.Models;

namespace RefactorThis.Tests.Common.Builders
{
    public class CreateProductRequestBuilder : CreateProductRequest
    {
        public CreateProductRequestBuilder WithName(string value)
        {
            Name = value;
            return this;
        }

        public CreateProductRequestBuilder WithDescription(string value)
        {
            Description = value;
            return this;
        }

        public CreateProductRequestBuilder WithPrice(decimal value)
        {
            Price = value;
            return this;
        }

        public CreateProductRequestBuilder WithDeliveryPrice(decimal value)
        {
            DeliveryPrice = value;
            return this;
        }
    }
}