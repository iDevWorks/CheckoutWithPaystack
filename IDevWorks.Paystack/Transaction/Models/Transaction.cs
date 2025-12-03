using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Transaction.Models
{
    public class Transaction(decimal amountInKobo, string reference, string status, string message, string gatewayResponse, decimal requestedAmount, Customer customer)
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
}
