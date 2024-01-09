using System.Text.Json.Serialization;

namespace iDevWorks.Paystack
{
    internal class Result<T> where T : class
    {
        public Result(bool status, string? message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        [JsonPropertyName("status")]
        public bool Status { get; }

        [JsonPropertyName("message")]
        public string? Message { get; }

        [JsonPropertyName("data")]
        public T Data { get; }
    }
}
