using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Catalog.Application.UseCases;
using Catalog.Domain.Interfaces;
using Catalog.Domain.Models;
using Catalog.Domain.Notifications;
using Moq;

namespace Catalog.UnitTests.ApplicationTests.UseCases
{
    [TestFixture]
    public class CatalogImportUseCaseTest
    {
        private ICatalogImportUseCase _catalogImportUseCase;
        private Mock<ICatalogService> _catalogService;
        private Mock<INotificationCollector> _notifications;

        [SetUp]
        public void Setup()
        {
            _catalogService = new Mock<ICatalogService>();
            _notifications = new Mock<INotificationCollector>();

            _catalogImportUseCase = new CatalogImportUseCase(
                _catalogService.Object,
                _notifications.Object
            );
        }

        [Test]
        public async Task Import_OneProduct_ShouldCallCatalogService()
        {
            var productsDto = new List<ProductDto>
            {
                new ProductDto
                {
                    Id = "1d8b97a7-df44-49e5-9633-dc9cb6ab705a",
                    Name = "Produto Completo",
                    Brand = "Marca",
                    Category = "Categoria",
                    SellerName = "Seller"
                }
            };

            _notifications.Setup(x => x.HasErrorNotification()).Returns(false);
            _notifications.Setup(x => x.GetAllErrorNotifications()).Returns(new List<Notification>());

            var result = await _catalogImportUseCase.Import(productsDto);

            Assert.That(result.TotalRecords, Is.EqualTo(1));
            Assert.That(result.TotalErrors, Is.EqualTo(0));
            Assert.That(result.Errors, Is.Empty);

            _catalogService.Verify(x => x.Import(It.IsAny<List<Product>>()), Times.Once);
        }

        [Test]
        public async Task Import_TwoProducts_ShouldCallCatalogServiceTwice()
        {
            var productsDto = new List<ProductDto>
            {
                new ProductDto
                {
                    Id = "1d8b97a7-df44-49e5-9633-dc9cb6ab705a",
                    Name = "Produto Completo",
                    Brand = "Marca",
                    Category = "Categoria",
                    SellerName = "Seller"
                },
                new ProductDto
                {
                    Id = "58cf2741-c17c-4e58-8f74-0f27d8ffdd63",
                    Name = "Produto Completo 2",
                    Brand = "Marca",
                    Category = "Categoria",
                    SellerName = "Seller"
                }
            };

            _notifications.Setup(x => x.HasErrorNotification()).Returns(false);
            _notifications.Setup(x => x.GetAllErrorNotifications()).Returns(new List<Notification>());

            var result = await _catalogImportUseCase.Import(productsDto);

            Assert.That(result.TotalRecords, Is.EqualTo(2));
            Assert.That(result.TotalErrors, Is.EqualTo(0));
            Assert.That(result.Errors, Is.Empty);

            _catalogService.Verify(x => x.Import(It.IsAny<List<Product>>()), Times.Once);
        }

        [Test]
        public async Task Import_OneProductWithErrors_ShouldCallCatalogService()
        {
            var productsDto = new List<ProductDto>
            {
                new ProductDto
                {
                    Id = "1d8b97a7-df44-49e5-9633-dc9cb6ab705a",
                    Name = "",
                    Brand = "Marca",
                    Category = "Categoria",
                    SellerName = "Seller"
                }
            };

            _notifications.Setup(x => x.HasErrorNotification()).Returns(true);
            _notifications.Setup(x => x.GetAllErrorNotifications()).Returns(new List<Notification>
                {
                    new Notification
                    {
                        Type = NotificationType.Error,
                        Message = "1d8b97a7-df44-49e5-9633-dc9cb6ab705a - Name is required"
                    }
                });

            var result = await _catalogImportUseCase.Import(productsDto);

            Assert.That(result.TotalRecords, Is.EqualTo(1));
            Assert.That(result.TotalErrors, Is.EqualTo(1));
            Assert.That(result.Errors, Is.Not.Empty);
            Assert.That(result.Errors.Single(), Is.EqualTo("1d8b97a7-df44-49e5-9633-dc9cb6ab705a - Name is required"));

            _catalogService.Verify(x => x.Import(It.IsAny<List<Product>>()), Times.Once);
        }
    }
}