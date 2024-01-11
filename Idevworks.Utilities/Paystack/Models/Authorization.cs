using System.Text.Json.Serialization;

namespace iDevWorks.Paystack
{
    public class Authorization(string authorizationUrl, string accessCode, string reference)
    {
        [JsonPropertyName("authorization_url")]
        public string AuthorizationUrl { get; } = authorizationUrl;

        [JsonPropertyName("access_code")]
        public string AccessCode { get; } = accessCode;

        [JsonPropertyName("reference")]
        public string Reference { get; } = reference;
    }
}
