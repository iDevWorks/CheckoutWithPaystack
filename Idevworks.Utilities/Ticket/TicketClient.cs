using System.Text.Json;
using System.Net.Http.Json;

namespace iDevWorks.Ticket
{
    public class TicketClient
    {
        private readonly HttpClient _httpClient;

        public TicketClient(string apiKey, HttpClient? httpClient = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

            _httpClient = httpClient ?? new HttpClient();
            _httpClient.BaseAddress = new Uri("https://teekeet-api.azurewebsites.net/v1/");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        public async Task<App> Register(string appName, string username, string password, int timeZone)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(appName);
            ArgumentException.ThrowIfNullOrWhiteSpace(username);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);
            ArgumentOutOfRangeException.ThrowIfLessThan(timeZone, -12);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(timeZone, 12);

            var requestUri = "apps";
            var request = new { appName, username, password, timeZone };
            var response = await _httpClient.PostAsJsonAsync(requestUri, request);
            return await ProcessResponse<App>(response);
        }

        public async Task<App> Login(string username, string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(username);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            var requestUri = "apps/login";
            var request = new { username, password };
            var response = await _httpClient.PostAsJsonAsync(requestUri, request);
            return await ProcessResponse<App>(response);
        }


        public async Task<List<MessageThread>> GetThreads(string userId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userId);

            var requestUri = $"users/{userId}/threads";
            var response = await _httpClient.GetAsync(requestUri);
            return await ProcessResponse<List<MessageThread>>(response);
        }

        public async Task<MessageThread> CreateThread(string userId, string[] receiverIds, string subject, string body)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userId);
            ArgumentException.ThrowIfNullOrWhiteSpace(subject);
            ArgumentException.ThrowIfNullOrWhiteSpace(body);
            ArgumentNullException.ThrowIfNull(receiverIds);

            var requestUri = $"users/{userId}/threads";
            var request = new { receiverIds, subject, body };
            var response = await _httpClient.PostAsJsonAsync(requestUri, request);
            return await ProcessResponse<MessageThread>(response);
        }

        public async Task<MessageThread> DeleteThread(string userId, Guid threadId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userId);

            var requestUri = $"users/{userId}/threads/{threadId}";
            var response = await _httpClient.DeleteAsync(requestUri);
            return await ProcessResponse<MessageThread>(response);
        }

        public async Task<MessageThread> ReadThread(string userId, Guid threadId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userId);

            var requestUri = $"users/{userId}/threads/{threadId}";
            var response = await _httpClient.PutAsync(requestUri, null);
            return await ProcessResponse<MessageThread>(response);
        }




        public async Task<List<Message>> GetMessages(string userId, Guid threadId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userId);

            var requestUri = $"users/{userId}/threads/{threadId}/messages";
            var response = await _httpClient.GetAsync(requestUri);
            return await ProcessResponse<List<Message>>(response);
        }

        public async Task<Message> ReplyMessage(string userId, Guid threadId, string[] receiverIds, string subject, string body)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userId);

            var requestUri = $"users/{userId}/threads/{threadId}/messages";
            var request = new { receiverIds, subject, body };
            var response = await _httpClient.PostAsJsonAsync(requestUri, request);
            return await ProcessResponse<Message>(response);
        }

        public async Task<Message> DeleteMessage(string userId, Guid threadId, int messageId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userId);

            var requestUri = $"users/{userId}/threads/{threadId}/messages/{messageId}";
            var response = await _httpClient.DeleteAsync(requestUri);
            return await ProcessResponse<Message>(response);
        }


        private static async Task<TResult> ProcessResponse<TResult>(HttpResponseMessage httpResponse)
        where TResult : class
        {
            var jsonString = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TResult>(jsonString);

            if (httpResponse.IsSuccessStatusCode)
            {
                if (result != null)
                    return result;

                throw new TicketException(Status.REQUEST_OK_JSON_FAIL, jsonString);
            }

            if (result != null)
                throw new TicketException(Status.REQUEST_FAIL_JSON_OK, jsonString);

            throw new TicketException(Status.REQUEST_FAIL_JSON_FAIL, jsonString);
        }
    }
}
