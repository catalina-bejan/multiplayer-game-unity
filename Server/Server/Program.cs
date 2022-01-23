using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Server
{
    class Program
    {
        private static IPAddress SERVER_ADDRESS = IPAddress.Parse("127.0.0.1");
        private static Int16 SERVER_PORT = 9999;

        private static TcpListener tcpListener;
        private static List<TcpClient> tcpClientsList = new List<TcpClient>();

        static void Main(string[] args)
        {
            // Create listener on port
            tcpListener = new TcpListener(SERVER_ADDRESS, SERVER_PORT);

            // Start listening
            tcpListener.Start();

            Console.WriteLine("Server started\n");

            while (true)
            {
                // Create client
                TcpClient tcpClient = tcpListener.AcceptTcpClient();

                // Add client to list
                tcpClientsList.Add(tcpClient);

                // Connect client to server
                Thread thread = new Thread(ClientListener);
                thread.Start(tcpClient);
            }
        }

        public static void ClientListener(Object client)
        {
            // Get client
            TcpClient tcpClient = (TcpClient)client;

            // Get reader to read from client
            StreamReader reader = new StreamReader(tcpClient.GetStream());
            Console.WriteLine("Client \"{0}\" connected!", tcpClient.Client.RemoteEndPoint);

            // Get data from client
            while (true)
            {
                string message = reader.ReadLine();
                Console.WriteLine(">>> \"{0}\": {1}", tcpClient.Client.RemoteEndPoint, message);

                // Broadcast the message
                BroadcastMessage(message);
            }
        }

        public static void BroadcastMessage(string msg)
        {
            foreach (TcpClient client in tcpClientsList)
            {
                StreamWriter sWriter = new StreamWriter(client.GetStream());
                sWriter.WriteLine(msg);
                sWriter.Flush();
            }
        }
    }
}