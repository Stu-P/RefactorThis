using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RefactorThis.Core.Exceptions;
using RefactorThis.Core.Extensions;
using RefactorThis.Core.Interfaces;
using RefactorThis.Core.Models;

namespace RefactorThis.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepo, IGuidGenerator guidGenerator, ILogger<ProductService> logger)
        {
            _productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
            _guidGenerator = guidGenerator ?? throw new ArgumentNullException(nameof(guidGenerator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Product>> GetProducts(string nameFilter = null)
        {
            var timer = Stopwatch.StartNew();
            var allMatchingProducts = await _productRepo.GetProducts(nameFilter);
            _logger.LogInformation("GetProducts returned {count} products after {responseTime} ms", allMatchingProducts.Count(), timer.ElapsedMilliseconds);

            return allMatchingProducts;
        }

        public async Task<Product> GetProduct(Guid productId) => await GetProductIfExists(productId);

        public async Task<Product> CreateProduct(Product newProduct)
        {
            newProduct.Id = _guidGenerator.NewGuid();
            await _productRepo.AddProduct(newProduct);
            return newProduct;
        }

        public async Task UpdateProduct(Product product)
        {
            await _productRepo.UpdateProduct(product);
        }

        public async Task DeleteProduct(Guid productId)
        {
            Product productToDelete = await GetProductIfExists(productId);
            await _productRepo.DeleteProduct(productToDelete);
        }

        public async Task<IEnumerable<ProductOption>> GetProductOptions(Guid productId)
        {
            var timer = Stopwatch.StartNew();

            Product product = await _productRepo.GetProductById(productId, true);
            if (product is null)
            {
                throw new EntityNotFoundException("No product found with prodict id {productId}");
            }
            _logger.LogInformation("Returned {count} options for product {productId} after {responseTime} ms", product.Options?.Count(), productId, timer.ElapsedMilliseconds);

            return product.Options;
        }

        public async Task<ProductOption> GetProductOption(Guid productId, Guid optionId)
        {
            var timer = new Stopwatch();
            timer.Start();

            var matchingOption = await _productRepo.GetProductOptionById(productId, optionId);

            if (matchingOption is null)
            {
                _logger.LogWarning("No product option found for {id} after {responseTime} ms", productId, timer.ElapsedMilliseconds);
                throw new EntityNotFoundException($"No product option found matching product id {productId} and option id {optionId}");
            }
            return matchingOption;
        }

        public async Task<ProductOption> AddOptionToProduct(Guid productId, ProductOption newOption)
        {
            var timer = Stopwatch.StartNew();

            var productToUpdate = await _productRepo.GetProductById(productId, true);

            if (productToUpdate is null)
            {
                throw new EntityNotFoundException("No product found with prodict id {productId}");
            }
            newOption.Id = _guidGenerator.NewGuid();
            productToUpdate.Options.Add(newOption);

            await _productRepo.UpdateProduct(productToUpdate);
            return newOption;
        }

        public async Task UpdateProductOption(Guid productId, ProductOption updatedOption)
        {
            var timer = Stopwatch.StartNew();

            var productToUpdate = await _productRepo.GetProductById(productId, true);
            var currentOption = productToUpdate?.Options?.FirstOrDefault(x => x.Id == updatedOption.Id);
            if (currentOption is null)
            {
                throw new EntityNotFoundException($"No product option found matching product id {productId} and option id {updatedOption.Id}");
            }
            currentOption.MapChanges(updatedOption);

            await _productRepo.UpdateProduct(productToUpdate);
            _logger.LogInformation("Product option {id} for product {productId} updated after {responseTime} ms", updatedOption.Id, productId, timer.ElapsedMilliseconds);
        }

        public async Task DeleteProductOption(Guid productId, Guid optionId)
        {
            var productToUpdate = await _productRepo.GetProductById(productId, true);
            ProductOption optionToRemove = productToUpdate?.Options?.FirstOrDefault(x => x.Id == optionId);
            if (optionToRemove is null)
            {
                throw new EntityNotFoundException($"No product option found matching product id {productId} and option id {optionId}");
            }
            productToUpdate.Options.Remove(optionToRemove);
            await _productRepo.UpdateProduct(productToUpdate);
        }

        private async Task<Product> GetProductIfExists(Guid productId)
        {
            var timer = Stopwatch.StartNew();

            var product = await _productRepo.GetProductById(productId);
            if (product is null)
            {
                _logger.LogWarning("No product found for {id} after {responseTime} ms", productId, timer.ElapsedMilliseconds);
                throw new EntityNotFoundException($"No product found with id {productId}");
            }
            _logger.LogInformation("Product {id} found after {responseTime} ms", productId, timer.ElapsedMilliseconds);

            return product;
        }
    }
}