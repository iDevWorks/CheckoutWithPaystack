using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models;

public sealed class Recipient
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("recipient_code")]
    public string RecipientCode { get; set; } = string.Empty;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("details")]
    public RecipientAccount? Details { get; set; }
}
