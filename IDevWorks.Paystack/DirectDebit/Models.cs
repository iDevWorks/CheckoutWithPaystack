using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.DirectDebit
{
    public class AuthInitResponse
    {
        [JsonPropertyName("redirect_url")]
        public string RedirectUrl { get; set; } = string.Empty;

        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;
    }

    public class AuthVerifyResponse
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
        public AuthCustomerResponse? Customer { get; set; }
    }

    public class AuthCustomerResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }

    public class ChargeResponse
    {
        [JsonPropertyName("amount")]
        public long Amount { get; set; } //should be in kobo/cents

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
        public ChargeAuthResponse? Authorization { get; set; }

        [JsonPropertyName("customer")]
        public ChargeCustomerResponse? Customer { get; set; }
    }

    public class ChargeAuthResponse
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

    public class ChargeCustomerResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("customer_code")]
        public string CustomerCode { get; set; } = string.Empty;
    }
}
