using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using RefactorThis.Api.Models;
using RefactorThis.Core.Models;
using RefactorThis.IntegrationTests.Fixtures;
using Xunit;

namespace RefactorThis.Api.IntegrationTests
{
    public class ProductTests
    {
        public class GetProductsShould : IClassFixture<TestServerFixture>
        {
            private readonly TestServerFixture _sut;

            public GetProductsShould(TestServerFixture sut) => _sut = sut;

            [Fact]
            public async Task ReturnAllProducts_WhenNoFilterProvided()
            {
                var response = await _sut.Client.GetAsync("api/Products");
                response.EnsureSuccessStatusCode();
                var actual = JsonConvert.DeserializeObject<GetAllResponse<Product>>(await response.Content.ReadAsStringAsync());

                actual.Should().NotBeNull();
                actual.Items.Should().NotBeNullOrEmpty().And.HaveCount(3);
                actual.Items.Should().Contain(x => x.Description == "Newest phone from Samsung");
            }

            [Fact]
            public async Task ReturnMatchingProductsOnly_WhenNameFilterProvided()
            {
                var response = await _sut.Client.GetAsync("api/Products?name=apple");
                response.EnsureSuccessStatusCode();
                var actual = JsonConvert.DeserializeObject<GetAllResponse<Product>>(await response.Content.ReadAsStringAsync());
                actual.Should().NotBeNull();
                actual.Items.Should().NotBeNull().And.HaveCount(1);
                actual.Items.First().Price.Should().Be(899.91m);
            }
        }

        public class GetProductShould : IClassFixture<TestServerFixture>
        {
            private readonly TestServerFixture _sut;

            public GetProductShould(TestServerFixture sut) => _sut = sut;

            [Fact]
            public async Task ReturnProduct_WhenEntityWithRequestedIdExists()
            {
                var response = await _sut.Client.GetAsync("api/Products/7928ca40-03b6-4859-b64d-f0ba26bc3de2");
                response.EnsureSuccessStatusCode();
                var actual = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());
                actual.Should().NotBeNull();
                actual.DeliveryPrice.Should().Be(25.01m);
            }

            [Fact]
            public async Task ReturnNotFound_WhenEntityWithRequestedIdDoesNotExist()
            {
                var invalidGuid = "93bf59d9-ab25-45a0-ae80-0650a27b4acf";
                var response = await _sut.Client.GetAsync($"api/Products/{invalidGuid}");
                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        public class CreateProductShould : IClassFixture<TestServerFixture>
        {
            private readonly TestServerFixture _sut;

            public CreateProductShould(TestServerFixture sut) => _sut = sut;

            [Fact]
            public async Task ReturnCreatedEntitiesLocation_WhenValidCreatePayload()
            {
                var product = new CreateProductRequest()
                {
                    Name = "Nokia ",
                    Description = "It's a brick",
                    Price = 14.12m,
                    DeliveryPrice = 20.00m
                };
                var payload = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                var response = await _sut.Client.PostAsync($"api/Products", payload);
                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.Created);

                var getResponse = await _sut.Client.GetAsync(response.Headers.Location);
                getResponse.EnsureSuccessStatusCode();
            }

            [Fact]
            public async Task ReturnBadRequest_WhenCreatePayloadIsMissingMandatoryProperties()
            {
                var badProduct = new CreateProductRequest()
                {
                    Name = "Alcatel",
                    Description = "I dont have a price",
                    DeliveryPrice = 20.00m
                };
                var payload = new StringContent(JsonConvert.SerializeObject(badProduct), Encoding.UTF8, "application/json");

                var response = await _sut.Client.PostAsync($"api/Products", payload);
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        public class UpdateProductShould : IClassFixture<TestServerFixture>
        {
            private readonly TestServerFixture _sut;

            public UpdateProductShould(TestServerFixture sut) => _sut = sut;

            [Fact]
            public async Task ReturnNoContent_WhenUpdateSuccessful()
            {
                var update = new UpdateProductRequest()
                {
                    Id = Guid.Parse("7928ca40-03b6-4859-b64d-f0ba26bc3de2"),
                    Name = "Alcatel",
                    Description = "Finland's finest",
                    Price = 980.10m,
                    DeliveryPrice = 20.00m
                };

                var payload = new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json");
                var response = await _sut.Client.PutAsync($"api/Products/7928ca40-03b6-4859-b64d-f0ba26bc3de2", payload);
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);

                var getResponse = await _sut.Client.GetAsync($"api/Products/7928ca40-03b6-4859-b64d-f0ba26bc3de2");
                getResponse.EnsureSuccessStatusCode();
                var actual = JsonConvert.DeserializeObject<Product>(await getResponse.Content.ReadAsStringAsync());
                actual.Should().NotBeNull();
                actual.Name.Should().Be("Alcatel");
            }
        }

        public class DeleteProductShould : IClassFixture<TestServerFixture>
        {
            private readonly TestServerFixture _sut;

            public DeleteProductShould(TestServerFixture sut) => _sut = sut;

            [Fact]
            public async Task ReturnNoContent_WhenDeleteSuccessful()
            {
                var response = await _sut.Client.DeleteAsync($"api/Products/7928ca40-03b6-4859-b64d-f0ba26bc3de2");
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);

                var getResponse = await _sut.Client.GetAsync($"api/Products/7928ca40-03b6-4859-b64d-f0ba26bc3de2");
                getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        public class GetProductOptionsShould : IClassFixture<TestServerFixture>
        {
            private readonly TestServerFixture _sut;

            public GetProductOptionsShould(TestServerFixture sut) => _sut = sut;

            [Fact]
            public async Task ReturnAllProductOptionsForSpecifiedProduct()
            {
                var response = await _sut.Client.GetAsync("api/Products/7928ca40-03b6-4859-b64d-f0ba26bc3de2/Options");
                response.EnsureSuccessStatusCode();
                var actual = JsonConvert.DeserializeObject<GetAllResponse<ProductOption>>(await response.Content.ReadAsStringAsync());

                actual.Should().NotBeNull();
                actual.Items.Should().NotBeNullOrEmpty().And.HaveCount(3);
                actual.Items.Should().Contain(x => x.Name == "Red");
            }

            [Fact]
            public async Task ReturnEmptyArray_WhenProductHasNoOptions()
            {
                var response = await _sut.Client.GetAsync("api/Products/6b354c4a-7bf0-40f9-a872-6990d06c31c5/Options");
                response.EnsureSuccessStatusCode();
                var actual = JsonConvert.DeserializeObject<GetAllResponse<ProductOption>>(await response.Content.ReadAsStringAsync());

                actual.Should().NotBeNull();
                actual.Items.Should().BeEmpty();
            }
        }

        public class GetProductOptionShould : IClassFixture<TestServerFixture>
        {
            private readonly TestServerFixture _sut;

            public GetProductOptionShould(TestServerFixture sut) => _sut = sut;

            [Fact]
            public async Task ReturnOptionForProduct_WhenSpecifiedProductOptionExists()
            {
                var response = await _sut.Client.GetAsync("api/Products/7928ca40-03b6-4859-b64d-f0ba26bc3de2/Options/0125bcb6-ca40-4cc0-ba0a-ce1bbd52ee42");
                response.EnsureSuccessStatusCode();
                var actual = JsonConvert.DeserializeObject<ProductOption>(await response.Content.ReadAsStringAsync());

                actual.Should().NotBeNull();
                actual.Name.Should().Be("White");
            }

            [Fact]
            public async Task ReturnNotFound_WhenProductOptionDoesntExist()
            {
                var invalidGuid = "93bf59d9-ab25-45a0-ae80-0650a27b4acf";

                var response = await _sut.Client.GetAsync($"api/Products/0125bcb6-ca40-4cc0-ba0a-ce1bbd52ee42/Options/{invalidGuid}");
                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        public class CreateProductOptionShould : IClassFixture<TestServerFixture>
        {
            private readonly TestServerFixture _sut;

            public CreateProductOptionShould(TestServerFixture sut) => _sut = sut;

            [Fact]
            public async Task ReturnCreatedEntitiesLocation_WhenValidCreatePayload()
            {
                var product = new CreateProductOptionRequest()
                {
                    Name = "Orange",
                    Description = "Is the new black"
                };
                var payload = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                var response = await _sut.Client.PostAsync("api/Products/6b354c4a-7bf0-40f9-a872-6990d06c31c5/Options", payload);
                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.Created);
                response.Headers.Location.Should().NotBeNull();

                var getResponse = await _sut.Client.GetAsync(response.Headers.Location);
                getResponse.EnsureSuccessStatusCode();
            }

            [Fact]
            public async Task ReturnBadRequest_WhenOptionCreatePayloadIsMissingMandatoryProperties()
            {
                var badProduct = new CreateProductOptionRequest()
                {
                    Description = "Im missing a name!"
                };
                var payload = new StringContent(JsonConvert.SerializeObject(badProduct), Encoding.UTF8, "application/json");

                var response = await _sut.Client.PostAsync($"api/Products/0125bcb6-ca40-4cc0-ba0a-ce1bbd52ee42/Options", payload);
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        public class UpdateProductOptionShould : IClassFixture<TestServerFixture>
        {
            private readonly TestServerFixture _sut;

            public UpdateProductOptionShould(TestServerFixture sut) => _sut = sut;

            [Fact]
            public async Task ReturnNoContent_WhenUpdateSuccessful()
            {
                var update = new UpdateProductOptionRequest()
                {
                    Id = Guid.Parse("060071ff-7c9c-4954-a468-b7450b13cc86"),
                    Name = "Orange",
                    Description = "Now in orange"
                };

                var payload = new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json");
                var response = await _sut.Client.PutAsync($"api/Products/7928ca40-03b6-4859-b64d-f0ba26bc3de2/Options/060071ff-7c9c-4954-a468-b7450b13cc86", payload);
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);

                var getResponse = await _sut.Client.GetAsync($"api/Products/7928ca40-03b6-4859-b64d-f0ba26bc3de2/Options/060071ff-7c9c-4954-a468-b7450b13cc86");
                getResponse.EnsureSuccessStatusCode();
                var actual = JsonConvert.DeserializeObject<Product>(await getResponse.Content.ReadAsStringAsync());
                actual.Should().NotBeNull();
                actual.Name.Should().Be("Orange");
            }
        }

        public class DeleteProductOptionShould : IClassFixture<TestServerFixture>
        {
            private readonly TestServerFixture _sut;

            public DeleteProductOptionShould(TestServerFixture sut) => _sut = sut;

            [Fact]
            public async Task ReturnNoContent_WhenDeleteSuccessful()
            {
                var response = await _sut.Client.DeleteAsync($"api/Products/7928ca40-03b6-4859-b64d-f0ba26bc3de2/Options/060071ff-7c9c-4954-a468-b7450b13cc86");
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);

                var getResponse = await _sut.Client.GetAsync($"api/Products/7928ca40-03b6-4859-b64d-f0ba26bc3de2/Options/060071ff-7c9c-4954-a468-b7450b13cc86");
                getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }
    }
}