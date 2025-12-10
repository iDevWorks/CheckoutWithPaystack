namespace iDevWorks.Paystack.DirectDebit;

public class Client(PaystackClient paystack)
{
    public Task<AuthInitResponse> InitializeAuthorization(string email, string accountNumber, string bankCode, string callbackUrl = "https://your-app.com/callback")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(accountNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(bankCode);

        var payload = new
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

        return paystack.PostAsync<AuthInitResponse>("customer/authorization/initialize", payload);
    }

    public Task<AuthVerifyResponse> VerifyAuthorization(string reference)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);

        return paystack.GetAsync<AuthVerifyResponse>($"customer/authorization/verify/{reference}");
    }

    public Task<ChargeResponse> ChargeAccount(string authorizationCode, string email, decimal amount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(authorizationCode);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        var payload = new
        {
            email,
            authorization_code = authorizationCode,
            amount = (int)(amount * 100) // Convert to kobo
        };

        return paystack.PostAsync<ChargeResponse>("transaction/charge_authorization", payload);
    }

    public Task<ChargeResponse> VerifyCharge(string reference)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reference);

        return paystack.GetAsync<ChargeResponse>($"transaction/verify/{reference}");
    }

    public async Task DeactivateAuthorization(string authorizationCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(authorizationCode);

        var payload = new { authorization_code = authorizationCode };
        await paystack.PostAsync<string>("customer/authorization/deactivate", payload);
    }
}