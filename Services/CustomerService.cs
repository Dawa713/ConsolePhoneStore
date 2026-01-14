using ConsolePhoneStore.Models;
using ConsolePhoneStore.Utils;

namespace ConsolePhoneStore.Services
{
    public static class CustomerService
    {
        private static List<Customer> customers = FileService.LoadCustomers();
        private static int nextId = customers.Any() ? customers.Max(c => c.Id) + 1 : 1;

        public static void Register(string name, string email, string password)
        {
            if (customers.Any(c => c.Email == email))
                throw new ArgumentException("Ya existe un cliente con ese email");

            var customer = new Customer(nextId++, name, email, password);
            customers.Add(customer);
            FileService.SaveCustomer(customer);
        }

        public static Customer Login(string email, string password)
        {
            Customer? customer = customers
                .FirstOrDefault(c => c.Email == email && c.Password == password);

            if (customer == null)
                throw new ArgumentException("Credenciales incorrectas");

            return customer;
        }
    }
}
