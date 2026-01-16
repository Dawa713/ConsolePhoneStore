namespace ConsolePhoneStore.Utils
{
    public static class Menu
    {
        // MENÚ CUANDO NO HAY USUARIO LOGUEADO
        public static int MostrarMenuPublico()
        {
            Console.Clear();
            Console.WriteLine("=== CONSOLE PHONE STORE ===");
            Console.WriteLine("1. Ver catálogo");
            Console.WriteLine("2. Registrarse");
            Console.WriteLine("3. Iniciar sesión");
            Console.WriteLine("0. Salir");
            Console.Write("Opción: ");

            return LeerOpcion();
        }

        // MENÚ CUANDO HAY USUARIO LOGUEADO
        public static int MostrarMenuPrivado(string nombreUsuario, bool esAdmin = false)
        {
            Console.Clear();
            Console.WriteLine($"=== BIENVENIDO {nombreUsuario.ToUpper()} ===");
            if (esAdmin)
                Console.WriteLine("👑 (ADMINISTRADOR)\n");
            else
                Console.WriteLine();
            
            Console.WriteLine("1. Ver catálogo");
            Console.WriteLine("2. Añadir producto al carrito");
            Console.WriteLine("3. Ver carrito");
            Console.WriteLine("4. Quitar producto del carrito");
            Console.WriteLine("5. Finalizar compra");
            
            if (esAdmin)
                Console.WriteLine("6. Añadir nuevo artículo al catálogo (ADMIN)");
            
            Console.WriteLine("0. Cerrar sesión");
            Console.Write("Opción: ");

            return LeerOpcion();
        }

        // MENÚ CATÁLOGO
        public static int MostrarMenuCatalogo()
        {
            Console.Clear();
            Console.WriteLine("📱 CATÁLOGO DE TELÉFONOS");
            Console.WriteLine("1. Listar todos");
            Console.WriteLine("2. Buscar por marca");
            Console.WriteLine("0. Volver");
            Console.Write("Opción: ");

            return LeerOpcion();
        }

        private static int LeerOpcion()
        {
            if (int.TryParse(Console.ReadLine(), out int opcion))
                return opcion;

            return -1;
        }
    }
}
