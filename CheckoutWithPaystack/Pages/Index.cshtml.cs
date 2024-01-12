using CheckoutWithPaystack.Model;
using CheckoutWithPaystack.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CheckoutWithPaystack.Pages
{
    public class IndexModel(Cache cacheService) : PageModel
    {
        public IEnumerable<Product> Products => cacheService.Products;
    }
}