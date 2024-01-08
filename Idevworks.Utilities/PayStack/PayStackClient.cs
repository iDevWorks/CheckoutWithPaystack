using Idevworks.Utilities.PayStack.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Idevworks.Utilities.PayStack
{
    internal class PayStackClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _testSecretKey;
        private readonly string _baseUrl = "https://api.paystack.co/";

        public PayStackClient(string paystackSecretKey)
        {
            _testSecretKey = paystackSecretKey;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_testSecretKey}");
        }

        public async Task<TransactionInitResponse> InitializeTransaction(string email, decimal totalAmount, string callbackUrl)
        {
            var amountInKobo = (int)totalAmount * 100;
            var reference = Guid.NewGuid().ToString();

            var initRequest = new TransactionInitRequest(email, amountInKobo, callbackUrl, reference);

            var url = "transaction/initialize";
            var response = await _httpClient.PostAsJsonAsync(url, initRequest);
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
            var response = await _httpClient.GetAsync(url);
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

    }
}
