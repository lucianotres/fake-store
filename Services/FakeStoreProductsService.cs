using FakeProduct.Models;
using System.ComponentModel;
using System.Net.Http.Json;

namespace FakeProduct.Services;

public class FakeStoreProductsService
{
    private ILogger<FakeStoreProductsService> _log;
    private IHttpClientFactory _httpClientFactory;

    public FakeStoreProductsService(ILogger<FakeStoreProductsService> logger, IHttpClientFactory httpClientFactory)
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

    public async Task<Product?> GetProductAsync(int id)
    {
        _log.LogInformation($"Buscando produto com ID {id}...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.GetAsync($"/products/{id}");

        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode retornar produto com ID {id}");
            return null;
        }

        var product = await response.Content.ReadFromJsonAsync<Product>();

        if (product == null)
        {
            _log.LogWarning($"Produto com ID {id} não encontrado");
            return null;
        }

        _log.LogInformation($"Retornado produto {id}.");
        return product;
    }

    public async Task<Product?> PostProductAsync(Product product)
    {
        _log.LogInformation("Criando novo produto...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.PostAsJsonAsync("/products", product);
        
        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode criar produto");
            return null;
        }

        var newProduct = await response.Content.ReadFromJsonAsync<Product>();

        _log.LogInformation($"Produto criado com sucesso, novo ID: {newProduct?.Id}");
        return newProduct;
    }

    public async Task<Product?> PutProductAsync(Product product)
    {
        _log.LogInformation($"Atualizando produto com ID {product.Id}...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.PutAsJsonAsync($"/products/{product.Id}", product);

        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode atualizar produto com ID {product.Id}");
            return null;
        }

        var updatedProduct = await response.Content.ReadFromJsonAsync<Product>();

        _log.LogInformation($"Produto com ID {updatedProduct?.Id} atualizado com sucesso.");
        return updatedProduct;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        _log.LogInformation($"Excluindo produto com ID {id}...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.DeleteAsync($"/products/{id}");
        
        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode excluir produto com ID {id}");
            return false;
        }

        _log.LogInformation($"Produto com ID {id} excluído com sucesso.");
        return true;
    }
}
