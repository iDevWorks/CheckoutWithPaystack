namespace CheckoutWithPaystack.Model
{
    public class Product 
    {
        public Product(int id, string name, decimal price, string imageUrl, string description = "")
        {
            Id = id;
            Name = name;
            Price = price;
            ImageUrl = imageUrl;
            Description = description;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }
    }


}
