using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models;

public sealed class Balance
{
    [JsonPropertyName("balance")]
    public long BalanceInKobo { get; set; } 

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;
}

public sealed class TransferItem
{
    public decimal Amount { get; init; }
    public string Reason { get; init; } = string.Empty;
    public string Reference { get; init; } = string.Empty;
    public string RecipientCode { get; init; } = string.Empty;
}

public sealed class TransferResponse
{
    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;

    [JsonPropertyName("recipient")]
    public string Recipient { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public long AmountInKobo { get; set; }

    [JsonPropertyName("transfer_code")]
    public string TransferCode { get; set; } = string.Empty;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}

public sealed class Transfer
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public long AmountInKobo { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("reason")]
    public string Reason { get; set; } = string.Empty;

    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;

    [JsonPropertyName("transfer_code")]
    public string TransferCode { get; set; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("recipient")]
    public Recipient? Recipient { get; set; }
}

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

public sealed class RecipientAccount
{
    [JsonPropertyName("account_number")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("bank_code")]
    public string BankCode { get; set; } = string.Empty;

    [JsonPropertyName("bank_name")]
    public string BankName { get; set; } = string.Empty;
}
