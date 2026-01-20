namespace AspireSimpleDbApp.Web;

internal class ProductApiClient(HttpClient httpClient)
{
    public async Task<List<Product>?> GetProductsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var response = await httpClient.GetAsync("products", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Product>>(
            cancellationToken: cancellationToken
        );
    }
}

internal class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
}
