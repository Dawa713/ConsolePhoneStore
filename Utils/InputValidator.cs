using System.Text.RegularExpressions;

namespace ConsolePhoneStore.Utils
{
    public static class InputValidator
    {
        public static string ReadNonEmptyString(string message, int maxLength)
        {
            string? input;

            do
            {
                Console.Write(message);
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("❌ No puede estar vacío");
                    continue;
                }

                if (input.Length > maxLength)
                {
                    Console.WriteLine($"❌ Máximo {maxLength} caracteres");
                    continue;
                }

                return input;

            } while (true);
        }

        public static string ReadPassword(string message, int min, int max)
        {
            string? input;

            do
            {
                Console.Write(message);
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("❌ La contraseña no puede estar vacía");
                    continue;
                }

                if (input.Length < min || input.Length > max)
                {
                    Console.WriteLine($"❌ Debe tener entre {min} y {max} caracteres");
                    continue;
                }

                return input;

            } while (true);
        }

        public static string ReadValidEmail(string message)
        {
            string? input;
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            do
            {
                Console.Write(message);
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("❌ El email no puede estar vacío");
                    continue;
                }

                if (!regex.IsMatch(input))
                {
                    Console.WriteLine("❌ Formato de email no válido");
                    continue;
                }

                return input;

            } while (true);
        }
    }
}
