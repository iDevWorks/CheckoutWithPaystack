namespace iDevWorks.Cart
{
    public interface IProduct
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
