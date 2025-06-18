using FakeProduct.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FakeProduct.Models;

public sealed class MinhasCotacoes : INotifyPropertyChanged
{
    public static readonly MinhaCotacao Dolar = new ("USD", "BRL", "Reais");
    public static readonly MinhaCotacao Euro = new ("USD", "EUR", "Euro");
    public static readonly MinhaCotacao Libra = new ("USD", "GBP", "Libra Esterlina");
    public static readonly MinhaCotacao Iene = new ("USD", "JPY", "Iene Japonês");
    public static readonly MinhaCotacao Bitcoin = new ("USD", "CAD", "Dólar Canadence");
    public static readonly MinhaCotacao Peso = new ("USD", "MXN", "Dólar Mexicano");

    public static readonly List<MinhaCotacao> Todas = new()
    {
        Dolar,
        Euro,
        Libra,
        Iene,
        Bitcoin,
        Peso
    };

    private readonly AwesomeApiService _AwesomeApiService;
    private readonly ILogger _log;

    public MinhasCotacoes(AwesomeApiService awesomeApiService, ILogger<MinhasCotacoes> logger)
    {
        _AwesomeApiService = awesomeApiService;
        _log = logger;
    }

    public MinhaCotacao? Escolhida { get; private set; } = null;
    public Cotacao? Cotacao { get; private set; } = null;

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? propertyName = default)
        => PropertyChanged?.Invoke(this, new(propertyName));
    private void SetCotacaoEscolhidaChanged()
    {
        OnPropertyChanged(nameof(Cotacao));
        OnPropertyChanged(nameof(Escolhida));       
    }

    public async Task DefinirCotacaoEscolhida(MinhaCotacao? cotacaoEscolhida)
    {
        if (cotacaoEscolhida == null)
        {
            _log.LogInformation("Limpando a cotação atual.");
            Cotacao = null;
            Escolhida = null;
            SetCotacaoEscolhidaChanged();
            return;
        }
                
        var cotacao = await _AwesomeApiService.UltimaCotacao(cotacaoEscolhida.De, cotacaoEscolhida.Para);
        if (cotacao != null)
        {
            _log.LogInformation($"Cotação atualizada: {cotacaoEscolhida.Nome} ({cotacaoEscolhida.De} para {cotacaoEscolhida.Para})");
            Cotacao = cotacao;
            Escolhida = cotacaoEscolhida;
            SetCotacaoEscolhidaChanged();
        }
    }
}

public sealed class MinhaCotacao
{
    public string De { get; private set; }
    public string Para { get; private set; }
    public string Nome { get; private set; }

    public MinhaCotacao(string de, string para, string nome)
    {
        De = de;
        Para = para;
        Nome = nome;
    }
}