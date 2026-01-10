namespace ConsolePhoneStore.Utils
{
    public static class Menu
    {
        public static int MostrarMenuPrincipal()
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("=== ConsolePhoneStore ===");
                Console.WriteLine("1. Ver catálogo de teléfonos");
                Console.WriteLine("2. Registrarse");
                Console.WriteLine("3. Iniciar sesión");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("Opción no válida. Pulse una tecla para continuar...");
                    Console.ReadKey();
                    opcion = -1;
                }

            } while (opcion < 0 || opcion > 3);

            return opcion;
        }
    }
}
