//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Net.Http.Json;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Billing.Paystack;

///// <summary>
///// Fetches, and if needed creates, a Paystack Dedicated Virtual Account (DVA) for a customer.
/////
///// Fetch order:
/////   1. GET  /customer/{email_or_code}            -> customer.dedicated_account (fast path)
/////   2. GET  /dedicated_account?customer={id}     -> list filter
///// Create (only when nothing active is found):
/////   3. POST /customer                            -> if the customer doesn't exist yet
/////      PUT  /customer/{code}                     -> if names/phone are missing (required for NG DVAs)
/////   4. POST /dedicated_account                   -> assign the DVA
/////
///// Register as a typed HttpClient:
/////   services.AddHttpClient&lt;PaystackDedicatedAccountClient&gt;(c =>
/////   {
/////       c.BaseAddress = new Uri("https://api.paystack.co/");
/////       c.DefaultRequestHeaders.Authorization =
/////           new AuthenticationHeaderValue("Bearer", config["Paystack:SecretKey"]);
/////   });
///// </summary>
//public sealed class PaystackDedicatedAccountClient
//{
//    private readonly HttpClient _http;

//    private static readonly JsonSerializerOptions Json = new()
//    {
//        PropertyNameCaseInsensitive = true,
//        NumberHandling = JsonNumberHandling.AllowReadingFromString,
//        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
//    };

//    public PaystackDedicatedAccountClient(HttpClient http)
//    {
//        _http = http ?? throw new ArgumentNullException(nameof(http));
//        _http.BaseAddress ??= new Uri("https://api.paystack.co/");
//    }

//    /// <summary>Convenience constructor when not using DI.</summary>
//    public PaystackDedicatedAccountClient(string secretKey) : this(new HttpClient())
//    {
//        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);
//    }

//    // ================================================================ get-or-create

//    /// <summary>
//    /// Returns the customer's active DVA, creating the customer and/or the DVA if none exists.
//    /// </summary>
//    public async Task<DedicatedAccount> GetOrCreateForCustomerAsync(
//        DvaCustomerDetails details, CancellationToken ct = default)
//    {
//        ArgumentNullException.ThrowIfNull(details);
//        details.Validate();

//        // 1) Existing customer + existing DVA?
//        var customer = await GetCustomerAsync(details.Email, ct);
//        if (customer is not null)
//        {
//            var existing = await FindActiveAsync(customer, details.PreferredBank, ct);
//            if (existing is not null) return existing;
//        }

//        // 2) Make sure the customer exists and has the fields Paystack needs for a DVA.
//        customer = customer is null
//            ? await CreateCustomerAsync(details, ct)
//            : await EnsureCustomerDetailsAsync(customer, details, ct);

//        // 3) Create the DVA.
//        return await CreateDedicatedAccountAsync(customer.CustomerCode!, details, ct);
//    }

//    // ================================================================ fetch

//    /// <summary>Returns the customer's active DVA, or null if none has been assigned.</summary>
//    public async Task<DedicatedAccount?> GetForCustomerAsync(
//        string emailOrCustomerCode, string? preferredBankSlug = null, CancellationToken ct = default)
//    {
//        if (string.IsNullOrWhiteSpace(emailOrCustomerCode))
//            throw new ArgumentException("Customer email or code is required.", nameof(emailOrCustomerCode));

//        var customer = await GetCustomerAsync(emailOrCustomerCode, ct);
//        return customer is null ? null : await FindActiveAsync(customer, preferredBankSlug, ct);
//    }

//    private async Task<DedicatedAccount?> FindActiveAsync(
//        PaystackCustomer customer, string? preferredBankSlug, CancellationToken ct)
//    {
//        // Fast path: Paystack embeds the DVA on the customer object.
//        var embedded = customer.DedicatedAccount;
//        if (embedded is { Active: true } &&
//            (preferredBankSlug is null || embedded.Bank?.Slug == preferredBankSlug))
//        {
//            return embedded;
//        }

//        // Fallback: list filtered by the customer's numeric ID.
//        var accounts = await ListForCustomerIdAsync(customer.Id, activeOnly: true, ct);
//        return accounts
//            .Where(a => preferredBankSlug is null || a.Bank?.Slug == preferredBankSlug)
//            .OrderByDescending(a => a.Assigned)
//            .ThenByDescending(a => a.CreatedAt)
//            .FirstOrDefault();
//    }

//    /// <summary>GET /dedicated_account/{id}</summary>
//    public async Task<DedicatedAccount?> GetByIdAsync(long dedicatedAccountId, CancellationToken ct = default)
//        => (await SendAsync<DedicatedAccount>(HttpMethod.Get, $"dedicated_account/{dedicatedAccountId}", null, ct))?.Data;

//    /// <summary>GET /dedicated_account?customer={id}&amp;active=true</summary>
//    public async Task<IReadOnlyList<DedicatedAccount>> ListForCustomerIdAsync(
//        long customerId, bool activeOnly = true, CancellationToken ct = default)
//    {
//        var url = $"dedicated_account?customer={customerId}&active={activeOnly.ToString().ToLowerInvariant()}";
//        var res = await SendAsync<List<DedicatedAccount>>(HttpMethod.Get, url, null, ct);
//        return res?.Data ?? new List<DedicatedAccount>();
//    }

//    /// <summary>GET /customer/{email_or_code}</summary>
//    public async Task<PaystackCustomer?> GetCustomerAsync(string emailOrCustomerCode, CancellationToken ct = default)
//        => (await SendAsync<PaystackCustomer>(
//            HttpMethod.Get, $"customer/{Uri.EscapeDataString(emailOrCustomerCode)}", null, ct))?.Data;

//    // ================================================================ create

//    /// <summary>POST /customer</summary>
//    public async Task<PaystackCustomer> CreateCustomerAsync(DvaCustomerDetails d, CancellationToken ct = default)
//    {
//        var body = new
//        {
//            email = d.Email,
//            first_name = d.FirstName,
//            last_name = d.LastName,
//            phone = d.Phone
//        };
//        var res = await SendAsync<PaystackCustomer>(HttpMethod.Post, "customer", body, ct);
//        return res?.Data ?? throw new PaystackApiException(0, "Paystack returned no customer data.");
//    }

//    /// <summary>PUT /customer/{code} when first name, last name or phone is missing.</summary>
//    private async Task<PaystackCustomer> EnsureCustomerDetailsAsync(
//        PaystackCustomer c, DvaCustomerDetails d, CancellationToken ct)
//    {
//        bool missing = string.IsNullOrWhiteSpace(c.FirstName)
//                    || string.IsNullOrWhiteSpace(c.LastName)
//                    || string.IsNullOrWhiteSpace(c.Phone);
//        if (!missing) return c;

//        var body = new
//        {
//            first_name = string.IsNullOrWhiteSpace(c.FirstName) ? d.FirstName : c.FirstName,
//            last_name = string.IsNullOrWhiteSpace(c.LastName) ? d.LastName : c.LastName,
//            phone = string.IsNullOrWhiteSpace(c.Phone) ? d.Phone : c.Phone
//        };
//        var res = await SendAsync<PaystackCustomer>(HttpMethod.Put, $"customer/{c.CustomerCode}", body, ct);
//        return res?.Data ?? c;
//    }

//    /// <summary>POST /dedicated_account</summary>
//    public async Task<DedicatedAccount> CreateDedicatedAccountAsync(
//        string customerCode, DvaCustomerDetails d, CancellationToken ct = default)
//    {
//        var body = new
//        {
//            customer = customerCode,
//            preferred_bank = d.PreferredBank,   // "wema-bank", "titan-paystack"; "test-bank" in test mode
//            subaccount = d.Subaccount,
//            split_code = d.SplitCode,
//            first_name = d.FirstName,
//            last_name = d.LastName,
//            phone = d.Phone
//        };

//        var res = await SendAsync<DedicatedAccount>(HttpMethod.Post, "dedicated_account", body, ct);
//        return res?.Data ?? throw new PaystackApiException(0, "Paystack returned no dedicated account data.");
//    }

//    // ================================================================ internals

//    private async Task<PaystackResponse<T>?> SendAsync<T>(
//        HttpMethod method, string relativeUrl, object? body, CancellationToken ct)
//    {
//        using var req = new HttpRequestMessage(method, relativeUrl);
//        if (body is not null) req.Content = JsonContent.Create(body, options: Json);

//        using var resp = await _http.SendAsync(req, ct);
//        var raw = await resp.Content.ReadAsStringAsync(ct);

//        // Only treat 404 as "not found" on reads; on writes it's a real error.
//        if (resp.StatusCode == HttpStatusCode.NotFound && method == HttpMethod.Get)
//            return null;

//        PaystackResponse<T>? parsed = null;
//        try { parsed = JsonSerializer.Deserialize<PaystackResponse<T>>(raw, Json); }
//        catch (JsonException) { /* fall through */ }

//        if (!resp.IsSuccessStatusCode || parsed is null || !parsed.Status)
//        {
//            throw new PaystackApiException(
//                (int)resp.StatusCode,
//                parsed?.Message ?? $"Unexpected Paystack response: {raw}");
//        }

//        return parsed;
//    }
//}

//// ==================================================================== inputs

//public sealed class DvaCustomerDetails
//{
//    public required string Email { get; init; }
//    public required string FirstName { get; init; }
//    public required string LastName { get; init; }
//    /// <summary>Required by Paystack for Nigerian DVAs, e.g. "+2348012345678".</summary>
//    public required string Phone { get; init; }

//    /// <summary>"wema-bank" or "titan-paystack" (live); "test-bank" in test mode. Null lets Paystack pick.</summary>
//    public string? PreferredBank { get; init; }
//    public string? Subaccount { get; init; }
//    public string? SplitCode { get; init; }

//    internal void Validate()
//    {
//        if (string.IsNullOrWhiteSpace(Email)) throw new ArgumentException("Email is required.");
//        if (string.IsNullOrWhiteSpace(FirstName)) throw new ArgumentException("FirstName is required.");
//        if (string.IsNullOrWhiteSpace(LastName)) throw new ArgumentException("LastName is required.");
//        if (string.IsNullOrWhiteSpace(Phone)) throw new ArgumentException("Phone is required.");
//    }
//}

//// ==================================================================== models

//public sealed class PaystackResponse<T>
//{
//    public bool Status { get; set; }
//    public string? Message { get; set; }
//    public T? Data { get; set; }
//}

//public sealed class PaystackCustomer
//{
//    public long Id { get; set; }
//    [JsonPropertyName("customer_code")] public string? CustomerCode { get; set; }
//    public string? Email { get; set; }
//    [JsonPropertyName("first_name")] public string? FirstName { get; set; }
//    [JsonPropertyName("last_name")] public string? LastName { get; set; }
//    public string? Phone { get; set; }
//    [JsonPropertyName("dedicated_account")] public DedicatedAccount? DedicatedAccount { get; set; }
//}


//public sealed class PaystackApiException : Exception
//{
//    public int StatusCode { get; }
//    public PaystackApiException(int statusCode, string message) : base(message) => StatusCode = statusCode;
//}

//// ==================================================================== usage
////
//// var client = new PaystackDedicatedAccountClient(Environment.GetEnvironmentVariable("PAYSTACK_SECRET_KEY")!);
////
//// var dva = await client.GetOrCreateForCustomerAsync(new DvaCustomerDetails
//// {
////     Email = "customer@example.com",
////     FirstName = "Ada",
////     LastName = "Obi",
////     Phone = "+2348012345678",
////     PreferredBank = "wema-bank"      // use "test-bank" with a test secret key
//// });
////
//// Console.WriteLine($"{dva.Bank?.Name} • {dva.AccountNumber} • {dva.AccountName}");