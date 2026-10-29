using System.Text;
using System.Text.Json;
using System.Security.Cryptography;
using iDevWorks.Paystack.Models;

namespace iDevWorks.Paystack;

public partial class PaystackClient
{
    public record EventData
    {
        public required string RawBody { get; init; }
        public required string? SignatureHex { get; init; }
        public required string SecretKey { get; init; }
    }

    public static async Task OnPaymentReceivedAsync(EventData data,
        Func<VirtualAccountCharge, CancellationToken, Task> handler,
        CancellationToken ct = default)
    {
        if (!IsValidSignature(data))
            throw new PaystackException(Status.INVALID_SIGNATURE, "Rejected webhook with invalid signature");

        // 1. Parse the envelope. Only JSON problems are expected here.
        PaystackEvent? evt;
        try
        {
            evt = JsonSerializer.Deserialize<PaystackEvent>(data.RawBody, _jsonOptions);
        }
        catch (JsonException ex)
        {
            throw new PaystackException(Status.MALFORMED_JSON, "Malformed webhook body", ex);
        }

        if (evt?.Event is null || evt.Data.ValueKind != JsonValueKind.Object)
            throw new PaystackException(Status.MALFORMED_JSON, "Webhook missing 'event' or 'data'");

        // 2. Dispatch. Anything unexpected here should trigger a Paystack retry.
        try
        {
            switch (evt.Event)
            {
                case "charge.success":
                    var charge = evt.Data.Deserialize<VirtualAccountCharge>(_jsonOptions)
                        ?? throw new PaystackException(Status.MALFORMED_JSON, "charge.success with empty data");

                    // Only DVA transfers; card/other charges are ignored here.
                    if (charge.Channel == "dedicated_nuban" || charge.Authorization?.Channel == "dedicated_nuban")
                         await handler(charge, ct);
                    return;

                default:
                    // Acknowledge (200) so Paystack stops sending it. Log evt.Event if you want visibility.
                    return;
            }
        }
        catch (PaystackException) { throw; }                       // already classified
        catch (OperationCanceledException) when (ct.IsCancellationRequested) { throw; }
        catch (JsonException ex)
        {
            throw new PaystackException(Status.MALFORMED_JSON, $"Malformed data for {evt.Event}", ex);
        }
        catch (Exception ex)
        {
            throw new PaystackException(Status.FAILED_TO_PROCESS_EVENT, $"Failed to process {evt.Event}", ex);
        }
    }

    internal static bool IsValidSignature(EventData data)
    {
        if (string.IsNullOrWhiteSpace(data.SignatureHex)) 
            return false;

        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(data.SecretKey));
        try
        {
            byte[] provided = Convert.FromHexString(data.SignatureHex);
            byte[] expected = hmac.ComputeHash(Encoding.UTF8.GetBytes(data.RawBody));
            return CryptographicOperations.FixedTimeEquals(expected, provided);
        }
        catch (Exception)
        {
            return false;
        }
    }
}
