using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Catalog.Application.Mappings;
using Catalog.Domain.Interfaces;

namespace Catalog.Application.UseCases
{
    public class CatalogImportUseCase : ICatalogImportUseCase
    {
        private ICatalogService _catalogService;

        private INotificationCollector _notificationCollector;

        public CatalogImportUseCase(ICatalogService catalogService, INotificationCollector notificationCollector)
        {
            _catalogService = catalogService;
            _notificationCollector = notificationCollector;
        }

        public async Task<ImportResultDto> Import(List<ProductDto> productsDto)
        {
            var products = ProductMapper.ToEntitySet(productsDto);

            await _catalogService.Import(products);

            var result = new ImportResultDto
            {
                TotalRecords = products.Count,
                TotalErrors = _notificationCollector.GetAllErrorNotifications().Count(),
            };

            if (_notificationCollector.HasErrorNotification())
                result.Errors.AddRange(_notificationCollector.GetAllErrorNotifications().Select(n => n.Message));

            return result;
        }
    }
}