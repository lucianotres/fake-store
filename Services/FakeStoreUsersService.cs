using FakeProduct.Models;
using System.Net.Http.Json;

namespace FakeProduct.Services;

public class FakeStoreUsersService
{
    private ILogger _log;
    private IHttpClientFactory _httpClientFactory;

    public FakeStoreUsersService(ILogger<FakeStoreUsersService> logger, IHttpClientFactory httpClientFactory)
    {
        _log = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        _log.LogInformation("Buscando lista de usu�rios...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.GetAsync("/users");
        
        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: n�o pode retornar lista de usu�rios");
            return new();
        }
        
        var users = await response.Content.ReadFromJsonAsync<List<User>>() ?? new();

        _log.LogInformation($"Retornado {users.Count} usu�rio(s)");
        return users;
    }

    public async Task<User?> GetUserAsync(int id)
    {
        _log.LogInformation($"Buscando usu�rio com ID {id}...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.GetAsync($"/users/{id}");
        
        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: n�o pode retornar usu�rio com ID {id}");
            return null;
        }
        
        var user = await response.Content.ReadFromJsonAsync<User>();
        
        if (user == null)
        {
            _log.LogWarning($"Usu�rio com ID {id} n�o encontrado");
            return null;
        }
        
        _log.LogInformation($"Retornado usu�rio {id}.");
        return user;
    }

    public async Task<User?> PostUserAsync(User user)
    {
        _log.LogInformation($"Adicionando novo usu�rio...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.PostAsJsonAsync("/users", user);

        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: n�o pode adicionar novo usu�rio");
            return null;
        }

        var newUser = await response.Content.ReadFromJsonAsync<User>();

        if (newUser == null)
        {
            _log.LogWarning("Usu�rio adicionado, mas n�o foi poss�vel ler o retorno");
            return null;
        }

        _log.LogInformation($"Novo usu�rio adicionado com ID {newUser.Id}.");
        return newUser;
    }

    public async Task<User?> PutUserAsync(User user)
    {
        _log.LogInformation($"Atualizando usu�rio com ID {user.Id}...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.PutAsJsonAsync($"/users/{user.Id}", user);
        
        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: n�o pode atualizar usu�rio com ID {user.Id}");
            return null;
        }
        
        var updatedUser = await response.Content.ReadFromJsonAsync<User>();
        
        if (updatedUser == null)
        {
            _log.LogWarning($"Usu�rio com ID {user.Id} atualizado, mas n�o foi poss�vel ler o retorno");
            return null;
        }
        
        _log.LogInformation($"Usu�rio com ID {user.Id} atualizado.");
        return updatedUser;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        _log.LogInformation($"Removendo usu�rio com ID {id}...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.DeleteAsync($"/users/{id}");

        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: n�o pode remover usu�rio com ID {id}");
            return false;
        }

        _log.LogInformation($"Usu�rio com ID {id} removido.");
        return true;
    }
}
