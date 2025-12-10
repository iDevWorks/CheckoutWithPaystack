using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace iDevWorks.Tuya;

public class Command
{
    [JsonPropertyName("commands")]
    public IEnumerable<CodeValue> Commands { get; set; } = [];
}

public class Response<TResult>
{
    [JsonPropertyName("success")]
    [MemberNotNullWhen(true, nameof(Result))]
    public required bool Success { get; set; }

    [JsonPropertyName("result")]
    public TResult? Result { get; set; }

    [JsonPropertyName("msg")]
    public string? Message { get; set; }

    [JsonPropertyName("tid")]
    public string? Tid { get; set; }

    [JsonPropertyName("t")]
    public long? T { get; set; }
}

public class Token
{
    private readonly DateTime time = DateTime.UtcNow;
    public bool HasExpired()
    {
        return time.AddSeconds(ExpireTime) < DateTime.UtcNow;
    }

    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; set; }

    [JsonPropertyName("expire_time")]
    public required int ExpireTime { get; set; }

    [JsonPropertyName("uid")]
    public required string Uid { get; set; }

}

public class Device
{
    [JsonPropertyName("active_time")]
    public required int ActiveTime { get; set; }

    [JsonPropertyName("biz_type")]
    public required int BizType { get; set; }

    [JsonPropertyName("category")]
    public required string Category { get; set; }

    [JsonPropertyName("create_time")]
    public required int CreateTime { get; set; }

    [JsonPropertyName("icon")]
    public required string Icon { get; set; }

    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("ip")]
    public required string Ip { get; set; }

    [JsonPropertyName("lat")]
    public required string Lat { get; set; }

    [JsonPropertyName("local_key")]
    public required string LocalKey { get; set; }

    [JsonPropertyName("lon")]
    public required string Lon { get; set; }

    [JsonPropertyName("model")]
    public required string Model { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("online")]
    public required bool Online { get; set; }

    [JsonPropertyName("owner_id")]
    public required string OwnerId { get; set; }

    [JsonPropertyName("product_id")]
    public required string ProductId { get; set; }

    [JsonPropertyName("product_name")]
    public required string ProductName { get; set; }

    [JsonPropertyName("status")]
    public required List<CodeValue> Status { get; set; }

    [JsonPropertyName("sub")]
    public required bool Sub { get; set; }

    [JsonPropertyName("time_zone")]
    public required string TimeZone { get; set; }

    [JsonPropertyName("uid")]
    public required string UserId { get; set; }

    [JsonPropertyName("update_time")]
    public required int UpdateTime { get; set; }

    [JsonPropertyName("uuid")]
    public required string Uuid { get; set; }

    public override string ToString() => Name;
}

public class CodeValue
{
    [JsonPropertyName("code")]
    public required string Code { get; set; }

    [JsonPropertyName("value")]
    public required object Value { get; set; }
}
