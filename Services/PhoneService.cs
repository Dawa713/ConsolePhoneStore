using ConsolePhoneStore.Models;

namespace ConsolePhoneStore.Services
{
    public static class PhoneService
    {
        private static List<Phone> phones = new();
        private static int nextId = 1;

        static PhoneService()
        {
            phones.Add(new Phone(nextId++, "Samsung", "Galaxy S24", 899.99m, 10));
            phones.Add(new Phone(nextId++, "Apple", "iPhone 15", 1099.99m, 8));
            phones.Add(new Phone(nextId++, "Xiaomi", "Redmi Note 13", 299.99m, 15));
        }

        public static List<Phone> GetAll()
        {
            return phones;
        }

        public static List<Phone> SearchByBrand(string brand)
        {
            return phones
                .Where(p => p.Brand.ToLower().Contains(brand.ToLower()))
                .ToList();
        }
        public static Phone? GetById(int id)
{
    return phones.FirstOrDefault(p => p.Id == id);
}

    }
}
