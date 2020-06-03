using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using RefactorThis.Core.Models;
using RefactorThis.UnitTests.Fixtures;
using Xunit;

namespace RefactorThis.UnitTests
{
    public class ProductsControllerShould
    {
        [Fact]
        public async Task ReturnBadRequest_WhenProductIdInPayloadAndPathDontMatch()
        {
            // Arrange
            var fixture = new ProductsControllerTestFixture();
            var request2Test = new UpdateProductRequest
            {
                Id = Guid.Parse("45ed1c70-4e21-4f2d-960e-4e0c9a301f42"),
                Name = "Shoe",
                Description = "Spy phone",
                Price = 1,
                DeliveryPrice = 2
            };

            // Act
            var response = await fixture.SUT().UpdateProduct(Guid.Parse("1fe9708e-d2b2-4ca7-8b20-627d41acad68"), request2Test);

            // Assert
            response.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async Task ReturnBadRequest_WhenProductOptionIdInPayloadAndPathDontMatch()
        {
            // Arrange
            var fixture = new ProductsControllerTestFixture();
            var request2Test = new UpdateProductOptionRequest
            {
                Id = Guid.Parse("45ed1c70-4e21-4f2d-960e-4e0c9a301f42"),
                Name = "Shoe",
                Description = "Spy phone"
            };
            // Act
            var response = await fixture.SUT().UpdateProductOption(
                Guid.Parse("90201b31-96a9-4371-9073-3500251b6765"),
                Guid.Parse("1fe9708e-d2b2-4ca7-8b20-627d41acad68"),
                request2Test);

            // Assert
            response.Should().BeOfType(typeof(BadRequestObjectResult));
        }
    }
}