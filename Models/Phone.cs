namespace ConsolePhoneStore.Models
{
    /// <summary>
    /// Clase que representa un teléfono móvil en el catálogo de la tienda.
    /// Contiene información sobre el producto como marca, modelo, precio y stock disponible.
    /// </summary>
    public class Phone
    {
        // Propiedades de solo lectura para los datos básicos del teléfono
        public int Id { get; }
        public string Brand { get; }
        public string Model { get; }
        public decimal Price { get; }
        // Stock es modificable porque disminuye cuando se hacen compras
        public int Stock { get; set; }
        public DateTime ReleaseDate { get; }
        public bool IsActive { get; }

        /// <summary>
        /// Constructor del teléfono que valida los datos de entrada.
        /// Asegura que el precio sea positivo y el stock no sea negativo.
        /// </summary>
        public Phone(int id, string brand, string model, decimal price, int stock)
        {
            // Validar que el precio sea válido (mayor que 0)
            if (price <= 0)
                throw new ArgumentOutOfRangeException("El precio debe ser mayor que 0");

            // Validar que el stock no sea negativo
            if (stock < 0)
                throw new ArgumentOutOfRangeException("El stock no puede ser negativo");

            // Asignar los valores a las propiedades
            Id = id;
            Brand = brand;
            Model = model;
            Price = price;
            Stock = stock;
            ReleaseDate = DateTime.Now;
            IsActive = true;
        }

        /// <summary>
        /// Reduce el stock del teléfono cuando se realiza una compra.
        /// Valida que la cantidad sea válida antes de restar.
        /// </summary>
        public void ReduceStock(int quantity)
        {
            if (quantity <= 0 || quantity > Stock)
                throw new ArgumentOutOfRangeException("Cantidad inválida");

            Stock -= quantity;
        }
    }
}
