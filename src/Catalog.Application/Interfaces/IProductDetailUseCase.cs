using Catalog.Application.Dtos;

namespace Catalog.Application.Interfaces
{
    public interface IProductDetailUseCase
    {
        Task<ProductDto> Detail(int id);
    }
}