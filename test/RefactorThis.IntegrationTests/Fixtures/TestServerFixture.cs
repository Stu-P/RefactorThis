using System;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RefactorThis.Data.Contexts;
using RefactorThis.Tests.Common.Builders;
using Serilog;

namespace RefactorThis.IntegrationTests.Fixtures
{
    public class TestServerFixture : IDisposable
    {
        private readonly IHost _testServer;
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webHost =>
                    webHost
                        .UseStartup<Startup>()
                        .ConfigureTestServices(services =>
                        {
                            // Replace DbContext registration with InMemory
                            var descriptor = services.SingleOrDefault(
                                d => d.ServiceType ==
                                    typeof(DbContextOptions<ProductDbContext>));

                            if (descriptor != null)
                            {
                                services.Remove(descriptor);
                            }
                            services.AddDbContext<ProductDbContext>(options => options.UseSqlite(CreateInMemoryDatabase()));
                        })
                        .UseEnvironment("Local")
                        .UseSerilog()
                        .UseTestServer()
                    );

            _testServer = hostBuilder.Start();

            Client = _testServer.GetTestClient();

            using (var scope = _testServer.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
                SeedData(context);
            }
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }

        private void SeedData(ProductDbContext context)
        {
            context.Products.Add(
                new ProductBuilder()
                    .WithId(Guid.Parse("3bd1ebce-38e2-4d40-b5d4-d8de25c7e523"))
                    .WithName("Apple iPhone 8")
                    .WithDescription("Newest phone from Apple")
                    .WithPrice(899.91m)
                    .WithDeliveryPrice(10)
                    .WithOption(
                        new ProductOptionBuilder()
                        .WithId(Guid.Parse("9f35e319-885e-4d3f-9cc3-b282b92bbe17"))
                        .WithProductId(Guid.Parse("3bd1ebce-38e2-4d40-b5d4-d8de25c7e523"))
                        .WithName("Red")
                        .WithDescription("Available August"))
                    .WithOption(
                        new ProductOptionBuilder()
                        .WithId(Guid.Parse("a4faea48-7800-492a-852a-1b7ecd17ab9f"))
                        .WithProductId(Guid.Parse("3bd1ebce-38e2-4d40-b5d4-d8de25c7e523"))
                        .WithName("Gold")
                        .WithDescription("Available now")));

            context.Products.Add(
                new ProductBuilder()
                    .WithId(Guid.Parse("7928ca40-03b6-4859-b64d-f0ba26bc3de2"))
                    .WithName("Samsung Galaxy S10")
                    .WithDescription("Newest phone from Samsung")
                    .WithPrice(1001.99m)
                    .WithDeliveryPrice(25.01m)
                    .WithOption(
                        new ProductOptionBuilder()
                        .WithId(Guid.Parse("060071ff-7c9c-4954-a468-b7450b13cc86"))
                        .WithProductId(Guid.Parse("7928ca40-03b6-4859-b64d-f0ba26bc3de2"))
                        .WithName("Black")
                        .WithDescription("Available March"))
                    .WithOption(
                        new ProductOptionBuilder()
                        .WithId(Guid.Parse("0125bcb6-ca40-4cc0-ba0a-ce1bbd52ee42"))
                        .WithProductId(Guid.Parse("7928ca40-03b6-4859-b64d-f0ba26bc3de2"))
                        .WithName("White")
                        .WithDescription("Sold Out"))
                    .WithOption(
                        new ProductOptionBuilder()
                        .WithId(Guid.Parse("8c2358bd-7a70-42fe-9131-6fd1825c391a"))
                        .WithProductId(Guid.Parse("7928ca40-03b6-4859-b64d-f0ba26bc3de2"))
                        .WithName("Red")
                        .WithDescription("A red phone, do you really think you can pull that off?")));

            context.Products.Add(
                    new ProductBuilder()
                        .WithId(Guid.Parse("6b354c4a-7bf0-40f9-a872-6990d06c31c5"))
                        .WithName("Xiaomi mi 9")
                        .WithDescription("Options not yet released")
                        .WithPrice(90.97m)
                        .WithDeliveryPrice(15.51m)
                    );

            context.SaveChanges();
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Client.Dispose();
                    _testServer.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}