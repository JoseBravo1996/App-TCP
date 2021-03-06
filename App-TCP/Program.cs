﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace App_TCP
{
    class Program
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        static void Main(string[] args)
        {
            Console.Title = "Cliente";
            LoopConnect();
            SeendLoop();
            Console.ReadLine();
        }

        private static void SeendLoop()
        {
            while (true)
            {
                Console.Write("Ingrese request: ");
                string req = Console.ReadLine();
                byte[] buffer = Encoding.ASCII.GetBytes(req);

                //Thread.Sleep(5000);
                //byte[] buffer = Encoding.ASCII.GetBytes("get time");
                _clientSocket.Send(buffer);

                byte[] receivedBuf = new byte[1024];
                int rec = _clientSocket.Receive(receivedBuf);
                byte[] data = new byte[rec];
                Array.Copy(receivedBuf, data, rec);
                Console.WriteLine("Recibido: " +  Encoding.ASCII.GetString(data));

            }
        }


        private static void LoopConnect() {
            int attemps = 0;

            while (!_clientSocket.Connected) {
                try
                {
                    attemps++;
                    IPAddress test1 = IPAddress.Parse("192.168.99.1");
                    _clientSocket.Connect(test1, 100);
                }
                catch (SocketException)
                {
                    Console.WriteLine("Intentos de coneccion: " + attemps.ToString());
                }
            }

            Console.Clear();
            Console.WriteLine("Conectado");
        }

   }
}
