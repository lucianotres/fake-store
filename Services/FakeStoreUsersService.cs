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
        _log.LogInformation("Buscando lista de usuários...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.GetAsync("/users");
        
        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode retornar lista de usuários");
            return new();
        }
        
        var users = await response.Content.ReadFromJsonAsync<List<User>>() ?? new();

        _log.LogInformation($"Retornado {users.Count} usuário(s)");
        return users;
    }

    public async Task<User?> GetUserAsync(int id)
    {
        _log.LogInformation($"Buscando usuário com ID {id}...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.GetAsync($"/users/{id}");
        
        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode retornar usuário com ID {id}");
            return null;
        }
        
        var user = await response.Content.ReadFromJsonAsync<User>();
        
        if (user == null)
        {
            _log.LogWarning($"Usuário com ID {id} não encontrado");
            return null;
        }
        
        _log.LogInformation($"Retornado usuário {id}.");
        return user;
    }

    public async Task<User?> PostUserAsync(User user)
    {
        _log.LogInformation($"Adicionando novo usuário...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.PostAsJsonAsync("/users", user);

        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode adicionar novo usuário");
            return null;
        }

        var newUser = await response.Content.ReadFromJsonAsync<User>();

        if (newUser == null)
        {
            _log.LogWarning("Usuário adicionado, mas não foi possível ler o retorno");
            return null;
        }

        _log.LogInformation($"Novo usuário adicionado com ID {newUser.Id}.");
        return newUser;
    }

    public async Task<User?> PutUserAsync(User user)
    {
        _log.LogInformation($"Atualizando usuário com ID {user.Id}...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.PutAsJsonAsync($"/users/{user.Id}", user);
        
        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode atualizar usuário com ID {user.Id}");
            return null;
        }
        
        var updatedUser = await response.Content.ReadFromJsonAsync<User>();
        
        if (updatedUser == null)
        {
            _log.LogWarning($"Usuário com ID {user.Id} atualizado, mas não foi possível ler o retorno");
            return null;
        }
        
        _log.LogInformation($"Usuário com ID {user.Id} atualizado.");
        return updatedUser;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        _log.LogInformation($"Removendo usuário com ID {id}...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.DeleteAsync($"/users/{id}");

        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode remover usuário com ID {id}");
            return false;
        }

        _log.LogInformation($"Usuário com ID {id} removido.");
        return true;
    }
}
