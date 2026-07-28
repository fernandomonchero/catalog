using Catalog.Domain.Interfaces;
using Catalog.Domain.Models;
using Catalog.Domain.Notifications;
using Catalog.Domain.Services;
using Moq;

namespace Catalog.UnitTests.DomainTests.Services
{
    [TestFixture]
    public class CatalogServiceTests
    {
        private ICatalogService _catalogService;
        private Mock<IProductRepository> _productRepository;
        private Mock<ISellerProductRepository> _sellerProductRepository;
        private Mock<INotificationCollector> _notifications;

        [SetUp]
        public void Setup()
        {
            _productRepository = new Mock<IProductRepository>();
            _notifications = new Mock<INotificationCollector>();
            _sellerProductRepository = new Mock<ISellerProductRepository>();

            _catalogService = new CatalogService(
                _productRepository.Object,
                _notifications.Object,
                _sellerProductRepository.Object
            );
        }

        [Test]
        public void Import_WhenProductDoesNotExist_ShouldAddProductAndSeller()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Galaxy Fit 3",
                    Brand = "Samsung",
                    Category = "Wearables",
                    SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString(),
                    Sellers = new List<SellerProduct>
                    {
                        new SellerProduct
                        {
                            SellerName = "Monchero",
                            SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()
                        }
                    }
                }
            };

            _productRepository.Setup(p => p.GetByNameAndBrand(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Product?)null);
            _sellerProductRepository.Setup(s => s.ProductAlreadyHasTheSeller(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            _catalogService.Import(products);

            _productRepository.Verify(r => r.Add(It.Is<Product>(p =>
                p.Name == "Galaxy Fit 3" &&
                p.Brand == "Samsung" &&
                p.Category == "Wearables")), Times.Once);
            _sellerProductRepository.Verify(r => r.Add(It.Is<SellerProduct>(s =>
                s.SellerName == "Monchero" &&
                s.SellerProductId == Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString())), Times.Once);
            _notifications.Verify(n => n.AddNotification(It.Is<Notification>(n =>
                n.Message == $"{Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()} - processed" &&
                n.Type == NotificationType.Processed)), Times.Once);
        }

        [Test]
        public void Import_WhenProductNameIsEmpty_ShouldNotAddProductAndSeller()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "",
                    Brand = "Samsung",
                    Category = "Wearables",
                    SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString(),
                    Sellers = new List<SellerProduct>
                    {
                        new SellerProduct
                        {
                            SellerName = "Monchero",
                            SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()
                        }
                    }
                }
            };

            _productRepository.Setup(p => p.GetByNameAndBrand(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Product?)null);
            _sellerProductRepository.Setup(s => s.ProductAlreadyHasTheSeller(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            _catalogService.Import(products);

            _productRepository.Verify(r => r.Add(It.IsAny<Product>()), Times.Never);
            _sellerProductRepository.Verify(r => r.Add(It.IsAny<SellerProduct>()), Times.Never);
            _notifications.Verify(n => n.AddNotification(It.Is<Notification>(n =>
                n.Message == $"{Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()} - Name is required" &&
                n.Type == NotificationType.Error)), Times.Once);
        }

        [Test]
        public void Import_WhenProductDoesNotExistAndBrandIsEmpty_ShouldAddProductAndSeller()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Galaxy Fit 3",
                    Brand = "",
                    Category = "Wearables",
                    SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString(),
                    Sellers = new List<SellerProduct>
                    {
                        new SellerProduct
                        {
                            SellerName = "Monchero",
                            SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()
                        }
                    }
                }
            };

            _productRepository.Setup(p => p.GetByNameAndBrand(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Product?)null);
            _sellerProductRepository.Setup(s => s.ProductAlreadyHasTheSeller(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            _catalogService.Import(products);

            _productRepository.Verify(r => r.Add(It.Is<Product>(p =>
                p.Name == "Galaxy Fit 3" &&
                p.Brand == "" &&
                p.Category == "Wearables")), Times.Once);
            _sellerProductRepository.Verify(r => r.Add(It.Is<SellerProduct>(s =>
                s.SellerName == "Monchero" &&
                s.SellerProductId == Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString())), Times.Once);
            _notifications.Verify(n => n.AddNotification(It.Is<Notification>(n =>
                n.Message == $"{Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()} - processed" &&
                n.Type == NotificationType.Processed)), Times.Once);
        }

        [Test]
        public void Import_WhenProductDoesNotExistAndCategoryIsEmpty_ShouldAddProductAndSeller()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Galaxy Fit 3",
                    Brand = "Samsung",
                    Category = "",
                    SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString(),
                    Sellers = new List<SellerProduct>
                    {
                        new SellerProduct
                        {
                            SellerName = "Monchero",
                            SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()
                        }
                    }
                }
            };

            _productRepository.Setup(p => p.GetByNameAndBrand(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Product?)null);
            _sellerProductRepository.Setup(s => s.ProductAlreadyHasTheSeller(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            _catalogService.Import(products);

            _productRepository.Verify(r => r.Add(It.Is<Product>(p =>
                p.Name == "Galaxy Fit 3" &&
                p.Brand == "Samsung" &&
                p.Category == "")), Times.Once);
            _sellerProductRepository.Verify(r => r.Add(It.Is<SellerProduct>(s =>
                s.SellerName == "Monchero" &&
                s.SellerProductId == Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString())), Times.Once);
            _notifications.Verify(n => n.AddNotification(It.Is<Notification>(n =>
                n.Message == $"{Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()} - processed" &&
                n.Type == NotificationType.Processed)), Times.Once);
        }

        [Test]
        public void Import_WhenProductDoesNotExistAndSellerNameIsEmpty_ShouldAddProductButNotSeller()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Galaxy Fit 3",
                    Brand = "Samsung",
                    Category = "Wearables",
                    SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString(),
                    Sellers = new List<SellerProduct>
                    {
                        new SellerProduct
                        {
                            SellerName = "",
                            SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()
                        }
                    }
                }
            };

            _productRepository.Setup(p => p.GetByNameAndBrand(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Product?)null);
            _sellerProductRepository.Setup(s => s.ProductAlreadyHasTheSeller(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            _catalogService.Import(products);

            _productRepository.Verify(r => r.Add(It.Is<Product>(p =>
                p.Name == "Galaxy Fit 3" &&
                p.Brand == "Samsung" &&
                p.Category == "Wearables")), Times.Once);
            _sellerProductRepository.Verify(r => r.Add(It.IsAny<SellerProduct>()), Times.Never);
            _notifications.Verify(n => n.AddNotification(It.Is<Notification>(n =>
                n.Message == $"{Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()} - processed" &&
                n.Type == NotificationType.Processed)), Times.Once);
            _notifications.Verify(n => n.AddNotification(It.Is<Notification>(n =>
                n.Message == $"{Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()} - SellerName is required" &&
                n.Type == NotificationType.Error)), Times.Once);
        }

        [Test]
        public void Import_WhenSellerProductIdIsAnInvalidGuid_ShouldAddProductAndSeller()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Galaxy Fit 3",
                    Brand = "Samsung",
                    Category = "Wearables",
                    SellerProductId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
                    Sellers = new List<SellerProduct>
                    {
                        new SellerProduct
                        {
                            SellerName = "Monchero",
                            SellerProductId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
                        }
                    }
                }
            };

            _productRepository.Setup(p => p.GetByNameAndBrand(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Product?)null);
            _sellerProductRepository.Setup(s => s.ProductAlreadyHasTheSeller(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            _catalogService.Import(products);

            _productRepository.Verify(r => r.Add(It.Is<Product>(p =>
                p.Name == "Galaxy Fit 3" &&
                p.Brand == "Samsung" &&
                p.Category == "Wearables")), Times.Once);
            _sellerProductRepository.Verify(r => r.Add(It.Is<SellerProduct>(s =>
                s.SellerName == "Monchero" &&
                s.SellerProductId == "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX")), Times.Once);
            _notifications.Verify(n => n.AddNotification(It.Is<Notification>(n =>
                n.Message == "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX - processed" &&
                n.Type == NotificationType.Processed)), Times.Once);
        }

        [Test]
        public void Import_WhenProductAlreadyExistsButSellerDoesNotExist_ShouldNotAddProductButAddSeller()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Galaxy Fit 3",
                    Brand = "Samsung",
                    Category = "Wearables",
                    SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString(),
                    Sellers = new List<SellerProduct>
                    {
                        new SellerProduct
                        {
                            SellerName = "Monchero",
                            SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()
                        }
                    }
                }
            };
            var existingProduct = new Product
            {
                Id = 1,
                Name = "Galaxy Fit 3",
                Brand = "Samsung",
                Category = "Wearables",
                SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()
            };

            _productRepository.Setup(p => p.GetByNameAndBrand(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(existingProduct);
            _sellerProductRepository.Setup(s => s.ProductAlreadyHasTheSeller(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            _catalogService.Import(products);

            _productRepository.Verify(r => r.Add(It.IsAny<Product>()), Times.Never);
            _sellerProductRepository.Verify(r => r.Add(It.Is<SellerProduct>(s =>
                s.ProductId == existingProduct.Id &&
                s.SellerName == "Monchero" &&
                s.SellerProductId == Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString())), Times.Once);
            _notifications.Verify(n => n.AddNotification(It.Is<Notification>(n =>
                n.Message == $"{Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()} - processed" &&
                n.Type == NotificationType.Processed)), Times.Once);
        }

        [Test]
        public void Import_WhenProductAlreadyExistsAndSellerToo_ShouldNotAddProductAndSeller()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Galaxy Fit 3",
                    Brand = "Samsung",
                    Category = "Wearables",
                    SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString(),
                    Sellers = new List<SellerProduct>
                    {
                        new SellerProduct
                        {
                            SellerName = "Monchero",
                            SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()
                        }
                    }
                }
            };
            var existingProduct = new Product
            {
                Id = 1,
                Name = "Galaxy Fit 3",
                Brand = "Samsung",
                Category = "Wearables",
                SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString(),
                Sellers = new List<SellerProduct>
                {
                    new SellerProduct
                    {
                        SellerName = "Monchero",
                        SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()
                    }
                }
            };

            _productRepository.Setup(p => p.GetByNameAndBrand(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(existingProduct);
            _sellerProductRepository.Setup(s => s.ProductAlreadyHasTheSeller(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            _catalogService.Import(products);

            _productRepository.Verify(r => r.Add(It.IsAny<Product>()), Times.Never);
            _sellerProductRepository.Verify(r => r.Add(It.IsAny<SellerProduct>()), Times.Never);
            _notifications.Verify(n => n.AddNotification(It.Is<Notification>(n =>
                n.Message == $"{Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()} - processed" &&
                n.Type == NotificationType.Processed)), Times.Once);
        }

        [Test]
        public void Import_WhenProductNameIsNotNormalized_ShouldNormalizeNameAndAddProduct()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "  Galaxy  Fit    3  ",
                    Brand = "Samsung",
                    Category = "Wearables",
                    SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString(),
                    Sellers = new List<SellerProduct>
                    {
                        new SellerProduct
                        {
                            SellerName = "Monchero",
                            SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()
                        }
                    }
                }
            };

            _productRepository.Setup(p => p.GetByNameAndBrand(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Product?)null);
            _sellerProductRepository.Setup(s => s.ProductAlreadyHasTheSeller(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            _catalogService.Import(products);

            _productRepository.Verify(r => r.Add(It.Is<Product>(p =>
                p.Name == "Galaxy Fit 3" &&
                p.Brand == "Samsung" &&
                p.Category == "Wearables")), Times.Once);
            _sellerProductRepository.Verify(r => r.Add(It.Is<SellerProduct>(s =>
                s.SellerName == "Monchero" &&
                s.SellerProductId == Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString())), Times.Once);
            _notifications.Verify(n => n.AddNotification(It.Is<Notification>(n =>
                n.Message == $"{Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()} - processed" &&
                n.Type == NotificationType.Processed)), Times.Once);
        }

        [Test]
        public void Import_WhenBrandNameIsNotNormalized_ShouldNormalizeNameAndAddProduct()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Galaxy Fit 3",
                    Brand = "  Samsung   Brazil  ",
                    Category = "Wearables",
                    SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString(),
                    Sellers = new List<SellerProduct>
                    {
                        new SellerProduct
                        {
                            SellerName = "Monchero",
                            SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()
                        }
                    }
                }
            };

            _productRepository.Setup(p => p.GetByNameAndBrand(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Product?)null);
            _sellerProductRepository.Setup(s => s.ProductAlreadyHasTheSeller(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            _catalogService.Import(products);

            _productRepository.Verify(r => r.Add(It.Is<Product>(p =>
                p.Name == "Galaxy Fit 3" &&
                p.Brand == "Samsung Brazil" &&
                p.Category == "Wearables")), Times.Once);
            _sellerProductRepository.Verify(r => r.Add(It.Is<SellerProduct>(s =>
                s.SellerName == "Monchero" &&
                s.SellerProductId == Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString())), Times.Once);
            _notifications.Verify(n => n.AddNotification(It.Is<Notification>(n =>
                n.Message == $"{Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()} - processed" &&
                n.Type == NotificationType.Processed)), Times.Once);
        }

        [Test]
        public void Import_WhenProductHasTwoSellers_ShouldAddProductAndSellers()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Galaxy Fit 3",
                    Brand = "Samsung",
                    Category = "Wearables",
                    SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString(),
                    Sellers = new List<SellerProduct>
                    {
                        new SellerProduct
                        {
                            SellerName = "Monchero",
                            SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()
                        }
                    }
                },
                new Product
                {
                    Name = "Galaxy Fit 3",
                    Brand = "Samsung",
                    Category = "Wearables",
                    SellerProductId = Guid.Parse("46e171b6-ef3a-4265-a6ee-4b4beff1d4f9").ToString(),
                    Sellers = new List<SellerProduct>
                    {
                        new SellerProduct
                        {
                            SellerName = "Fernando",
                            SellerProductId = Guid.Parse("46e171b6-ef3a-4265-a6ee-4b4beff1d4f9").ToString()
                        }
                    }
                }
            };

            var existingProduct = new Product
            {
                Id = 1,
                Name = "Galaxy Fit 3",
                Brand = "Samsung",
                Category = "Wearables",
                SellerProductId = Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()
            };

            _productRepository.SetupSequence(p => p.GetByNameAndBrand(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((Product?)null)
                .ReturnsAsync(existingProduct);
            _sellerProductRepository.Setup(s => s.ProductAlreadyHasTheSeller(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            _catalogService.Import(products);

            _productRepository.Verify(r => r.Add(It.Is<Product>(p =>
                p.Name == "Galaxy Fit 3" &&
                p.Brand == "Samsung" &&
                p.Category == "Wearables")), Times.Once);
            _sellerProductRepository.Verify(r => r.Add(It.IsAny<SellerProduct>()), Times.Exactly(2));
            _sellerProductRepository.Verify(r => r.Add(It.Is<SellerProduct>(s =>
                s.SellerName == "Monchero" &&
                s.SellerProductId == Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString())), Times.Once);
            _sellerProductRepository.Verify(r => r.Add(It.Is<SellerProduct>(s =>
                s.SellerName == "Fernando" &&
                s.SellerProductId == Guid.Parse("46e171b6-ef3a-4265-a6ee-4b4beff1d4f9").ToString())), Times.Once);
            _notifications.Verify(n => n.AddNotification(It.IsAny<Notification>()), Times.Exactly(2));
            _notifications.Verify(n => n.AddNotification(It.Is<Notification>(n =>
                n.Message == $"{Guid.Parse("9e188c6c-4346-4671-8d77-ed4446e189ee").ToString()} - processed" &&
                n.Type == NotificationType.Processed)), Times.Once);
            _notifications.Verify(n => n.AddNotification(It.Is<Notification>(n =>
                n.Message == $"{Guid.Parse("46e171b6-ef3a-4265-a6ee-4b4beff1d4f9").ToString()} - processed" &&
                n.Type == NotificationType.Processed)), Times.Once);
        }
    }
}