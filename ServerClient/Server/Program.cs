﻿/*
 * TCPEchoServer
 *
 * Author Michael Claudius, ZIBAT Computer Science
 * Version 1.0. 2014.02.10
 * Copyright 2014 by Michael Claudius
 * Revised 2014.09.01
 * All rights reserved
 */


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {

        public static void Main(string[] args)
        {

            // IPAddress ip = new IPAddress("127.0.0.1");

            IPAddress ip = IPAddress.Parse("127.0.0.1");

           
            TcpListener serverSocket = new TcpListener(ip, 6789);
            //Alternatively but deprecated
            //TcpListener serverSocket = new TcpListener(6789);


            serverSocket.Start();
            Console.WriteLine("Server started");

            var connectionSocket = serverSocket.AcceptTcpClient();
            //Socket connectionSocket = serverSocket.AcceptSocket();
            Console.WriteLine("Server activated");

            var ns = connectionSocket.GetStream();
            // Stream ns = new NetworkStream(connectionSocket);

            var sr = new StreamReader(ns);
            var sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing

            var message = sr.ReadLine();
            var answer = "";
            while (message != null && message != "")
            {

                Console.WriteLine("Client: " + message);
                answer = message.ToUpper();
                sw.WriteLine(answer);
                message = sr.ReadLine();

            }

            ns.Close();
            connectionSocket.Close();
            serverSocket.Stop();

        }
    }
    
}

