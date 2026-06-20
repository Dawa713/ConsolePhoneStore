using Spectre.Console;

namespace ConsolePhoneStore.Utils
{
    /// <summary>
    /// Clase de utilidad para mostrar los diferentes menús de la aplicación.
    /// Usa Spectre.Console (SelectionPrompt) en vez de menús numerados:
    /// el usuario navega con flechas y Enter, y nunca puede introducir
    /// una opción inválida porque solo existen las opciones de la lista.
    /// </summary>
    public static class Menu
    {
        /// Muestra el menú principal para usuarios NO logueados.
        public static string MostrarMenuPrincipal()
        {
            ConsoleHelper.SafeClear();
            AnsiConsole.Write(new Rule("[yellow]CONSOLE PHONE STORE[/]").Centered());

            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("¿Qué deseas hacer?")
                    .AddChoices("Ver catálogo", "Registrarse", "Iniciar sesión", "Salir"));
        }

        /// Muestra el menú para usuarios logueados.
        /// Si es administrador, añade la opción de gestionar el catálogo.
        public static string MostrarMenuPrivado(string nombreUsuario, bool esAdmin = false)
        {
            ConsoleHelper.SafeClear();
            string titulo = $"Bienvenido {Markup.Escape(nombreUsuario.ToUpper())}";
            if (esAdmin) titulo += " (ADMIN)";
            AnsiConsole.Write(new Rule($"[green]{titulo}[/]").Centered());

            var opciones = new List<string> { "Añadir producto al carrito", "Ver carrito", "Mis compras" };
            if (esAdmin)
                opciones.Add("Añadir nuevo artículo al catálogo");
            opciones.Add("Cerrar sesión");

            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Elige una opción")
                    .AddChoices(opciones));
        }

        /// Muestra el submenú del catálogo de teléfonos.
        public static string MostrarMenuCatalogo()
        {
            ConsoleHelper.SafeClear();
            AnsiConsole.Write(new Rule("[blue]CATÁLOGO DE TELÉFONOS[/]").Centered());

            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Elige una opción")
                    .AddChoices("Listar todos", "Buscar por marca", "Volver"));
        }
    }
}
