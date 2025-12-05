using System.Net.Http.Json;
using System.Text.Json;

namespace iDevWorks.Paystack
{
    public class PaystackClient
    {
        public DirectDebit.Client DirectDebit { get; }
        public Transaction.Client Transaction { get; }
        public Transfer.Client Transfer { get; }

        public PaystackClient(string secretKey, HttpClient? httpClient = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(secretKey);

            _httpClient = httpClient ?? new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.paystack.co/");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {secretKey}");

            DirectDebit = new DirectDebit.Client(this);
            Transaction = new Transaction.Client(this);
            Transfer = new Transfer.Client(this);
        }

        public async Task<TResult> GetAsync<TResult>(
            string endpoint, CancellationToken cancellation = default) where TResult : class
        {
            var response = await _httpClient.GetAsync(endpoint, cancellation);
            return ProcessResponse<TResult>(response, cancellation).Result;
        }

        public async Task<TResult> PostAsync<TResult>(
            string endpoint, object payload, CancellationToken cancellation = default) where TResult : class
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, payload, cancellation);
            return ProcessResponse<TResult>(response, cancellation).Result;
        }

        private static async Task<TResult> ProcessResponse<TResult>(HttpResponseMessage httpResponse, CancellationToken cancellation) where TResult: class 
        {
            var jsonString = await httpResponse.Content.ReadAsStringAsync(cancellation);
            var result = JsonSerializer.Deserialize<Result<TResult>>(jsonString, _jsonOptions);

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
