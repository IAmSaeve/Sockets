﻿/*
 * TCPEchoClient
 *
 * Author Michael Claudius, ZIBAT Computer Scienc
 * Version 1.0. 2014.02.10
 * Copyright 2014 by Michael Claudius
 * Revised 2014.09.01, 2016.09.14
 * All rights reserved
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new TCPClient
            TcpClient clientSocket = new TcpClient();
            try
            {
                clientSocket = new TcpClient("127.0.0.1", 6789);
            }
            catch (Exception e)
            {
                
                Debug.WriteLine("Der opstod en fejl ved forsøget på at oprette en forbinelse\n" + e + "\n");

                System.Console.WriteLine("Det lader til at der er et problem med forbindelsen til serveren!");
                System.Console.WriteLine("Prgrammet sover i 5 sekunder og prøver at oprette forbindelse igen.");

                Thread.Sleep(5000);
                Console.Clear();
                Main(null);
            }
            
            Console.WriteLine("Client ready");

            Stream ns = clientSocket.GetStream();  //provides a Stream
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    string message = Console.ReadLine();
                    sw.WriteLine(message);
                    string serverAnswer = sr.ReadLine();

                    Console.WriteLine("Server: " + serverAnswer);
                }
                catch (IOException)
                {
                    Console.WriteLine("Forbindelsen til servenren blev afbrudt uventet.\nLukker ned...");
                    Thread.Sleep(3000);
                    ns.Close();
                    clientSocket.Close();
                    Environment.Exit(0);
                }
            }

            Console.WriteLine("No more from server. Press Enter");
            Console.ReadLine();

            ns.Close();
            clientSocket.Close();

        }

    }

}