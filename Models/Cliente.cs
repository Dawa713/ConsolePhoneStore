namespace Models
{
    public class Cliente
    {
        private string _email = string.Empty;

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException("Email no válido");

                _email = value;
            }
        }
        public string Password { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }

        public Cliente()
        {
            FechaRegistro = DateTime.Now;
            Activo = true;
        }
    }
}
