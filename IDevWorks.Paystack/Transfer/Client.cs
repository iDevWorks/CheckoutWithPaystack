namespace iDevWorks.Paystack.Transfer;

public class Client(PaystackClient paystack)
{
    public async Task<BalanceResponse> GetWalletBalance()
    {
        var response = await paystack.GetAsync<IEnumerable<BalanceResponse>>("balance");
        return response.First();

    }

    public Task<RecipientResponse> CreateRecipient(string name, string accountNumber, string bankCode)
    {
        var payload = new
        {
            name,
            type = "nuban",
            account_number = accountNumber,
            bank_code = bankCode,
            currency = "NGN",
        };

        return paystack.PostAsync<RecipientResponse>("transferrecipient", payload);
    }

    public Task<RecipientResponse> FetchRecipient(string recipientCode)
    {
        return paystack.GetAsync<RecipientResponse>($"transferrecipient/{recipientCode}");
    }

    public Task<InitTransferResponse> InitiateTransfer(string recipientCode, decimal amount, string reason, string reference)
    {
        var payload = new
        {
            reason,
            reference,
            source = "balance",
            amount = (int)(amount * 100),
            recipient = recipientCode,
        };

        return paystack.PostAsync<InitTransferResponse>("transfer", payload);
    }

    public Task<List<InitBulkTransferResponse>> InitiateBulkTransfer(IEnumerable<TransferItem> transfers)
    {
        var payload = new
        {
            currency = "NGN",
            source = "balance",
            transfers = transfers.Select(t => new
            {
                t.Reason,
                t.Reference,
                recipient = t.RecipientCode,
                amount = (int)(t.Amount * 100), 
            })
        };

        return paystack.PostAsync<List<InitBulkTransferResponse>>("transfer/bulk", payload);
    }

    public Task<VerifyTransferResponse> VerifyTransfer(string reference)
    {
        return paystack.GetAsync<VerifyTransferResponse>($"transfer/verify/{reference}");
    }
}

public class TransferItem
{
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string RecipientCode { get; set; } = string.Empty;
}
