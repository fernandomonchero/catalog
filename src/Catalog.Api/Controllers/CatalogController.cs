using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/catalog")]
    public class CatalogController : Controller 
    {
        private ICatalogImportUseCase _catalogImportUseCase;
        private ICatalogListUseCase _catalogListUseCase;
        private IProductDetailUseCase _productDetailUseCase;

        public CatalogController(ICatalogImportUseCase catalogImportUseCase, ICatalogListUseCase catalogListUseCase, IProductDetailUseCase productDetailUseCase)
        {
            _catalogImportUseCase = catalogImportUseCase;
            _catalogListUseCase = catalogListUseCase;
            _productDetailUseCase = productDetailUseCase;
        }

        [HttpPost]
        [Route("import")]
        public async Task<ActionResult<ProductDto>> Import(List<ProductDto> productsDto)
        {
            var result = await _catalogImportUseCase.Import(productsDto);

            return Ok(new ApiResponse<ImportResultDto>
            {
                Success = true,
                Data = result
            });
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> Get()
        {
            return await _catalogListUseCase.List();
        }

        [HttpGet("product/{id:int}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            var productDto = await _productDetailUseCase.Detail(id);

            if (productDto != null)
                return productDto;

            return NotFound();
        }
    }
}