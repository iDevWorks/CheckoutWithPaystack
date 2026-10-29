using System.Text.Json.Serialization;

namespace iDevWorks.Paystack;

internal sealed class PaystackResponse<TResult> where TResult : class
{
    [JsonPropertyName("status")]
    public bool Status { get; init; } = false;

    [JsonPropertyName("message")]
    public string? Message { get; init; } = string.Empty;

    [JsonPropertyName("data")]
    public TResult Data { get; init; } = default!;
}
