using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.DirectDebit;

public class Client(PaystackClient paystack)
{
    public Task<AuthInitData> InitializeAuthorization(string email, string accountNumber, string bankCode)
    {
        var payload = new
        {
            email,
            channel = "direct_debit",
            account = new
            {
                bank_code = bankCode,
                number = accountNumber
            },
            callback_url = "https://your-app.com/callback"
        };

        return paystack.PostAsync<AuthInitData>("customer/authorization/initialize", payload);
    }

    public Task<AuthorizationData> VerifyAuthorization(string reference)
    {
        return paystack.GetAsync<AuthorizationData>($"customer/authorization/verify/{reference}");
    }

    public async Task DeactivateAuthorization(string authorizationCode)
    {
        var payload = new { authorization_code = authorizationCode };
        await paystack.PostAsync<string>("customer/authorization/deactivate", payload);
    }

    public Task<ChargeData> ChargeAccount(string authorizationCode, string email, decimal amount)
    {
        var payload = new
        {
            authorization_code = authorizationCode,
            email,
            amount = (int)(amount * 100) // Convert to kobo
        };

        return paystack.PostAsync<ChargeData>("transaction/charge_authorization", payload);
    }

    public Task<ChargeData> VerifyCharge(string reference)
    {
        return paystack.GetAsync<ChargeData>($"transaction/verify/{reference}");
    }


}