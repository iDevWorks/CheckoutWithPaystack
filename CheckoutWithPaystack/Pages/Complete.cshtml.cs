using iDevWorks.Paystack;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CheckoutWithPaystack.Pages
{
    public class PaymentCompleteModel(PaystackClient paystack) : PageModel
    {
        public Transaction? ResponseData { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<PageResult> OnGetAsync(string reference)
        {
            try
            {
                ResponseData = await paystack.VerifyTransaction(reference);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return Page();
        }
    }
}
