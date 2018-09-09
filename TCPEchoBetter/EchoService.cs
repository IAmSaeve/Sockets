using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace TCPEchoBetter
{
    public class EchoService
    {
        private TcpClient ConnectionSocket { get; }

        public EchoService(TcpClient connectionSocket)
        {
            ConnectionSocket = connectionSocket;
        }

        public void DoIt()
        {
            //var ns = ConnectionSocket.GetStream();
            Stream ns = new NetworkStream(ConnectionSocket.Client);
            
            while (true)
            {
                if (!ConnectionSocket.Connected)
                {
                    Console.WriteLine("No connection to client, restarting server.");
                    Thread.Sleep(2300);
                    Console.Clear();
                    break;
                }
                
                var sr = new StreamReader(ns);
                var sw = new StreamWriter(ns) {AutoFlush = true};
                var message = sr.ReadLine();
                var answer = "";
                
                if (message == string.Empty)
                {
                    Console.WriteLine("Empty string detected!");
                    Console.WriteLine("Either the client sent an empty string or the connection is lost.\nRestarting server");
                    Thread.Sleep(2300);
                    Console.Clear();
                    ns.Close();
                    ConnectionSocket.Close();
                    break;
                }

                while (!string.IsNullOrEmpty(message))
                {
                    Console.WriteLine("Client: " + message);

                    if (message == "stop")
                    {
                        Console.WriteLine("Received interrupt signal!");
                        Console.WriteLine("The server will close the connection and wait for a new client.");
                        sw.WriteLine("Received interrupt signal! Closing connection...");
                        Thread.Sleep(2300);
                        Console.Clear();
                        ns.Close();
                        ConnectionSocket.Close();
                        break;
                    }

                    answer = message.ToUpper();
                    sw.WriteLine(answer);
                    message = sr.ReadLine();
                }
                
                ns.Close();
                ConnectionSocket.Close();
            }
        }
    }
}