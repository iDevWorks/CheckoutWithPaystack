using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models;

public sealed class DirectDebitCharge
{
    [JsonPropertyName("amount")]
    public long AmountInKobo { get; set; } 

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("transaction_date")]
    public DateTime TransactionDate { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;

    [JsonPropertyName("gateway_response")]
    public string GatewayResponse { get; set; } = string.Empty;

    [JsonPropertyName("channel")]
    public string Channel { get; set; } = string.Empty;

    [JsonPropertyName("fees")]
    public long? Fees { get; set; }

    [JsonPropertyName("authorization")]
    public DirectDebitChargeAuth? Authorization { get; set; }

    [JsonPropertyName("customer")]
    public Customer? Customer { get; set; }
}
