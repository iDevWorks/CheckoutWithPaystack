using System.Net.Http.Json;
using System.Text.Json;

namespace iDevWorks.Paystack;

public class PaystackClient
{
    public VirtualAccountClient VirtualAccount { get; }
    public DirectDebitClient DirectDebit { get; }
    public TransactionClient Transaction { get; }
    public TransferClient Transfer { get; }
    public CustomerClient Customer { get; }

    public PaystackClient(string secretKey, HttpClient? httpClient = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(secretKey);

        _httpClient = httpClient ?? new HttpClient
        {
            BaseAddress = new Uri("https://api.paystack.co/")
        };
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {secretKey}");

        VirtualAccount = new VirtualAccountClient(this);
        DirectDebit = new DirectDebitClient(this);
        Transaction = new TransactionClient(this);
        Transfer = new TransferClient(this);
        Customer = new CustomerClient(this);
    }

    internal Task<TResult> GetAsync<TResult>(
        string endpoint, CancellationToken ct = default) where TResult : class
        => SendAsync<TResult>(() => _httpClient.GetAsync(endpoint, ct), ct);

    internal Task<TResult> PostAsync<TResult>(
        string endpoint, object body, CancellationToken ct = default) where TResult : class
        => SendAsync<TResult>(() => _httpClient.PostAsJsonAsync(endpoint, body, ct), ct);

    internal Task<TResult> PutAsync<TResult>(
        string endpoint, object body, CancellationToken ct = default) where TResult : class
        => SendAsync<TResult>(() => _httpClient.PutAsJsonAsync(endpoint, body, ct), ct);

    internal Task<TResult> DeleteAsync<TResult>(
        string endpoint, CancellationToken ct = default) where TResult : class
        => SendAsync<TResult>(() => _httpClient.DeleteAsync(endpoint, ct), ct);

    private static async Task<TResult> SendAsync<TResult>(
        Func<Task<HttpResponseMessage>> request, CancellationToken ct) where TResult : class
    {
        try
        {
            var httpResponse = await request();

            using (httpResponse)
            {
                var jsonString = await httpResponse.Content.ReadAsStringAsync(ct);
                var result = JsonSerializer.Deserialize<PaystackResponse<TResult>>(jsonString, JsonOptions.Options);

                if (httpResponse.IsSuccessStatusCode)
                {
                    if (result != null)
                        return result.Data;

                    throw new PaystackException(PaystackErrorCode.MALFORMED_JSON, jsonString);
                }

                if (result != null)
                    throw new PaystackException(PaystackErrorCode.API_ERROR, result.Message);

                throw new PaystackException(PaystackErrorCode.UNKNOWN_ERROR, jsonString);
            }
        }
        catch (JsonException ex)
        {
            // e.g. an HTML 502 page from a proxy/Cloudflare
            throw new PaystackException(PaystackErrorCode.MALFORMED_JSON, ex.Message);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw; // caller cancelled — let it propagate as-is
        }
        catch (TaskCanceledException ex)
        {
            throw new PaystackException(PaystackErrorCode.TIMEOUT, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            throw new PaystackException(PaystackErrorCode.NETWORK_ERROR, ex.Message);
        }
        catch (Exception ex)
        {
            throw new PaystackException(PaystackErrorCode.UNKNOWN_ERROR, ex.Message);
        }
    }

    private readonly HttpClient _httpClient;
}
