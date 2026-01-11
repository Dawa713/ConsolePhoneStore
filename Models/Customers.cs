namespace ConsolePhoneStore.Models
{
    public class Customer
    {
        public int Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Password { get; }
        public DateTime CreatedAt { get; }
        public bool IsActive { get; }

        public Customer(int id, string name, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede estar vacío");

            if (!email.Contains("@"))
                throw new ArgumentException("Email no válido");

            if (password.Length < 4)
                throw new ArgumentException("La contraseña debe tener al menos 4 caracteres");

            Id = id;
            Name = name;
            Email = email;
            Password = password;
            CreatedAt = DateTime.Now;
            IsActive = true;
        }
    }
}
