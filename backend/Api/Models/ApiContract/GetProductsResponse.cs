using Api.Models.Database;

namespace Api.Models.ApiContract
{
    public class GetProductsResponse
    {
        public int Count => Products.Count;

        public required IList<Product> Products { get; init; }
    }
}