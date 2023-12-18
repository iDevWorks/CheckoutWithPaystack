using CheckoutWithPaystack.Model;

namespace CheckoutWithPaystack.Services
{
    public class CacheService
    {
        public IEnumerable<Cart> Carts { get; }
        public IEnumerable<Product> Products { get; }

        public CacheService()
        {
            Products = new List<Product> {
                new Product(1, "Blue Washed Denim Button Up Jacket", 10000, "images/image-product-1-removebg-preview.png"),
                new Product(2, "Red Dress", 25000, "images/irene-kredenets-dwKiHoqqxk8-unsplash-removebg-preview.png"),
                new Product(3, "Black Men's Loafers Shoe", 1500, "images/imani-bahati-LxVxPA1LOVM-unsplash-removebg-preview.png"),
                new Product(4, "Blue Washed Denim Button Up Jacket", 2200, "images/domino-164_6wVEHfI-unsplash-removebg-preview.png"),
                new Product(5, "Red Dress", 25, "images/luis-felipe-lins-LG88A2XgIXY-unsplash-removebg-preview.png"),
                new Product(6, "Black Men's Loafers Shoe", 17300, "images/usama-akram-kP6knT7tjn4-unsplash-removebg-preview.png"),
                new Product(7, "Blue Washed Denim Button Up Jacket", 100, "images/paul-gaudriault-a-QH9MAAVNI-unsplash-removebg-preview.png"),
                new Product(8, "Blue Washed Denim Button Up Jacket", 100, "images/ryan-plomp-jvoZ-Aux9aw-unsplash-removebg-preview.png"),
                new Product(9, "Blue Washed Denim Button Up Jacket", 100, "images/andres-jasso-PqbL_mxmaUE-unsplash-removebg-preview.png"),
                new Product(10, "Blue Washed Denim Button Up Jacket", 21000, "images/maksim-larin-NOpsC3nWTzY-unsplash-removebg-preview.png"),
                new Product(11, "Blue Washed Denim Button Up Jacket", 3200, "images/359f7c855b2fd9cfba03e3876fff9ed4-removebg-preview.png"),
                new Product(12, "Blue Washed Denim Button Up Jacket", 40100, "images/R (1).png")
            };
            var cart = new Cart();
            var products = Products.ToList();

            cart.AddToCart(products[1], 1);
            cart.AddToCart(products[3], 3);
            cart.AddToCart(products[5], 5);

            Carts = new List<Cart>() { cart, cart };
        }

    }
}
