using CheckoutWithPaystack.Model.Paystack;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CheckoutWithPaystack.Services
{
    public class PaystackService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _testSecretKey = "sk_test_01f11480564bc34f6fde4c5c078741adb4ba738a";

        public PaystackService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<TransactionInitResponse> InitializeTransaction(string email, decimal totalAmount, string callbackUrl)
        {
            var amountInKobo = (int)totalAmount * 100;
            var reference = Guid.NewGuid().ToString();

            var initRequest = new TransactionInitRequest(email, amountInKobo, callbackUrl, reference);

            var client = CreateHttpClient();
            var url = "https://api.paystack.co/transaction/initialize";
            var response = await client.PostAsJsonAsync(url, initRequest);
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PaystackResponse<TransactionInitResponse>>(jsonString);

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

        public async Task<TransactionVerifyResponse> VerifyTransaction(string reference)
        {
            var url = $"https://api.paystack.co/transaction/verify/{reference}";

            var client = CreateHttpClient();
            var response = await client.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PaystackResponse<TransactionVerifyResponse>>(jsonString);

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

        private HttpClient CreateHttpClient()
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", _testSecretKey);
            return client;
        }
    }
}
