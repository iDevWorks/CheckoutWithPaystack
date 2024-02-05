using CheckoutWithPaystack.Model;
using CheckoutWithPaystack.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using iDevWorks.Ticket;

namespace CheckoutWithPaystack.Pages
{
    public class IndexModel(Cache cacheService) : PageModel
    {
        public IEnumerable<Product> Products => cacheService.Products;

        public async Task<PageResult> OnGet()
        {
            var userId = "Sender 1";
            var apiKey = "478599B8-74EF-4A38-9170-FE266335A5E1";

            var client = new TicketClient(apiKey);
            var Threads = await client.GetThreads(userId);

            //var threadId = Guid.NewGuid();
            //Messages = await client.GetMessages(userId, threadId);

            return Page();
        }

    }
}