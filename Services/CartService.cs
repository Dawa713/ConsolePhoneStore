using ConsolePhoneStore.Models;

namespace ConsolePhoneStore.Services
{
    public static class CartService
    {
        private static List<(Phone phone, int quantity)> cart = new();

        public static void AddToCart(Phone phone, int quantity)
        {
            var item = cart.FirstOrDefault(c => c.phone.Id == phone.Id);

            if (item.phone != null)
            {
                cart.Remove(item);
                cart.Add((phone, item.quantity + quantity));
            }
            else
            {
                cart.Add((phone, quantity));
            }
        }

        public static List<(Phone phone, int quantity)> GetCart()
        {
            return cart;
        }

        public static void ClearCart()
        {
            cart.Clear();
        }

        public static decimal CalculateSubtotal()
        {
            decimal total = 0;

            foreach (var item in cart)
            {
                total += item.phone.Price * item.quantity;
            }

            return total;
        }
    }
}
