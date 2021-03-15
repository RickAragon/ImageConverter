using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using ImageMagick;

using static ConversorDeImagenes.WriteToConsole;

namespace ConversorDeImagenes
{
    public class ImageConverter
    {
        private bool resFlag;
        private bool basear64;
        private int[] resPipe;
        private string dirPath;

        public ImageConverter(bool resFlag, int[] resPipe, bool basear64, string outputDir)
        {
            this.resFlag = !resFlag;
            this.basear64 = basear64;
            this.resPipe = resPipe;
            this.dirPath = outputDir;
            Directory.CreateDirectory(outputDir);
        }

        public async Task ConvertImage(FileInfo fileInfo, int totalPaths)
        {
            await Task.Run(()=>{
                string path = fileInfo.FullName;
                double interval = 100d/totalPaths;
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
            });
        }
    }
}