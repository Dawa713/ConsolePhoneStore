using ConsolePhoneStore.Services;
using ConsolePhoneStore.Models;
using ConsolePhoneStore.Utils;
using Spectre.Console;

class Program
{
    static void Main()
    {
        Customer? clienteLogueado = null;
        bool salir = false;
        // Historial de compras en memoria: cada Purchase queda ligada a su Customer
        List<Purchase> historialCompras = new();

        while (!salir)
        {
            try
            {
                // ==================== MENÚ SEGÚN ESTADO DE SESIÓN ====================
                if (clienteLogueado == null)
                {
                    // ---- ZONA PÚBLICA ----
                    string opcion = Menu.MostrarMenuPrincipal();

                    switch (opcion)
                    {
                        case "Ver catálogo":
                            bool volver = false;
                            while (!volver)
                            {
                                string opcionCatalogo = Menu.MostrarMenuCatalogo();
                                switch (opcionCatalogo)
                                {
                                    case "Listar todos":
                                        ConsoleHelper.SafeClear();
                                        MostrarTablaTelefonos(PhoneService.GetAll());
                                        AnsiConsole.MarkupLine("\n[grey]Pulsa una tecla para continuar...[/]");
                                        Console.ReadKey();
                                        break;

                                    case "Buscar por marca":
                                        ConsoleHelper.SafeClear();
                                        string brand = AnsiConsole.Ask<string>("Marca a buscar:");
                                        var results = PhoneService.SearchByBrand(brand);

                                        if (!results.Any())
                                            AnsiConsole.MarkupLine("[red]No se encontraron teléfonos.[/]");
                                        else
                                            MostrarTablaTelefonos(results);

                                        AnsiConsole.MarkupLine("\n[grey]Pulsa una tecla para continuar...[/]");
                                        Console.ReadKey();
                                        break;

                                    case "Volver":
                                        volver = true;
                                        break;
                                }
                            }
                            break;

                        case "Registrarse":
                            ConsoleHelper.SafeClear();
                            AnsiConsole.MarkupLine("[bold]📝 REGISTRO DE CLIENTE[/]\n");

                            string nombre = InputValidator.ReadNonEmptyString("Nombre (máx 10 caracteres): ", 10);
                            string email = InputValidator.ReadValidEmail("Email: ");
                            string password = InputValidator.ReadPassword("Contraseña (6-10 caracteres): ", 6, 10);

                            CustomerService.Register(nombre, email, password);
                            AnsiConsole.MarkupLine("\n[green]✅ Registro completado correctamente[/]");
                            AnsiConsole.MarkupLine("\n[grey]Pulsa una tecla para continuar...[/]");
                            Console.ReadKey();
                            break;

                        case "Iniciar sesión":
                            ConsoleHelper.SafeClear();
                            string emailLogin = AnsiConsole.Ask<string>("Email:");
                            string passLogin = AnsiConsole.Prompt(
                                new TextPrompt<string>("Contraseña:").Secret());

                            clienteLogueado = CustomerService.Login(emailLogin, passLogin);

                            if (clienteLogueado == null)
                                AnsiConsole.MarkupLine("[red]❌ Email o contraseña incorrectos[/]");
                            else
                                AnsiConsole.MarkupLine($"[green]✔️ Bienvenido {Markup.Escape(clienteLogueado.Name)}[/]");

                            Console.ReadKey();
                            break;

                        case "Salir":
                            salir = true;
                            break;
                    }
                }
                else
                {
                    // ---- ZONA PRIVADA ----
                    bool esAdmin = clienteLogueado.Role == "ADMIN";
                    string opcion = Menu.MostrarMenuPrivado(clienteLogueado.Name, esAdmin);

                    switch (opcion)
                    {
                        case "Añadir producto al carrito":
                            ConsoleHelper.SafeClear();
                            MostrarTablaTelefonos(PhoneService.GetAll());

                            int id = AnsiConsole.Ask<int>("\nID del teléfono:");
                            int quantity = AnsiConsole.Ask<int>("Cantidad:");

                            var selectedPhone = PhoneService.GetById(id);
                            if (selectedPhone == null)
                                throw new Exception("Teléfono no encontrado");
                            if (quantity > selectedPhone.Stock)
                                throw new Exception("Stock insuficiente");

                            CartService.AddToCart(selectedPhone, quantity);
                            AnsiConsole.MarkupLine("[green]✔️ Producto añadido al carrito[/]");
                            AnsiConsole.MarkupLine("\n[grey]Pulsa una tecla para continuar...[/]");
                            Console.ReadKey();
                            break;

                        case "Ver carrito":
                            ConsoleHelper.SafeClear();
                            AnsiConsole.MarkupLine("[bold]🛒 CARRITO[/]\n");
                            var cart = CartService.GetCart();

                            if (!cart.Any())
                            {
                                AnsiConsole.MarkupLine("[grey]Carrito vacío[/]");
                                AnsiConsole.MarkupLine("\n[grey]Pulsa una tecla para continuar...[/]");
                                Console.ReadKey();
                                break;
                            }

                            var tablaCarrito = new Table();
                            tablaCarrito.AddColumn("Teléfono");
                            tablaCarrito.AddColumn("Cantidad");
                            tablaCarrito.AddColumn("Total");

                            foreach (var item in cart)
                                tablaCarrito.AddRow(
                                    $"{item.phone.Brand} {item.phone.Model}",
                                    item.quantity.ToString(),
                                    $"{item.phone.Price * item.quantity:F2}€");

                            AnsiConsole.Write(tablaCarrito);
                            AnsiConsole.MarkupLine($"\nSubtotal: {CartService.CalculateSubtotal():F2}€");

                            if (AnsiConsole.Confirm("¿Finalizar compra?"))
                            {
                                // La propia Purchase calcula subtotal, IVA y total
                                var purchase = new Purchase(clienteLogueado, CartService.GetCart());

                                AnsiConsole.MarkupLine($"\nSubtotal: {purchase.Subtotal:F2}€");
                                AnsiConsole.MarkupLine($"IVA (21%): {purchase.Iva:F2}€");
                                AnsiConsole.MarkupLine($"[bold]TOTAL: {purchase.Total:F2}€[/]");

                                if (AnsiConsole.Confirm("Confirmar compra"))
                                {
                                    foreach (var item in CartService.GetCart())
                                        item.phone.Stock -= item.quantity;

                                    PhoneService.SavePhonesToFile();
                                    FileService.SavePurchase(purchase);
                                    historialCompras.Add(purchase);

                                    CartService.ClearCart();
                                    AnsiConsole.MarkupLine("[green]✅ Compra guardada correctamente[/]");
                                }
                            }

                            AnsiConsole.MarkupLine("\n[grey]Pulsa una tecla para continuar...[/]");
                            Console.ReadKey();
                            break;

                        case "Mis compras":
                            ConsoleHelper.SafeClear();
                            AnsiConsole.MarkupLine("[bold]🧾 MIS COMPRAS[/]\n");

                            // Zona privada: cada cliente solo ve su propia información asociada
                            var misCompras = historialCompras
                                .Where(p => p.Customer.Email == clienteLogueado.Email)
                                .ToList();

                            if (!misCompras.Any())
                            {
                                AnsiConsole.MarkupLine("[grey]Todavía no has realizado ninguna compra[/]");
                            }
                            else
                            {
                                var tablaCompras = new Table();
                                tablaCompras.AddColumn("Fecha");
                                tablaCompras.AddColumn("Artículos");
                                tablaCompras.AddColumn("Total");
                                tablaCompras.AddColumn("Estado");

                                foreach (var p in misCompras)
                                    tablaCompras.AddRow(
                                        p.Date.ToString("dd/MM/yyyy HH:mm"),
                                        p.Items.Count.ToString(),
                                        $"{p.Total:F2}€",
                                        p.Status);

                                AnsiConsole.Write(tablaCompras);
                            }

                            AnsiConsole.MarkupLine("\n[grey]Pulsa una tecla para continuar...[/]");
                            Console.ReadKey();
                            break;

                        case "Añadir nuevo artículo al catálogo":
                            ConsoleHelper.SafeClear();
                            AnsiConsole.MarkupLine("[bold]➕ AÑADIR TELÉFONO AL CATÁLOGO[/]\n");

                            string newBrand = AnsiConsole.Ask<string>("Marca:");
                            string newModel = AnsiConsole.Ask<string>("Modelo:");
                            decimal newPrice = AnsiConsole.Ask<decimal>("Precio:");
                            int newStock = AnsiConsole.Ask<int>("Stock:");

                            PhoneService.AddPhone(newBrand, newModel, newPrice, newStock);
                            AnsiConsole.MarkupLine("[green]✅ Teléfono añadido correctamente[/]");
                            AnsiConsole.MarkupLine("\n[grey]Pulsa una tecla para continuar...[/]");
                            Console.ReadKey();
                            break;

                        case "Cerrar sesión":
                            clienteLogueado = null;
                            CartService.ClearCart();
                            AnsiConsole.MarkupLine("[grey]👋 Sesión cerrada[/]");
                            Console.ReadKey();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ ERROR: {Markup.Escape(ex.Message)}[/]");
                AnsiConsole.MarkupLine("\n[grey]Pulsa una tecla para continuar...[/]");
                Console.ReadKey();
            }
        }
    }

    /// Muestra una lista de teléfonos como tabla con Spectre.Console.
    static void MostrarTablaTelefonos(List<Phone> phones)
    {
        var tabla = new Table();
        tabla.AddColumn("ID");
        tabla.AddColumn("Marca");
        tabla.AddColumn("Modelo");
        tabla.AddColumn("Precio");
        tabla.AddColumn("Stock");

        foreach (var phone in phones)
            tabla.AddRow(
                phone.Id.ToString(),
                phone.Brand,
                phone.Model,
                $"{phone.Price:F2}€",
                phone.Stock.ToString());

        AnsiConsole.Write(tabla);
    }
}
