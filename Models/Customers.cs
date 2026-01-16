namespace ConsolePhoneStore.Models
{
    /// <summary>
    /// Clase que representa un cliente/usuario de la tienda.
    /// Almacena la información personal y de autenticación del usuario.
    /// </summary>
    public class Customer
    {
        // Propiedades del cliente
        public int Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Password { get; }
        public string Role { get; set; } // ADMIN o CLIENT
        public DateTime CreatedAt { get; }
        public bool IsActive { get; }

        /// <summary>
        /// Constructor del cliente que valida los datos de entrada.
        /// El rol por defecto es "CLIENT" si no se especifica.
        /// </summary>
        public Customer(int id, string name, string email, string password, string role = "CLIENT")
        {
            // Validar que el nombre no esté vacío y tenga máximo 10 caracteres
            if (string.IsNullOrWhiteSpace(name) || name.Length > 10)
                throw new ArgumentException("El nombre es obligatorio y máximo 10 caracteres");

            // Validar que el email tenga un formato válido
            if (!EsEmailValido(email))
                throw new ArgumentException("Email no válido");

            // Validar que la contraseña tenga entre 6 y 10 caracteres
            if (password.Length < 6 || password.Length > 10)
                throw new ArgumentException("La contraseña debe tener entre 6 y 10 caracteres");

            // Asignar los valores a las propiedades
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            Role = role;
            CreatedAt = DateTime.Now;
            IsActive = true;
        }

        /// <summary>
        /// Método privado para validar el formato del email.
        /// Comprueba que contenga @ y al menos un punto (.).
        /// </summary>
        private bool EsEmailValido(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }
    }
}
