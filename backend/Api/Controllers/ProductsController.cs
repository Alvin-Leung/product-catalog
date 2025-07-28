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
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await _context.Products.ToListAsync();
            }

            // Add a wildcard (*) to the search term to allow for prefix matching (e.g., "comp" matches "computer").
            // FTS5 uses a specific query syntax. We'll keep it simple here.
            var ftsQuery = $"{searchTerm}*";

            var products = await _context.Products
                .FromSqlInterpolated($@"
                    SELECT p.*
                    FROM Products p
                    JOIN (
                        SELECT ProductId, rank
                        FROM ProductFTS
                        WHERE ProductFTS MATCH {ftsQuery}
                    ) AS fts ON p.Id = fts.ProductId
                    ORDER BY fts.rank
                ")
                .AsNoTracking() // Avoid overhead of change tracking since we are only reading data
                .ToListAsync();

            return products; // TODO: Add dto-to-response mapping
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
