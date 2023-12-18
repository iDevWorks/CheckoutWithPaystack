using CheckoutWithPaystack.Model;
using CheckoutWithPaystack.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CheckoutWithPaystack.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<Product> Products => _cacheService.Products;
        public CacheService _cacheService;

        public IndexModel(CacheService cacheService)
        {
            _cacheService = cacheService;
        }
    }
}