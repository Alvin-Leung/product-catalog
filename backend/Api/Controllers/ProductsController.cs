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
        public async Task<IResult> GenerateProducts(GenerateProductsRequest request)
        {
            if (request.NumProductsToGenerate <= 0)
            {
                return Results.Problem(new ProblemDetails
                {
                    Type = "Bad Request",
                    Title = "Invalid count",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Count must be greater than 0"
                });
            }
            
            
            var products = FakeProductGenerator.GenerateProducts(request.NumProductsToGenerate);

            try
            {
                await _context.Products.AddRangeAsync(products);
                await _context.SaveChangesAsync();

                return Results.Ok(value: new GenerateProductsResponse
                {
                    NumProductsGenerated = request.NumProductsToGenerate,
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = $"An error occurred while inserting products: {ex.Message}"
                });
            }
        }
    }
}
