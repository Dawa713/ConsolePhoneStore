namespace ConsolePhoneStore.Utils
{
    /// <summary>
    /// Clase de utilidad para mostrar los diferentes menús de la aplicación.
    /// Proporciona interfaces visuales para usuarios logueados y no logueados.
    /// </summary>
    public static class Menu
    {
 
        /// Muestra el menú principal para usuarios NO logueados.
        /// Opciones: Ver catálogo, Registrarse, Iniciar sesión, Salir.
 
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

 
        /// Muestra el menú principal para usuarios logueados.
        /// Si es administrador, muestra opción adicional para añadir productos.
        /// Opciones: Añadir carrito, Ver carrito (con submenú para vaciar/finalizar), (Admin: Añadir), Logout.
 
        public static int MostrarMenuPrivado(string nombreUsuario, bool esAdmin = false)
        {
            Console.Clear();
            Console.WriteLine($"=== BIENVENIDO {nombreUsuario.ToUpper()} ===");
            if (esAdmin)
                Console.WriteLine("(ADMINISTRADOR)\n");
            else
                Console.WriteLine();
            
            Console.WriteLine("1. Añadir producto al carrito");
            Console.WriteLine("2. Ver carrito ");
            
            if (esAdmin)
                Console.WriteLine("3. Añadir nuevo artículo al catálogo (ADMIN)");
            
            Console.WriteLine("0. Cerrar sesión");
            Console.Write("Opción: ");

            return LeerOpcion();
        }

 
        /// Muestra el submenú del catálogo de teléfonos.
        /// Opciones: Listar todos, Buscar por marca, Volver.
 
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

 
        /// Lee una opción de menú de forma segura.
        /// Devuelve -1 si la entrada no es un número válido.
 
        private static int LeerOpcion()
        {
            if (int.TryParse(Console.ReadLine(), out int opcion))
                return opcion;

            return -1;
        }
    }
}
