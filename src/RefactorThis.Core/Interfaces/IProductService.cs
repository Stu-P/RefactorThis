using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RefactorThis.Core.Models;

namespace RefactorThis.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts(string nameFilter = null);

        Task<Product> GetProduct(Guid productId);

        Task<Product> CreateProduct(Product newProduct);

        Task UpdateProduct(Product product);

        Task DeleteProduct(Guid productId);

        Task<IEnumerable<ProductOption>> GetProductOptions(Guid productId);

        Task<ProductOption> GetProductOption(Guid productId, Guid optionId);

        Task<ProductOption> AddOptionToProduct(Guid productId, ProductOption newOption);

        Task UpdateProductOption(Guid productId, ProductOption option);

        Task DeleteProductOption(Guid productId, Guid optionId);
    }
}