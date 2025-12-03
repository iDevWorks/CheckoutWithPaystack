using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace iDevWorks.Paystack.Transfer
{
    //public class CreateRecipientResponse //: PaystackBaseResponse
    //{
    //    [JsonPropertyName("data")]
    //    public RecipientData? Data { get; set; }
    //}

    public class RecipientData
    {
        [JsonPropertyName("recipient_code")]
        public string RecipientCode { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    //public class InitializeTransferResponse //: PaystackBaseResponse
    //{
    //    [JsonPropertyName("data")]
    //    public TransferData? Data { get; set; }
    //}

    public class TransferData
    {
        [JsonPropertyName("transfer_code")]
        public string TransferCode { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonPropertyName("reference")]
        public string? Reference { get; set; }
    }

    //public class VerifyTransferResponse //: PaystackBaseResponse
    //{
    //    [JsonPropertyName("data")]
    //    public VerifyTransferData? Data { get; set; }
    //}

    public class VerifyTransferData
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public long Amount { get; set; }

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
        public VerifyTransferRecipientData? Recipient { get; set; }
    }

    public class VerifyTransferRecipientData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("recipient_code")]
        public string RecipientCode { get; set; } = string.Empty;

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("details")]
        public VerifyTransferRecipientDetailsData? Details { get; set; }
    }

    public class VerifyTransferRecipientDetailsData
    {
        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; } = string.Empty;

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; } = string.Empty;

        [JsonPropertyName("bank_name")]
        public string BankName { get; set; } = string.Empty;
    }

    //public class BulkTransferResponse //: PaystackBaseResponse
    //{
    //    [JsonPropertyName("data")]
    //    public List<BulkTransferItemResponse> Data { get; set; } = [];
    //}

    public class BulkTransferItemResponse
    {
        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;

        [JsonPropertyName("recipient")]
        public string Recipient { get; set; } = string.Empty;

        // Amount in kobo as returned by Paystack
        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonPropertyName("transfer_code")]
        public string TransferCode { get; set; } = string.Empty;

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }
}
