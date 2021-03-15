//#define VERBOSE
// Uncomment the previous line so that the application asks for resolution through the input (in spanish)
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImageMagick;

using static ConversorDeImagenes.WriteToConsole;


namespace ConversorDeImagenes
{
    class Program
    {
        private const bool RES_FLAG = false;
        private const bool BASEAR64 = false;
        private static readonly string OUTPUT_DIR;
        private static int[] RES_PIPE = new int[]{100 , 100};

        private static double Progreso = 0;

        static Program()
        {
            OUTPUT_DIR = $"{Directory.GetCurrentDirectory()}\\ImagenesTransformadas";
        }

        static void Main(string[] args)
        {
            copyright();
            if(args.Length < 1)
            {
                WriteError("La cantidad minima de argumentos (u objetos arrastrados) es 1.");
                WriteError("** Cerrando programa **");
                Pause();
                return;
            }
            
            #if VERBOSE
                Console.WriteLine("\tDesea ingresar una resolucion?"); 
                WriteClarification("* Los formatos de resolucion aceptados son: 100x100, 100:100, 100 100, 100/100");
                WriteClarification("\t[Los números presentados anteriormente son solo ejemplos, \n\t\tse pueden utilizar otras denominaciones]");
                WriteClarification("* La relación de aspecto se mantedrá por lo que la resolución final \n\t\tpuede ser solamente una aproximación a lo ingresado");
                WriteClarification("(deje el campo vacío y presione enter para la resolución original por defecto)");
                Console.Write("\t");

                string resolution = Console.ReadLine();
                int[] resPipe = new int[2];
                string[] preResPipe;
                Console.Clear();
                copyright();
                if(resolution.Contains("x"))
                {
                    resolution.ToLower();
                    preResPipe = resolution.Split('x');
                }
                else if(resolution.Contains(":"))
                {
                    preResPipe = resolution.Split(':');
                }
                else if(resolution.Contains("/"))
                {
                    preResPipe = resolution.Split('/');
                }
                else
                {
                    preResPipe = resolution.Split(' ');
                }
                
                if(!int.TryParse(preResPipe[0], out resPipe[0]))
                {
                    WriteError("Se ingresó un formato inválido o el primer valor ingresado para la resolución es incorrecto.");
                    Pause();
                    return;
                }
                if(!int.TryParse(preResPipe[1], out resPipe[1]))
                {
                    WriteError("Se ingresó un formato inválido o el segundo valor ingresado para la resolución es incorrecto.");
                }
            #endif
            ImageConverter ic = new ImageConverter(RES_FLAG, RES_PIPE, BASEAR64, OUTPUT_DIR);
            List<string> paths = new List<string>();
            foreach(string x in args)
            {
                if(!File.Exists(x))
                {
                    WriteWarn($"La ruta {x} es incorrecta! Omitiendo...");
                }
                else
                {
                    WriteLog($"La ruta {x} es correcta! Agregando...");
                    paths.Add(x);
                }
            }

            foreach(string path in paths)
            {
                FileInfo fileInfo = new FileInfo(path);
                try
                {
                    ic.ConvertImage(fileInfo, paths.Count, ref Progreso);
                } 
                catch(Exception ex)
                {
                    WriteError($"Error!!! M: {ex}");
                    return;
                }
            }
            Console.WriteLine();
            WriteMessage("Éxito!");
            Pause();
        }
    }
}
