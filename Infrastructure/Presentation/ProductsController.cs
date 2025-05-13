using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        private readonly IServiceManager _serviceManager = serviceManager;

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(int? brandId, int? typeId, string? sort, int pageIndex = 1, int pageSize = 5)
        {
            var result = await _serviceManager.ProductService.GetAllProductsAsync(brandId,typeId,sort,pageIndex,pageSize);
            if (result is null) return BadRequest();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _serviceManager.ProductService.GetProductByIdAsync(id);
            if (result is null) return NotFound($"There is no Product associated with {id}");
            return Ok(result);
        }
        [HttpGet("brands")]
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await _serviceManager.ProductService.GetAllBrandsAsync();
            if (result is null) return BadRequest();
            return Ok(result);
        }
        [HttpGet("types")]
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await _serviceManager.ProductService.GetAllTypesAsync();
            if (result is null) return BadRequest();
            return Ok(result);
        }
    }
}
