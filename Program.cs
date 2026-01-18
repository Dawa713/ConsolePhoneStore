using ConsolePhoneStore.Services;
using ConsolePhoneStore.Models;
using ConsolePhoneStore.Utils;

// ==================== VARIABLES GLOBALES DE LA APLICACIÓN ====================
// Usuario autenticado actualmente (null si no hay sesión activa)
Customer? clienteLogueado = null;
// Flag para controlar si el usuario desea salir de la aplicación
bool salir = false;

// ==================== MÉTODOS PRIVADOS DE LA APLICACIÓN ====================

/// <summary>
/// Muestra el catálogo completo de teléfonos disponibles.
/// Lista todos los productos con su ID, marca, modelo y precio en euros.
/// </summary>
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

/// <summary>
/// Busca teléfonos por marca específica.
/// Realiza búsqueda insensible a mayúsculas usando el servicio de búsqueda.
/// </summary>
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

/// <summary>
/// Lee un número entero del usuario de forma segura.
/// Repite el ciclo hasta que el usuario ingrese un número válido.
/// </summary>
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

// ==================== BUCLE PRINCIPAL DE LA APLICACIÓN ====================

// Bucle principal que mantiene la aplicación en ejecución hasta que el usuario elige salir
while (!salir)
{
    // Mostrar menú diferente según si existe sesión autenticada
    int opcion = clienteLogueado == null
        ? Menu.MostrarMenuPublico()           // Menú para usuarios NO autenticados
        : Menu.MostrarMenuPrivado(clienteLogueado.Name, clienteLogueado.Role == "ADMIN"); // Menú para usuarios autenticados

    try
    {
        // ==================== SECCIÓN: USUARIOS NO AUTENTICADOS ====================
        // Procesar opciones solo si no hay usuario logueado
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
        // ==================== SECCIÓN: USUARIOS AUTENTICADOS ====================
        // Procesar opciones cuando hay usuario autenticado
        else
        {
            switch (opcion)
            {
                // ================= AÑADIR AL CARRITO =================
                case 1:
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

                // ================= VER CARRITO (submenú vaciar/finalizar) =================
                case 2:
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

                    bool salirSubmenu = false;
                    while (!salirSubmenu)
                    {
                        Console.WriteLine("\nOpciones del carrito:");
                        Console.WriteLine("1. Vaciar carrito (doble confirmación)");
                        Console.WriteLine("2. Finalizar compra");
                        Console.WriteLine("0. Volver");
                        int opcionCart = LeerEnteroSeguro("Opción: ");

                        switch (opcionCart)
                        {
                            case 1:
                                // Vaciar con doble confirmación
                                Console.WriteLine("\n⚠️ ¿DESEAS BORRAR TODO el carrito?");
                                Console.WriteLine("0 - No, volver atrás");
                                Console.WriteLine("1 - Sí, borrar TODO");

                                bool confirmOk = false;
                                while (!confirmOk)
                                {
                                    Console.Write("Confirmación (0 o 1): ");
                                    string? confirm = Console.ReadLine();

                                    if (confirm == "0")
                                    {
                                        Console.WriteLine("Operación cancelada. Carrito no modificado.");
                                        confirmOk = true;
                                    }
                                    else if (confirm == "1")
                                    {
                                        CartService.ClearCart();
                                        Console.WriteLine("✔️ Carrito vaciado completamente");
                                        confirmOk = true;
                                        salirSubmenu = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("❌ Opción inválida. Solo puedes escribir 0 o 1.");
                                        Console.WriteLine("   0 = No borrar  |  1 = Borrar todo");
                                    }
                                }
                                break;

                            case 2:
                                // Finalizar compra
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

                                    PhoneService.SavePhonesToFile();

                                    CartService.ClearCart();
                                    Console.WriteLine("✅ Compra guardada correctamente");
                                    salirSubmenu = true;
                                }
                                else
                                {
                                    Console.WriteLine("Operación cancelada");
                                }
                                break;

                            case 0:
                                salirSubmenu = true;
                                break;

                            default:
                                Console.WriteLine("❌ Opción inválida");
                                break;
                        }
                    }

                    Console.ReadKey();
                    break;

                // ================= AÑADIR NUEVO ARTÍCULO (ADMIN) =================
                case 3:
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

                // ================= LOGOUT: CERRAR SESIÓN =================
                case 0:
                    // Limpiar la variable de sesión para volver al menú público
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
