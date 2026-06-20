namespace ConsolePhoneStore.Models
{
    /// <summary>
    /// Clase que representa una compra (ticket) realizada por un cliente.
    /// Es la 3ª clase de dominio del proyecto, junto a Customer y Phone, y es
    /// la que materializa las relaciones entre ambas:
    ///   - Purchase -> Customer (qué cliente ha comprado)
    ///   - Purchase -> Phone, a través de Items (qué teléfonos ha comprado)
    /// </summary>
    public class Purchase
    {
        private const decimal IVA_RATE = 0.21m;
        private static int nextId = 1;

        public int Id { get; set; }
        public Customer Customer { get; set; }                     // Relación con Customer
        public List<(Phone phone, int quantity)> Items { get; set; } // Relación con Phone
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public bool IsPaid { get; set; }

        /// Constructor que valida los datos y calcula subtotal, IVA y total
        /// a partir de los artículos del carrito. Misma idea que Customer/Phone:
        /// validar en el constructor para que nunca exista una Purchase inválida.
        public Purchase(Customer customer, List<(Phone phone, int quantity)> items)
        {
            if (customer == null)
                throw new ArgumentException("La compra debe estar asociada a un cliente");

            if (items == null || !items.Any())
                throw new ArgumentException("La compra debe contener al menos un artículo");

            Id = nextId++;
            Customer = customer;
            Items = items;
            Date = DateTime.Now;
            Status = "Completada";
            IsPaid = true;

            Subtotal = items.Sum(i => i.phone.Price * i.quantity);
            Iva = Subtotal * IVA_RATE;
            Total = Subtotal + Iva;
        }

        /// Genera el texto del ticket de compra listo para guardar en purchases.txt
        /// o mostrar por pantalla. Antes esta lógica vivía en FileService;
        /// ahora la propia Purchase sabe representarse a sí misma.
        public string ToTicket()
        {
            string ticket = "=================================\n";
            ticket += "  CONSOLE PHONE STORE\n";
            ticket += $"  Fecha: {Date:dd/MM/yyyy HH:mm:ss}\n";
            ticket += $"  Cliente: {Customer.Name} ({Customer.Email})\n";
            ticket += "---------------------------------\n";

            foreach (var item in Items)
            {
                decimal lineTotal = item.phone.Price * item.quantity;
                ticket += $"  {item.phone.Brand} {item.phone.Model}\n";
                ticket += $"    {item.quantity} x {item.phone.Price:F2}€ = {lineTotal:F2}€\n";
            }

            ticket += "---------------------------------\n";
            ticket += $"  Subtotal:    {Subtotal:F2}€\n";
            ticket += $"  IVA (21%):   {Iva:F2}€\n";
            ticket += $"  TOTAL:       {Total:F2}€\n";
            ticket += $"  Estado:      {Status}\n";
            ticket += "=================================\n";

            return ticket;
        }
    }
}
