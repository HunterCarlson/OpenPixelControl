using System;
using Newtonsoft.Json;
using SuperSocket.ClientEngine;
using WebSocket4Net;

namespace OpenPixelControl
{
    public class OpcWebSocketClient
    {
        private readonly string _socketAddress;
        private readonly WebSocket _webSocket;

        public OpcWebSocketClient(string server = "127.0.0.1", int port = 7890)
        {
            _socketAddress = $"ws://{server}:{port}/";
            _webSocket = new WebSocket(_socketAddress);

            _webSocket.Opened += websocket_Opened;
            _webSocket.Error += websocket_Error;
            _webSocket.Closed += websocket_Closed;
            _webSocket.MessageReceived += websocket_MessageReceived;

            _webSocket.Open();
        }

        public void ListConnectedDevices()
        {
            var json = "{ \"type\": \"list_connected_devices\" }";
            _webSocket.Send(json);
        }

        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var thing = JsonConvert.DeserializeObject(e.Message);

            Console.WriteLine("Message recieved:");
            Console.WriteLine(e.Message);
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("WebSocket Closed on {0}", _socketAddress);
        }

        private void websocket_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("WebSocket Error: {0}", e.Exception);
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("WebSocket Opened on {0}", _socketAddress);
        }
    }
}