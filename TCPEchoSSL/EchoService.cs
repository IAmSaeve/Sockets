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
            while (true)
            {
                // Detects a lost or closed connection and restarts the server.
                if (!ConnectionSocket.Connected)
                {
                    Console.WriteLine("No connection to client, restarting server.");
                    Thread.Sleep(2300);
                    Console.Clear();
                    break;
                }

                /*
                 * Sets variables for use later.
                 * Enforces message flushing with, (AutoFlush = true), needed to push messages instantly.
                 */
                Stream ns = new NetworkStream(ConnectionSocket.Client);
                var sr = new StreamReader(ns);
                var sw = new StreamWriter(ns) {AutoFlush = true};
                var message = sr.ReadLine();
                var answer = "";

                /*
                 * This method stops the server if an empty string is detected.
                 * This is done because a lost connection to a client usually sends empty strings.
                 * The second reason is prevent recourse waste since the server always expects a not null or empty
                 * string object to read and answer.
                 */
                if (message == string.Empty)
                {
                    Console.WriteLine("Empty string detected!");
                    Console.WriteLine(
                        "Either the client sent an empty string or the connection is lost.\nRestarting server");
                    Thread.Sleep(2300);
                    Console.Clear();
                    ns.Close();
                    ConnectionSocket.Close();
                    break;
                }

                while (!string.IsNullOrEmpty(message))
                {
                    Console.WriteLine("Client: " + message);

                    /*
                     * This method handles interrupt messages sent by the client.
                     * When the server receives "stop", it informs the client and the server that it will restart.
                     * After the connection is closed the method break the while loop and waits for a new client to
                     * connect.
                     */
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

                    // Responds to client.
                    answer = message.ToUpper();
                    sw.WriteLine(answer);
                    message = sr.ReadLine();
                }

                // Closes connection after client disconnect.
                ns.Close();
                ConnectionSocket.Close();
            }
        }
    }
}