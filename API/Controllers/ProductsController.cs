

using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly StoreContext _context;
        public ProductsController(ILogger<ProductsController> logger,StoreContext context)
        {
            _logger = logger;
            _context=context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts(){
             var products=await _context.Products.ToListAsync();
             return products;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GtProduct(int id)
        {
            return await _context.Products.FindAsync(id);
            
        }
    }
}