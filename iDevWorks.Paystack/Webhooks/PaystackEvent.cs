using System.Text.Json;
using System.Text.Json.Serialization;

namespace iDevWorks.Paystack;

internal sealed class PaystackEvent
{
    [JsonPropertyName("event")]
    public string Event { get; init; } = string.Empty;

    [JsonPropertyName("data")]
    public JsonElement Data { get; init; } = default!;
}
