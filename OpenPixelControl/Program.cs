using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPixelControl
{
    class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                OpcClient opcClient = new OpcClient();

                //var pixels = new List<OpcClient.Pixel>();
                //pixels.Add(new OpcClient.Pixel(255, 0, 0));
                //pixels.Add(new OpcClient.Pixel(0, 255, 0));
                //pixels.Add(new OpcClient.Pixel(0, 0, 255));

                //opcClient.WritePixels(pixels);

                opcClient.SetStatusLed(false);

                var breakpoint = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }

    }
}
