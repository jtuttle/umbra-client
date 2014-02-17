using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrawLib.Entity;
using UmbraServer.Entities;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace UmbraServer {
    public class UmbraGameServer {
        private NetPeerConfiguration _netConfig;
        private NetServer _netServer;

        private Dictionary<long, Player> _players;
        private World _world;

        private float _updatesPerSecond = 30.0f;
        private double _nextSendUpdates = NetTime.Now;

        public UmbraGameServer() {
            _world = (World)EntityFactory.CreateEntity(typeof(World), "World", null);

            _players = new Dictionary<long, Player>();

            // TODO: probably want to move this to an XML file or something at some point
            _netConfig = new NetPeerConfiguration("Umbra");
            _netConfig.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            _netConfig.LocalAddress = NetUtility.Resolve("localhost");
            _netConfig.Port = 14242;
        }

        public void Start() {
            _netServer = new NetServer(_netConfig);
            _netServer.Start();

            Console.WriteLine("Server running on " + _netConfig.LocalAddress + ":" + _netServer.Port);
        }
        
        public void Shutdown() {
            Console.WriteLine("Server shutting down");

            _netServer.Shutdown("app exiting");
        }

        public void Update() {
            ReadMessages();

            // do physics step

            SendUpdates();
        }

        private void ReadMessages() {
            NetIncomingMessage msg;

            // read messages from server
            while((msg = _netServer.ReadMessage()) != null) {
                Console.WriteLine("Received " + msg.MessageType + " message from " + msg.SenderEndpoint);

                switch(msg.MessageType) {
                    case NetIncomingMessageType.DiscoveryRequest:
                        _netServer.SendDiscoveryResponse(null, msg.SenderEndpoint);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();

                        long playerId = msg.SenderConnection.RemoteUniqueIdentifier;
                        Console.WriteLine("Player " + NetUtility.ToHexString(playerId) + " " + status.ToString());

                        switch(status) {
                            case NetConnectionStatus.Connected:
                                Player player = (Player)EntityFactory.CreateEntity(typeof(Player), "Player", _world);
                                _players[playerId] = player;

                                break;
                            case NetConnectionStatus.Disconnected:
                                // remove player

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
        }

        private void SendUpdates() {
    		if(NetTime.Now > _nextSendUpdates) {
                // send update messages to clients
			    foreach (NetConnection player in _netServer.Connections) {
					foreach (NetConnection otherPlayer in _netServer.Connections) {
                        // send position update about 'otherPlayer' to 'player'
                        NetOutgoingMessage om = _netServer.CreateMessage();

                        long playerId = otherPlayer.RemoteUniqueIdentifier;

                        // write who this position is for
                        om.Write(playerId);

                        Vector3 position = _players[playerId].Position;
                        om.Write(position.X);
                        om.Write(position.Y);
                            
                            // send message
                        _netServer.SendMessage(om, player, NetDeliveryMethod.Unreliable);
					}
				}

				// schedule next update
                _nextSendUpdates += (1.0 / _updatesPerSecond);
			}
        }
    }
}
