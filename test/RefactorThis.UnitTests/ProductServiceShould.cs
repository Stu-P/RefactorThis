using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using Moq;
using RefactorThis.Core.Exceptions;
using RefactorThis.Core.Models;
using RefactorThis.UnitTests.Fixtures;
using RefactorThis.UnitTests.Theories;
using Xunit;

namespace RefactorThis.UnitTests
{
    public class ProductServiceShould
    {
        [Fact]
        public async Task MapCreateProductRequestToEntity_AndAssignUniqueIdentifier()
        {
            // Arrange
            var fixture = new ProductServiceTestFixture();
            var request = new CreateProductRequest
            {
                Name = "iPhone",
                Description = "think different",
                Price = 1,
                DeliveryPrice = 1
            };
            var testGuid = Guid.Parse("085cba18-79ed-46e3-9c50-8607e20e351e");

            fixture.MockKeyGenerator.Setup(x => x.NewKey()).Returns(testGuid);

            // Act
            var result = await fixture.SUT().CreateProduct(request);

            // Assert
            result.Id.IsSameOrEqualTo(testGuid);

            fixture.MockRepository.Verify(x => x.AddProduct(It.Is<Product>(p =>
                 p.Id.IsSameOrEqualTo(testGuid))), Times.Once());
        }

        [Theory]
        [ClassData(typeof(ProductOptionCreateTheories))]
        public async Task MapCreateOptionRequestToEntity_AndAssignUniqueIdentifier(string description, ProductOptionCreateTheoryData theory)
        {
            // Arrange
            var fixture = new ProductServiceTestFixture();
            var testGuid = Guid.Parse("085cba18-79ed-46e3-9c50-8607e20e351e");

            fixture.MockKeyGenerator.Setup(x => x.NewKey()).Returns(testGuid);
            fixture.MockRepository.Setup(x => x.GetProductById(testGuid, It.IsAny<bool>())).ReturnsAsync(theory.Initial);

            // Act
            var result = await fixture.SUT().AddOptionToProduct(testGuid, theory.Request);

            // Assert
            result.Id.IsSameOrEqualTo(testGuid);
            fixture.MockRepository.Verify(x => x.UpdateProduct(It.Is<Product>(p =>
                 p.IsSameOrEqualTo(theory.Subsequent))), Times.Once(), $"scenario {description}");
        }

        [Fact]
        public async Task ThrowEntityFoundExceptionOnProductUpdate_IfNoExistingEntityFound()
        {
            // Arrange
            var fixture = new ProductServiceTestFixture();
            var testGuid = Guid.Parse("085cba18-79ed-46e3-9c50-8607e20e351e");

            var request = new UpdateProductRequest
            {
                Id = testGuid,
                Name = "iPhone",
                Description = "think different",
                Price = 1,
                DeliveryPrice = 1
            };

            fixture.MockRepository.Setup(x => x.GetProductById(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync((Product)null);
            fixture.MockKeyGenerator.Setup(x => x.NewKey()).Returns(testGuid);

            // Act + Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(async () => await fixture.SUT().UpdateProduct(request));
        }

        [Fact]
        public async Task ReturnEmptyCollectionOnGetProductOptions_IfProductOptionsNull()
        {
            // Arrange
            var fixture = new ProductServiceTestFixture();
            var testGuid = Guid.Parse("085cba18-79ed-46e3-9c50-8607e20e351e");

            var product = new Product
            {
                Id = testGuid,
                Name = "iPhone",
                Description = "think different",
                Price = 1,
                DeliveryPrice = 1,
                Options = null
            };

            fixture.MockRepository.Setup(x => x.GetProductById(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(product);


            // Act
            var actual = await fixture.SUT().GetProductOptions(testGuid);

            // Assert
            actual.Should().NotBeNull();
            actual.Should().BeEmpty();
        }
    }
}