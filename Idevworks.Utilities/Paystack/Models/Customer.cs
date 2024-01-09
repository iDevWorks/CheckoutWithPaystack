using System.Text.Json.Serialization;

namespace iDevWorks.Paystack
{
    public class Customer
    {
        public Customer(string firstName, string lastName, string email)
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
