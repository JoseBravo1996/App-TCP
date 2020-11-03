using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static List<Socket> _clientSockets = new List<Socket>();
        private static byte[] _buffer = new byte[1024];

        static void Main(string[] args)
        {
            Console.Title = "Servidor";
            SetupServer();
            Console.ReadLine();
        }

        private static void SetupServer()
        {
            Console.WriteLine("Configuracion del servidor...");
            IPAddress test1 = IPAddress.Parse("192.168.99.1");
            _serverSocket.Bind(new IPEndPoint(test1, 100));
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }

        private static void AcceptCallBack(IAsyncResult AR)
        {
            Socket socket = _serverSocket.EndAccept(AR);
            _clientSockets.Add(socket);
            Console.WriteLine("Cliente Conectado...");
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }

        private static void ReceiveCallBack(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int received = socket.EndReceive(AR);
            byte[] dataBuf = new byte[received];
            Array.Copy(_buffer, dataBuf, received);

            string text = Encoding.ASCII.GetString(dataBuf);
            Console.WriteLine("Texto Recibido: " + text);

            string response = string.Empty;

            if (text.ToLower() != "get time")
            {
                response = "Request invalido";
            }
            else
            {
                response = DateTime.Now.ToLongTimeString();

            }

            byte[] data = Encoding.ASCII.GetBytes(response);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
        }

        public static void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }
    }
}
