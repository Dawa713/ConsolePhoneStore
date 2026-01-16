using ConsolePhoneStore.Services;
using ConsolePhoneStore.Models;
using ConsolePhoneStore.Utils;

Customer? clienteLogueado = null;
bool salir = false;

void MostrarCatalogo()
{
    ConsoleHelper.SafeClear();
    foreach (var phone in PhoneService.GetAll())
    {
        Console.WriteLine(
            $"{phone.Id}. {phone.Brand} {phone.Model} - {phone.Price:F2}€"
        );
    }
    Console.WriteLine("\nPulsa cualquier tecla para continuar...");
    Console.ReadKey();
}

void BuscarPorMarca()
{
    Console.Write("Marca a buscar: ");
    string brand = Console.ReadLine() ?? "";

    var results = PhoneService.SearchByBrand(brand);

    foreach (var phone in results)
    {
        Console.WriteLine(
            $"{phone.Brand} {phone.Model} - {phone.Price:F2}€"
        );
    }
    Console.WriteLine("\nPulsa cualquier tecla para continuar...");
    Console.ReadKey();
}

int LeerEnteroSeguro(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        if (int.TryParse(Console.ReadLine(), out int resultado))
        {
            return resultado;
        }
        Console.WriteLine("❌ Entrada inválida. Por favor, ingrese un número válido.");
    }
}

while (!salir)
{
    int opcion = clienteLogueado == null
        ? Menu.MostrarMenuPublico()
        : Menu.MostrarMenuPrivado(clienteLogueado.Name, clienteLogueado.Role == "ADMIN");

    try
    {
        // Si el usuario NO está logueado
        if (clienteLogueado == null)
        {
            switch (opcion)
            {
                // ================= CATÁLOGO =================
                case 1:
                    bool volver = false;

                    while (!volver)
                    {
                        int opcionCatalogo = Menu.MostrarMenuCatalogo();

                        switch (opcionCatalogo)
                        {
                            case 1:
                                MostrarCatalogo();
                                break;

                            case 2:
                                BuscarPorMarca();
                                break;

                            case 0:
                                volver = true;
                                break;
                        }
                    }
                    break;

                // ================= REGISTRO =================
                case 2:
                    ConsoleHelper.SafeClear();
                    Console.WriteLine("📝 REGISTRO DE CLIENTE\n");

                    string nombre = InputValidator.ReadNonEmptyString(
                        "Nombre (máx 10 caracteres): ", 10);

                    string email = InputValidator.ReadValidEmail("Email: ");

                    string password = InputValidator.ReadPassword(
                        "Contraseña (6-10 caracteres): ", 6, 10);

                    CustomerService.Register(nombre, email, password);

                    Console.WriteLine("\n✅ Registro completado correctamente");
                    Console.ReadKey();
                    break;

                // ================= LOGIN =================
                case 3:
                    bool loginExitoso = false;
                    int intentos = 0;
                    int maxIntentos = 3;

                    while (!loginExitoso && intentos < maxIntentos)
                    {
                        ConsoleHelper.SafeClear();
                        Console.WriteLine("🔐 INICIAR SESIÓN\n");

                        string nameLogin = InputValidator.ReadNonEmptyString("Nombre de usuario: ", 50);
                        string passLogin = InputValidator.ReadPasswordLogin("Contraseña: ");

                        Customer? usuarioLogueado = CustomerService.Login(nameLogin, passLogin);

                        if (usuarioLogueado != null)
                        {
                            clienteLogueado = usuarioLogueado;
                            Console.WriteLine($"\n✔️ Bienvenido {clienteLogueado.Name}");
                            loginExitoso = true;
                            Console.ReadKey();
                        }
                        else
                        {
                            intentos++;
                            if (intentos >= maxIntentos)
                            {
                                Console.WriteLine($"\n❌ Usuario y/o contraseña incorrectos");
                                Console.WriteLine($"❌ Ha excedido el número de intentos ({maxIntentos}). Volviendo al menú principal...");
                                Console.ReadKey();
                                break;
                            }
                            else
                            {
                                Console.WriteLine($"\n❌ Usuario y/o contraseña incorrectos. Por favor introduzca datos correctos");
                                Console.WriteLine($"Intento {intentos}/{maxIntentos}");
                                Console.ReadKey();
                            }
                        }
                    }
                    break;

                // ================= SALIR =================
                case 0:
                    salir = true;
                    break;

                default:
                    Console.WriteLine("Opción inválida");
                    Console.ReadKey();
                    break;
            }
        }
        // Si el usuario SÍ está logueado
        else
        {
            switch (opcion)
            {
                // ================= CATÁLOGO =================
                case 1:
                    bool volver = false;

                    while (!volver)
                    {
                        int opcionCatalogo = Menu.MostrarMenuCatalogo();

                        switch (opcionCatalogo)
                        {
                            case 1:
                                MostrarCatalogo();
                                break;

                            case 2:
                                BuscarPorMarca();
                                break;

                            case 0:
                                volver = true;
                                break;
                        }
                    }
                    break;

                // ================= AÑADIR AL CARRITO =================
                case 2:
                    ConsoleHelper.SafeClear();

                    var phones = PhoneService.GetAll();

                    foreach (var phone in phones)
                    {
                        Console.WriteLine(
                            $"{phone.Id}. {phone.Brand} {phone.Model} - {phone.Price:F2}€ (Stock {phone.Stock})"
                        );
                    }

                    int id = LeerEnteroSeguro("\nID del teléfono: ");

                    var selectedPhone = PhoneService.GetById(id);

                    if (selectedPhone == null)
                        throw new Exception("Teléfono no encontrado");

                    int quantity = 0;
                    bool cantidadValida = false;

                    while (!cantidadValida)
                    {
                        quantity = LeerEnteroSeguro("Cantidad: ");

                        if (quantity > selectedPhone.Stock)
                        {
                            Console.WriteLine($"❌ No tenemos tanta cantidad en stock. Stock disponible: {selectedPhone.Stock}. Escriba otra cantidad");
                        }
                        else
                        {
                            cantidadValida = true;
                        }
                    }

                    CartService.AddToCart(selectedPhone, quantity);
                    Console.WriteLine("✔️ Producto añadido al carrito");
                    Console.ReadKey();
                    break;

                // ================= VER CARRITO =================
                case 3:
                    ConsoleHelper.SafeClear();
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
                        Console.WriteLine(
                            $"{item.phone.Id}. {item.phone.Brand} {item.phone.Model} x{item.quantity}"
                        );
                    }

                    Console.WriteLine($"\nSubtotal: {CartService.CalculateSubtotal():F2}€");
                    Console.ReadKey();
                    break;

                // ================= QUITAR DEL CARRITO =================
                case 4:
                    ConsoleHelper.SafeClear();

                    var cartRemove = CartService.GetCart();

                    if (!cartRemove.Any())
                    {
                        Console.WriteLine("Carrito vacío");
                        Console.ReadKey();
                        break;
                    }

                    Console.WriteLine("🛒 CARRITO ACTUAL\n");

                    foreach (var item in cartRemove)
                    {
                        Console.WriteLine(
                            $"{item.phone.Id}. {item.phone.Brand} {item.phone.Model} x{item.quantity}"
                        );
                    }

                    int removeId = LeerEnteroSeguro("\nID del producto a quitar: ");
                    int removeQty = LeerEnteroSeguro("Cantidad a quitar: ");

                    CartService.RemoveFromCart(removeId, removeQty);

                    Console.WriteLine("✔️ Producto actualizado en el carrito");
                    Console.ReadKey();
                    break;

                // ================= FINALIZAR COMPRA =================
                case 5:
                    ConsoleHelper.SafeClear();

                    var subtotal = CartService.CalculateSubtotal();
                    var iva = subtotal * 0.21m;
                    var total = subtotal + iva;

                    Console.WriteLine($"Subtotal: {subtotal:F2}€");
                    Console.WriteLine($"IVA (21%): {iva:F2}€");
                    Console.WriteLine($"TOTAL: {total:F2}€");

                    Console.Write("\nConfirmar compra (s/n): ");
                    if (Console.ReadLine()?.ToLower() == "s")
                    {
                        foreach (var item in CartService.GetCart())
                        {
                            item.phone.Stock -= item.quantity;
                        }

                        FileService.SavePurchase(
                            clienteLogueado!.Email,
                            CartService.GetCart(),
                            total
                        );

                        CartService.ClearCart();
                        Console.WriteLine("✅ Compra guardada correctamente");
                    }

                    Console.ReadKey();
                    break;

                // ================= AÑADIR NUEVO ARTÍCULO (ADMIN) =================
                case 6:
                    if (clienteLogueado?.Role != "ADMIN")
                        throw new Exception("Solo administradores pueden añadir artículos");

                    ConsoleHelper.SafeClear();
                    Console.WriteLine("➕ AÑADIR NUEVO ARTÍCULO AL CATÁLOGO\n");

                    string newBrand = InputValidator.ReadNonEmptyString("Marca: ", 50);
                    string newModel = InputValidator.ReadNonEmptyString("Modelo: ", 50);

                    // Validar que no exista un producto con la misma marca y modelo
                    var existingPhone = PhoneService.GetAll().FirstOrDefault(p =>
                        p.Brand.ToLower() == newBrand.ToLower() &&
                        p.Model.ToLower() == newModel.ToLower());

                    if (existingPhone != null)
                        throw new Exception($"El producto {newBrand} {newModel} ya existe en el catálogo");

                    // Validar precio
                    decimal newPrice = 0;
                    bool precioValido = false;
                    while (!precioValido)
                    {
                        Console.Write("Precio (debe ser mayor a 0): ");
                        if (decimal.TryParse(Console.ReadLine(), out newPrice) && newPrice > 0)
                        {
                            precioValido = true;
                        }
                        else
                        {
                            Console.WriteLine("❌ Precio inválido. Debe ser un número mayor a 0");
                        }
                    }

                    // Validar stock
                    int newStock = 0;
                    bool stockValido = false;
                    while (!stockValido)
                    {
                        Console.Write("Stock (debe ser 0 o mayor): ");
                        if (int.TryParse(Console.ReadLine(), out newStock) && newStock >= 0)
                        {
                            stockValido = true;
                        }
                        else
                        {
                            Console.WriteLine("❌ Stock inválido. Debe ser un número igual o mayor a 0");
                        }
                    }

                    PhoneService.AddPhone(newBrand, newModel, newPrice, newStock);

                    Console.WriteLine("\n✅ Artículo añadido correctamente");
                    Console.ReadKey();
                    break;

                // ================= LOGOUT =================
                case 0:
                    clienteLogueado = null;
                    break;

                default:
                    Console.WriteLine("Opción inválida");
                    Console.ReadKey();
                    break;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ ERROR: {ex.Message}");
        Console.ReadKey();
    }
}
