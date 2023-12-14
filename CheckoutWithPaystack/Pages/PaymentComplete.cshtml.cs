using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CheckoutWithPaystack.Pages
{
    public class PaymentCompleteModel : PageModel
    {
        public PaystackTransactionVerificationResponseDto ResponseData { get; set; }
        public string ErrorMessage { get; set; }


        private readonly IHttpClientFactory _clientFactory;

        public PaymentCompleteModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<PageResult> OnGetAsync(string reference)
        {
            if (!string.IsNullOrWhiteSpace(reference))
            {
                var url = $"https://api.paystack.co/transaction/verify/{reference}";
                var testSecretKey = "sk_test_01f11480564bc34f6fde4c5c078741adb4ba738a";

                var client = _clientFactory.CreateClient();

                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", testSecretKey);

                    //send request.
                    var httpResponse = await client.GetAsync(url);

                    var jsonString = await httpResponse.Content.ReadAsStringAsync();
                    //deserialize to json
                    var jsonResponse = JsonConvert.DeserializeObject<PaystackResponseDto<PaystackTransactionVerificationResponseDto>>(jsonString);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        ResponseData = jsonResponse.data;
                    }
                    else
                    {
                        ErrorMessage = jsonResponse.message;
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }

            return Page();
        }
    }

    public class PaystackTransactionVerificationResponseDto
    {
        [JsonProperty("amount")]
        public decimal AmountInKobo { get; set; }
        public string Reference { get; set; }
        public string Status { get; set; }
        //only for when there's error (http code != 200)
        public string Message { get; set; }
        [JsonProperty("gateway_response")]
        public string GatewayResponse { get; set; }
        [JsonProperty("requested_amount")]
        public decimal RequestedAmount { get; set; }
        public Customer Customer { get; set; }
    }

    public class Customer
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
