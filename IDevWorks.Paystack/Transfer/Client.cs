using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Transfer;

public class Client(PaystackClient paystack)
{
    public Task<RecipientData> CreateRecipient(string name, string accountNumber, string bankCode)
    {
        var payload = new
        {
            type = "nuban",
            name,
            account_number = accountNumber,
            bank_code = bankCode,
            currency = "NGN"
        };

        return paystack.PostAsync<RecipientData>("transferrecipient", payload);
    }

    public Task<TransferData> InitializeTransfer(decimal amount, string recipientCode, string reason, string reference)
    {
        var payload = new
        {
            source = "balance",
            amount = (int)(amount * 100),
            recipient = recipientCode,
            reason,
            reference
        };

        return paystack.PostAsync<TransferData>("transfer", payload);
    }

    public Task<List<BulkTransferItemResponse>> InitiateBulkTransfer(IEnumerable<BulkTransferItem> transfers)
    {
        var payload = new
        {
            currency = "NGN",
            source = "balance",
            transfers = transfers.Select(t => new
            {
                amount = (int)(t.Amount * 100), // convert to kobo
                t.reference,
                t.reason,
                recipient = t.RecipientCode
            })
        };

        return paystack.PostAsync<List<BulkTransferItemResponse>>("transfer/bulk", payload);
    }

    public Task<VerifyTransferData> VerifyTransfer(string reference)
    {
        return paystack.GetAsync<VerifyTransferData>($"transfer/verify/{reference}");
    }
}

public class BulkTransferItem
{
    public decimal Amount { get; set; }

    public string reference { get; set; } = string.Empty;

    public string reason { get; set; } = string.Empty;

    public string RecipientCode { get; set; } = string.Empty;
}
