using System;
using System.Net.Sockets;

namespace TCPEchoBetter
{
    class Program
    {
        static void Main(string[] args)
        {
            EchoService es = new EchoService(new TcpClient("localhost", 6789));
            es.DoIt();
        }
    }
}
