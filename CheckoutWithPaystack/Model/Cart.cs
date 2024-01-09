namespace CheckoutWithPaystack.Model
{
    public class Cart 
    {
        private readonly Dictionary<int, CartItem> _cartItems = new();

        public Cart() { }

        public IEnumerable<CartItem> Items => _cartItems.Values;
        public decimal TotalAmount => _cartItems.Values.Sum(x => x.Quantity * x.Product.Price);


        public bool AddToCart(Product product, uint quantity)
        {
            //check if product exists in store
            if ( _cartItems.TryGetValue(product.Id, out CartItem? item)) {
                item.AddQuantity(quantity);
                return true;
            }

            var cartItem = new CartItem(product, quantity);
            _cartItems.Add(product.Id, cartItem);
            return true;
        }

        public bool UpdateQuantity(int productId, uint quantity)
        {
            if (_cartItems.TryGetValue(productId, out CartItem? item))
            {
                if (quantity == 0)
                {
                    _cartItems.Remove(productId);
                    return false;
                }

                item.UpdateQuantity(quantity);
                return true;
            }
            return false;
        }

        public bool RemoveFromCart(int productId)
        {
            if (!_cartItems.ContainsKey(productId))
                return false;

            _cartItems.Remove(productId);
            return true;
        }

    }
}
