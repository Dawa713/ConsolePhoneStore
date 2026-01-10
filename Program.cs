using ConsolePhoneStore.Utils;

class Program
{
    static void Main()
    {
        int opcion;

        do
        {
            opcion = Menu.MostrarMenuPrincipal();

            switch (opcion)
            {
                case 1:
                    Console.WriteLine("Mostrando catálogo de teléfonos...");
                    Console.ReadKey();
                    break;

                case 2:
                    Console.WriteLine("Registro de cliente (pendiente)...");
                    Console.ReadKey();
                    break;

                case 3:
                    Console.WriteLine("Inicio de sesión (pendiente)...");
                    Console.ReadKey();
                    break;

                case 0:
                    Console.WriteLine("Saliendo de la aplicación...");
                    break;
            }

        } while (opcion != 0);
    }
}
