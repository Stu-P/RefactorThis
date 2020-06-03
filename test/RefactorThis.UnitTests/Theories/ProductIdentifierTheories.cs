using System;
using RefactorThis.Core.Models;
using RefactorThis.Tests.Common.Builders;

namespace RefactorThis.UnitTests.Theories
{
    public class ProductOptionCreateTheories : Xunit.TheoryData<string, ProductOptionCreateTheoryData>
    {
        public ProductOptionCreateTheories()
        {
            ValidRequestForProductWithNoExistingOptions();
        }

        private void ValidRequestForProductWithNoExistingOptions()
        {
            var initial = new ProductBuilder().WithId(Guid.Parse("7928ca40-03b6-4859-b64d-f0ba26bc3de2"))
                                    .WithName("Samsung Galaxy S10")
                                    .WithDescription("Newest phone from Samsung")
                                    .WithPrice(1001.99m)
                                    .WithDeliveryPrice(25.01m);

            Add(nameof(ValidRequestForProductWithNoExistingOptions),
                new ProductOptionCreateTheoryData
                {
                    Request = new CreateProductOptionRequest
                    {
                        Name = "Red",
                        Description = "Available now"
                    },
                    Initial = initial,
                    Subsequent = initial.WithOption(
                                            new ProductOptionBuilder()
                                            .WithId(Guid.Parse("7928ca40-03b6-4859-b64d-f0ba26bc3de2"))
                                            .WithName("Red")
                                            .WithDescription("Available now")
                                            )
                });
        }
    }

    public class ProductOptionCreateTheoryData
    {
        public CreateProductOptionRequest Request { get; set; }
        public Product Initial { get; set; }
        public Product Subsequent { get; set; }
    }
}