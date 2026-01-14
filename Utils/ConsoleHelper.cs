namespace ConsolePhoneStore.Utils
{
    public static class ConsoleHelper
    {
        public static void SafeClear()
        {
            try
            {
                if (!Console.IsOutputRedirected)
                {
                    Console.Clear();
                }
            }
            catch
            {
                // Ignorar errores de consola
            }
        }
    }
}
