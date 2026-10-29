using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models;

public sealed class TransferItem
{
    public decimal Amount { get; init; }
    public string Reason { get; init; } = string.Empty;
    public string Reference { get; init; } = string.Empty;
    public string RecipientCode { get; init; } = string.Empty;
}
