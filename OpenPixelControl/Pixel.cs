namespace OpenPixelControl
{
    public class Pixel
    {
        public Pixel(int red, int blue, int green)
        {
            Red = (byte)red;
            Blue = (byte)blue;
            Green = (byte)green;
        }

        public Pixel(byte red, byte blue, byte green)
        {
            Red = red;
            Blue = blue;
            Green = green;
        }

        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
    }
}