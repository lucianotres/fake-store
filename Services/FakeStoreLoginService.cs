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

    public async Task<string?> FazerLogin(string username, string password)
    {
        _log.LogInformation($"Tentando fazer login com usuário {username}...");
        var client = _httpClientFactory.CreateClient("fakestore");
        var response = await client.PostAsJsonAsync("/auth/login", new { username, password });

        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não foi possível fazer login");
            return null;
        }

        var responseContent = await response.Content.ReadFromJsonAsync<LoginTokenResponse>();

        if (responseContent == null || string.IsNullOrWhiteSpace(responseContent.Token))
        {
            _log.LogError("Resposta vazia do servidor");
            return null;
        }

        return responseContent.Token;
    }
}

public class LoginTokenResponse
{
    public string Token { get; set; } = string.Empty;
}