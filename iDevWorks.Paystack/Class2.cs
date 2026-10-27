//using System;
//using System.Collections.Concurrent;
//using System.IO;
//using System.Security.Cryptography;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Routing;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

//namespace Billing.Paystack;

//// =============================================================================
//// Wiring (Program.cs)
////
////   builder.Services.AddPaystackWebhook(builder.Configuration);
////   builder.Services.AddScoped<IPaystackDvaEventHandler, MyDvaEventHandler>();
////   // Replace the in-memory store with a DB-backed one in production:
////   // builder.Services.AddScoped<IWebhookEventStore, SqlWebhookEventStore>();
////
////   app.MapPaystackWebhook("/webhooks/paystack");
////
//// appsettings.json:
////   "Paystack": {
////     "SecretKey": "sk_live_...",
////     "EnforceIpAllowlist": true
////   }
////
//// Set this URL as the webhook URL in Paystack Dashboard → Settings → API Keys & Webhooks.
//// =============================================================================

//public static class PaystackWebhookExtensions
//{
//    public static IServiceCollection AddPaystackWebhook(this IServiceCollection services, IConfiguration config)
//    {
//        services.AddSingleton(new PaystackWebhookOptions
//        {
//            SecretKey = config["Paystack:SecretKey"]
//                ?? throw new InvalidOperationException("Paystack:SecretKey is not configured."),
//            EnforceIpAllowlist = config.GetValue("Paystack:EnforceIpAllowlist", false)
//        });
//        services.AddSingleton<IWebhookEventStore, InMemoryWebhookEventStore>(); // swap for DB store in prod
//        return services;
//    }

//    public static IEndpointConventionBuilder MapPaystackWebhook(this IEndpointRouteBuilder app, string pattern)
//        => app.MapPost(pattern, PaystackWebhookEndpoint.HandleAsync);
//}

//public sealed class PaystackWebhookOptions
//{
//    public required string SecretKey { get; init; }

//    /// <summary>
//    /// Only accept requests from Paystack's published webhook IPs. Turn off if you sit behind a
//    /// proxy that doesn't forward the client IP (or configure ForwardedHeaders first).
//    /// </summary>
//    public bool EnforceIpAllowlist { get; init; }

//    public static readonly string[] PaystackIps = { "52.31.139.75", "52.49.173.169", "52.214.14.220" };
//}

//// =============================================================================
//// Endpoint
//// =============================================================================

//internal static class PaystackWebhookEndpoint
//{
//    private static readonly JsonSerializerOptions Json = new()
//    {
//        PropertyNameCaseInsensitive = true,
//        NumberHandling = JsonNumberHandling.AllowReadingFromString
//    };

//    public static async Task<IResult> HandleAsync(
//        HttpContext http,
//        PaystackWebhookOptions options,
//        IWebhookEventStore store,
//        IPaystackDvaEventHandler handler,
//        ILoggerFactory loggerFactory,
//        CancellationToken ct)
//    {
//        var log = loggerFactory.CreateLogger("PaystackWebhook");

//        // 1) Optional IP allowlist
//        if (options.EnforceIpAllowlist)
//        {
//            var ip = http.Connection.RemoteIpAddress?.MapToIPv4().ToString();
//            if (ip is null || Array.IndexOf(PaystackWebhookOptions.PaystackIps, ip) < 0)
//            {
//                log.LogWarning("Rejected Paystack webhook from unexpected IP {Ip}", ip);
//                return Results.StatusCode(StatusCodes.Status403Forbidden);
//            }
//        }

//        // 2) Read the raw body (signature is computed over the exact bytes)
//        string rawBody;
//        using (var reader = new StreamReader(http.Request.Body, Encoding.UTF8))
//            rawBody = await reader.ReadToEndAsync(ct);

//        // 3) Verify x-paystack-signature = HMAC-SHA512(rawBody, secretKey), hex
//        var signature = http.Request.Headers["x-paystack-signature"].ToString();
//        if (!IsValidSignature(rawBody, signature, options.SecretKey))
//        {
//            log.LogWarning("Rejected Paystack webhook with invalid signature");
//            return Results.Unauthorized();
//        }

//        // 4) Parse the envelope
//        PaystackEvent? evt;
//        try { evt = JsonSerializer.Deserialize<PaystackEvent>(rawBody, Json); }
//        catch (JsonException ex)
//        {
//            log.LogWarning(ex, "Malformed Paystack webhook body");
//            return Results.BadRequest();
//        }
//        if (evt?.Event is null) return Results.BadRequest();

//        // 5) Idempotency — Paystack retries, so the same event can arrive more than once
//        var eventKey = BuildEventKey(evt);
//        if (!await store.TryBeginAsync(eventKey, ct))
//        {
//            log.LogInformation("Duplicate Paystack event {Key} ignored", eventKey);
//            return Results.Ok();
//        }

//        // 6) Dispatch. Keep handler work short; Paystack expects a quick 200.
//        try
//        {
//            switch (evt.Event)
//            {
//                case "charge.success":
//                    {
//                        var charge = evt.Data.Deserialize<ChargeData>(Json)!;
//                        // Only DVA transfers; card/other charges are ignored here.
//                        if (charge.Channel == "dedicated_nuban" || charge.Authorization?.Channel == "dedicated_nuban")
//                            await handler.OnDvaPaymentReceivedAsync(charge, ct);
//                        break;
//                    }
//                case "dedicatedaccount.assign.success":
//                    {
//                        var data = evt.Data.Deserialize<DvaAssignData>(Json)!;
//                        await handler.OnDvaAssignedAsync(data, ct);
//                        break;
//                    }
//                case "dedicatedaccount.assign.failed":
//                    {
//                        var data = evt.Data.Deserialize<DvaAssignData>(Json)!;
//                        await handler.OnDvaAssignFailedAsync(data, ct);
//                        break;
//                    }
//                default:
//                    log.LogDebug("Unhandled Paystack event {Event}", evt.Event);
//                    break;
//            }

//            await store.MarkCompletedAsync(eventKey, ct);
//            return Results.Ok();
//        }
//        catch (Exception ex)
//        {
//            // Release the key so Paystack's retry can be processed, and return 500 to trigger it.
//            await store.ReleaseAsync(eventKey, ct);
//            log.LogError(ex, "Failed processing Paystack event {Key}", eventKey);
//            return Results.StatusCode(StatusCodes.Status500InternalServerError);
//        }
//    }

//    internal static bool IsValidSignature(string rawBody, string signatureHex, string secretKey)
//    {
//        if (string.IsNullOrWhiteSpace(signatureHex)) return false;

//        byte[] expected;
//        using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey)))
//            expected = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawBody));

//        byte[] provided;
//        try { provided = Convert.FromHexString(signatureHex); }
//        catch (FormatException) { return false; }

//        return CryptographicOperations.FixedTimeEquals(expected, provided);
//    }

//    private static string BuildEventKey(PaystackEvent evt)
//    {
//        string? id = null;
//        if (evt.Data.ValueKind == JsonValueKind.Object)
//        {
//            if (evt.Data.TryGetProperty("reference", out var r) && r.ValueKind == JsonValueKind.String)
//                id = r.GetString();
//            else if (evt.Data.TryGetProperty("dedicated_account", out var da) &&
//                     da.ValueKind == JsonValueKind.Object && da.TryGetProperty("id", out var daId))
//                id = daId.ToString();
//            else if (evt.Data.TryGetProperty("customer", out var c) &&
//                     c.ValueKind == JsonValueKind.Object && c.TryGetProperty("customer_code", out var cc))
//                id = cc.GetString();
//        }
//        return $"{evt.Event}:{id ?? Guid.NewGuid().ToString("N")}";
//    }
//}

//// =============================================================================
//// Your business logic goes here
//// =============================================================================

//public interface IPaystackDvaEventHandler
//{
//    /// <summary>
//    /// A bank transfer landed in a customer's DVA. Credit their wallet / mark invoice paid.
//    /// Amount is in kobo: divide by 100 for naira.
//    /// </summary>
//    Task OnDvaPaymentReceivedAsync(ChargeData charge, CancellationToken ct);

//    /// <summary>DVA was assigned (fires after POST /dedicated_account). Save the account number.</summary>
//    Task OnDvaAssignedAsync(DvaAssignData data, CancellationToken ct);

//    /// <summary>Assignment failed (e.g. customer validation). Notify the customer or retry.</summary>
//    Task OnDvaAssignFailedAsync(DvaAssignData data, CancellationToken ct);
//}

///// <summary>Example implementation — replace the bodies with your own persistence.</summary>
//public sealed class ExampleDvaEventHandler : IPaystackDvaEventHandler
//{
//    private readonly ILogger<ExampleDvaEventHandler> _log;
//    public ExampleDvaEventHandler(ILogger<ExampleDvaEventHandler> log) => _log = log;

//    public Task OnDvaPaymentReceivedAsync(ChargeData c, CancellationToken ct)
//    {
//        if (c.Status != "success" || c.Currency != "NGN") return Task.CompletedTask;

//        var naira = c.Amount / 100m;
//        _log.LogInformation(
//            "DVA payment {Ref}: ₦{Amount} from {Sender} ({SenderBank}) to {Account} for {Customer}",
//            c.Reference, naira, c.Authorization?.SenderName, c.Authorization?.SenderBank,
//            c.Authorization?.ReceiverBankAccountNumber, c.Customer?.CustomerCode);

//        // e.g. await _wallets.CreditAsync(c.Customer!.CustomerCode!, naira, c.Reference!, ct);
//        return Task.CompletedTask;
//    }

//    public Task OnDvaAssignedAsync(DvaAssignData d, CancellationToken ct)
//    {
//        _log.LogInformation("DVA assigned to {Customer}: {Bank} {Account}",
//            d.Customer?.CustomerCode, d.DedicatedAccount?.Bank?.Name, d.DedicatedAccount?.AccountNumber);

//        // e.g. await _customers.SaveDvaAsync(d.Customer!.CustomerCode!, d.DedicatedAccount!, ct);
//        return Task.CompletedTask;
//    }

//    public Task OnDvaAssignFailedAsync(DvaAssignData d, CancellationToken ct)
//    {
//        _log.LogWarning("DVA assignment failed for {Customer}: {Status}",
//            d.Customer?.CustomerCode, d.Identification?.Status);
//        return Task.CompletedTask;
//    }
//}

//// =============================================================================
//// Idempotency store
//// =============================================================================

//public interface IWebhookEventStore
//{
//    /// <summary>Returns false if this event is already processing or completed.</summary>
//    Task<bool> TryBeginAsync(string eventKey, CancellationToken ct);
//    Task MarkCompletedAsync(string eventKey, CancellationToken ct);
//    Task ReleaseAsync(string eventKey, CancellationToken ct);
//}

///// <summary>
///// Dev/single-instance only. In production, use a table with a UNIQUE constraint on the key
///// (INSERT ... fails on duplicate → return false), ideally in the same transaction as the wallet credit.
///// </summary>
//public sealed class InMemoryWebhookEventStore : IWebhookEventStore
//{
//    private readonly ConcurrentDictionary<string, bool> _seen = new();
//    public Task<bool> TryBeginAsync(string k, CancellationToken ct) => Task.FromResult(_seen.TryAdd(k, false));
//    public Task MarkCompletedAsync(string k, CancellationToken ct) { _seen[k] = true; return Task.CompletedTask; }
//    public Task ReleaseAsync(string k, CancellationToken ct) { _seen.TryRemove(k, out _); return Task.CompletedTask; }
//}

//// =============================================================================
//// Payload models
//// =============================================================================

//public sealed class PaystackEvent
//{
//    public string? Event { get; set; }
//    public JsonElement Data { get; set; }
//}

//public sealed class ChargeData
//{
//    public long Id { get; set; }
//    public string? Reference { get; set; }
//    /// <summary>In kobo.</summary>
//    public long Amount { get; set; }
//    public string? Currency { get; set; }
//    public string? Status { get; set; }
//    public string? Channel { get; set; }
//    public long? Fees { get; set; }
//    [JsonPropertyName("paid_at")] public DateTime? PaidAt { get; set; }
//    public WebhookCustomer? Customer { get; set; }
//    public DvaAuthorization? Authorization { get; set; }
//    public JsonElement? Metadata { get; set; }
//}

//public sealed class DvaAuthorization
//{
//    public string? Channel { get; set; }
//    [JsonPropertyName("sender_bank")] public string? SenderBank { get; set; }
//    [JsonPropertyName("sender_bank_account_number")] public string? SenderBankAccountNumber { get; set; }
//    [JsonPropertyName("sender_name")] public string? SenderName { get; set; }
//    [JsonPropertyName("receiver_bank_account_number")] public string? ReceiverBankAccountNumber { get; set; }
//    [JsonPropertyName("receiver_bank")] public string? ReceiverBank { get; set; }
//    public string? Narration { get; set; }
//}

//public sealed class WebhookCustomer
//{
//    public long Id { get; set; }
//    [JsonPropertyName("customer_code")] public string? CustomerCode { get; set; }
//    public string? Email { get; set; }
//    [JsonPropertyName("first_name")] public string? FirstName { get; set; }
//    [JsonPropertyName("last_name")] public string? LastName { get; set; }
//    public string? Phone { get; set; }
//}

//public sealed class DvaAssignData
//{
//    public WebhookCustomer? Customer { get; set; }
//    /// <summary>Same model as in PaystackDedicatedAccountClient.cs. Null on assign.failed.</summary>
//    [JsonPropertyName("dedicated_account")] public DedicatedAccount? DedicatedAccount { get; set; }
//    public DvaIdentification? Identification { get; set; }
//}

//public sealed class DvaIdentification
//{
//    public string? Status { get; set; }
//}