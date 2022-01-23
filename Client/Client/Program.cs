using System;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Client
{
    class Program
    {
        private static String SERVER_ADDRESS = "127.0.0.1";
        private static Int16 SERVER_PORT = 9999;

        static void Main(string[] args)
        {

            Console.WriteLine("DELIM:" + Command.DELIMITER);
            Console.Read();
            try
            {
                // Create client
                TcpClient tcpClient = new TcpClient(SERVER_ADDRESS, SERVER_PORT);
                Console.WriteLine("Connected!\n");

                // Listen to server on separate thread (what other clients send)
                Thread thread = new Thread(Read);
                thread.Start(tcpClient);

                // Get writer to write to server
                StreamWriter sWriter = new StreamWriter(tcpClient.GetStream());

                while (true)
                {
                    if (tcpClient.Connected)
                    {
                        // Write to server
                        string input = Console.ReadLine();
                        sWriter.WriteLine(input);
                        sWriter.Flush();
                    }
                }

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            Console.ReadKey();
        }

        static void Read(Object server)
        {
            // Get client
            TcpClient tcpClient = (TcpClient)server;

            // Get reader to read from server (other clients)
            StreamReader sReader = new StreamReader(tcpClient.GetStream());

            while (true)
            {
                try
                {
                    string message = sReader.ReadLine();
                    Console.WriteLine(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
            }
        }
    }
}