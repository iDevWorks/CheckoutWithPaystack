namespace iDevWorks.Cart
{
    public class Cart 
    {
        private readonly Dictionary<string, CartItem> _cartItems = new();

        public IEnumerable<CartItem> Items => _cartItems.Values;
        public decimal TotalAmount => _cartItems.Values.Sum(x => x.Quantity * x.Price);


        public bool AddToCart(string itemId, decimal price, uint quantity)
        {
            //check if product exists in store
            if ( _cartItems.TryGetValue(itemId, out CartItem? item)) {
                item.AddQuantity(quantity);
                return true;
            }

            var cartItem = new CartItem(itemId, price, quantity);
            _cartItems.Add(itemId, cartItem);
            return true;
        }

        public bool UpdateQuantity(string itemId, uint quantity)
        {
            if (_cartItems.TryGetValue(itemId, out CartItem? item))
            {
                if (quantity == 0)
                {
                    _cartItems.Remove(itemId);
                    return false;
                }

                item.UpdateQuantity(quantity);
                return true;
            }
            return false;
        }

        public bool RemoveFromCart(string itemId)
        {
            if (!_cartItems.ContainsKey(itemId))
                return false;

            _cartItems.Remove(itemId);
            return true;
        }

    }
}
