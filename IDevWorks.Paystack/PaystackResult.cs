using System.Text.Json;
using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models
{
    internal sealed class PaystackResult<TResult> where TResult : class
    {
        [JsonPropertyName("status")]
        public bool Status { get; init; }  = false;

        [JsonPropertyName("message")]
        public string? Message { get; init; } = string.Empty;

        [JsonPropertyName("data")]
        public TResult Data { get; init; } = default!;
    }

    internal sealed class PaystackEvent() 
    {
        [JsonPropertyName("event")]
        public string Event { get; init; } = string.Empty;

        [JsonPropertyName("data")]
        public JsonElement Data { get; init; } = default!;
    }
}
