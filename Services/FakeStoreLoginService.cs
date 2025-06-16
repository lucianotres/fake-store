using System.Net.Http.Json;

namespace FakeProduct.Services;

public class FakeStoreLoginService
{
    private ILogger _log;
    private IHttpClientFactory _httpClientFactory;

    public FakeStoreLoginService(ILogger<FakeStoreLoginService> logger, IHttpClientFactory httpClientFactory)
    {
        _log = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> FazerLogin(string username, string password)
    {
        _log.LogInformation($"Tentando fazer login com usuário {username}...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.PostAsJsonAsync("/auth/login", new { username, password });

        if (response.IsSuccessStatusCode)
            _log.LogInformation($"Login realizado com sucesso!");
        else
            _log.LogError($"Falha {response.StatusCode}: não foi possível fazer login");

        return response.IsSuccessStatusCode;
    }
}
