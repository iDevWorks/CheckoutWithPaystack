using System.Text.Json.Serialization;

namespace CheckoutWithPaystack.Model.Paystack
{
    public class PaystackResponse<T> where T : class
    {
        public PaystackResponse(bool status, string? message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        [JsonPropertyName("status")]
        public bool Status { get; }

        [JsonPropertyName("message")]
        public string? Message { get; }

        [JsonPropertyName("data")]
        public T Data { get; }
    }

    public class TransactionInitRequest
    {
        public TransactionInitRequest(string email, int amount, string callbackUrl, string reference)
        {
            Email = email;
            Amount = amount;
            CallbackUrl = callbackUrl;
            Reference = reference;
        }

        [JsonPropertyName("email")]
        public string Email { get; }

        [JsonPropertyName("amount")]
        public int Amount { get; }

        [JsonPropertyName("callback_url")]
        public string CallbackUrl { get; }

        [JsonPropertyName("reference")]
        public string Reference { get; }
    }

    public class TransactionInitResponse
    {
        public TransactionInitResponse(string authorizationUrl, string accessCode, string reference)
        {
            AuthorizationUrl = authorizationUrl;
            AccessCode = accessCode;
            Reference = reference;
        }

        [JsonPropertyName("authorization_url")]
        public string AuthorizationUrl { get; }

        [JsonPropertyName("access_code")]
        public string AccessCode { get; }

        [JsonPropertyName("reference")]
        public string Reference { get; }
    }

    public class TransactionVerifyResponse
    {
        public TransactionVerifyResponse(decimal amountInKobo, string reference, string status, string message, string gatewayResponse, decimal requestedAmount, CustomerData customer)
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
        public CustomerData Customer { get; }
    }

    public class CustomerData
    {
        public CustomerData(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        [JsonPropertyName("first_name")]
        public string FirstName { get; }

        [JsonPropertyName("last_name")]
        public string LastName { get; }

        [JsonPropertyName("email")]
        public string Email { get; }
    }
}
