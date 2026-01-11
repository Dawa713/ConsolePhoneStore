using ConsolePhoneStore.Models;

namespace ConsolePhoneStore.Services
{
    public static class CustomerService
    {
        private static List<Customer> customers = new();
        private static int nextId = 1;

        public static void Register(string name, string email, string password)
        {
            if (customers.Any(c => c.Email == email))
                throw new ArgumentException("Ya existe un cliente con ese email");

            customers.Add(new Customer(nextId++, name, email, password));
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
