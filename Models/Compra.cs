namespace Models
{
    public class Compra
    {
        public static int ContadorCompras = 0;

        public int Id { get; private set; }
        public int ClienteId { get; set; }
        public int TelefonoId { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal PrecioFinal { get; set; }
        public bool GarantiaExtendida { get; set; }

        public Compra()
        {
            Id = ++ContadorCompras;
            FechaCompra = DateTime.Now;
        }
    }
}
