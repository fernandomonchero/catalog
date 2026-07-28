using Catalog.Application.Dtos;

namespace Catalog.Application.Interfaces
{
    public interface ICatalogImportUseCase
    {
       Task<ImportResultDto> Import(List<ProductDto> productsDto);
    }
}