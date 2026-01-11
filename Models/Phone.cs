namespace ConsolePhoneStore.Models
{
    public class Phone
    {
        public int Id { get; }
        public string Brand { get; }
        public string Model { get; }
        public decimal Price { get; }
        public int Stock { get; private set; }
        public DateTime ReleaseDate { get; }
        public bool IsActive { get; }

        public Phone(int id, string brand, string model, decimal price, int stock)
        {
            if (price <= 0)
                throw new ArgumentOutOfRangeException("El precio debe ser mayor que 0");

            if (stock < 0)
                throw new ArgumentOutOfRangeException("El stock no puede ser negativo");

            Id = id;
            Brand = brand;
            Model = model;
            Price = price;
            Stock = stock;
            ReleaseDate = DateTime.Now;
            IsActive = true;
        }

        public void ReduceStock(int quantity)
        {
            if (quantity <= 0 || quantity > Stock)
                throw new ArgumentOutOfRangeException("Cantidad inválida");

            Stock -= quantity;
        }
    }
}
