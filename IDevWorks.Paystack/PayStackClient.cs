using iDevWorks.Paystack.Exceptions;
using iDevWorks.Paystack.Transaction.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace iDevWorks.Paystack
{
    public class PaystackClient
    {
        private readonly HttpClient _httpClient;

        public DirectDebit.Client DirectDebit { get; }
        public Transaction.Client Transaction { get; }
        public Transfer.Client Transfer { get; }

        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

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

        public async Task<TResult> GetAsync<TResult>(string endpoint) where TResult : class
        {
            var response = await _httpClient.GetAsync(endpoint);
            return ProcessResponse<TResult>(response).Result;
        }

        public async Task<TResult> PostAsync<TResult>(string endpoint, object payload) where TResult : class
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, payload);
            return ProcessResponse<TResult>(response).Result;
        }


        private static async Task<TResult> ProcessResponse<TResult>(HttpResponseMessage httpResponse) 
            where TResult: class 
        {
            
            var jsonString = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Result<TResult>>(jsonString, jsonOptions);

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

        
    }
}
