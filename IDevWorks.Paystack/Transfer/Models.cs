using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Transfer
{

    public class BalanceResponse
    {
        [JsonPropertyName("balance")]
        public long BalanceInKobo { get; set; } 

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
    }

    //public class RecipientResponse
    //{
    //    [JsonPropertyName("recipient_code")]
    //    public string RecipientCode { get; set; } = string.Empty;

    //    [JsonPropertyName("name")]
    //    public string Name { get; set; } = string.Empty;
    //}

    //public class InitTransferResponse
    //{
    //    [JsonPropertyName("reference")]
    //    public string? Reference { get; set; }

    //    [JsonPropertyName("transfer_code")]
    //    public string TransferCode { get; set; } = string.Empty;

    //    [JsonPropertyName("status")]
    //    public string Status { get; set; } = string.Empty;
    //    [JsonPropertyName("amount")]
    //    public long AmountInKobo { get; set; }
    //}

    public class InitTransferResponse
    {
        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;

        [JsonPropertyName("recipient")]
        public string Recipient { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public long AmountInKobo { get; set; }

        [JsonPropertyName("transfer_code")]
        public string TransferCode { get; set; } = string.Empty;

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }

    public class VerifyTransferResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public long AmountInKobo { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;

        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;

        [JsonPropertyName("transfer_code")]
        public string TransferCode { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("recipient")]
        public RecipientResponse? Recipient { get; set; }
    }

    public class RecipientResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("recipient_code")]
        public string RecipientCode { get; set; } = string.Empty;

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("details")]
        public RecipientAccountResponse? Details { get; set; }
    }

    public class RecipientAccountResponse
    {
        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; } = string.Empty;

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; } = string.Empty;

        [JsonPropertyName("bank_name")]
        public string BankName { get; set; } = string.Empty;
    }
}
