using FakeProduct.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace FakeProduct.Services;

public class AwesomeApiService
{
    private ILogger _log;
    private IHttpClientFactory _httpClientFactory;

    public AwesomeApiService(ILogger<AwesomeApiService> logger, IHttpClientFactory httpClientFactory)
    {
        _log = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Cotacao?> UltimaCotacao(string de, string para)
    {
        _log.LogInformation($"Buscando última cotação de {de} para {para}...");

        var response = await _httpClientFactory
            .CreateClient("awesomeapi")
            .GetAsync($"/json/last/{de}-{para}");

        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Falha {response.StatusCode}: não pode retornar última cotação de {de} para {para}");
            return null;
        }

        var jsonResponse = await response.Content.ReadFromJsonAsync<JsonObject>();
        if (jsonResponse == null)
        {
            _log.LogError("Resposta JSON inválida ou vazia.");
            return null;
        }

        var cotacaoJson = jsonResponse
            .FirstOrDefault(x => x.Key == $"{de}{para}")
            .Value;

        _log.LogTrace(cotacaoJson?.ToJsonString() ?? "Resposta JSON vazia ou inválida.");

        return cotacaoJson.Deserialize<Cotacao>();
    }

}
