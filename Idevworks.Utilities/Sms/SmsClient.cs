using Idevworks.Utilities.SMS.Dtos;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;

namespace Idevworks.Utilities.SMS
{
    public class SmsClient
    {
        private static string _baseUrl = "https://api.smslive247.com";
        private string _apiKey;
        private readonly HttpClient _httpClient;

        public SmsClient(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public Task<ApiResponse<SmsMessageResponse>> SendSmsAsync(string mobileNumber, string messageText, string senderId, string route = "")
        {
            return SendSmsAsync(_apiKey, mobileNumber, messageText, senderId, route, _httpClient);
        }

        public static Task<ApiResponse<SmsMessageResponse>> SendSmsAsync(string apiKey, string mobileNumber, string messageText, string senderId, string route = "", HttpClient? httpclient = null)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("Please enter your api key", nameof(apiKey));

            if (httpclient == null)
                httpclient = new HttpClient();

            httpclient.BaseAddress = new Uri(_baseUrl);
            httpclient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new SmsMessageRequest
            {
                MessageText = messageText,
                MobileNumber = mobileNumber,
                SenderID = senderId,
                Route = route
            };

            return SendPostRequest<SmsMessageRequest, ApiResponse<SmsMessageResponse>>("/api/v4/sms", requestBody, httpclient);
        }

        
        

        //public Task<ApiResponse<AccountResponse>> GetAccountInfoAsync()
        //{
        //    var url = "/api/v4/accounts";
        //    return SendGetRequestAsync<ApiResponse<AccountResponse>>(url);
        //}

        //public Task<ApiResponse<List<AccountResponse>>> GetSubAccountsAsync()
        //{
        //    var url = "/api/v4/accounts/sub";
        //    return SendGetRequestAsync<ApiResponse<List<AccountResponse>>>(url);
        //}




        //public Task<ApiResponse<List<SenderIdResponse>>> GetSenderIdsAsync()
        //{
        //    var url = "/api/v4/senderids";
        //    return SendGetRequestAsync<ApiResponse<List<SenderIdResponse>>>(url);
        //}




        //public Task<ApiResponse<List<SmsMessageResponse>>> GetSmsMessagesAsync(int pageNo = 1, int pageSize = 20, DateTime? dateFrom = null, DateTime? dateTo = null)
        //{
        //    var url = $"/api/v4/sms?pageNo={pageNo}&pageSize={pageSize}";
        //    if (dateFrom != null)
        //        url += $"&dateFrom={dateFrom.Value.ToString("yyyy-MM-ddTHH-mm-ss")}";

        //    if (dateTo != null)
        //        url += $"&dateTo={dateTo.Value.ToString("yyyy-MM-ddTHH-mm-ss")}";
        //    return SendGetRequestAsync<ApiResponse<List<SmsMessageResponse>>>(url);
        //}

        //public Task<ApiResponse<SmsMessageResponse>> GetSmsMessageAsync(string messageId)
        //{
        //    var url = $"/api/v4/sms/{messageId}";
        //    return SendGetRequestAsync<ApiResponse<SmsMessageResponse>>(url);
        //}

        //public Task<ApiResponse<SmsMessageResponse>> NumberLookupAsync(string mobileNumber)
        //{
        //    throw new NotImplementedException();

        //    var url = $"/api/v4/sms/lookup/{mobileNumber}";
        //    return SendGetRequestAsync<ApiResponse<SmsMessageResponse>>(url);
        //}





        //public Task<ApiResponse<List<SmsCampaignResponse>>> GetSmsCampaignsAsync(int pageNo = 1, int pageSize = 20, DateTime? dateFrom = null, DateTime? dateTo = null)
        //{
        //    var url = $"/api/v4/sms/batch?pageNo={pageNo}&pageSize={pageSize}";
        //    if (dateFrom != null)
        //        url += $"&dateFrom={dateFrom.Value.ToString("yyyy-MM-ddTHH-mm-ss")}";

        //    if (dateTo != null)
        //        url += $"&dateTo={dateTo.Value.ToString("yyyy-MM-ddTHH-mm-ss")}";
        //    return SendGetRequestAsync<ApiResponse<List<SmsCampaignResponse>>>(url);
        //}

        //public Task<ApiResponse<SmsCampaignResponse>> GetSmsCampaignAsync(int batchId)
        //{
        //    var url = $"/api/v4/sms/batch/{batchId}";
        //    return SendGetRequestAsync<ApiResponse<SmsCampaignResponse>>(url);
        //}

        //public Task<ApiResponse<List<SmsMessageResponse>>> GetSmsCampaignReportsAsync(int batchId)
        //{
        //    var url = $"/api/v4/sms/batch/{batchId}/reports";
        //    return SendGetRequestAsync<ApiResponse<List<SmsMessageResponse>>>(url);
        //}

        //public Task<ApiResponse<SmsCampaignResponse>> SendSmsCampaign(string messageText, string senderId, string[] mobileNumbers, DateTime? deliveryTime = null, string route = "")
        //{
        //    var requestBody = new SmsCampaignRequest
        //    {
        //        MessageText = messageText,
        //        MobileNumbers = mobileNumbers.ToList(),
        //        DeliveryTime = deliveryTime,
        //        SenderID = senderId,
        //        Route = route
        //    };

        //    return SendPostRequest<SmsCampaignRequest, ApiResponse<SmsCampaignResponse>>("/api/v4/sms/batch", requestBody);
        //}

        //public Task<ApiResponse<SmsCampaignResponse>> PauseSmsCampaignAsync(int batchId)
        //{
        //    var url = $"/api/v4/sms/batch/{batchId}/pause";
        //    return SendPostRequest<ApiResponse<SmsCampaignResponse>>(url);
        //}

        //public Task<ApiResponse<SmsCampaignResponse>> ResumeSmsCampaignAsync(int batchId)
        //{
        //    var url = $"/api/v4/sms/batch/{batchId}/resume";
        //    return SendPostRequest<ApiResponse<SmsCampaignResponse>>(url);
        //}

        //public Task<ApiResponse<SmsCampaignResponse>> AbortSmsCampaignAsync(int batchId)
        //{
        //    var url = $"/api/v4/sms/batch/{batchId}/abort";
        //    return SendPostRequest<ApiResponse<SmsCampaignResponse>>(url);
        //}







        private static async Task<TResponse> SendPostRequest<TRequest, TResponse>(string url, TRequest? requestBody, HttpClient httpClient)
        {
            var res = await httpClient.PostAsJsonAsync(url, requestBody);
            var jsonString = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
            {
                var errResult = JsonConvert.DeserializeObject<ApiErrorException>(jsonString);
                if (errResult != null)
                    throw errResult;

                errResult = new ApiErrorException()
                {
                    Code = ((int)res.StatusCode),
                    Message = "An http error happened."
                };
                throw errResult;
            }

            return JsonConvert.DeserializeObject<TResponse>(jsonString)!;
        }

        private async Task<TResponse> SendPostRequest<TResponse>(string url)
        {
            var res = await _httpClient.PostAsJsonAsync(url, "");
            var jsonString = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
            {
                var errResult = JsonConvert.DeserializeObject<ApiErrorException>(jsonString);
                if (errResult != null)
                    throw errResult;

                errResult = new ApiErrorException()
                {
                    Code = ((int)res.StatusCode),
                    Message = "An http error happened."
                };
                throw errResult;

            }

            return JsonConvert.DeserializeObject<TResponse>(jsonString)!;
        }

        private async Task<TResponse> SendGetRequestAsync<TResponse>(string url)
        {
            var res = await _httpClient.GetAsync(url);
            var jsonString = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
            {
                var errResult = JsonConvert.DeserializeObject<ApiErrorException>(jsonString);
                if (errResult != null)
                    throw errResult;

                errResult = new ApiErrorException();
                if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    errResult.Message = "Unauthorized";
                    errResult.Code = 401;
                }
                else if (res.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    errResult.Message = "Internal Server Error";
                    errResult.Code = 500;
                }

                throw errResult;
            }

            return JsonConvert.DeserializeObject<TResponse>(jsonString);
        }
    }
}