using Microsoft.Extensions.Logging;
using Moq;
using RefactorThis.Controllers;
using RefactorThis.Core.Factories;
using RefactorThis.Core.Interfaces;
using RefactorThis.Core.Models;

namespace RefactorThis.UnitTests.Fixtures
{
    public class ProductsControllerTestFixture
    {
        public Mock<IProductService> MockProductService { get; }
        public Mock<ILogger<ProductsController>> MockLogger { get; }
        private ISpecificationFactory<Product> _specificationFactory;

        public ProductsControllerTestFixture()
        {
            MockProductService = new Mock<IProductService>();
            _specificationFactory = new SpecificationFactory();
            MockLogger = new Mock<ILogger<ProductsController>>();
        }

        public ProductsController SUT()
        {
            return new ProductsController(MockProductService.Object, _specificationFactory, MockLogger.Object);
        }
    }
}