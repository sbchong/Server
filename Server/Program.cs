﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            //new ServerControl().Start();
            //return;
            ServerControl server = new ServerControl();
            server.Start();

            Console.ReadKey();
        }
    }
}
