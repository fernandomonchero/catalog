using Catalog.Application.Interfaces;
using Catalog.Application.UseCases;
using Catalog.Domain.Interfaces;
using Catalog.Domain.Models;
using Moq;

namespace Catalog.UnitTests.ApplicationTests.UseCases
{
    [TestFixture]
    public class ProductDetailUseCaseTests
    {
        private IProductDetailUseCase _productDetailUseCase;
        private Mock<IProductRepository> _productRepository;

        [SetUp]
        public void Setup()
        {
            _productRepository = new Mock<IProductRepository>();

            _productDetailUseCase = new ProductDetailUseCase(
                _productRepository.Object
            );
        }

        [Test]
        public async Task Detail_ProductDoesNotExists()
        {
            _productRepository.Setup(p => p.Get(It.IsAny<int>())).ReturnsAsync((Product)null);

            var result = await _productDetailUseCase.Detail(1);

            Assert.That(result, Is.Null);

            _productRepository.Verify(p => p.Get(It.Is<int>(x => x == 1)), Times.Once);
        }

        [Test]
        public async Task Detail_ProductExists()
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

            _productRepository.Setup(p => p.Get(It.IsAny<int>())).ReturnsAsync(product);

            var result = await _productDetailUseCase.Detail(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(product.Id.ToString()));
            Assert.That(result.Name, Is.EqualTo(product.Name));
            Assert.That(result.Brand, Is.EqualTo(product.Brand));
            Assert.That(result.Category, Is.EqualTo(product.Category));
            Assert.That(result.SellerName, Is.EqualTo(product.Sellers.Single().SellerName));
            _productRepository.Verify(p => p.Get(It.Is<int>(x => x == 1)), Times.Once);
        }

        [Test]
        public async Task Detail_ProductExistsWithoutSeller()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Produto Completo",
                Brand = "Marca",
                Category = "Categoria",
                Sellers = new List<SellerProduct>()
            };

            _productRepository.Setup(p => p.Get(It.IsAny<int>())).ReturnsAsync(product);

            var result = await _productDetailUseCase.Detail(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(product.Id.ToString()));
            Assert.That(result.Name, Is.EqualTo(product.Name));
            Assert.That(result.Brand, Is.EqualTo(product.Brand));
            Assert.That(result.Category, Is.EqualTo(product.Category));
            Assert.That(result.SellerName, Is.Empty);
            _productRepository.Verify(p => p.Get(It.Is<int>(x => x == 1)), Times.Once);
        }

        [Test]
        public async Task Detail_ProductExistsWithTwoSellers()
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
                        SellerName = "Seller 1"
                    },
                    new SellerProduct
                    {
                        Id = 2,
                        SellerName = "Seller 2"
                    }
                },
            };

            _productRepository.Setup(p => p.Get(It.IsAny<int>())).ReturnsAsync(product);

            var result = await _productDetailUseCase.Detail(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(product.Id.ToString()));
            Assert.That(result.Name, Is.EqualTo(product.Name));
            Assert.That(result.Brand, Is.EqualTo(product.Brand));
            Assert.That(result.Category, Is.EqualTo(product.Category));
            Assert.That(result.SellerName, Is.EqualTo("Seller 1, Seller 2"));
            _productRepository.Verify(p => p.Get(It.Is<int>(x => x == 1)), Times.Once);
        }
    }
}