namespace ConsolePhoneStore.Models
{
    public class Telefono : Producto
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public bool Stock { get; set; }

        public Telefono(
            int id,
            string marca,
            string modelo,
            decimal precio,
            bool stock)
        {
            Id = id;
            Marca = marca;
            Modelo = modelo;
            Precio = precio; // valida desde Producto
            Stock = stock;
        }

        public override string GetDescripcion()
        {
            return $"{Marca} {Modelo} - {Precio:C}";
        }
    }
}
