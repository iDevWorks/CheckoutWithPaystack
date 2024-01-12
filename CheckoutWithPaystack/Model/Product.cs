using iDevWorks.Cart;

namespace CheckoutWithPaystack.Model
{
    public class Product(int id, string name, decimal price, string imageUrl, string description = "") : IProduct
    {
        public string Id { get; set; } = id.ToString();
        public string Name { get; set; } = name;
        public decimal Price { get; set; } = price;
        public string? Description { get; set; } = description;
        public string ImageUrl { get; set; } = imageUrl;
    }


}
