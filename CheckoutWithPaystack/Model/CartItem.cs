namespace CheckoutWithPaystack.Model
{
    public class CartItem 
    {
        public CartItem(Product product, uint quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (quantity == 0) 
                throw new ArgumentOutOfRangeException(nameof(quantity));

            Product = product;
            Quantity = quantity;
        }

        public Product Product { get; private set; }
        public uint Quantity { get; private set; }
        public decimal Amount => Product.Price * Quantity;

        public uint UpdateQuantity(uint quantity)
        {
            if (quantity == 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            return Quantity = quantity;
        }

        public uint AddQuantity(uint quantity)
        {
            return Quantity += quantity;
        }

        public uint ReduceQuantity(uint quantity)
        {
            if (quantity > Quantity)
                quantity = Quantity; //Quantity cannot be 0

            return Quantity -= quantity;
        }
    }
}
