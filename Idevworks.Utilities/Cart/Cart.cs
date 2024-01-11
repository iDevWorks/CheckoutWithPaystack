namespace iDevWorks.Cart
{
    public class Cart<TProduct> : Cart where TProduct : IProduct
    {
        public bool AddToCart(TProduct product, uint quantity)
        {
            if (AddMoreQuantity(product.Id, quantity))
                return true;

            var cartItem = new CartItem<TProduct>(product, quantity);
            _cartItems.Add(product.Id, cartItem);
            return true;
        }
    }

    public class Cart 
    {
        protected readonly Dictionary<string, CartItem> _cartItems = [];

        public bool AddToCart(string id, string name, decimal price, uint quantity)
        {
            if (AddMoreQuantity(id, quantity))
                return true;

            var cartItem = new CartItem(id, name, price, quantity);
            _cartItems.Add(id, cartItem);
            return true;
        }

        public bool RemoveFromCart(string id)
        {
            if (!_cartItems.ContainsKey(id))
                return false;

            _cartItems.Remove(id);
            return true;
        }

        public bool AddMoreQuantity(string id, uint quantity)
        {
            if (_cartItems.TryGetValue(id, out CartItem? item))
            {
                item.AddQuantity(quantity);
                return true;
            }
            return false;
        }

        public bool SubtractQuantity(string id, uint quantity)
        {
            if (_cartItems.TryGetValue(id, out CartItem? item))
            {
                item.SubtractQuantity(quantity);
                return true;
            }
            return false;
        }


        public bool SetQuantity(string id, uint quantity)
        {
            if (_cartItems.TryGetValue(id, out CartItem? item))
            {
                if (quantity == 0)
                {
                    _cartItems.Remove(id);
                    return false;
                }

                item.SetQuantity(quantity);
                return true;
            }
            return false;
        }

        public IEnumerable<CartItem> Items => _cartItems.Values;
        public decimal TotalAmount => _cartItems.Values.Sum(x => x.Quantity * x.Price);
    }
}
