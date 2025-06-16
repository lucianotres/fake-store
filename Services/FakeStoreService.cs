using FakeProduct.Models;
using System.Net.Http.Json;

namespace FakeProduct.Services;

public class FakeStoreService
{
    private ILogger<FakeStoreService> _log;
    private IHttpClientFactory _httpClientFactory;

    public FakeStoreService(ILogger<FakeStoreService> logger, IHttpClientFactory httpClientFactory)
    {
        _log = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        _log.LogInformation("Buscando lista de produtos...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.GetAsync("/products");

        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode retornar lista de produtos");
            return new ();
        }

        var products = await response.Content.ReadFromJsonAsync<List<Product>>() ?? new ();
        
        _log.LogInformation($"Retornado {products.Count} produto(s)");
        return products;
    }

}
