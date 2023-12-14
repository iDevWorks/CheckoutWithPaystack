namespace CheckoutWithPaystack.Model
{
    public class CommerceItem
    {
        public string Title { get; set; }
        private string? Description { get; set; }
        public decimal Amount { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CartItem : CommerceItem
    {
        public CartItem(string title, decimal amount, string imageUrl)
        {
            Title = title;
            Amount = amount;
            Quantity = 1;
            ImageUrl = imageUrl;
        }
        public string Title { get; set; }
        private string? Description { get; set; }
        public int Quantity { get; private set; }
        public decimal Amount { get; set; }

        public void AddQuantity(int newQuantity)
        {
            Quantity += newQuantity;
        }

        public void ReduceQuantity(int newQuantity)
        {
            if (newQuantity > Quantity)
            {
                throw new InvalidOperationException("The new quantity cannot be greater than the product's current quantity, as this will result in a negative value.");
            }

            if (Quantity > 0)
            {
                Quantity -= newQuantity;
            }
        }
    }
}
