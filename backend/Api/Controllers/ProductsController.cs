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
        // Improvements
        // - Add logging and observability
        // - Implement better error handling, especially in GetProducts endpoint
        // - Implement pagination in GetProducts endpoint
        // - Consider using a service layer for business logic separation
        // - Use DTOs for request and response models to decouple from database models

        private readonly ProductCatalogContext _context;

        public ProductsController(ProductCatalogContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<GetProductsResponse>> GetProducts([FromQuery] string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new GetProductsResponse
                {
                    Products = await _context.Products.ToListAsync()
                };
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

            return new GetProductsResponse
            {
                Products = products
            };
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
