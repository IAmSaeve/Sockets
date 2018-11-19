using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace TCPEchoSSL
{
    class TcpEchoServer
    {
        [SuppressMessage("ReSharper", "FunctionNeverReturns")]
        static void Main(string[] args)
        {
            // prepares the server to accept client on localhost at port 6789.
            //var ip = IPAddress.Parse("127.0.0.1");
            var serverSocket = new TcpListener(IPAddress.Any, 6789);

            // SSL
            var serverCertificateFile = Environment.CurrentDirectory + "/Cert/ServerSSL.cer";
            if (!File.Exists(serverCertificateFile))
            {
                throw new FileNotFoundException("The certificate could not be found in the current directory.");
            }
            var clientCertificateRequired = false;
            var checkCertificateRevocation = true;
            var enabledSSLProtocols = SslProtocols.Tls;
            var serverCertificate = new X509Certificate(serverCertificateFile, "password");

            // Starts the server
            serverSocket.Start();
            Console.WriteLine("Server started\n");
            
            // Infinite loop to wait for clients and running the echo service.
            while (true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                var connectionSocket = serverSocket.AcceptTcpClient();
                Stream insecureStream = connectionSocket.GetStream();
                var leaveInnerStreamOpen = false;
                var sslStream = new SslStream(insecureStream, leaveInnerStreamOpen);
                sslStream.AuthenticateAsServer(serverCertificate);
                Console.WriteLine("Server activated\n");

                var es = new EchoService(connectionSocket);
                es.DoIt();
            }
        }
    }
}
