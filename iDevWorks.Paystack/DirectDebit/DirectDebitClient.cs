using iDevWorks.Paystack.Models;

namespace iDevWorks.Paystack;

public class DirectDebitClient(PaystackClient paystack)
{
    public Task<Authorization> Initialize(string email, string accountNumber, string bankCode, string callbackUrl, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(accountNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(bankCode);

        var body = new
        {
            email,
            channel = "direct_debit",
            account = new
            {
                bank_code = bankCode,
                number = accountNumber
            },
            callback_url = callbackUrl,
        };

        return paystack.PostAsync<Authorization>("customer/authorization/initialize", body, ct);
    }

    public Task<DirectDebitAuth> Verify(string reference, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);

        return paystack.GetAsync<DirectDebitAuth>($"customer/authorization/verify/{reference}", ct);
    }

    public Task<DirectDebitCharge> Charge(string authorizationCode, string email, decimal amount, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(authorizationCode);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        var body = new
        {
            email,
            authorization_code = authorizationCode,
            amount = (int)(amount * 100) // Convert to kobo
        };

        return paystack.PostAsync<DirectDebitCharge>("transaction/charge_authorization", body, ct);
    }

    public Task<DirectDebitCharge> VerifyCharge(string reference, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);

        return paystack.GetAsync<DirectDebitCharge>($"transaction/verify/{reference}", ct);
    }

    public async Task Deactivate(string authorizationCode, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(authorizationCode);

        var body = new { authorization_code = authorizationCode };
        await paystack.PostAsync<string>("customer/authorization/deactivate", body, ct);
    }
}