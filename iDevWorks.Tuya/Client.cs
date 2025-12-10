using System.Text;
using System.Text.Json;
using System.Net.Http.Json;
using System.Security.Cryptography;

namespace iDevWorks.Tuya;

public class Client(Region region, string clientId, string clientSecret, HttpClient? httpClient = null)
{
    private readonly Region _region = region;
    private readonly string _clientId = clientId;
    private readonly string _clientSecret = clientSecret;
    private Token? token = null;

    public Task<bool> SendInstructions(string deviceId, IEnumerable<CodeValue> commands)
    {
        var body = CreateCommandBody(commands);
        var path = $"v1.0/devices/{deviceId}/commands";
        return RequestAsync<bool>(HttpMethod.Post, path, body);
    }

    public Task<Device> GetDevice(string deviceId)
    {
        var path = $"v1.0/devices/{deviceId}";
        return RequestAsync<Device>(HttpMethod.Get, path);
    }

    public Task<Device[]> GetAllDevices(string userId)
    {
        var path = $"v1.0/users/{userId}/devices";
        return RequestAsync<Device[]>(HttpMethod.Get, path);
    }

    private async Task<TResult> RequestAsync<TResult>(
        HttpMethod method, string path, string? body = null,
        bool skipToken = false, bool forceTokenRefresh = false,
        CancellationToken ct = default)
    {
        httpClient ??= new HttpClient();

        var payload = _clientId;
        var headers = new Dictionary<string, string>();
        var url = new Uri($"https://{GetHostname(_region)}/{path}");
        var t = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds.ToString("0");

        string headersStr = "";
        //if (headers.Count > 0)
        //{
        //    headersStr = string.Concat(headers.Select(kv => $"{kv.Key}:{kv.Value}\n"));
        //    headers.Add("Signature-Headers", string.Join(":", headers.Keys));
        //}

        if (skipToken)
        {
            payload += t;
            headers["secret"] = _clientSecret;
        }
        else
        {
            await RefreshAccessToken(forceTokenRefresh, ct);
            payload += token?.AccessToken + t;
        }

        payload += $"{method}\n" +
                ComputeSha256(body) + '\n' +
                headersStr + '\n' +
                url.PathAndQuery;

        headers["t"] = t;
        headers["client_id"] = _clientId;
        headers["sign"] = ComputeHmacSha256(payload, _clientSecret);
        headers["sign_method"] = "HMAC-SHA256";
        if (!skipToken && token != null)
            headers["access_token"] = token.AccessToken;

        var request = CreateRequestMessage(_region, method, path, body, headers);
        var response = await httpClient.SendAsync(request, ct).ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<Response<TResult>>(ct)
            ?? throw new InvalidDataException("Failed to deserialize");

        if (!result.Success)
            throw new Exception(result.Message);

        return result.Result;
    }

    private static string ComputeSha256(string? input)
    {
        return string.Concat(SHA256.HashData(Encoding.UTF8.GetBytes(input ?? "")).Select(b => $"{b:x2}"));
    }

    private static string ComputeHmacSha256(string? input, string key)
    {
        using var algorithm = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        return string.Concat(algorithm.ComputeHash(Encoding.UTF8.GetBytes(input ?? "")).Select(b => $"{b:X2}"));
    }

    private static HttpRequestMessage CreateRequestMessage(Region region,
        HttpMethod method, string path, string? body, Dictionary<string, string> headers)
    {
        var request = new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri($"https://{GetHostname(region)}/{path}"),
        };

        if (headers != null)
        {
            foreach (var h in headers)
                request.Headers.Add(h.Key, h.Value);
        }
        if (body != null)
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

        return request;
    }

    private async Task RefreshAccessToken(bool force, CancellationToken ct)
    {
        if (force || token == null || token.HasExpired())
        {
            var path = "v1.0/token?grant_type=1";
            token = await RequestAsync<Token>(HttpMethod.Get,
                path, null, skipToken: true, false, ct);
        }
    }

    private static string CreateCommandBody(IEnumerable<CodeValue> commands)
    {
        var request = new Command { Commands = commands };
        return JsonSerializer.Serialize(request);
    }

    private static string GetHostname(Region region)
    {
        return region switch
        {
            Region.China => "openapi.tuyacn.com",
            Region.WesternAmerica => "openapi.tuyaus.com",
            Region.EasternAmerica => "openapi-ueaz.tuyaus.com",
            Region.CentralEurope => "openapi.tuyaeu.com",
            Region.WesternEurope => "openapi-weaz.tuyaeu.com",
            Region.India => "openapi.tuyain.com",
            _ => throw new Exception("Unknown region"),
        };
    }
}

public enum Region
{
    China,
    WesternAmerica,
    EasternAmerica,
    CentralEurope,
    WesternEurope,
    India
}