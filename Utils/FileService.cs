using ConsolePhoneStore.Models;

namespace ConsolePhoneStore.Utils
{
    public static class FileService
    {
        private static readonly string filePath = "customers.txt";

        // 🔹 Cargar clientes desde fichero
        public static List<Customer> LoadCustomers()
        {
            List<Customer> customers = new();

            if (!File.Exists(filePath))
                return customers;

            foreach (var line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Formato: id|name|email|password
                var parts = line.Split('|');

                if (parts.Length != 4)
                    continue;

                customers.Add(new Customer(
                    int.Parse(parts[0]),
                    parts[1],
                    parts[2],
                    parts[3]
                ));
            }

            return customers;
        }

        // 🔹 Guardar UN cliente (append)
        public static void SaveCustomer(Customer customer)
        {
            string line = $"{customer.Id}|{customer.Name}|{customer.Email}|{customer.Password}";
            File.AppendAllText(filePath, line + Environment.NewLine);
        }

        // 🔹 Guardar TODOS los clientes (opcional)
        public static void SaveCustomers(List<Customer> customers)
        {
            List<string> lines = new();

            foreach (var c in customers)
            {
                lines.Add($"{c.Id}|{c.Name}|{c.Email}|{c.Password}");
            }

            File.WriteAllLines(filePath, lines);
        }
        public static void SavePurchase(
    string customerEmail,
    List<(Phone phone, int quantity)> cart,
    decimal total)
{
    string path = "purchases.txt";

    using StreamWriter writer = new(path, append: true);

    writer.WriteLine("=================================");
    writer.WriteLine($"Fecha: {DateTime.Now}");
    writer.WriteLine($"Cliente: {customerEmail}");

    foreach (var item in cart)
    {
        writer.WriteLine(
            $"{item.phone.Brand} {item.phone.Model} x{item.quantity} = {item.phone.Price * item.quantity}€"
        );
    }

    writer.WriteLine($"TOTAL: {total}€");
    writer.WriteLine();
}
    }
}
