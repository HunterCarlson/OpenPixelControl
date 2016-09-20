using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
                OpcWebSocketClient opcWebSocketClient = new OpcWebSocketClient();

                //var pixels = new List<OpcClient.Pixel>();
                //pixels.Add(new OpcClient.Pixel(255, 0, 0));
                //pixels.Add(new OpcClient.Pixel(0, 255, 0));
                //pixels.Add(new OpcClient.Pixel(0, 0, 255));

                //opcClient.WritePixels(pixels);

                //opcClient.SetStatusLed(false);

                //wait for socket to open
                //TODO: there is an open event, wait for the socket to open before sending messages
                Thread.Sleep(1000);
                opcWebSocketClient.ListConnectedDevices();

                Console.Read();
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
