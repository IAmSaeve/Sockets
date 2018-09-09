using System;
using System.Net;
using System.Net.Sockets;

namespace TCPEchoBetter
{
    class TCPEchoServer
    {
        static void Main(string[] args)
        {
            var ip = IPAddress.Parse("127.0.0.1");
           
            var serverSocket = new TcpListener(ip, 6789);

            serverSocket.Start();
            Console.WriteLine("Server started\n");

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
