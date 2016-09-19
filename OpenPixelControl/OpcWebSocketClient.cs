using WebSocket4Net;

namespace OpenPixelControl
{
    public class OpcWebSocketClient
    {
        private WebSocket webSocket;

        public OpcWebSocketClient(string server = "127.0.0.1", int port = 7890)
        {
            Server = server;
            Port = port;
        }

        public string Server { get; set; }
        public int Port { get; set; }
    }
}