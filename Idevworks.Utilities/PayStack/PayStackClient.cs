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
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Result<Authorization>>(jsonString);

            if (response.IsSuccessStatusCode)
            {
                if (result != null)
                    return result.Data;

                throw new Exception(jsonString);
            }

            if (result != null)
                throw new Exception(result.Message);

            throw new Exception(jsonString);
        }

        public async Task<Transaction> VerifyTransaction(string reference)
        {
            var response = await _httpClient.GetAsync($"verify/{reference}");
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Result<Transaction>>(jsonString);

            if (response.IsSuccessStatusCode)
            {
                if (result != null)
                    return result.Data;

                throw new Exception(jsonString);
            }

            if (result != null)
                throw new Exception(result.Message);

            throw new Exception(jsonString);
        }
    }
}
