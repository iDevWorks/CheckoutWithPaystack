using System.Text.Json.Serialization;

namespace iDevWorks.Paystack
{
    internal class Initializer(string email, int amount, string callbackUrl, string reference)
    {
        [JsonPropertyName("email")]
        public string Email { get; } = email;

        [JsonPropertyName("amount")]
        public int Amount { get; } = amount;

        [JsonPropertyName("callback_url")]
        public string CallbackUrl { get; } = callbackUrl;

        [JsonPropertyName("reference")]
        public string Reference { get; } = reference;
    }
}
