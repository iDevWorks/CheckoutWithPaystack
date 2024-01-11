namespace iDevWorks.Cart
{
    public class CartItem<TProduct>(TProduct product, uint quantity) 
        : CartItem(product.Id, product.Name, product.Price, quantity) where TProduct : IProduct
    {
        public TProduct Product { get; private set; } = product;
    }

    public class CartItem 
    {
        public CartItem(string id, string name, decimal price, uint quantity)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(id);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

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

        public uint SetQuantity(uint quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

            return Quantity = quantity;
        }

        public uint AddQuantity(uint quantity)
        {
            return Quantity += quantity;
        }

        public uint SubtractQuantity(uint quantity)
        {
            if (quantity > Quantity)
                quantity = Quantity; //Quantity cannot be 0

            return Quantity -= quantity;
        }
    }
}
