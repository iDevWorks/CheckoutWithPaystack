using iDevWorks.Paystack.Models;

namespace iDevWorks.Paystack.DirectDebit;

public class Client(PaystackClient paystack)
{
    public Task<AuthInitResponse> InitializeAuthorization(string email, string accountNumber, string bankCode, string callbackUrl, CancellationToken ct = default)
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

        return paystack.PostAsync<AuthInitResponse>("customer/authorization/initialize", body, ct);
    }

    public Task<AuthVerifyResponse> VerifyAuthorization(string reference, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);

        return paystack.GetAsync<AuthVerifyResponse>($"customer/authorization/verify/{reference}", ct);
    }

    public Task<ChargeResponse> ChargeAccount(string authorizationCode, string email, decimal amount, CancellationToken ct = default)
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

        return paystack.PostAsync<ChargeResponse>("transaction/charge_authorization", body, ct);
    }

    public Task<ChargeResponse> VerifyCharge(string reference, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);

        return paystack.GetAsync<ChargeResponse>($"transaction/verify/{reference}", ct);
    }

    public async Task DeactivateAuthorization(string authorizationCode, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(authorizationCode);

        var body = new { authorization_code = authorizationCode };
        await paystack.PostAsync<string>("customer/authorization/deactivate", body, ct);
    }
}