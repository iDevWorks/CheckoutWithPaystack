using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CheckoutWithPaystack.Services;
using CheckoutWithPaystack.Model;
using iDevWorks.Paystack;
using iDevWorks.Cart;

namespace CheckoutWithPaystack.Pages
{
    public class CartModel(PaystackClient paystack, Cache cacheService) : PageModel
    {
        public Cart<Product> Cart { get; } = cacheService.DemoCart;

        public async Task<ActionResult> OnPostInitializePaymentAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var reference = Guid.NewGuid().ToString();
                    var callbackUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Complete";
                    var result = await paystack.InitializeTransaction("nathan@idevworks.com", Cart.TotalAmount, callbackUrl, reference);

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
