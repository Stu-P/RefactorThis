using Microsoft.Extensions.Logging;
using Moq;
using RefactorThis.Controllers;
using RefactorThis.Core.Interfaces;

namespace RefactorThis.UnitTests.Fixtures
{
    public class ProductsControllerTestFixture
    {
        public Mock<IProductService> MockProductService { get; }
        public Mock<ILogger<ProductsController>> MockLogger { get; }

        public ProductsControllerTestFixture()
        {
            MockProductService = new Mock<IProductService>();
            MockLogger = new Mock<ILogger<ProductsController>>();
        }

        public ProductsController SUT()
        {
            return new ProductsController(MockProductService.Object, MockLogger.Object);
        }
    }
}