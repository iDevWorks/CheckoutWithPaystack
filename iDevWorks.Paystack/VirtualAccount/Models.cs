using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models;

public sealed class VirtualAccount
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

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("customer")]
    public Customer? Customer { get; set; }

    [JsonPropertyName("bank")]
    public VirtualAccountProvider? Bank { get; set; }
}

public sealed class VirtualAccountProvider
{
    [JsonPropertyName("bank_id"), JsonAlias("id")] 
    public int ID { get; set; } = -1;

    [JsonPropertyName("bank_name"), JsonAlias("name")] 
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("provider_slug"), JsonAlias("slug")] 
    public string Slug { get; set; } = string.Empty;
}

public sealed class VirtualAccountCharge
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("reference")]
    public string? Reference { get; set; }

    [JsonPropertyName("amount")]
    public long AmountInKobo { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("channel")]
    public string? Channel { get; set; }

    [JsonPropertyName("fees")]
    public long? Fees { get; set; }

    [JsonPropertyName("paid_at")]
    public DateTime? PaidAt { get; set; }

    [JsonPropertyName("customer")]
    public Customer? Customer { get; set; }

    [JsonPropertyName("authorization")]
    public VirtualAccountChargeAuth? Authorization { get; set; }

    [JsonPropertyName("metadata")]
    public JsonElement? Metadata { get; set; }

    public Guid GetTransactionGuid()
    {
        string number = Reference ?? string.Empty;
        if (string.IsNullOrEmpty(number) || number.Length > 32 || !number.All(char.IsAsciiDigit))
            throw new ArgumentException("Expected 1–32 decimal digits.", nameof(Reference));

        return Guid.ParseExact(number.PadLeft(32, '0'), "N");
    }

}

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

//public static class ProviderIdGuid
//{
//    // Set this to the provider's fixed ID length so leading zeros survive the round trip.
//    private const int IdLength = 30;

//    public static Guid ToGuid(string number)
//    {
//        if (string.IsNullOrEmpty(number) || number.Length > 32 || !number.All(char.IsAsciiDigit))
//            throw new ArgumentException("Expected 1–32 decimal digits.", nameof(number));

//        return Guid.ParseExact(number.PadLeft(32, '0'), "N");
//    }

//    public static string ToNumber(Guid guid)
//    {
//        var hex = guid.ToString("N"); // 32 chars, no dashes
//        if (hex.Any(c => c > '9'))
//            throw new FormatException("This GUID wasn't created from a provider number.");

//        return hex.Substring(32 - IdLength);
//        // If lengths vary and IDs never start with 0, use this instead:
//        // var s = hex.TrimStart('0'); return s.Length == 0 ? "0" : s;
//    }
//}