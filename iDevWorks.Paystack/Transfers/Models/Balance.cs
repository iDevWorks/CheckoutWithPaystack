using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models;

public sealed class Balance
{
    [JsonPropertyName("balance")]
    public long BalanceInKobo { get; set; } 

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;
}
