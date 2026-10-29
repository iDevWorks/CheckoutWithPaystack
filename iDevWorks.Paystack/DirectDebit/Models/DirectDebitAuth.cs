using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models;

public sealed class DirectDebitAuth
{
    [JsonPropertyName("authorization_code")]
    public string AuthorizationCode { get; set; } = string.Empty;

    [JsonPropertyName("channel")]
    public string Channel { get; set; } = string.Empty;

    [JsonPropertyName("bank")]
    public string Bank { get; set; } = string.Empty;

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("customer")]
    public Customer? Customer { get; set; }
}
