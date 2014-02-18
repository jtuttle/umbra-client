using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CrawLib.Entity;
using Microsoft.Xna.Framework;
using CrawLib.Entity.Interface;

namespace UmbraServer {
    class Program {
        static void Main(string[] args) {
            UmbraGameServer server = new UmbraGameServer();

            server.Initialize();
            server.Start();

            while(!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape) {
                server.Update();
                
                // sleep to allow other processes to run smoothly (not sure if this is necessary)
                Thread.Sleep(1);
            }

            server.Shutdown();
        }
    }
}
