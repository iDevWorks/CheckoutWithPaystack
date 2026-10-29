using iDevWorks.Paystack.Models;

namespace iDevWorks.Paystack;

public class TransferClient(PaystackClient paystack)
{
    public async Task<Balance> GetBalance(string currency = "NGN", CancellationToken ct = default)
    {
        var balances = await GetBalances(ct);

        return balances.FirstOrDefault(b => b.Currency == currency)
            ?? throw new InvalidOperationException($"No balance found for currency: {currency}");
    }

    public Task<List<Balance>> GetBalances(CancellationToken ct = default)
    {
        return paystack.GetAsync<List<Balance>>("balance", ct);
    }

    public Task<Recipient> CreateRecipient(string name, string accountNumber, string bankCode, CancellationToken ct = default)
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

        return paystack.PostAsync<Recipient>("transferrecipient", body, ct);
    }

    public Task<Recipient> FetchRecipient(string recipientCode, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(recipientCode);

        return paystack.GetAsync<Recipient>($"transferrecipient/{recipientCode}", ct);
    }

    public Task<TransferResponse> Initiate(string recipientCode, decimal amount, string reason, string reference, CancellationToken ct = default)
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

        return paystack.PostAsync<TransferResponse>("transfer", body, ct);
    }

    public Task<List<TransferResponse>> InitiateBulk(IEnumerable<TransferItem> transfers, string currency = "NGN", CancellationToken ct = default)
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

        return paystack.PostAsync<List<TransferResponse>>("transfer/bulk", body, ct);
    }

    public Task<Transfer> Verify(string reference, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);

        return paystack.GetAsync<Transfer>($"transfer/verify/{reference}", ct);
    }
}
