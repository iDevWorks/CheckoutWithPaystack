using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models;

public sealed class DirectDebitChargeAuth
{
    [JsonPropertyName("authorization_code")]
    public string AuthorizationCode { get; set; } = string.Empty;

    [JsonPropertyName("bin")]
    public string? Bin { get; set; }

    [JsonPropertyName("last4")]
    public string? Last4 { get; set; }

    [JsonPropertyName("exp_month")]
    public string? ExpMonth { get; set; }

    [JsonPropertyName("exp_year")]
    public string? ExpYear { get; set; }

    [JsonPropertyName("channel")]
    public string? Channel { get; set; }

    [JsonPropertyName("card_type")]
    public string? CardType { get; set; }

    [JsonPropertyName("bank")]
    public string? Bank { get; set; }

    [JsonPropertyName("country_code")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("brand")]
    public string? Brand { get; set; }

    [JsonPropertyName("reusable")]
    public bool Reusable { get; set; }
}
