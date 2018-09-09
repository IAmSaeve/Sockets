using System;
using System.Net;
using System.Net.Sockets;

namespace TCPEchoBetter
{
    class TCPEchoServer
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");

           
            TcpListener serverSocket = new TcpListener(ip, 6789);
            //Alternatively but deprecated
            //TcpListener serverSocket = new TcpListener(6789);


            serverSocket.Start();
            Console.WriteLine("Server started\n");

            while (true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                var connectionSocket = serverSocket.AcceptTcpClient();
                //Socket connectionSocket = serverSocket.AcceptSocket();
                Console.WriteLine("Server activated\n");
            
                EchoService es = new EchoService(connectionSocket);
                es.DoIt();
            }
        }
    }
}
