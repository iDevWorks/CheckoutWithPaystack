using System.Text.Json.Serialization;

namespace iDevWorks.Paystack
{
    internal class Result<TResult>(bool status, string? message, TResult data) where TResult : class
    {
        [JsonPropertyName("status")]
        public bool Status { get; } = status;

        [JsonPropertyName("message")]
        public string? Message { get; } = message;

        [JsonPropertyName("data")]
        public TResult Data { get; } = data;
    }
}
