using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models
{
    public class CustomerResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("customer_code")]
        public string CustomerCode { get; set; } = string.Empty;

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("dedicated_account")] 
        public DedicatedAccount? DedicatedAccount { get; set; }
    }
}
