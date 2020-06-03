using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using RefactorThis.Core.Interfaces;
using RefactorThis.Core.Mappers;
using RefactorThis.Core.Services;

namespace RefactorThis.UnitTests.Fixtures
{
    public class ProductServiceTestFixture
    {
        public Mock<IProductRepository> MockRepository { get; }
        public Mock<IKeyGenerator> MockKeyGenerator { get; }
        public Mock<ILogger<ProductService>> MockLogger { get; }

        private IMapper _mapper;

        public ProductServiceTestFixture()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductRequestMapping>();
            });
            MockKeyGenerator = new Mock<IKeyGenerator>();
            MockRepository = new Mock<IProductRepository>();
            _mapper = new Mapper(config);
            MockLogger = new Mock<ILogger<ProductService>>();
        }

        public ProductService SUT()
        {
            return new ProductService(MockRepository.Object, MockKeyGenerator.Object, _mapper, MockLogger.Object);
        }
    }
}