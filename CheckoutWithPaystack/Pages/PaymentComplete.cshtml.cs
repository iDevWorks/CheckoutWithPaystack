using iDevWorks.Paystack;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CheckoutWithPaystack.Pages
{
    public class PaymentCompleteModel : PageModel
    {
        public Transaction? ResponseData { get; set; }
        public string? ErrorMessage { get; set; }

        private readonly PaystackClient _paystack;

        public PaymentCompleteModel()
        {
            _paystack = new PaystackClient("sdfsfsfsdf");
        }

        public async Task<PageResult> OnGetAsync(string reference)
        {
            try
            {
                ResponseData = await _paystack.VerifyTransaction(reference);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return Page();
        }
    }
}
