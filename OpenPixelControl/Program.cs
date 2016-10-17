using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace OpenPixelControl
{
    internal static class Procgram
    {
        public static int Main(string[] args)
        {
            try
            {
                var opcClient = new OpcClient();

                opcClient.SetDitheringAndInterpolation(false);

                //color test
                //var pixels = new List<Pixel>();
                //pixels.Add(Pixel.RedPixel());
                //pixels.Add(Pixel.YellowPixel());
                //pixels.Add(Pixel.GreenPixel());
                //pixels.Add(Pixel.BluePixel());
                //opcClient.WriteFrame(pixels);
                //Thread.Sleep(5000);
                //opcClient.TurnOffAllPixels();


                // single led chase
                //init frame
                opcClient.SetDitheringAndInterpolation(true);
                var pixels = new Queue<Pixel>();
                pixels.Enqueue(new Pixel(100, 100, 100));
                pixels.Enqueue(new Pixel(180, 180, 180));
                pixels.Enqueue(new Pixel(255, 255, 255));
                pixels.Enqueue(new Pixel(180, 160, 180));
                pixels.Enqueue(new Pixel(100, 100, 100));
                //pad with dark
                while (pixels.Count < 50)
                {
                    pixels.Enqueue(OpcConstants.DarkPixel);
                }
                //loop
                while (true)
                {
                    var temp = pixels.Dequeue();
                    pixels.Enqueue(temp);
                    opcClient.WriteFrame(pixels.ToList());
                    Thread.Sleep(200);
                }


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