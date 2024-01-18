using System.Net.Http.Json;
using System.Text.Json;

namespace iDevWorks.BulkSMS
{
    public class SmsliveClient
    {
        private readonly HttpClient _httpClient;

        public SmsliveClient(string apiKey, HttpClient? httpClient = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

            _httpClient = httpClient ?? new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.smslive247.com");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        public Task<Result<Account>> GetAccount()
        {
            var url = "/api/v4/accounts";
            return SendGetRequest<Result<Account>>(_httpClient, url);
        }

        public Task<Result<List<Account>>> GetSubAccounts()
        {
            var url = "/api/v4/accounts/sub";
            return SendGetRequest<Result<List<Account>>>(_httpClient, url);
        }

        public Task<Result<List<Sender>>> GetSenderIds()
        {
            var url = "/api/v4/senderids";
            return SendGetRequest<Result<List<Sender>>>(_httpClient, url);
        }

        public Task<Result<Message>> NumberLookup(string mobileNumber)
        {
            throw new NotImplementedException();
            //var url = $"/api/v4/sms/lookup/{mobileNumber}";
            //return SendGetRequest<Result<SmsMessageResponse>>(_httpClient, url);
        }

        public Task<Result<List<Message>>> GetMessages(int pageNo = 1, 
            int pageSize = 20, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var query = CreateFilterQueryString(pageNo, pageSize, dateFrom, dateTo);
            var url = $"/api/v4/sms?{query}";

            return SendGetRequest<Result<List<Message>>>(_httpClient, url);
        }

        public Task<Result<Message>> GetMessage(string messageId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(messageId);

            var url = $"/api/v4/sms/{messageId}";
            return SendGetRequest<Result<Message>>(_httpClient, url);
        }

        public Task<Result<Message>> SendMessage(string mobileNumber, 
            string messageText, string senderId, string route = "")
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(mobileNumber);
            ArgumentException.ThrowIfNullOrWhiteSpace(messageText);
            ArgumentException.ThrowIfNullOrWhiteSpace(senderId);

            var body = new MessageRequest
            {
                MessageText = messageText,
                MobileNumber = mobileNumber,
                SenderID = senderId,
                Route = route
            };

            return SendPostRequest<MessageRequest, Result<Message>>
                (_httpClient, "/api/v4/sms", body);
        }

        public Task<Result<List<Campaign>>> GetCampaigns(int pageNo = 1, 
            int pageSize = 20, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var query = CreateFilterQueryString(pageNo, pageSize, dateFrom, dateTo);
            var url = $"/api/v4/sms/batch?{query}";

            return SendGetRequest<Result<List<Campaign>>>(_httpClient, url);
        }

        public Task<Result<Campaign>> GetCampaign(int batchId)
        {
            var url = $"/api/v4/sms/batch/{batchId}";
            return SendGetRequest<Result<Campaign>>(_httpClient, url);
        }

        public Task<Result<List<Message>>> GetCampaignReports(int batchId)
        {
            var url = $"/api/v4/sms/batch/{batchId}/reports";
            return SendGetRequest<Result<List<Message>>>(_httpClient, url);
        }

        public Task<Result<Campaign>> SendCampaign(string messageText, string senderId, 
            string[] mobileNumbers, DateTime? deliveryTime = null, string route = "")
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(messageText);
            ArgumentException.ThrowIfNullOrWhiteSpace(senderId);

            ArgumentNullException.ThrowIfNull(mobileNumbers);
            ArgumentOutOfRangeException.ThrowIfZero(mobileNumbers.Length);

            var body = new CampaignRequest
            {
                MessageText = messageText,
                MobileNumbers = [.. mobileNumbers],
                DeliveryTime = deliveryTime,
                SenderID = senderId,
                Route = route
            };

            return SendPostRequest<CampaignRequest, Result<Campaign>>
                (_httpClient, "/api/v4/sms/batch", body);
        }

        public Task<Result<Campaign>> PauseCampaign(int batchId)
        {
            var url = $"/api/v4/sms/batch/{batchId}/pause";
            return SendPostRequest<Result<Campaign>>(_httpClient, url);
        }

        public Task<Result<Campaign>> ResumeCampaign(int batchId)
        {
            var url = $"/api/v4/sms/batch/{batchId}/resume";
            return SendPostRequest<Result<Campaign>>(_httpClient, url);
        }

        public Task<Result<Campaign>> AbortCampaign(int batchId)
        {
            var url = $"/api/v4/sms/batch/{batchId}/abort";
            return SendPostRequest<Result<Campaign>>(_httpClient, url);
        }


        private static string CreateFilterQueryString(int pageNo = 1,
            int pageSize = 20, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "pageNo", $"{pageNo}" },
                { "pageSize", $"{pageSize}" },
                { "dateFrom", dateFrom.HasValue ? $"{dateFrom.Value:yyyy-MM-ddTHH-mm-ss}" : "" },
                { "dateTo",   dateTo.HasValue   ? $"{dateTo.Value:yyyy-MM-ddTHH-mm-ss}"   : "" },
            };
            return string.Join("&", queryParams.Select(x => $"{x.Key}={x.Value}"));

            //var url = $"pageNo={pageNo}&pageSize={pageSize}";

            //if (dateFrom.HasValue)
                //url += $"&dateFrom={dateFrom.Value:yyyy-MM-ddTHH-mm-ss}";

            //if (dateTo.HasValue)
                //url += $"&dateTo={dateTo.Value:yyyy-MM-ddTHH-mm-ss}";

            //return url;
        }

        private static async Task<TResponse> SendPostRequest<TRequest, TResponse>(HttpClient httpClient, string url, TRequest? requestBody)
        {
            var httpResponse = await httpClient.PostAsJsonAsync(url, requestBody);
            return await ProcessResponse<TResponse>(httpResponse);
        }

        private static async Task<TResponse> SendPostRequest<TResponse>(HttpClient httpClient, string url)
        {
            var httpResponse = await httpClient.PostAsJsonAsync(url, string.Empty);
            return await ProcessResponse<TResponse>(httpResponse);
        }

        private static async Task<TResponse> SendGetRequest<TResponse>(HttpClient httpClient, string url)
        {
            var httpResponse = await httpClient.GetAsync(url);
            return await ProcessResponse<TResponse>(httpResponse);
        }

        private static async Task<TResponse> ProcessResponse<TResponse>(HttpResponseMessage httpResponse)
        {
            var jsonString = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errResult = JsonSerializer.Deserialize<SmsliveException>(jsonString);

                if (errResult != null)
                    throw errResult;

                throw new SmsliveException(httpResponse);
            }

            return JsonSerializer.Deserialize<TResponse>(jsonString)!;
        }

    }
}