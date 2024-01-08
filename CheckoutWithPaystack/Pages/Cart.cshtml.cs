using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using CheckoutWithPaystack.Services;
using CheckoutWithPaystack.Model;

namespace CheckoutWithPaystack.Pages
{
    public class CartModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CartService _cartService;

        public string Message { get; set; } = "";
        public decimal DeliveryPercent { get; private set; } = 0.10M;

        public IEnumerable<CartItem> CartItems { get; private set; } = new List<CartItem>();

        public CartModel(IHttpClientFactory httpClientFactory, CartService cartService)
        {
            _httpClientFactory = httpClientFactory;
            _cartService = cartService;
        }

        public void OnGet()
        {
            CartItems = _cartService.GetCartItems();
        }

        public async Task<ActionResult> OnPostInitializePaymentAsync()
        {
            if (ModelState.IsValid)
            {
                var url = "https://api.paystack.co/transaction/initialize";
                var testSecretKey = "sk_test_01f11480564bc34f6fde4c5c078741adb4ba738a";
                //TransactionDetail.Callback_Url = $"{HttpContext.Request.Host.Value}/PaymentComplete";

                var client = _httpClientFactory.CreateClient();

                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", testSecretKey);

                    CartItems = _cartService.GetCartItems();

                    var cartDetail = new InitializeTransactionDto();

                    var amount = CartItems.Sum(c => c.Quantity * c.Amount);
                    var deliveryFee = amount * DeliveryPercent ;
                    var totalAmount = amount + ((deliveryFee > 3000) ? 3000 : deliveryFee);

                    cartDetail.amount = (int)totalAmount * 100;

                    //set callback url
                    cartDetail.callback_url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/PaymentComplete";

                    //serialize data
                    var stringContent = new StringContent(JsonConvert.SerializeObject(cartDetail), Encoding.UTF8, "application/json");
                    //send request.
                    var httpResponse = await client.PostAsync(url, stringContent);

                    var jsonString = await httpResponse.Content.ReadAsStringAsync();
                    //deserialize to json
                    var jsonResponse = JsonConvert.DeserializeObject<PaystackResponseDto<PaystackTransactionInitializationResponseDto>>(jsonString);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        return Redirect(jsonResponse.data.authorization_url);
                    }
                    else
                    {
                        Message = jsonResponse.message;
                    }
                }
                catch (Exception ex)
                {
                    Message = $"An error occurred: {ex.Message}";
                }
            }

            return Page();
        }
    }


    public class InitializeTransactionDto
    {
        [Required, EmailAddress]
        public string email { get; set; } = "nathan@idevworks.com";
        public int amount { get; set; } = 14000;
        public string callback_url { get; set; }
        public string reference { get; set; } = Guid.NewGuid().ToString();
    }

    public class PaystackResponseDto<T> where T : class
    {
        public bool status { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }

    public class PaystackTransactionInitializationResponseDto
    {
        public string authorization_url { get; set; }
        public string access_code { get; set; }
        public string reference { get; set; }
    }
}
