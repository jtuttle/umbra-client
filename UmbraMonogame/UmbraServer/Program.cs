using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UmbraServer {
    class Program {
        static void Main(string[] args) {
            // configure and start server
            NetPeerConfiguration config = new NetPeerConfiguration("MyExampleName");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.LocalAddress = NetUtility.Resolve("localhost");
            config.Port = 14242;

            NetServer server = new NetServer(config);
            server.Start();

            Console.WriteLine("Server running on " + config.LocalAddress + ":" + server.Port);

            // schedule initial sending of client updates
            float updatesPerSecond = 30.0f;
			double nextSendUpdates = NetTime.Now;

            // run until escape key is pressed
            while(!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape) {
                NetIncomingMessage msg;

                // read messages from server
                while((msg = server.ReadMessage()) != null) {
                    Console.WriteLine("Received " + msg.MessageType + " message from " + msg.SenderEndpoint);

                    switch(msg.MessageType) {
                        case NetIncomingMessageType.DiscoveryRequest:
                            server.SendDiscoveryResponse(null, msg.SenderEndpoint);
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();

                            long id = msg.SenderConnection.RemoteUniqueIdentifier;
                            Console.WriteLine("Player " + NetUtility.ToHexString(id) + " " + status.ToString());

                            switch(status) {
                                case NetConnectionStatus.Connected:
                                    // instantiate client, etc

                                    break;
                            }

                            break;
                        case NetIncomingMessageType.Data:
                            // process client data

                            break;
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.ErrorMessage:
                            Console.WriteLine(msg.ReadString());
                            break;
                    }
                }

				if(NetTime.Now > nextSendUpdates) {
                    // send update messages to clients
				    foreach (NetConnection player in server.Connections) {
						foreach (NetConnection otherPlayer in server.Connections) {
							NetOutgoingMessage om = server.CreateMessage();

                            // write message contents

							server.SendMessage(om, player, NetDeliveryMethod.Unreliable);
						}
					}

					// schedule next update
                    nextSendUpdates += (1.0 / updatesPerSecond);
				}
			
                // sleep to allow other processes to run smoothly
                Thread.Sleep(1);
            }

            server.Shutdown("app exiting");
        }
    }
}
