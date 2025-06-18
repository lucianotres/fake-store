using System.Text.Json.Serialization;

namespace FakeProduct.Models;

public class Cotacao
{
    public string code { get; set; } = string.Empty;
    
    public string codein { get; set; } = string.Empty;

    public string? name { get; set; }

    /// <summary>
    /// Máximo
    /// </summary>
    [JsonConverter(typeof(StringToDecimalConverter))]
    public decimal? high { get; set; }

    /// <summary>
    /// Mínimo
    /// </summary>
    [JsonConverter(typeof(StringToDecimalConverter))]
    public decimal? low { get; set; }

    /// <summary>
    /// Variação
    /// </summary>
    [JsonConverter(typeof(StringToDecimalConverter))]
    public decimal? varbid { get; set; }

    /// <summary>
    /// Porcentual de variação
    /// </summary>
    [JsonConverter(typeof(StringToDecimalConverter))]
    public decimal? pctchange { get; set; }

    /// <summary>
    /// Compra
    /// </summary>
    [JsonConverter(typeof(StringToDecimalConverter))]
    public decimal? bid { get; set; }

    /// <summary>
    /// Venda
    /// </summary>
    [JsonConverter(typeof(StringToDecimalConverter))]
    public decimal? ask { get; set; }

    /// <summary>
    /// Hora da negociação UTC
    /// </summary>
    public string? timestamp { get; set; }

    /// <summary>
    /// Hora da negociação UTC-3
    /// </summary>
    public string? create_date { get; set; }


    public decimal? ConvertFromUSD(decimal? original) => 
        original.HasValue && ask.HasValue ? 
        Math.Round(original.Value * ask.Value, 2)
        : null;
}
