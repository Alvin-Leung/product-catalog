using Bogus;
using Api.Models;
using Api.Models.Database;

namespace Api.Domain
{
    public static class FakeProductGenerator
    {
        public static List<Product> GenerateProducts(int count)
        {
            return GetProductFaker().Generate(count);
        }

        private static Faker<Product> GetProductFaker()
        {
            var productFaker = new Faker<Product>()
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.CreatedTimestampUtc, f => f.Date.Past(yearsToGoBack: 2))
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Category, f => f.Commerce.Categories(1).First())
                .RuleFor(p => p.Brand, f => f.Company.CompanyName())
                .RuleFor(p => p.Price, GenerateDecimal)
                .RuleFor(p => p.StockQuantity, f => f.Random.Number(0, 500))
                .RuleFor(p => p.Sku, f => f.Random.Int(100000, 999999))
                .RuleFor(p => p.ReleaseTimestampUtc, f => f.Date.Past(1, DateTime.UtcNow.AddMonths(-1)))
                .RuleFor(p => p.AvailabilityStatus, f => f.PickRandom<Availability>())
                .RuleFor(p => p.CustomerRating, f => f.Random.Number(1, 5))
                .RuleFor(p => p.AvailableColors, f => f.Random.Bool(0.8f) ? // 80% chance of having colors
                                new List<string>(f.Commerce.Color().Split(' ').Take(f.Random.Number(1, 3))) : null)
                .RuleFor(p => p.AvailableSizes, f => f.Random.Bool(0.7f) ? // 70% chance of having sizes
                                f.PickRandom(new[] { "XS", "S", "M", "L", "XL", "XXL" }, f.Random.Number(1, 4)).ToList() : null);

            return productFaker;
        }

        private static decimal GenerateDecimal(Faker faker)
        {
            return decimal.Parse(faker.Commerce.Price(min: 10, max: 2000, decimals: 2));
        }
    }
}