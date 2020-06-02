using Microsoft.EntityFrameworkCore;
using RefactorThis.Core.Models;

namespace RefactorThis.Data.Contexts
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(t => t.Id).ValueGeneratedNever();
            modelBuilder.Entity<ProductOption>().Property(t => t.Id).ValueGeneratedNever();
        }

        public DbSet<Product> Products { get; set; }
    }
}