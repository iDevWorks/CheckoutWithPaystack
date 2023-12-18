using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CheckoutWithPaystack.Services;
using CheckoutWithPaystack.Model;

namespace CheckoutWithPaystack.Pages
{
    public class CartModel : PageModel
    {
        private readonly PaystackService _paystackService;

        public Cart Cart { get; } 

        public CartModel(PaystackService paystackService, CacheService cacheService)
        {
            _paystackService = paystackService;
            Cart = cacheService.Carts.ToList().First();
        }

        public async Task<ActionResult> OnPostInitializePaymentAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var callbackUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/PaymentComplete";
                    var result = await _paystackService.InitializeTransaction("nathan@idevworks.com", Cart.TotalAmount, callbackUrl);

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
