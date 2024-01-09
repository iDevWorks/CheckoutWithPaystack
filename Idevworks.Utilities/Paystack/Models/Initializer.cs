using System.Text.Json.Serialization;

namespace iDevWorks.Paystack
{
    internal class Initializer
    {
        public Initializer(string email, int amount, string callbackUrl, string reference)
        {
            Email = email;
            Amount = amount;
            CallbackUrl = callbackUrl;
            Reference = reference;
        }

        [JsonPropertyName("email")]
        public string Email { get; }

        [JsonPropertyName("amount")]
        public int Amount { get; }

        [JsonPropertyName("callback_url")]
        public string CallbackUrl { get; }

        [JsonPropertyName("reference")]
        public string Reference { get; }
    }
}
