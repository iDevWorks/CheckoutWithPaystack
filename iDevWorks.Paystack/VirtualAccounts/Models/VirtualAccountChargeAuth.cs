using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models;

public sealed class VirtualAccountChargeAuth
{
    [JsonPropertyName("channel")]
    public string? Channel { get; set; }

    [JsonPropertyName("sender_name")]
    public string? SenderName { get; set; }

    [JsonPropertyName("sender_bank")]
    public string? SenderBank { get; set; }

    [JsonPropertyName("sender_bank_account_number")]
    public string? SenderBankAccountNumber { get; set; }

    [JsonPropertyName("receiver_bank_account_number")]
    public string? ReceiverBankAccountNumber { get; set; }

    [JsonPropertyName("receiver_bank")]
    public string? ReceiverBank { get; set; }

    [JsonPropertyName("narration")]
    public string? Narration { get; set; }
}
