using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications;
using Shared;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork,IMapper mapper) : IProductService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<IEnumerable<ProductResultDto>> GetAllProductsAsync(int? brandId, int? typeId,string? sort)
        {
            var spec = new ProductWithBrandsAndTypesSpecifications(brandId,typeId,sort);
            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(spec);
            var result = _mapper.Map<IEnumerable<ProductResultDto>>(products);
            return result;
        }
        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandsAndTypesSpecifications(id);
            var product = await _unitOfWork.GetRepository<Product, int>().GetAsync(spec);
            if (product is null) return null;
            var result = _mapper.Map<ProductResultDto>(product);
            return result;
            
        }
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<BrandResultDto>>(brands);
            return result;
        }
       
        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<TypeResultDto>>(types);
            return result;
        }

       
    }
}
