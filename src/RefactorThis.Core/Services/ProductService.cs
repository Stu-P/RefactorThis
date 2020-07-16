using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RefactorThis.Core.Exceptions;
using RefactorThis.Core.Interfaces;
using RefactorThis.Core.Models;

namespace RefactorThis.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IKeyGenerator _keyGenerator;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepo, IKeyGenerator keyGenerator, IMapper mapper, ILogger<ProductService> logger)
        {
            _productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
            _keyGenerator = keyGenerator ?? throw new ArgumentNullException(nameof(keyGenerator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Product>> GetProducts(ISpecification<Product> spec)
        {
            var timer = Stopwatch.StartNew();
            var allMatchingProducts = await _productRepo.GetProducts(spec);
            _logger.LogInformation("GetProducts returned {count} products after {responseTime} ms", allMatchingProducts.Count(), timer.ElapsedMilliseconds);
            return allMatchingProducts ?? new List<Product>(); ;
        }

        public async Task<Product> GetProduct(Guid productId) => await GetProductIfExists(productId);

        public async Task<Product> CreateProduct(CreateProductRequest newProductRequest)
        {
            Product newProduct = _mapper.Map<Product>(newProductRequest);
            newProduct.Id = _keyGenerator.NewKey();
            await _productRepo.AddProduct(newProduct);
            return newProduct;
        }

        public async Task UpdateProduct(UpdateProductRequest updateProductRequest)
        {
            Product existing = await GetProductIfExists(updateProductRequest.Id)
                ?? throw new EntityNotFoundException("No product found with prodict id {productId}"); ;

            _mapper.Map(updateProductRequest, existing);
            await _productRepo.UpdateProduct(existing);
        }

        public async Task DeleteProduct(Guid productId)
        {
            Product productToDelete = await GetProductIfExists(productId);
            await _productRepo.DeleteProduct(productToDelete);
        }

        public async Task<IEnumerable<ProductOption>> GetProductOptions(Guid productId)
        {
            var timer = Stopwatch.StartNew();
            Product product = await _productRepo.GetProductById(productId, true) ??
                throw new EntityNotFoundException("No product found with prodict id {productId}");

            _logger.LogInformation("Returned {count} options for product {productId} after {responseTime} ms", product.Options?.Count(), productId, timer.ElapsedMilliseconds);
            return product?.Options ?? new List<ProductOption>();
        }

        public async Task<ProductOption> GetProductOption(Guid productId, Guid optionId) =>
            await _productRepo.GetProductOptionById(productId, optionId) ??
                throw new EntityNotFoundException($"No product option found matching product id {productId} and option id {optionId}");

        public async Task<ProductOption> AddOptionToProduct(Guid productId, CreateProductOptionRequest newOptionRequest)
        {
            Product productToUpdate = await _productRepo.GetProductById(productId, true) ??
                throw new EntityNotFoundException("No product found with prodict id {productId}");

            ProductOption newOption = _mapper.Map<ProductOption>(newOptionRequest);
            newOption.Id = _keyGenerator.NewKey();
            productToUpdate.Options.Add(newOption);
            await _productRepo.UpdateProduct(productToUpdate);
            return newOption;
        }

        public async Task UpdateProductOption(Guid productId, UpdateProductOptionRequest updatedOptionRequest)
        {
            Product productToUpdate = await _productRepo.GetProductById(productId, true);
            ProductOption currentOption = productToUpdate?.Options?.FirstOrDefault(x => x.Id == updatedOptionRequest.Id) ??
                throw new EntityNotFoundException($"No product option found matching product id {productId} and option id {updatedOptionRequest.Id}");

            _mapper.Map(updatedOptionRequest, currentOption);
            await _productRepo.UpdateProduct(productToUpdate);
        }

        public async Task DeleteProductOption(Guid productId, Guid optionId)
        {
            Product productToUpdate = await _productRepo.GetProductById(productId, true);
            ProductOption optionToRemove = productToUpdate?.Options?.FirstOrDefault(x => x.Id == optionId) ??
                throw new EntityNotFoundException($"No product option found matching product id {productId} and option id {optionId}");

            productToUpdate.Options.Remove(optionToRemove);
            await _productRepo.UpdateProduct(productToUpdate);
        }

        private async Task<Product> GetProductIfExists(Guid productId, bool includeOptions = false)
        {
            var timer = Stopwatch.StartNew();
            Product product = await _productRepo.GetProductById(productId, includeOptions) ??
                throw new EntityNotFoundException($"No product found with id {productId}");

            _logger.LogInformation("Product {id} found after {responseTime} ms", productId, timer.ElapsedMilliseconds);
            return product;
        }
    }
}