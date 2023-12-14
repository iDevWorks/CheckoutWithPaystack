using CheckoutWithPaystack.Model;
using CheckoutWithPaystack.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace CheckoutWithPaystack.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<Product> Products { get; private set; } = new List<Product>();
        public IEnumerable<CartItem> CartItems { get; private set; } = new List<CartItem>();


        private readonly CartService _cartService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, CartService cartService)
        {
            _logger = logger;
            _cartService = cartService;
        }

        public PageResult OnGet()
        {
            Products = _cartService.GetAllProduct();
            //to be saved in browser storeage for easy access
            //CartItems = _cartService.GetCartItems();
            return Page();
        }
    }
}