using iDevWorks.Paystack.Models;

namespace iDevWorks.Paystack.Transfer;

public class Client(PaystackClient paystack)
{
    public async Task<BalanceResponse> GetBalance(string currency = "NGN", CancellationToken ct = default)
    {
        var balances = await GetBalances(ct);

        return balances.FirstOrDefault(b => b.Currency == currency)
            ?? throw new InvalidOperationException($"No balance found for currency: {currency}");
    }

    public Task<List<BalanceResponse>> GetBalances(CancellationToken ct = default)
    {
        return paystack.GetAsync<List<BalanceResponse>>("balance", ct);
    }

    public Task<RecipientResponse> CreateRecipient(string name, string accountNumber, string bankCode, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(accountNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(bankCode);

        var body = new
        {
            name,
            type = "nuban",
            account_number = accountNumber,
            bank_code = bankCode,
            currency = "NGN",
        };

        return paystack.PostAsync<RecipientResponse>("transferrecipient", body, ct);
    }

    public Task<RecipientResponse> FetchRecipient(string recipientCode, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(recipientCode);

        return paystack.GetAsync<RecipientResponse>($"transferrecipient/{recipientCode}", ct);
    }

    public Task<InitTransferResponse> InitiateTransfer(string recipientCode, decimal amount, string reason, string reference, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(recipientCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        var body = new
        {
            reason,
            reference,
            source = "balance",
            amount = (int)(amount * 100),
            recipient = recipientCode,
        };

        return paystack.PostAsync<InitTransferResponse>("transfer", body, ct);
    }

    public Task<List<InitTransferResponse>> InitiateBulkTransfer(IEnumerable<TransferRequest> transfers, string currency = "NGN", CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(transfers);
        ArgumentOutOfRangeException.ThrowIfZero(transfers.Count());

        var body = new
        {
            currency,
            source = "balance",
            transfers = transfers.Select(t => new
            {
                t.Reason,
                t.Reference,
                recipient = t.RecipientCode,
                amount = (int)(t.Amount * 100),
            })
        };

        return paystack.PostAsync<List<InitTransferResponse>>("transfer/bulk", body, ct);
    }

    public Task<VerifyTransferResponse> VerifyTransfer(string reference, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);

        return paystack.GetAsync<VerifyTransferResponse>($"transfer/verify/{reference}", ct);
    }
}
