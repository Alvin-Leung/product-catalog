using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models.Database;
using Api.Domain;
using Api.Models.ApiContract;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductCatalogContext _context;

        public ProductsController(ProductCatalogContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts() // TODO: Create dto-to-response mapping
        {
            return await _context.Products.ToListAsync();
        }

        [HttpPost("generate")]
        public async Task<ActionResult<GenerateProductsResponse>> GenerateProducts(int count)
        {
            var products = FakeProductGenerator.GenerateProducts(count);

            try
            {
                await _context.Products.AddRangeAsync(products);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GenerateProducts", value: new GenerateProductsResponse
                {
                    Count = products.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while inserting products: {ex.Message}");
            }
        }
    }
}
