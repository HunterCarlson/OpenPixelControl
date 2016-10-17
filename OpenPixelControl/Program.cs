using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

                BlinkRandomThenBright(opcClient);
                //RgbyColorTest(opcClient);

                //opcClient.TurnOffAllPixels();


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

        public static void BlinkRandomThenBright(OpcClient opcClient)
        {
            opcClient.SetDitheringAndInterpolation(true);

            Pixel[] lightColors =
            {
                Pixel.RedPixel(), Pixel.YellowPixel(), Pixel.GreenPixel(), Pixel.BluePixel()
            };

            var rng = new Random();

            var basePixels = new Pixel[50];
            for (var i = 0; i < basePixels.Length; i++)
            {
                var pixel = lightColors[rng.Next(4)];
                basePixels[i] = pixel;
            }
            var allMax = new Pixel[50];
            for (var i = 0; i < allMax.Length; i++)
            {
                var baseBright = 128;
                var red = baseBright;
                var green = baseBright;
                var blue = baseBright;
                if (basePixels[i].Red > red)
                    red = basePixels[i].Red;
                if (basePixels[i].Green > green)
                    green = basePixels[i].Green;
                if (basePixels[i].Blue > blue)
                    blue = basePixels[i].Blue;
                allMax[i] = new Pixel(red, green, blue);
            }
            var fadeCount = 0;
            while (fadeCount < 20)
            {
                Thread.Sleep(150);
                var pixels = new Pixel[50];
                var fade = rng.NextDouble();
                for (var i = 0; i < pixels.Length; i++)
                {
                    var basePixel = basePixels[i];
                    var brightAdjustedPixel = new Pixel(
                        (byte) (basePixel.Red*fade),
                        (byte) (basePixel.Green*fade),
                        (byte) (basePixel.Blue*fade)
                    );
                    pixels[i] = brightAdjustedPixel;
                }
                opcClient.WriteFrame(pixels.ToList());
                fadeCount++;
            }

            Thread.Sleep(500);
            opcClient.WriteFrame(basePixels.ToList());

            Thread.Sleep(1000);
            opcClient.WriteFrame(allMax.ToList());

            Thread.Sleep(1000);
            opcClient.WriteFrame(opcClient.SingleColorFrame(OpcConstants.DarkPixel));
            Thread.Sleep(200);
            opcClient.WriteFrame(opcClient.SingleColorFrame(OpcConstants.DarkPixel));
        }

        public static void RgbyColorTest(OpcClient opcClient)
        {
            opcClient.SetDitheringAndInterpolation(false);

            var frame = new List<Pixel>();
            frame.Add(Pixel.RedPixel());
            frame.Add(Pixel.YellowPixel());
            frame.Add(Pixel.GreenPixel());
            frame.Add(Pixel.BluePixel());
            opcClient.WriteFrame(frame);

            Thread.Sleep(1000);
        }
    }
}