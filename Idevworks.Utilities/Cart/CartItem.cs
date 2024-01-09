namespace iDevWorks.Cart
{
    public class CartItem 
    {
        public CartItem(string itemId, decimal price, uint quantity)
        {
            if (string.IsNullOrWhiteSpace(itemId))
                throw new ArgumentNullException(nameof(itemId));

            if (price <= 0)
                throw new ArgumentOutOfRangeException(nameof(price));

            if (quantity == 0) 
                throw new ArgumentOutOfRangeException(nameof(quantity));

            ItemId = itemId;
            Price = price;
            Quantity = quantity;
        }

        public string ItemId { get; private set; }
        public decimal Price { get; private set; }
        public uint Quantity { get; private set; }
        public decimal Amount => Price * Quantity;

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
