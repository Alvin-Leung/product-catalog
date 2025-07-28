namespace Api.Models.ApiContract;

public class GenerateProductsRequest
{
    /// <summary>
    /// Optional count of products to generate. Defaults to 1000 if not provided.
    /// </summary>
    public int NumProductsToGenerate { get; init; } = 1000;
}