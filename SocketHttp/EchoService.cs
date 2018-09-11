using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace SocketHttp
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
                try
                {
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
                    
                    /*
                     * This block handles HTTP GET requests.
                     * When the server receives a the, GET request, string it then split the string to extract the URI.
                     * After the URI is extracted a HTTP response is sent back to the client. 
                     */
                    if (message != null && message.Contains("GET") && message.Contains(".html") &&
                        message.Contains("HTTP"))
                    {
                        /*
                         * Splits the string to extract the URI form the HTTP GET request.
                         * This is done by removing everything before the first space and the second space.
                         */
                        var s = message.Split(' ', ' ')[0 + 1];
                        var page = File.ReadAllText(Environment.CurrentDirectory + "/somefile.html");
                        
                        // Forges HTTP response with a html page that says "Hello user!", then sends it to the client.
                        answer = "HTTP/1.1 200 OK\r\n" +
                                 "Content-Type: text/html\r\n" +
                                 "Connection: close\r\n" +
                                 "\r\n" +
                                 page +
                                 "\r\n";

                        
                        File.ReadAllText(Environment.CurrentDirectory + "/somefile.html");
                        sw.WriteLine(answer);
                        
                        // Tries to close the connection to avoid server side spam.
                        // Console.Clear();
                        ns.Close();
                        ConnectionSocket.Close();
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
                catch (ArgumentNullException)
                {
                    Console.WriteLine("Caught a ArgumentNullException");
                    Console.WriteLine("Probably lost connection to client.\nEnding thread...");
                    ConnectionSocket.Close();
                    break;
                }
                catch (IOException)
                {
                    Console.WriteLine("Caught a IOException");
                    Console.WriteLine("Probably lost connection to client.\nEnding thread...");
                    ConnectionSocket.Close();
                    break;
                }
            }
        }
    }
}