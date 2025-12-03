namespace iDevWorks.Paystack.Transaction
{
    public class Client(PaystackClient paystack)
    {
        public Task<InitTransactionResponse> InitializeTransaction(string email, decimal amount, string callbackUrl, string reference)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(callbackUrl);
            ArgumentException.ThrowIfNullOrWhiteSpace(reference);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

            var payload = new
            {
                email,
                reference,
                amount = (int)amount * 100,
                callback_url = callbackUrl,
            };

            return paystack.PostAsync<InitTransactionResponse>("transaction/initialize", payload);
        }

        public Task<TransactionResponse> VerifyTransaction(string reference)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(reference);

            return paystack.GetAsync<TransactionResponse>($"transaction/verify/{reference}");
        }
    }
}
