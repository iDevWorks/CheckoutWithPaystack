using iDevWorks.Cart;

namespace CheckoutWithPaystack.Model
{
    public class Product : IProduct
    {
        public Product(int id, string name, decimal price, string imageUrl, string description = "")
        {
            Id = id.ToString();
            Name = name;
            Price = price;
            ImageUrl = imageUrl;
            Description = description;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }
    }


}
