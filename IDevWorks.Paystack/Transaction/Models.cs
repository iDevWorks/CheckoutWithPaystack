using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models;

public sealed class Authorization
{
    [JsonPropertyName("authorization_url"), JsonAlias("redirect_url")]
    public string AuthorizationUrl { get; init; } = string.Empty;

    [JsonPropertyName("access_code")]
    public string AccessCode { get; init; } = string.Empty;

    [JsonPropertyName("reference")]
    public string Reference { get; init; } = string.Empty;
}

public sealed class Transaction
{
    [JsonPropertyName("amount")]
    public decimal AmountInKobo { get; init; } = decimal.Zero;

    [JsonPropertyName("reference")]
    public string Reference { get; init; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;

    [JsonPropertyName("gateway_response")]
    public string GatewayResponse { get; init; } = string.Empty;

    [JsonPropertyName("requested_amount")]
    public decimal RequestedAmount { get; init; } = decimal.Zero;

    [JsonPropertyName("customer")]
    public Customer? Customer { get; init; } 

    public bool IsSuccessful => Status == "success";
}
