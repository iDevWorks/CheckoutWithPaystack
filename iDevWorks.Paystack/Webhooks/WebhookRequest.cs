namespace iDevWorks.Paystack;

public record WebhookRequest
{
    public required string RawBody { get; init; }
    public required string? SignatureHex { get; init; }
    public required string SecretKey { get; init; }
}
