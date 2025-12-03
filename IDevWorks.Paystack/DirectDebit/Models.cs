using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace iDevWorks.Paystack.DirectDebit
{
    //public class InitializeAuthorizationResponse //: PaystackBaseResponse
    //{
    //    [JsonPropertyName("data")]
    //    public AuthInitData? Data { get; set; }
    //}

    public class AuthInitData
    {
        [JsonPropertyName("redirect_url")]
        public string RedirectUrl { get; set; } = string.Empty;

        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;
    }

    //public class VerifyAuthorizationResponse //: PaystackBaseResponse
    //{
    //    [JsonPropertyName("data")]
    //    public AuthorizationData? Data { get; set; }
    //}

    public class AuthorizationData
    {
        [JsonPropertyName("authorization_code")]
        public string AuthorizationCode { get; set; } = string.Empty;

        [JsonPropertyName("channel")]
        public string Channel { get; set; } = string.Empty;

        [JsonPropertyName("bank")]
        public string Bank { get; set; } = string.Empty;

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("customer")]
        public AuthorizationCustomerData? Customer { get; set; }
    }

    public class AuthorizationCustomerData
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }

    //public class DeactivateAuthorizationResponse //: PaystackBaseResponse
    //{
    //}

    //public class ChargeAccountResponse //: PaystackBaseResponse
    //{
    //    [JsonPropertyName("data")]
    //    public ChargeData? Data { get; set; }
    //}

    public class ChargeData
    {
        // Amount in kobo (lowest currency unit)
        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("transaction_date")]
        public DateTime TransactionDate { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;

        [JsonPropertyName("gateway_response")]
        public string GatewayResponse { get; set; } = string.Empty;

        [JsonPropertyName("channel")]
        public string Channel { get; set; } = string.Empty;

        [JsonPropertyName("fees")]
        public long? Fees { get; set; }

        [JsonPropertyName("authorization")]
        public ChargeAuthorizationData? Authorization { get; set; }

        [JsonPropertyName("customer")]
        public ChargeCustomerData? Customer { get; set; }
    }

    public class ChargeAuthorizationData
    {
        [JsonPropertyName("authorization_code")]
        public string AuthorizationCode { get; set; } = string.Empty;

        [JsonPropertyName("bin")]
        public string? Bin { get; set; }

        [JsonPropertyName("last4")]
        public string? Last4 { get; set; }

        [JsonPropertyName("exp_month")]
        public string? ExpMonth { get; set; }

        [JsonPropertyName("exp_year")]
        public string? ExpYear { get; set; }

        [JsonPropertyName("channel")]
        public string? Channel { get; set; }

        [JsonPropertyName("card_type")]
        public string? CardType { get; set; }

        [JsonPropertyName("bank")]
        public string? Bank { get; set; }

        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("brand")]
        public string? Brand { get; set; }

        [JsonPropertyName("reusable")]
        public bool Reusable { get; set; }
    }

    public class ChargeCustomerData
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("customer_code")]
        public string CustomerCode { get; set; } = string.Empty;
    }

    //public class VerifyChargeResponse //: PaystackBaseResponse
    //{
    //    [JsonPropertyName("data")]
    //    public ChargeData? Data { get; set; }
    //}
}
