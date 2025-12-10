namespace iDevWorks.Paystack.Transfer;

public class Client(PaystackClient paystack)
{
    public Task<BalanceResponse> GetBalance()
    {
        return paystack.GetAsync<BalanceResponse>("balance");
    }

    public Task<RecipientResponse> CreateRecipient(string name, string accountNumber, string bankCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(accountNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(bankCode);

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
        ArgumentException.ThrowIfNullOrWhiteSpace(recipientCode);

        return paystack.GetAsync<RecipientResponse>($"transferrecipient/{recipientCode}");
    }

    public Task<InitTransferResponse> InitiateTransfer(string recipientCode, decimal amount, string reason, string reference)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(recipientCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

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

    public Task<List<InitTransferResponse>> InitiateBulkTransfer(IEnumerable<TransferItem> transfers)
    {
        ArgumentNullException.ThrowIfNull(transfers);
        ArgumentOutOfRangeException.ThrowIfZero(transfers.Count());

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

        return paystack.PostAsync<List<InitTransferResponse>>("transfer/bulk", payload);
    }

    public Task<VerifyTransferResponse> VerifyTransfer(string reference)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);

        return paystack.GetAsync<VerifyTransferResponse>($"transfer/verify/{reference}");
    }
}

public class TransferItem
{
    public decimal Amount { get; init; }
    public string Reason { get; init; } = string.Empty;
    public string Reference { get; init; } = string.Empty;
    public string RecipientCode { get; init; } = string.Empty;
}
