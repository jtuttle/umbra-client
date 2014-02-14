using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UmbraServer.Entities;
using CrawLib.Entity;
using Microsoft.Xna.Framework;
using CrawLib.Entity.Interface;

namespace UmbraServer {
    class Program {
        static void Main(string[] args) {
            World world = (World)EntityFactory.CreateEntity(typeof(World), "World", null);

            Dictionary<long, Player> _players = new Dictionary<long, Player>();

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

                            long playerId = msg.SenderConnection.RemoteUniqueIdentifier;
                            Console.WriteLine("Player " + NetUtility.ToHexString(playerId) + " " + status.ToString());

                            switch(status) {
                                case NetConnectionStatus.Connected:
                                    // instantiate client, etc

                                    Player player = (Player)EntityFactory.CreateEntity(typeof(Player), "Player", world);
                                    _players[playerId] = player;

                                    break;
                            }

                            break;
                        case NetIncomingMessageType.Data:
                            // process client data

                            int xInput = msg.ReadInt32();
							int yInput = msg.ReadInt32();

                            _players[msg.SenderConnection.RemoteUniqueIdentifier].UpdatePosition(xInput, yInput);

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
                            // send position update about 'otherPlayer' to 'player'
                            NetOutgoingMessage om = server.CreateMessage();

                            long playerId = otherPlayer.RemoteUniqueIdentifier;

                            // write who this position is for
                            om.Write(playerId);

                            Vector3 position = _players[playerId].Position;
                            om.Write(position.X);
                            om.Write(position.Y);
                            
                            // send message
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
