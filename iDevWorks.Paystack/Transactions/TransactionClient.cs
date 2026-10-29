using iDevWorks.Paystack.Models;

namespace iDevWorks.Paystack;

public class TransactionClient(PaystackClient paystack)
{
    public Task<Authorization> Initialize(
        string email, decimal amount, string reference, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        var body = new
        {
            email,
            reference,
            amount = (int)amount * 100,
            //callback_url = callbackUrl,
        };

        return paystack.PostAsync<Authorization>("transaction/initialize", body, ct);
    }

    public Task<Transaction> Verify(string reference, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);

        return paystack.GetAsync<Transaction>($"transaction/verify/{reference}", ct);
    }
}
