using System.Text.Json.Serialization;

namespace iDevWorks.Paystack.Models;

public sealed class VirtualAccountProvider
{
    [JsonPropertyName("bank_id"), JsonAlias("id")] 
    public int ID { get; set; } = -1;

    [JsonPropertyName("bank_name"), JsonAlias("name")] 
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("provider_slug"), JsonAlias("slug")] 
    public string Slug { get; set; } = string.Empty;
}
