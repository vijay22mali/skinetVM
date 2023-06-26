

using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    
    public class ProductsController : BaseApiController
    {
        private readonly ILogger<ProductsController> _logger;
         private readonly IGenericRepository<Product> _productsRepo;
         private readonly IGenericRepository<ProductBrand> _productBrandRepo;
         private readonly IGenericRepository<ProductType> _productTypeRepo;
         private readonly IMapper _mapper;
        public ProductsController(ILogger<ProductsController> logger,
        IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductBrand> productBrandRepo,
        IGenericRepository<ProductType> productTypeRepo,
        IMapper mapper)
        {
            _logger = logger;
           _productsRepo=productsRepo;
           _productBrandRepo=productBrandRepo;
           _productTypeRepo=productTypeRepo;
           _mapper=mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
           [FromQuery] ProductSpecParams productParams)
        {
            var spec=new ProductwithTypesAndBrandsSpecification(productParams);
            var countSpec=new ProductWithFiltersForCountSpecification(productParams);

            var totalItems= await _productsRepo.CountAsync(countSpec);
            var products=await _productsRepo.ListAsnyc(spec);

            var data =_mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,productParams.PageSize,totalItems,data));
             
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
          [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GtProduct(int id)
        {
            var spec=new ProductwithTypesAndBrandsSpecification(id);
            var product= await _productsRepo.GetEntityWithSpec(spec);

            if(product==null)
                return NotFound(new ApiResponse(404));

            return _mapper.Map<Product,ProductToReturnDto>(product);
            
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAnsyc());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAnsyc());
        }
    }
}