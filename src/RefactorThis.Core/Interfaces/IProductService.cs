using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RefactorThis.Core.Models;

namespace RefactorThis.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts(ISpecification<Product> spec);

        Task<Product> GetProduct(Guid productId);

        Task<Product> CreateProduct(CreateProductRequest newProduct);

        Task UpdateProduct(UpdateProductRequest product);

        Task DeleteProduct(Guid productId);

        Task<IEnumerable<ProductOption>> GetProductOptions(Guid productId);

        Task<ProductOption> GetProductOption(Guid productId, Guid optionId);

        Task<ProductOption> AddOptionToProduct(Guid productId, CreateProductOptionRequest newOption);

        Task UpdateProductOption(Guid productId, UpdateProductOptionRequest option);

        Task DeleteProductOption(Guid productId, Guid optionId);
    }
}