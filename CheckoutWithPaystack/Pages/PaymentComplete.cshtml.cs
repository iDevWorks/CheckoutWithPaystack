using CheckoutWithPaystack.Model.Paystack;
using CheckoutWithPaystack.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CheckoutWithPaystack.Pages
{
    public class PaymentCompleteModel : PageModel
    {
        public TransactionVerifyResponse? ResponseData { get; set; }
        public string? ErrorMessage { get; set; }

        private readonly PaystackService _paystackService;

        public PaymentCompleteModel(PaystackService paystackService)
        {
            _paystackService = paystackService;
        }

        public async Task<PageResult> OnGetAsync(string reference)
        {
            try
            {
                ResponseData = await _paystackService.VerifyTransaction(reference);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return Page();
        }
    }
}
