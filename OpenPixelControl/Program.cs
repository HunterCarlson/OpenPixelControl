using System;
using System.Collections.Generic;
using System.Threading;

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

                //var frame = new List<Pixel>();
                //pixels.Add(new Pixel(255, 0, 0));
                //pixels.Add(new Pixel(0, 255, 0));
                //pixels.Add(new Pixel(0, 0, 255));
                //pixels.Add(new Pixel(255, 0, 0));
                //pixels.Add(new Pixel(0, 255, 0));
                //pixels.Add(new Pixel(0, 0, 255));

                var frame = LetterWall.CreateLetterFrame('A');

                opcClient.WritePixels(frame);

                Thread.Sleep(1000);
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