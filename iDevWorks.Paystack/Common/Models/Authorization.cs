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
