using System;

namespace NewMapParser.Core
{
    public class GenFrames
    {
        public static void Run()
        {
            for (int i = 1; i <= 100; i++)
            {
                // Console.WriteLine(
                //     $@"<shape id=""{i + 130}"" top=""0"" left=""0"" bottom=""64"" right=""64"" geometry=""StandardCommandButton_geometry\mask_{i}.ru"">");
                // Console.WriteLine("<ru><![CDATA[");
                // Console.WriteLine("c");
                // Console.WriteLine("s s:255:255:255:255");
                // Console.WriteLine($"t 0.0:46.0:46.0:46.0:0.0:{46f * i / 100f}");
                // Console.WriteLine($"t 46.0:46.0:0.0:{46f * i / 100f}:46.0:{46f * i / 100f}");
                // Console.WriteLine("]]></ru>");
                // Console.WriteLine("</shape>");
                // Console.WriteLine();
                
                Console.WriteLine($@"<frame id=""{i}"">");
                Console.WriteLine($@"<placeobject flags=""Move"" depth=""1"" character=""{i + 130}"">");
                Console.WriteLine($@"</placeobject>");
                Console.WriteLine($@"</frame>");
            }
        }
    }
}