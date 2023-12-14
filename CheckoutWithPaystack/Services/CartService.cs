using CheckoutWithPaystack.Model;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Immutable;

namespace CheckoutWithPaystack.Services
{
    public class CartService
    {
        private static int _unversalProductId = 0;
        private readonly IList<Product> _products;
        private readonly Dictionary<int, CartItem> _cartStore = new();

        public CartService()
        {
            _products = new List<Product>
            {
                new Product("Blue Washed Denim Button Up Jacket", 10000, "images/image-product-1-removebg-preview.png"),
                new Product("Red Dress", 25000, "images/irene-kredenets-dwKiHoqqxk8-unsplash-removebg-preview.png"),
                new Product("Black Men's Loafers Shoe", 1500, "images/imani-bahati-LxVxPA1LOVM-unsplash-removebg-preview.png"),
                new Product("Blue Washed Denim Button Up Jacket", 2200, "images/domino-164_6wVEHfI-unsplash-removebg-preview.png"),
                new Product("Red Dress", 25, "images/luis-felipe-lins-LG88A2XgIXY-unsplash-removebg-preview.png"),
                new Product("Black Men's Loafers Shoe", 17300, "images/usama-akram-kP6knT7tjn4-unsplash-removebg-preview.png"),
                new Product("Blue Washed Denim Button Up Jacket", 100, "images/paul-gaudriault-a-QH9MAAVNI-unsplash-removebg-preview.png"),
                new Product("Blue Washed Denim Button Up Jacket", 100, "images/ryan-plomp-jvoZ-Aux9aw-unsplash-removebg-preview.png"),
                new Product("Blue Washed Denim Button Up Jacket", 100, "images/andres-jasso-PqbL_mxmaUE-unsplash-removebg-preview.png"),
                new Product("Blue Washed Denim Button Up Jacket", 21000, "images/maksim-larin-NOpsC3nWTzY-unsplash-removebg-preview.png"),
                new Product("Blue Washed Denim Button Up Jacket", 3200, "images/359f7c855b2fd9cfba03e3876fff9ed4-removebg-preview.png"),
                new Product("Blue Washed Denim Button Up Jacket", 40100, "images/R (1).png")
            };

            //add the first 3 products to cart store/cache
            for (var i = 0; i < 3; i++)
            {
                AddToCart(_products[i]);
            }
        }

        public IEnumerable<Product> GetAllProduct()
        {
            return _products.ToImmutableList();
        }

        public IEnumerable<CartItem> GetCartItems()
        {
            return _cartStore.Values.ToImmutableList();
        }

        public void AddToCart(Product item)
        {
            var cartItem = new CartItem(item.Title, item.Amount, item.ImageUrl);
            if (!_cartStore.ContainsKey(item.Id))
            {
                _cartStore.Add(item.Id, cartItem);
            }

            //product already exists in store, increment the number of the product in the store
            var th = _cartStore[item.Id];
            th.AddQuantity(1);
            _cartStore[item.Id] = th;  
        }

        public void ReduceCartProductQuantity(int productId)
        {
            if (!_cartStore.ContainsKey(productId))
            {
                throw new KeyNotFoundException($"No product with {productId} was not found in the cart.");
            }

            var cartProduct = _cartStore[productId];
            if (cartProduct.Quantity > 1)
            {
                cartProduct.ReduceQuantity(1);

                //add the product with the updated quentity
                _cartStore[productId] = cartProduct;
            }

            if (cartProduct.Quantity == 1)
            {
                _cartStore.Remove(productId);
            }
        }

        public void RemoveFromCart(int productId)
        {
            if (!_cartStore.ContainsKey(productId))
            {
                throw new KeyNotFoundException($"No product with {productId} was not found in the cart.");
            }
            _cartStore.Remove(productId);
        }

        public static int GenerateNextProductId()
        {
            _unversalProductId += 1;
            return _unversalProductId;
        }
    }
}
