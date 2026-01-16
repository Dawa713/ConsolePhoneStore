using ConsolePhoneStore.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsolePhoneStore.Utils
{
    public static class FileService
    {
        private static readonly string customersPath = "Data/customers.json";
        private static readonly string phonesPath = "Data/phones.json";

        // ==================== CLIENTES ====================
        // 🔹 Cargar clientes desde fichero JSON
        public static List<Customer> LoadCustomers()
        {
            List<Customer> customers = new();

            if (!File.Exists(customersPath))
                return customers;

            try
            {
                string json = File.ReadAllText(customersPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                customers = JsonSerializer.Deserialize<List<Customer>>(json, options) ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar clientes: {ex.Message}");
            }

            return customers;
        }

        // 🔹 Guardar TODOS los clientes en JSON
        public static void SaveCustomers(List<Customer> customers)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(customers, options);
                File.WriteAllText(customersPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar clientes: {ex.Message}");
            }
        }

        // ==================== TELÉFONOS ====================
        // 🔹 Cargar teléfonos desde fichero JSON
        public static List<Phone> LoadPhones()
        {
            List<Phone> phones = new();

            if (!File.Exists(phonesPath))
                return phones;

            try
            {
                string json = File.ReadAllText(phonesPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                phones = JsonSerializer.Deserialize<List<Phone>>(json, options) ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar teléfonos: {ex.Message}");
            }

            return phones;
        }

        // 🔹 Guardar TODOS los teléfonos en JSON
        public static void SavePhones(List<Phone> phones)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(phones, options);
                File.WriteAllText(phonesPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar teléfonos: {ex.Message}");
            }
        }

        // ==================== COMPRAS ====================
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
