using System;
using System.Collections.Generic;

namespace OpenPixelControl
{
    public static class MessageGenerator
    {
        public static List<byte> Generate(List<Pixel> pixels, int channel = 0, PixelOrder pixelOrder = PixelOrder.GRB)
        {
            var message = new List<byte>();

            BuildHeader(pixels, message, channel);
            BuildPixels(pixels, message, pixelOrder);

            return message;
        }

        static void BuildHeader(List<Pixel> pixels, List<byte> message, int channel)
        {
            var lenHighByte = pixels.Count * 3 / 256;
            var lenLowByte = pixels.Count * 3 % 256;

            message.Add((byte)channel);
            message.Add(0x00);
            message.Add((byte)lenHighByte);
            message.Add((byte)lenLowByte);
        }

        static void BuildPixels(List<Pixel> pixels, List<byte> message, PixelOrder pixelOrder)
        {
            switch (pixelOrder)
            {
                case PixelOrder.GRB:
                    foreach (var pixel in pixels)
                    {
                        message.Add(pixel.Green);
                        message.Add(pixel.Red);
                        message.Add(pixel.Blue);
                    }

                    break;

                case PixelOrder.RGB:
                    foreach (var pixel in pixels)
                    {
                        message.Add(pixel.Red);
                        message.Add(pixel.Green);
                        message.Add(pixel.Blue);
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(pixelOrder), pixelOrder, null);
            }
        }
    }
}
