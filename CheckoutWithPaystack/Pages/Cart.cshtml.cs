using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CheckoutWithPaystack.Services;
using CheckoutWithPaystack.Model;
using iDevWorks.Cart;
using iDevWorks.Paystack;

namespace CheckoutWithPaystack.Pages
{
    public class CartModel : PageModel
    {
        private readonly PaystackClient _paystack;

        public Cart<Product> Cart { get; } 

        public CartModel(CacheService cacheService)
        {
            _paystack = new PaystackClient("sdfsfsfsdf");
            Cart = cacheService.DemoCart;
        }

        public async Task<ActionResult> OnPostInitializePaymentAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var callbackUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/PaymentComplete";
                    var result = await _paystack.InitializeTransaction("nathan@idevworks.com", Cart.TotalAmount, callbackUrl);

                    return Redirect(result.AuthorizationUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }      
            }
            return Page();
        }
    }
}
