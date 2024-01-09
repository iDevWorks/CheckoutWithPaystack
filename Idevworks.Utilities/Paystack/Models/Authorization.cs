using System.Text.Json.Serialization;

namespace iDevWorks.Paystack
{
    public class Authorization
    {
        public Authorization(string authorizationUrl, string accessCode, string reference)
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
}
