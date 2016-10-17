using System;
using System.Drawing;

namespace OpenPixelControl
{
    public class Pixel
    {
        public Pixel(int red, int green, int blue)
        {
            Red = (byte) red;
            Green = (byte) green;
            Blue = (byte) blue;
        }

        public Pixel(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public Pixel(Color color)
        {
            Red = color.R;
            Green = color.G;
            Blue = color.B;
        }

        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public static Pixel RedPixel()
        {
            return new Pixel(255, 0, 0);
        }

        public static Pixel GreenPixel()
        {
            return new Pixel(0, 255, 0);
        }

        public static Pixel BluePixel()
        {
            return new Pixel(0, 0, 255);
        }

        public static Pixel YellowPixel()
        {
            return new Pixel(255, 168, 0);
        }

        /// <summary>
        ///     Creates new rgb pixel from HSV values
        /// </summary>
        /// <param name="hue">hue from 0-360</param>
        /// <param name="saturation">saturation from 0-1</param>
        /// <param name="value">value from 0-1</param>
        /// <returns></returns>
        public static Pixel PixelFromHsv(double hue, double saturation = 1, double value = 1)
        {
            var color = ColorFromHSV(hue, saturation, value);
            var pixel = new Pixel(color.R, color.G, color.B);
            return pixel;
        }

        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = max == 0 ? 0 : 1d - 1d*min/max;
            value = max/255d;
        }

        //The ranges are 0 - 360 for hue, and 0 - 1 for saturation and value
        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            var hi = Convert.ToInt32(Math.Floor(hue/60))%6;
            var f = hue/60 - Math.Floor(hue/60);

            value = value*255;
            var v = Convert.ToInt32(value);
            var p = Convert.ToInt32(value*(1 - saturation));
            var q = Convert.ToInt32(value*(1 - f*saturation));
            var t = Convert.ToInt32(value*(1 - (1 - f)*saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            return Color.FromArgb(255, v, p, q);
        }

    }
}