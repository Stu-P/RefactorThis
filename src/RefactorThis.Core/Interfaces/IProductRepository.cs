using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RefactorThis.Core.Models;

namespace RefactorThis.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts(PagingRequest pagingRequest, string nameFilter = null);

        Task<Product> GetProductById(Guid productId, bool includeOptions = false);

        Task AddProduct(Product newProduct);

        Task UpdateProduct(Product updatedProduct);

        Task DeleteProduct(Product productId);

        Task<ProductOption> GetProductOptionById(Guid productId, Guid optionId);
    }
}