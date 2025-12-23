namespace ConsolePhoneStore.Models
{
    public abstract class Producto
    {
        private decimal _precio;

        public int Id { get; set; }
        public decimal Precio
        {
            get => _precio;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("El precio debe ser mayor que 0");

                _precio = value;
            }
        }

        public DateTime FechaAlta { get; set; }

        protected Producto()
        {
            FechaAlta = DateTime.Now;
        }

        public abstract string GetDescripcion();
    }
}
