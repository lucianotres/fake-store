using FakeProduct.Models;
using System.Net.Http.Json;

namespace FakeProduct.Services;

public class FakeStoreCartsService
{
    private ILogger _log;
    private IHttpClientFactory _httpClientFactory;

    public FakeStoreCartsService(ILogger<FakeStoreCartsService> logger, IHttpClientFactory httpClientFactory)
    {
        _log = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<Cart>> GetCartsAsync()
    {
        _log.LogInformation("Buscando lista de carrinhos...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.GetAsync("/carts");
        
        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode retornar lista de carrinhos");
            return new();
        }
        
        var carts = await response.Content.ReadFromJsonAsync<List<Cart>>() ?? new();

        _log.LogInformation($"Retornado {carts.Count} carrinho(s)");
        return carts;
    }

    public async Task<Cart?> GetCartAsync(int id)
    {
        _log.LogInformation($"Buscando carrinho com ID {id}...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.GetAsync($"/carts/{id}");
        
        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode retornar carrinho com ID {id}");
            return null;
        }
        
        var cart = await response.Content.ReadFromJsonAsync<Cart>();
        
        if (cart == null)
        {
            _log.LogWarning($"Carrinho com ID {id} não encontrado");
            return null;
        }
        
        _log.LogInformation($"Retornado carrinho {id}.");
        return cart;
    }

    public async Task<Cart?> PostCartAsync(Cart cart)
    {
        _log.LogInformation($"Adicionando novo carrinho...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.PostAsJsonAsync("/carts", cart);

        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode adicionar novo carrinho");
            return null;
        }

        var newCart = await response.Content.ReadFromJsonAsync<Cart>();

        if (newCart == null)
        {
            _log.LogWarning("Carrinho adicionado, mas não foi possível ler o retorno");
            return null;
        }

        _log.LogInformation($"Novo carrinho adicionado com ID {newCart.Id}.");
        return newCart;
    }

    public async Task<Cart?> PutCartAsync(Cart cart)
    {
        _log.LogInformation($"Atualizando carrinho com ID {cart.Id}...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.PutAsJsonAsync($"/carts/{cart.Id}", cart);
        
        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode atualizar carrinho com ID {cart.Id}");
            return null;
        }
        
        var updatedCart = await response.Content.ReadFromJsonAsync<Cart>();
        
        if (updatedCart == null)
        {
            _log.LogWarning($"Carrinho com ID {cart.Id} atualizado, mas não foi possível ler o retorno");
            return null;
        }
        
        _log.LogInformation($"Carrinho com ID {cart.Id} atualizado.");
        return updatedCart;
    }

    public async Task<bool> DeleteCartAsync(int id)
    {
        _log.LogInformation($"Removendo carrinho com ID {id}...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.DeleteAsync($"/carts/{id}");

        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode remover carrinho com ID {id}");
            return false;
        }

        _log.LogInformation($"Carrinho com ID {id} removido.");
        return true;
    }
}
