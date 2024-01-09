namespace iDevWorks.Cart
{
    public class CartItem 
    {
        public CartItem(string id, string name, decimal price, uint quantity)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (price <= 0)
                throw new ArgumentOutOfRangeException(nameof(price));

            if (quantity == 0) 
                throw new ArgumentOutOfRangeException(nameof(quantity));

            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
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
