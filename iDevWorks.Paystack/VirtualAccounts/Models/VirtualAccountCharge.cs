using System.Text.Json;
using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models;

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
