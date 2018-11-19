using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace TCPEchoSSL
{
    class TcpEchoServer
    {
        [SuppressMessage("ReSharper", "FunctionNeverReturns")]
        static void Main(string[] args)
        {
            // prepares the server to accept client on localhost at port 6789.
            var ip = IPAddress.Parse("127.0.0.1");
            var serverSocket = new TcpListener(ip, 6789);

            // Starts the server
            serverSocket.Start();
            Console.WriteLine("Server started\n");

            // Infinite loop to wait for clients and running the echo service.
            while (true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                var connectionSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine("Server activated\n");

                var es = new EchoService(connectionSocket);
                es.DoIt();
            }
        }
    }
}
