using System.Text;
using ConsolePhoneStore.Models;

namespace ConsolePhoneStore.Services
{
    public static class FileService
    {
        private static readonly string filePath = "data/purchases.txt";

        public static void SavePurchase(
            Customer customer,
            List<(Phone phone, int quantity)> cart,
            decimal subtotal,
            decimal iva,
            decimal total)
        {
            Directory.CreateDirectory("data");

            var sb = new StringBuilder();

            sb.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            sb.AppendLine($"Cliente: {customer.Email}");

            foreach (var item in cart)
            {
                sb.AppendLine(
                    $"- {item.phone.Brand} {item.phone.Model} x{item.quantity} → {(item.phone.Price * item.quantity):C}"
                );
            }

            sb.AppendLine($"Subtotal: {subtotal:C}");
            sb.AppendLine($"IVA: {iva:C}");
            sb.AppendLine($"TOTAL: {total:C}");
            sb.AppendLine(new string('-', 30));

            File.AppendAllText(filePath, sb.ToString());
        }
    }
}
