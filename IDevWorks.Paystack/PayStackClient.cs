using System.Net.Http.Json;
using System.Text.Json;

namespace iDevWorks.Paystack
{
    public class PaystackClient
    {
        public VirtualAccount.Client VirtualAccount { get; }
        public DirectDebit.Client DirectDebit { get; }
        public Transaction.Client Transaction { get; }
        public Transfer.Client Transfer { get; }
        public Customer.Client Customer { get; }

        public PaystackClient(string secretKey, HttpClient? httpClient = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(secretKey);

            _httpClient = httpClient ?? new HttpClient
            {
                BaseAddress = new Uri("https://api.paystack.co/")
            };
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {secretKey}");

            VirtualAccount = new VirtualAccount.Client(this);
            DirectDebit = new DirectDebit.Client(this);
            Transaction = new Transaction.Client(this);
            Transfer = new Transfer.Client(this);
            Customer = new Customer.Client(this);
        }

        public async Task<TResult> GetAsync<TResult>(
            string endpoint, CancellationToken ct = default) where TResult : class
        {
            var response = await _httpClient.GetAsync(endpoint, ct);
            return await ProcessResponse<TResult>(response, ct);
        }

        public async Task<TResult> PostAsync<TResult>(
            string endpoint, object body, CancellationToken ct = default) where TResult : class
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, body, ct);
            return await ProcessResponse<TResult>(response, ct);
        }

        public async Task<TResult> PutAsync<TResult>(
            string endpoint, object body, CancellationToken ct = default) where TResult : class
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, body, ct);
            return await ProcessResponse<TResult>(response, ct);
        }

        public async Task<TResult> DeleteAsync<TResult>(
            string endpoint, CancellationToken ct = default) where TResult : class
        {
            var response = await _httpClient.DeleteAsync(endpoint, ct);
            return await ProcessResponse<TResult>(response, ct);
        }

        private static async Task<TResult> ProcessResponse<TResult>(
            HttpResponseMessage httpResponse, CancellationToken ct) where TResult: class 
        {
            var jsonString = await httpResponse.Content.ReadAsStringAsync(ct);
            var result = JsonSerializer.Deserialize<Models.PaystackResult<TResult>>(jsonString, _jsonOptions);

            if (httpResponse.IsSuccessStatusCode)
            {
                if (result != null)
                    return result.Data;

                throw new PaystackException(Status.REQUEST_OK_JSON_FAIL, jsonString);
            }

            if (result != null)
                throw new PaystackException(Status.REQUEST_FAIL_JSON_OK, result.Message);

            throw new PaystackException(Status.REQUEST_FAIL_JSON_FAIL, jsonString);
        }

        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }
}
