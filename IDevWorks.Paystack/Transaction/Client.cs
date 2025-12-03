using iDevWorks.Paystack.Transaction.Models;

namespace iDevWorks.Paystack.Transaction
{
    public class Client(PaystackClient paystack)
    {
        public Task<Authorization> InitializeTransaction(string email, decimal amount, string callbackUrl, string reference)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(callbackUrl);
            ArgumentException.ThrowIfNullOrWhiteSpace(reference);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

            var amountInKobo = (int)amount * 100;
            var payload = new Initializer(email, amountInKobo, callbackUrl, reference);

            //var response = await _httpClient.PostAsJsonAsync("initialize", initRequest);
            //return await ProcessResponse<Authorization>(response);

            return paystack.PostAsync<Authorization>("transaction/initialize", payload);
        }

        public Task<Models.Transaction> VerifyTransaction(string reference)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(reference);

            //var response = await _httpClient.GetAsync($"verify/{reference}");
            //return await ProcessResponse<Models.Transaction>(response);

            return paystack.GetAsync<Models.Transaction>($"transaction/verify/{reference}");
        }
    }
}
