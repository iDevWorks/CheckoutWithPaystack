using CheckoutWithPaystack.Services;

namespace CheckoutWithPaystack.Model
{
    public class Product : CommerceItem
    {
        public Product(string tilte, decimal amount, string imgUrl)
        {
            Id = CartService.GenerateNextProductId();
            Title = tilte;
            Amount = amount;
            ImageUrl = imgUrl;
        }

        public int Id { get; set; }
    }
}
