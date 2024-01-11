using System.Net.Http.Json;
using System.Text.Json;

namespace iDevWorks.Paystack
{
    public class PaystackClient
    {
        private readonly HttpClient _httpClient;

        public PaystackClient(string secretKey, HttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.paystack.co/transaction/");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {secretKey}");
        }

        public async Task<Authorization> InitializeTransaction(string email, decimal totalAmount, string callbackUrl)
        {
            var amountInKobo = (int)totalAmount * 100;
            var reference = Guid.NewGuid().ToString();

            var initRequest = new Initializer(email, amountInKobo, callbackUrl, reference);

            var response = await _httpClient.PostAsJsonAsync("initialize", initRequest);
            return await ProcessResponse<Authorization>(response);
        }

        public async Task<Transaction> VerifyTransaction(string reference)
        {
            var response = await _httpClient.GetAsync($"verify/{reference}");
            return await ProcessResponse<Transaction>(response);
        }

        private static async Task<TResult> ProcessResponse<TResult>(HttpResponseMessage httpResponse) 
            where TResult: class 
        {
            var jsonString = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Result<TResult>>(jsonString);

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
