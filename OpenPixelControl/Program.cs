using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

                BlinkRandomThenBright(opcClient);
                //RgbyColorTest(opcClient);
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