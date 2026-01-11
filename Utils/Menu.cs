namespace ConsolePhoneStore.Utils
{
    class Menu
    {
        public static int MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("=== MENU PRINCIPAL ===");
            Console.WriteLine("1. Catálogo de teléfonos");
            Console.WriteLine("2. Registro de usuario");
            Console.WriteLine("3. Iniciar sesión");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opción: ");
            
            if (int.TryParse(Console.ReadLine(), out int opcion))
            {
                return opcion;
            }
            return -1;
        }
    }
}
