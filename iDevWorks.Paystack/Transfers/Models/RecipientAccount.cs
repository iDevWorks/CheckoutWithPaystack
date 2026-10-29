using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models;

public sealed class RecipientAccount
{
    [JsonPropertyName("account_number")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("bank_code")]
    public string BankCode { get; set; } = string.Empty;

    [JsonPropertyName("bank_name")]
    public string BankName { get; set; } = string.Empty;
}
