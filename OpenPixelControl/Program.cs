using System;
using System.Collections.Generic;
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

                // turn off dithering and interpolation
                opcClient.SetDitheringAndInterpolation(false);

                var frame = new List<Pixel>();
                frame.Add(Pixel.RedPixel());
                frame.Add(Pixel.YellowPixel());
                frame.Add(Pixel.GreenPixel());
                frame.Add(Pixel.BluePixel());

                //var frame = LetterWall.CreateLetterFrame('A');

                opcClient.WriteFrame(frame);

                //Thread.Sleep(5000);
                //opcClient.TurnOffAllPixels();


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