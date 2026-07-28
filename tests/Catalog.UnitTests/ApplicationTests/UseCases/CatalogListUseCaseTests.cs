using Catalog.Application.Interfaces;
using Catalog.Application.UseCases;
using Catalog.Domain.Interfaces;
using Catalog.Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace Catalog.UnitTests.ApplicationTests.UseCases
{
    [TestFixture]
    public class CatalogListUseCaseTests
    {
        private ICatalogListUseCase _catalogListUseCase;
        private Mock<IProductRepository> _productRepository;

        [SetUp]
        public void Setup()
        {
            _productRepository = new Mock<IProductRepository>();

            _catalogListUseCase = new CatalogListUseCase(
                _productRepository.Object
            );
        }

        [Test]
        public async Task List_NoProductsExists()
        {
            _productRepository.Setup(p => p.All(It.IsAny<Expression<Func<Product, object>>[]>())).ReturnsAsync(new List<Product>());

            var result = await _catalogListUseCase.List();

            Assert.That(result, Is.Empty);

            _productRepository.Verify(p => p.All(It.IsAny<Expression<Func<Product, object>>[]>()), Times.Once);
        }

        [Test]
        public async Task List_OneProductExists()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Produto Completo",
                Brand = "Marca",
                Category = "Categoria",
                Sellers = new List<SellerProduct>
                {
                    new SellerProduct
                    {
                        Id = 1,
                        SellerName = "Seller"
                    }
                },
            };

            _productRepository.Setup(p => p.All(It.IsAny<Expression<Func<Product, object>>[]>())).ReturnsAsync(new List<Product> { product });

            var result = await _catalogListUseCase.List();

            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(product.Id.ToString()));
            Assert.That(result.First().Name, Is.EqualTo(product.Name));
            Assert.That(result.First().Brand, Is.EqualTo(product.Brand));
            Assert.That(result.First().Category, Is.EqualTo(product.Category));
            Assert.That(result.First().SellerName, Is.EqualTo(product.Sellers.Single().SellerName));
            _productRepository.Verify(p => p.All(It.IsAny<Expression<Func<Product, object>>[]>()), Times.Once);
        }

        [Test]
        public async Task List_TwoProductsExists()
        {
            var product1 = new Product
            {
                Id = 1,
                Name = "Produto Completo",
                Brand = "Marca",
                Category = "Categoria",
                Sellers = new List<SellerProduct>
                {
                    new SellerProduct
                    {
                        Id = 1,
                        SellerName = "Seller"
                    }
                },
            };
            var product2 = new Product
            {
                Id = 2,
                Name = "Produto Completo 2",
                Brand = "Marca 2",
                Category = "Categoria 2",
                Sellers = new List<SellerProduct>
                {
                    new SellerProduct
                    {
                        Id = 1,
                        SellerName = "Seller"
                    }
                },
            };

            _productRepository.Setup(p => p.All(It.IsAny<Expression<Func<Product, object>>[]>())).ReturnsAsync(new List<Product> { product1, product2 });

            var result = await _catalogListUseCase.List();

            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(product1.Id.ToString()));
            Assert.That(result.First().Name, Is.EqualTo(product1.Name));
            Assert.That(result.First().Brand, Is.EqualTo(product1.Brand));
            Assert.That(result.First().Category, Is.EqualTo(product1.Category));
            Assert.That(result.First().SellerName, Is.EqualTo(product1.Sellers.Single().SellerName));
            Assert.That(result.Last().Id, Is.EqualTo(product2.Id.ToString()));
            Assert.That(result.Last().Name, Is.EqualTo(product2.Name));
            Assert.That(result.Last().Brand, Is.EqualTo(product2.Brand));
            Assert.That(result.Last().Category, Is.EqualTo(product2.Category));
            Assert.That(result.Last().SellerName, Is.EqualTo(product2.Sellers.Single().SellerName));
            _productRepository.Verify(p => p.All(It.IsAny<Expression<Func<Product, object>>[]>()), Times.Once);
        }
    }
}