using Shared;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IProductService
    {
        // Get All Products
        Task<IEnumerable<ProductResultDto>> GetAllProductsAsync(int? brandId,int? typeId,string? sort,int pageIndex = 1, int pageSize = 5);
        // Get Product By Id
        Task<ProductResultDto?> GetProductByIdAsync(int id);
        // Get All Types
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();

        // Get All Brands
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();


    }
}
