using System;

namespace ConversorDeImagenes
{
    public static class WriteToConsole
    {
        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\t{message}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void WritePercentage(double percentage)
        {
            if(percentage >= 100)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
            Console.Write($"\r\tCompletado: {percentage}%");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void Pause()
        {
            Console.WriteLine("\tPresione una tecla para continuar . . .");
            Console.ReadKey(true);
        }

        public static void copyright()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"\t\t\t<< Compresor de imágenes >>");
            Console.WriteLine($"\t\t\t© Ricardo José Guevara Aragón");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n");
        }

        public static void WriteClarification(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\t{message}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void WriteLog(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\t{message}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void WriteMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\t{message}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void WriteWarn(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\t{message}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}