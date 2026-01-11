using ConsolePhoneStore.Utils;
using ConsolePhoneStore.Services;
using ConsolePhoneStore.Models;

class Program
{
    static void Main()
    {
        Customer? clienteLogueado = null;
        bool salir = false;

        while (!salir)
        {
            int opcion = Menu.MostrarMenuPrincipal();

            try
            {
                switch (opcion)
                {
                   case 1:
    Console.WriteLine("📱 CATÁLOGO DE TELÉFONOS\n");

    foreach (var phone in PhoneService.GetAll())
    {
        Console.WriteLine(
            $"{phone.Id}. {phone.Brand} {phone.Model} - {phone.Price:C} (Stock: {phone.Stock})"
        );
    }

    Console.WriteLine("\nBuscar por marca (o ENTER para volver): ");
    string search = Console.ReadLine() ?? "";

    if (!string.IsNullOrWhiteSpace(search))
    {
        var results = PhoneService.SearchByBrand(search);

        Console.WriteLine("\nResultados:");
        foreach (var phone in results)
        {
            Console.WriteLine(
                $"{phone.Brand} {phone.Model} - {phone.Price:C}"
            );
        }
    }

    Console.ReadKey();
    break;

                    case 2:
                        Console.Write("Nombre: ");
                        string nombre = Console.ReadLine() ?? string.Empty;

                        Console.Write("Email: ");
                        string email = Console.ReadLine() ?? string.Empty;

                        Console.Write("Contraseña: ");
                        string password = Console.ReadLine() ?? string.Empty;

                        CustomerService.Register(nombre, email, password);
                        Console.WriteLine("✔️ Registro completado");
                        Console.ReadKey();
                        break;

                    case 3:
                        Console.Write("Email: ");
                        string emailLogin = Console.ReadLine() ?? string.Empty;

                        Console.Write("Contraseña: ");
                        string passLogin = Console.ReadLine() ?? string.Empty;

                        clienteLogueado = CustomerService.Login(emailLogin, passLogin);
                        Console.WriteLine($"✔️ Bienvenido {clienteLogueado.Name}");
                        Console.ReadKey();
                        break;

                    case 0:
                        salir = true;
                        break;

                    default:
                        Console.WriteLine("Opción inválida");
                        Console.ReadKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: {ex.Message}");
                Console.ReadKey();
            }
        }
    }
}
