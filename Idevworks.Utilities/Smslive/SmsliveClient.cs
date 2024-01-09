using iDevWorks.BulkSMS.Contracts;
using System.Net.Http.Json;
using System.Text.Json;

namespace iDevWorks.BulkSMS
{
    public class SmsliveClient
    {
        private readonly HttpClient _httpClient;

        public SmsliveClient(string apiKey, HttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.smslive247.com");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        public Task<Result<Account>> GetAccount()
        {
            var url = "/api/v4/accounts";
            return SendGetRequestAsync<Result<Account>>(_httpClient, url);
        }

        public Task<Result<List<Account>>> GetSubAccounts()
        {
            var url = "/api/v4/accounts/sub";
            return SendGetRequestAsync<Result<List<Account>>>(_httpClient, url);
        }

        public Task<Result<List<Sender>>> GetSenderIds()
        {
            var url = "/api/v4/senderids";
            return SendGetRequestAsync<Result<List<Sender>>>(_httpClient, url);
        }

        public Task<Result<Message>> NumberLookup(string mobileNumber)
        {
            throw new NotImplementedException();

            //var url = $"/api/v4/sms/lookup/{mobileNumber}";
            //return SendGetRequestAsync<Result<SmsMessageResponse>>(_httpClient, url);
        }

        public Task<Result<List<Message>>> GetMessages(int pageNo = 1, int pageSize = 20, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var query = CreateFilterQueryString(pageNo, pageSize, dateFrom, dateTo);
            var url = $"/api/v4/sms?{query}";

            return SendGetRequestAsync<Result<List<Message>>>(_httpClient, url);
        }

        public Task<Result<Message>> GetMessage(string messageId)
        {
            var url = $"/api/v4/sms/{messageId}";
            return SendGetRequestAsync<Result<Message>>(_httpClient, url);
        }

        public Task<Result<Message>> SendMessage(string mobileNumber, string messageText, string senderId, string route = "")
        {
            var requestBody = new MessageRequest
            {
                MessageText = messageText,
                MobileNumber = mobileNumber,
                SenderID = senderId,
                Route = route
            };

            return SendPostRequest<MessageRequest, Result<Message>>
                (_httpClient, "/api/v4/sms", requestBody);
        }

        public Task<Result<List<Campaign>>> GetCampaigns(int pageNo = 1, int pageSize = 20, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var query = CreateFilterQueryString(pageNo, pageSize, dateFrom, dateTo);
            var url = $"/api/v4/sms/batch?{query}";

            return SendGetRequestAsync<Result<List<Campaign>>>(_httpClient, url);
        }

        public Task<Result<Campaign>> GetCampaign(int batchId)
        {
            var url = $"/api/v4/sms/batch/{batchId}";
            return SendGetRequestAsync<Result<Campaign>>(_httpClient, url);
        }

        public Task<Result<List<Message>>> GetCampaignReports(int batchId)
        {
            var url = $"/api/v4/sms/batch/{batchId}/reports";
            return SendGetRequestAsync<Result<List<Message>>>(_httpClient, url);
        }

        public Task<Result<Campaign>> SendCampaign(string messageText, string senderId, string[] mobileNumbers, DateTime? deliveryTime = null, string route = "")
        {
            var requestBody = new CampaignRequest
            {
                MessageText = messageText,
                MobileNumbers = mobileNumbers.ToList(),
                DeliveryTime = deliveryTime,
                SenderID = senderId,
                Route = route
            };

            return SendPostRequest<CampaignRequest, Result<Campaign>>
                (_httpClient, "/api/v4/sms/batch", requestBody);
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





        private static string CreateFilterQueryString(int pageNo = 1, int pageSize = 20, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var url = $"pageNo={pageNo}&pageSize={pageSize}";

            if (dateFrom != null)
                url += $"&dateFrom={dateFrom.Value:yyyy-MM-ddTHH-mm-ss}";

            if (dateTo != null)
                url += $"&dateTo={dateTo.Value:yyyy-MM-ddTHH-mm-ss}";

            return url;
        }

        private static async Task<TResponse> SendPostRequest<TRequest, TResponse>(HttpClient httpClient, string url, TRequest? requestBody)
        {
            var httpResponse = await httpClient.PostAsJsonAsync(url, requestBody);
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

        private static async Task<TResponse> SendPostRequest<TResponse>(HttpClient httpClient, string url)
        {
            var httpResponse = await httpClient.PostAsJsonAsync(url, "");
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

        private static async Task<TResponse> SendGetRequestAsync<TResponse>(HttpClient httpClient, string url)
        {
            var httpResponse = await httpClient.GetAsync(url);
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