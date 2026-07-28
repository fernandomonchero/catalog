using Catalog.Application.Dtos;

namespace Catalog.Application.Interfaces
{
    public interface ICatalogListUseCase
    {
        Task<List<ProductDto>> List();
    }
}