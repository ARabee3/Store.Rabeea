using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using Store.Rabeea.Api.ErrorsModels;
namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        private readonly IServiceManager _serviceManager = serviceManager;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<ProductResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<PaginationResponse<ProductResultDto>>> GetAllProducts([FromQuery]ProductSpecificationParameters specParams)
        {
            var result = await _serviceManager.ProductService.GetAllProductsAsync(specParams);
            if (result is null) return BadRequest();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResultDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<ProductResultDto>> GetById(int id)
        {
            var result = await _serviceManager.ProductService.GetProductByIdAsync(id);
            if (result is null) throw new ProductNotFoundException(id);
            return Ok(result);
        }

        [HttpGet("brands")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrandResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<BrandResultDto>> GetAllBrands()
        {
            var result = await _serviceManager.ProductService.GetAllBrandsAsync();
            if (result is null) return BadRequest();
            return Ok(result);
        }
        [HttpGet("types")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TypeResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<TypeResultDto>> GetAllTypes()
        {
            var result = await _serviceManager.ProductService.GetAllTypesAsync();
            if (result is null) return BadRequest();
            return Ok(result);
        }
    }
}
