using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RefactorThis.Core.Interfaces;
using RefactorThis.Core.Models;
using RefactorThis.Data.Contexts;

namespace RefactorThis.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _productDb;

        public ProductRepository(ProductDbContext productDb)
        {
            _productDb = productDb ?? throw new ArgumentNullException(nameof(productDb));
        }

        public async Task<IEnumerable<Product>> GetProducts(ISpecification<Product> spec) =>
            await _productDb.Products
                            .Where(spec.Criteria)
                            .ToListAsync();

        public async Task<Product> GetProductById(Guid productId, bool includeOptions = false) =>
            includeOptions ?
                await _productDb.Products.Include(x => x.Options).FirstOrDefaultAsync(x => x.Id == productId) :
                await _productDb.Products.FindAsync(productId);

        public async Task AddProduct(Product newProduct)
        {
            _productDb.Products.Add(newProduct);
            await _productDb.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product updatedProduct)
        {
            _productDb.Entry(updatedProduct).State = EntityState.Modified;
            await _productDb.SaveChangesAsync();
        }

        public async Task DeleteProduct(Product product)
        {
            _productDb.Products.Remove(product);
            await _productDb.SaveChangesAsync();
        }

        public async Task<ProductOption> GetProductOptionById(Guid productId, Guid optionId) =>
            await _productDb.Products
                            .Where(x => x.Id == productId)
                            .Include(x => x.Options)
                            .SelectMany(x => x.Options)
                            .FirstOrDefaultAsync(x => x.Id == optionId);
    }
}