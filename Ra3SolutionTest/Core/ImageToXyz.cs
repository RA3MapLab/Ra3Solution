using System.Drawing;
using System.IO;

namespace NewMapParser.Core
{
    public class ImageToXyz
    {
        public static void Run()
        {
            foreach (var image in Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Capture"), "*.png", SearchOption.AllDirectories))
            {
                using (var sw = new StreamWriter(File.OpenWrite(Path.Combine(Directory.GetCurrentDirectory(), "Capture2", Path.GetFileNameWithoutExtension(image) + ".xyz"))))
                {
                    using (var bitmap = new Bitmap(image))
                    {
                        for (int i  = 0; i  < bitmap.Width; i ++)
                        {
                            for (int j = 0; j < bitmap.Height; j++)
                            {
                                sw.WriteLine($"{i},{j},{(bitmap.GetPixel(i,j).R == 255 ? 1 : 0)}");
                            }
                        }
                    }
                }
            }
        }
    }
}