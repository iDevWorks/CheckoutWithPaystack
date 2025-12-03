using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Transaction
{
    public class InitTransactionResponse(string authorizationUrl, string accessCode, string reference)
    {
        [JsonPropertyName("authorization_url")]
        public string AuthorizationUrl { get; } = authorizationUrl;

        [JsonPropertyName("access_code")]
        public string AccessCode { get; } = accessCode;

        [JsonPropertyName("reference")]
        public string Reference { get; } = reference;
    }

    public class TransactionResponse(decimal amountInKobo, string reference, string status, string message, string gatewayResponse, decimal requestedAmount, Customer customer)
    {
        [JsonPropertyName("amount")]
        public decimal AmountInKobo { get; } = amountInKobo;

        [JsonPropertyName("reference")]
        public string Reference { get; } = reference;

        [JsonPropertyName("status")]
        public string Status { get; } = status;

        [JsonPropertyName("message")]
        public string Message { get; } = message;

        [JsonPropertyName("gateway_response")]
        public string GatewayResponse { get; } = gatewayResponse;

        [JsonPropertyName("requested_amount")]
        public decimal RequestedAmount { get; } = requestedAmount;

        [JsonPropertyName("customer")]
        public Customer Customer { get; } = customer;

        public bool IsSuccessful => Status == "success";
    }

    public class Customer(string firstName, string lastName, string email)
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; } = firstName;

        [JsonPropertyName("last_name")]
        public string LastName { get; } = lastName;

        [JsonPropertyName("email")]
        public string Email { get; } = email;
    }

}
