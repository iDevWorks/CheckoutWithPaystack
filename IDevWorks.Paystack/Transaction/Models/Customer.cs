using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Transaction.Models
{
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
