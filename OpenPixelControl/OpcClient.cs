using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace OpenPixelControl
{
    public class OpcClient
    {
        public OpcClient(string server = "127.0.0.1", int port = 7890)
        {
            Server = server;
            Port = port;
        }

        public string Server { get; set; }
        public int Port { get; set; }

        public void WritePixels(List<Pixel> pixels, int channel = 0)
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
                message.Add(pixel.Red);
                message.Add(pixel.Green);
                message.Add(pixel.Blue);
            }

            // send pixel data
            SendMessage(message.ToArray());
        }

        public void SetStatusLed(bool ledStatus)
        {
            var message = new List<byte>();

            // build header
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

            byte configByte;
            if (ledStatus)
            {
                // config data to turn on led
                configByte = Convert.ToByte("00001100", 2);
            }
            else
            {
                // config data to turn off led and set back to show usb activity
                configByte = Convert.ToByte("00000000", 2);
            }

            message.Add(configByte);

            //send data
            SendMessage(message.ToArray());
        }

        public void SendMessage(byte[] data)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                var tcpClient = new TcpClient(Server, Port);

                // Get a client stream for reading and writing.
                var stream = tcpClient.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Channel: {0} \tSent: {1} pixels", data[0], (data.Length - 4)/3);

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


    }
}