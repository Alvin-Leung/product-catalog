namespace Api.Models.Database
{
    public class Product
    {
        public required Guid Id { get; init; }
        public required DateTime CreatedTimestampUtc { get; init; } = DateTime.UtcNow;

        public required string Name { get; init; }
        public required string Description { get; init; }
        public required string Category { get; init; }
        public required string Brand { get; init; }
        public required decimal Price { get; init; }
        public required int StockQuantity { get; init; }
        public required int Sku { get; init; }
        public required DateTime ReleaseTimestampUtc { get; init; }
        public required Availability AvailabilityStatus { get; init; }
        public required int? CustomerRating { get; init; }
        public IEnumerable<string>? AvailableColors { get; init; }
        public IEnumerable<string>? AvailableSizes { get; init; }
    }
}