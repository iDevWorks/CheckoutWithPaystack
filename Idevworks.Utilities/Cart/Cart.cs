namespace iDevWorks.Cart
{
    public class Cart 
    {
        private readonly Dictionary<string, CartItem> _cartItems = new();

        public bool AddToCart(string id, string name, decimal price, uint quantity)
        {
            //check if product exists in store
            if ( _cartItems.TryGetValue(id, out CartItem? item)) {
                item.AddQuantity(quantity);
                return true;
            }

            var cartItem = new CartItem(id, name, price, quantity);
            _cartItems.Add(id, cartItem);
            return true;
        }

        public bool UpdateQuantity(string id, uint quantity)
        {
            if (_cartItems.TryGetValue(id, out CartItem? item))
            {
                if (quantity == 0)
                {
                    _cartItems.Remove(id);
                    return false;
                }

                item.UpdateQuantity(quantity);
                return true;
            }
            return false;
        }

        public bool RemoveFromCart(string id)
        {
            if (!_cartItems.ContainsKey(id))
                return false;

            _cartItems.Remove(id);
            return true;
        }

        public IEnumerable<CartItem> Items => _cartItems.Values;
        public decimal TotalAmount => _cartItems.Values.Sum(x => x.Quantity * x.Price);
    }
}
