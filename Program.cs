using System;
using System.Collections.Generic;
using System.IO;
using ImageMagick;

using static ConversorDeImagenes.WriteToConsole;

namespace ConversorDeImagenes
{
    class Program
    {
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
            
            Console.WriteLine("\tDesea ingresar una resolucion?"); 
            WriteClarification("* Los formatos de resolucion aceptados son: 100x100, 100:100, 100 100, 100/100");
            WriteClarification("\t[Los números presentados anteriormente son solo ejemplos, \n\t\tse pueden utilizar otras denominaciones]");
            WriteClarification("* La relación de aspecto se mantedrá por lo que la resolución final \n\t\tpuede ser solamente una aproximación a lo ingresado");
            WriteClarification("(deje el campo vacío y presione enter para la resolución original por defecto)");
            Console.Write("\t");

            string resolution = Console.ReadLine();
            bool resFlag = true;
            int[] resPipe = new int[2];
            string[] preResPipe;
            bool basear64 = false;
            Console.WriteLine("\tDesea obtener las imágenes en base64 también? (s/n)");
            Console.Write("\t");
            ConsoleKeyInfo clave = Console.ReadKey();
            Console.Clear();
            copyright();

            if(clave.Key == ConsoleKey.S)
            {
                basear64 = true;
            }

            if(String.IsNullOrEmpty(resolution) || String.IsNullOrWhiteSpace(resolution))
            {
                resFlag = false;
            }
            else
            {
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
            }
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

            string dirPath = $"{Directory.GetCurrentDirectory()}\\ImagenesTransformadas";
            Directory.CreateDirectory(dirPath);
            double interval = 100d/paths.Count;
            double percentage = 0;
            double microInterval = interval / 6;
            string a = "jpeg", b = "png", c = "gif";
            Dictionary<string, string> posiblesTipos = new Dictionary<string, string>()
            {
                {"jpeg", a},
                {"jpg", a},
                {"JPEG", a},
                {"JPG", a},
                {"PNG", b},
                {"png", b},
                {"gif", c},
                {"GIF", c}
            };

            foreach(string path in paths)
            {
                FileInfo fileInfo = new FileInfo(path);
                try
                {
                    using(MagickImage image = new MagickImage(path))
                    {
                        image.Strip();
                        percentage += microInterval;
                        WritePercentage(Math.Round(percentage));
                        image.Interlace = Interlace.Plane;
                        image.Alpha(AlphaOption.Deactivate);
                        image.FilterType = FilterType.Undefined;
                        image.Trim();
                        image.ColorType = ColorType.Optimize;
                        image.ColorSpace = ColorSpace.RGB;
                        percentage += microInterval;
                        WritePercentage(Math.Round(percentage));
                        image.GaussianBlur(0.05, 0.1);
                        percentage += microInterval;
                        WritePercentage(Math.Round(percentage));
                        image.Quality = 85;
                        percentage += microInterval;
                        WritePercentage(Math.Round(percentage));
                        if(resFlag)
                            image.Resize(new MagickGeometry(resPipe[0], resPipe[1]));
                        percentage += microInterval;
                        WritePercentage(Math.Round(percentage));
                        image.Write($"{dirPath}\\{fileInfo.Name}");
                        if(basear64)
                        {
                            string codigo = image.ToBase64();
                            string extension = fileInfo.Extension.Substring(1);
                            if(posiblesTipos.ContainsKey(extension))
                            {
                                codigo = $"data:image/{posiblesTipos[extension]};base64,{codigo}";
                            }
                            else
                            {
                                codigo = $"data:image/{extension};base64,{codigo}";
                            }
                            using(TextWriter tw = new StreamWriter(new FileStream($"{dirPath}\\{fileInfo.Name}.txt", FileMode.Append)))
                            {
                                tw.Write(codigo);
                            }
                        }
                        percentage += microInterval;
                        WritePercentage(Math.Round(percentage));
                    }
                } 
                catch(Exception ex)
                {
                    WriteError($"Error!!! M: {ex}");
                }
            }
            Console.WriteLine();
            WriteMessage("Éxito!");
            Pause();
        }
    }
}
