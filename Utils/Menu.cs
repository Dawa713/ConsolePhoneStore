namespace ConsolePhoneStore.Utils
{
    class Menu
    {
        public static int MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("=== MENU PRINCIPAL ===");
            Console.WriteLine("1. Ver catálogo");
            Console.WriteLine("2. Registrarse");
            Console.WriteLine("3. Iniciar sesión");
            Console.WriteLine("4. Añadir al carrito");
            Console.WriteLine("5. Ver carrito");
            Console.WriteLine("6. Finalizar compra");
            Console.WriteLine("0. Salir");
            
            if (int.TryParse(Console.ReadLine(), out int opcion))
            {
                return opcion;
            }
            return -1;
        }
        public static int MostrarMenuCatalogo()
{
    int opcion;

    do
    {
        Console.Clear();
        Console.WriteLine("📱 CATÁLOGO DE TELÉFONOS");
        Console.WriteLine("1. Listar todos");
        Console.WriteLine("2. Buscar por marca");
        Console.WriteLine("0. Volver");
        Console.Write("Opción: ");

        if (!int.TryParse(Console.ReadLine(), out opcion))
        {
            opcion = -1;
        }

    } while (opcion < 0 || opcion > 2);

    return opcion;
}

    }
}
