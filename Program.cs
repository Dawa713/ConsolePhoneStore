using ConsolePhoneStore.Models;

class Program
{
    static void Main()
    {
        Telefono t = new Telefono(1, "Samsung", "Galaxy S23", 799.99m, true);
        Cliente c = new Cliente
        {
            Id = 1,
            Nombre = "Fran",
            Email = "fran@email.com",
            Password = "1234"
        };

        Console.WriteLine(t.GetDescripcion());
        Console.WriteLine($"Cliente: {c.Nombre} - {c.Email}");
    }
}
