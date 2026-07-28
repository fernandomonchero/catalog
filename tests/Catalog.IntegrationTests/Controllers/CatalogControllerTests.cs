using Catalog.Application.Dtos;
using Catalog.IntegrationTests.Helpers;
using Catalog.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Catalog.IntegrationTests.Controllers
{
    [TestFixture]
    public class CatalogControllerTests
    {
        private IntegrationTestFactory _factory = null!;
        private HttpClient _client = null!;

        [OneTimeSetUp]
        public void Setup()
        {
            _factory = new IntegrationTestFactory();
            _client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task Get_ShouldReturnAllProducts()
        {
            var response = await _client.GetAsync("/api/catalog");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var products = await response.ReadAsync<List<ProductDto>>();

            Assert.NotNull(products);
            Assert.That(products.Count, Is.EqualTo(1));
            Assert.That(products![0].Name, Is.EqualTo("iPhone 15"));
        }

        [Test]
        public async Task Get_ById_ShouldReturnProduct()
        {
            var response = await _client.GetAsync("/api/catalog/product/1");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var product = await response.ReadAsync<ProductDto>();

            Assert.NotNull(product);   
            Assert.That(product!.Name, Is.EqualTo("iPhone 15"));
        }

        [Test]
        public async Task Get_ById_ShouldReturnNotFound()
        {
            var response = await _client.GetAsync("/api/catalog/product/999");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Import_ShouldCreateNewProduct()
        {
            var request = new[]
            {
                new ProductDto
                {
                    Name = "Galaxy S24",
                    Brand = "Samsung",
                    Category = "Electronics",
                    SellerName = "MegaStore",
                    Id = "441f017b-c021-4132-9c17-a5b626a7e827"
                }
            };

            var response = await _client.PostAsJsonAsync("/api/catalog/import", request);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ImportResultDto>>();

            var list = await _client.GetFromJsonAsync<List<ProductDto>>("/api/catalog");

            Assert.That(list!.Count(x => x.Name == "Galaxy S24"), Is.EqualTo(1));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data!.TotalRecords, Is.EqualTo(1));
            Assert.That(result.Data.TotalErrors, Is.EqualTo(0));
            Assert.That(result.Data.Errors, Has.Count.EqualTo(0));
        }

        [Test]
        public async Task Import_Should_NotCreateDuplicateProduct()
        {
            var request = new[]
            {
                new ProductDto
                {
                    Name = "iPhone 15",
                    Brand = "Apple",
                    Category = "Electronics",
                    SellerName = "Another Seller",
                    Id = "dc131aad-2cee-4e5f-a6d8-46cb39658c6a"
                }
            };

            var response = await _client.PostAsJsonAsync("/api/catalog/import", request);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ImportResultDto>>();

            var products = await _client.GetFromJsonAsync<List<ProductDto>>("/api/catalog");

            Assert.That(products.Count(x => x.Name == "iPhone 15"), Is.EqualTo(1));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data!.TotalRecords, Is.EqualTo(1));
            Assert.That(result.Data.TotalErrors, Is.EqualTo(0));
            Assert.That(result.Data.Errors, Has.Count.EqualTo(0));
        }

        [Test]
        public async Task Import_ShouldNormalizeName()
        {
            var request = new[]
            {
                new ProductDto
                {
                    Name = " iPhone   15  ",
                    Brand = " Apple  ",
                    Category = "Electronics",
                    SellerName = "Seller X",
                    Id = "7c856228-f4aa-4997-8d9d-ccad6206d807"
                }
            };

            var response =await _client.PostAsJsonAsync("/api/catalog/import", request);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ImportResultDto>>();

            var products = await _client.GetFromJsonAsync<List<ProductDto>>("/api/catalog");

            Assert.That(products.First(x => x.Name.Contains("iPhone 15")).Brand, Is.EqualTo("Apple"));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data!.TotalRecords, Is.EqualTo(1));
            Assert.That(result.Data.TotalErrors, Is.EqualTo(0));
            Assert.That(result.Data.Errors, Has.Count.EqualTo(0));
        }

        [Test]
        public async Task Import_Should_ReturnOkWithNotificationsWhenProductNameIsInvalid()
        {
            var request = new[]
            {
                new ProductDto()
            };

            var response = await _client.PostAsJsonAsync("/api/catalog/import", request);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ImportResultDto>>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data!.TotalRecords, Is.EqualTo(1));
            Assert.That(result.Data.TotalErrors, Is.EqualTo(1));
            Assert.That(result.Data.Errors, Has.Count.EqualTo(1));
            Assert.That(result.Data.Errors.First(), Does.Contain("Name is required"));
        }
    }
}