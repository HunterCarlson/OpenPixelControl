using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace OpenPixelControl
{
    public class OpcClient
    {
        public OpcClient(string server = "127.0.0.1", int port = OpcConstants.DefaultPort)
        {
            Server = server;
            Port = port;
        }

        public string Server { get; set; }
        public int Port { get; set; }

        public void WriteFrame(List<Pixel> pixels, int channel = 0)
        {
            var lenHighByte = pixels.Count*3/256;
            var lenLowByte = pixels.Count*3%256;

            var message = new List<byte>();

            // build header
            message.Add((byte) channel);
            message.Add(0x00);
            message.Add((byte) lenHighByte);
            message.Add((byte) lenLowByte);

            // add pixel data
            foreach (var pixel in pixels)
            {
                // bad string of LEDs or is the data swapped???
                // shoud be R G B not G R B
                message.Add(pixel.Green);
                message.Add(pixel.Red);
                message.Add(pixel.Blue);
            }

            // send pixel data
            SendMessage(message.ToArray());
        }

        public async Task WriteFrame(Frame frame)
        {
            WriteFrame(frame.Pixels);
            await Task.Delay(frame.Delay);
        }

        public async Task PlayAnimation(FrameAnimation frameAnimation)
        {
            foreach (var frame in frameAnimation.Frames)
            {
                await WriteFrame(frame);
            }
        }

        public void TurnOffAllPixels()
        {
            var frame = SingleColorFrame(OpcConstants.DarkPixel);
            // write twice to bypass interpolation
            WriteFrame(frame);
            WriteFrame(frame);
        }

        public List<Pixel> SingleColorFrame(int red, int green, int blue)
        {
            var frame =
                Enumerable.Range(0, OpcConstants.FadeCandy.MaxPixels).Select(i => new Pixel(red, green, blue)).ToList();
            return frame;
        }

        public List<Pixel> SingleColorFrame(Color color)
        {
            var frame = SingleColorFrame(color.R, color.G, color.B);
            return frame;
        }

        public List<Pixel> SingleColorFrame(Pixel pixel)
        {
            var frame = Enumerable.Range(0, OpcConstants.FadeCandy.MaxPixels).Select(i => pixel).ToList();
            return frame;
        }


        public void SetStatusLed(bool ledStatus)
        {
            var message = new List<byte>();

            // build header
            BuildFirmwareConfigHeader(message);

            byte configByte;
            if (ledStatus)
                configByte = Convert.ToByte("00001100", 2);
            else
                configByte = Convert.ToByte("00000000", 2);

            message.Add(configByte);

            //send data
            SendMessage(message.ToArray());
        }

        public void SetDitheringAndInterpolation(bool setMode)
        {
            var message = new List<byte>();

            // build header
            BuildFirmwareConfigHeader(message);

            // config data to defaults
            var configByte = Convert.ToByte("000000", 2);

            if (!setMode)
            {
                // config data to disable dithering and keyframe interpolation
                configByte = Convert.ToByte("00000011", 2);
            }

            message.Add(configByte);

            //send data
            SendMessage(message.ToArray());
        }

        private static void BuildFirmwareConfigHeader(List<byte> message)
        {
            // channel
            message.Add(0x00);
            // command
            message.Add(0xFF);
            // data length
            message.Add(0x00);
            message.Add(1 + 4);
            // system ID
            message.Add(0x00);
            message.Add(0x01);
            // sysex ID
            message.Add(0x00);
            message.Add(0x02);
        }

        private void SendMessage(byte[] data)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                var tcpClient = new TcpClient(Server, Port) {NoDelay = true};

                // Get a client stream for reading and writing.
                var stream = tcpClient.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Channel: {0} \tSent: {1} bytes", data[0], data.Length);

                //OpenPixelControl doesn't have a response over tcp???
                // Receive the TcpServer.response.

                //// Buffer to store the response bytes.
                //data = new byte[256];

                //// String to store the response ASCII representation.
                //var responseData = string.Empty;

                //// Read the first batch of the TcpServer response bytes.
                //var bytes = stream.Read(data, 0, data.Length);
                //responseData = Encoding.ASCII.GetString(data, 0, bytes);
                //Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                tcpClient.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public async void BlinkRandomThenBright()
        {
            SetDitheringAndInterpolation(true);

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
                await Task.Delay(150);
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
                WriteFrame(pixels.ToList());
                fadeCount++;
            }
            await Task.Delay(500);
            WriteFrame(basePixels.ToList());
            await Task.Delay(1000);
            WriteFrame(allMax.ToList());
            await Task.Delay(1000);
            WriteFrame(SingleColorFrame(OpcConstants.DarkPixel));
            await Task.Delay(200);
            WriteFrame(SingleColorFrame(OpcConstants.DarkPixel));
        }

        public static async void RgbyColorTest(OpcClient opcClient)
        {
            opcClient.SetDitheringAndInterpolation(false);

            var frame = new List<Pixel>();
            frame.Add(Pixel.RedPixel());
            frame.Add(Pixel.YellowPixel());
            frame.Add(Pixel.GreenPixel());
            frame.Add(Pixel.BluePixel());
            opcClient.WriteFrame(frame);

            await Task.Delay(1000);
        }

        public static async void SingleLedChase(OpcClient opcClient, int frameDelay)
        {
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
                await Task.Delay(frameDelay);
            }
        }

        public static async void RainbowCycle(OpcClient opcClient, int frameDelay)
        {
            //init frame
            opcClient.SetDitheringAndInterpolation(true);
            var pixels = new Queue<Pixel>();
            double hue = 0;
            for (var i = 0; i < OpcConstants.StrandLength; i++)
            {
                var pixel = Pixel.PixelFromHsv(hue);
                pixels.Enqueue(pixel);
                hue += 360.0/OpcConstants.StrandLength;
            }
            //loop
            while (true)
            {
                var temp = pixels.Dequeue();
                pixels.Enqueue(temp);
                opcClient.WriteFrame(pixels.ToList());
                await Task.Delay(frameDelay);
            }
        }
    }
}