using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models
{
    public class DedicatedAccount
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; } = string.Empty;

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; } = string.Empty;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("assigned")]
        public bool Assigned { get; set; }

        [JsonPropertyName("bank")]
        public BankProviderResponse? Bank { get; set; }

        [JsonPropertyName("customer")]
        public CustomerResponse? Customer { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }
    }

    public class BankProviderResponse
    {
        [JsonPropertyName("bank_id")] //id
        public int BankID { get; set; } = -1;

        [JsonPropertyName("bank_name")] //name
        public string BankName { get; set; } = string.Empty;

        [JsonPropertyName("provider_slug")] //slug
        public string ProviderCode { get; set; } = string.Empty;
    }

    public sealed class DvaBank
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
    }

}
