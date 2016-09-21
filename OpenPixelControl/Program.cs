using System;
using System.Collections.Generic;

namespace OpenPixelControl
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                var opcClient = new OpcClient();
                //OpcWebSocketClient opcWebSocketClient = new OpcWebSocketClient();

                opcClient.DisableDitheringAndInterpolation();

                var pixels = new List<Pixel>();
                pixels.Add(new Pixel(255, 0, 0));
                pixels.Add(new Pixel(0, 255, 0));
                pixels.Add(new Pixel(0, 0, 255));
                pixels.Add(new Pixel(255, 0, 0));
                pixels.Add(new Pixel(0, 255, 0));
                pixels.Add(new Pixel(0, 0, 255));

                opcClient.WritePixels(pixels);

                opcClient.TurnOffAllPixels();


                //opcClient.SetStatusLed(false);

                //wait for socket to open
                //TODO: there is an open event, wait for the socket to open before sending messages
                //Thread.Sleep(1000);
                //opcWebSocketClient.ListConnectedDevices();

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