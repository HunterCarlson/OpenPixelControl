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

                //var frame = opcClient.SingleColorFrame(Color.DarkBlue);
                //opcClient.WriteFrame(frame);

                //opcClient.BlinkRandomThenBright();

                //opcClient.BlinkRandomThenBright();
                //RgbyColorTest(opcClient);
                //SingleLedChase(opcClient, 100);
                //opcClient.SingleLedChase(100);
                //RainbowCycle(opcClient, 50);
                opcClient.RainbowCycle(100);


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