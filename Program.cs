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
    bool volver = false;

    while (!volver)
    {
        int opcionCatalogo = Menu.MostrarMenuCatalogo();

        switch (opcionCatalogo)
        {
            case 1:
                Console.Clear();
                foreach (var phone in PhoneService.GetAll())
                {
                    Console.WriteLine(
                        $"{phone.Id}. {phone.Brand} {phone.Model} - {phone.Price:C}"
                    );
                }
                Console.ReadKey();
                break;

            case 2:
                Console.Write("Marca a buscar: ");
                string brand = Console.ReadLine() ?? "";

                var results = PhoneService.SearchByBrand(brand);

                foreach (var phone in results)
                {
                    Console.WriteLine(
                        $"{phone.Brand} {phone.Model} - {phone.Price:C}"
                    );
                }
                Console.ReadKey();
                break;

            case 0:
                volver = true;
                break;
        }
    }
    break;


                    case 2:
    Console.Clear();
    Console.WriteLine("📝 REGISTRO DE CLIENTE\n");

    string nombre = InputValidator.ReadNonEmptyString(
        "Nombre (máx 10 caracteres): ", 10);

    string email = InputValidator.ReadValidEmail(
        "Email: ");

    string password = InputValidator.ReadPassword(
        "Contraseña (6-10 caracteres): ", 6, 10);

    CustomerService.Register(nombre, email, password);

    Console.WriteLine("\n✅ Registro completado correctamente");
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

                    case 4:
    Console.Clear();
    var phones = PhoneService.GetAll();

    foreach (var phone in phones)
    {
        Console.WriteLine($"{phone.Id}. {phone.Brand} {phone.Model} - {phone.Price}€ (Stock {phone.Stock})");
    }

    Console.Write("\nID del teléfono: ");
    int id = int.Parse(Console.ReadLine()!);

    Console.Write("Cantidad: ");
    int quantity = int.Parse(Console.ReadLine()!);

    var selectedPhone = PhoneService.GetById(id);

    if (selectedPhone == null)
        throw new Exception("Teléfono no encontrado");

    if (quantity > selectedPhone.Stock)
        throw new Exception("Stock insuficiente");

    CartService.AddToCart(selectedPhone, quantity);
    Console.WriteLine("✔️ Producto añadido al carrito");
    Console.ReadKey();
    break;
                    case 5:
    Console.Clear();
    Console.WriteLine("🛒 CARRITO\n");

    var cart = CartService.GetCart();

    if (!cart.Any())
    {
        Console.WriteLine("Carrito vacío");
        Console.ReadKey();
        break;
    }

    foreach (var item in cart)
    {
        Console.WriteLine($"{item.phone.Brand} {item.phone.Model} x{item.quantity} = {item.phone.Price * item.quantity}€");
    }

    Console.WriteLine($"\nSubtotal: {CartService.CalculateSubtotal()}€");
    Console.ReadKey();
    break;
                   case 6:
    Console.Clear();

    var subtotal = CartService.CalculateSubtotal();
    var iva = subtotal * 0.21m;
    var total = subtotal + iva;

    Console.WriteLine($"Subtotal: {subtotal:C}");
    Console.WriteLine($"IVA (21%): {iva:C}");
    Console.WriteLine($"TOTAL: {total:C}");

    Console.Write("\nConfirmar compra (s/n): ");
    if (Console.ReadLine()?.ToLower() == "s")
    {
        foreach (var item in CartService.GetCart())
        {
            item.phone.Stock -= item.quantity;
        }

        FileService.SavePurchase(
            clienteLogueado!,
            CartService.GetCart(),
            subtotal,
            iva,
            total
        );

        CartService.ClearCart();
        Console.WriteLine("✅ Compra guardada correctamente");
    }

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
