using System.Text.Json.Serialization;

namespace iDevWorks.Paystack
{
    public class Transaction
    {
        public Transaction(decimal amountInKobo, string reference, string status, string message, string gatewayResponse, decimal requestedAmount, Customer customer)
        {
            AmountInKobo = amountInKobo;
            Reference = reference;
            Status = status;
            Message = message;
            GatewayResponse = gatewayResponse;
            RequestedAmount = requestedAmount;
            Customer = customer;
        }

        [JsonPropertyName("amount")]
        public decimal AmountInKobo { get; }

        [JsonPropertyName("reference")]
        public string Reference { get; }

        [JsonPropertyName("status")]
        public string Status { get; }

        [JsonPropertyName("message")]
        public string Message { get; }

        [JsonPropertyName("gateway_response")]
        public string GatewayResponse { get; }

        [JsonPropertyName("requested_amount")]
        public decimal RequestedAmount { get; }

        [JsonPropertyName("customer")]
        public Customer Customer { get; }
    }
}
