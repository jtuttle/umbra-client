using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using CrawLib.Artemis.Components;
using CrawLib.Network;
using CrawLib.Network.Messages;
using UmbraLib;

namespace UmbraServer {
    public class UmbraGameServer {
        private NetworkAgent _netAgent;

        private EntityWorld _entityWorld;
        private Dictionary<long, Entity> _players;

        private float _updatesPerSecond = 30.0f;
        private double _nextSendUpdates = NetTime.Now;

        public UmbraGameServer() {
            
        }

        public void Initialize() {
            _entityWorld = new EntityWorld();
            _entityWorld.InitializeAll(new[] { GetType().Assembly });

            _players = new Dictionary<long, Entity>();

            _netAgent = new NetworkAgent(AgentRole.Server, "Umbra");
            _netAgent.OnPlayerConnect += OnPlayerConnect;
        }

        public void Start() {
            
        }
        
        public void Shutdown() {
            _netAgent.Shutdown();
        }

        public void Update() {
            List<NetIncomingMessage> messages = _netAgent.ReadMessages();

            _entityWorld.Update();

            //SendUpdates();
        }

        private void OnPlayerConnect() {
            Entity player = _entityWorld.CreateEntity();

            TransformComponent transform = new TransformComponent(100, 100);
            player.AddComponent(transform);
            player.AddComponent(new VelocityComponent());

            EntityAddMessage<UmbraEntityType> msg = new EntityAddMessage<UmbraEntityType>(player.UniqueId, UmbraEntityType.Player, transform.Position);
            _netAgent.BroadcastMessage(msg);
        }

        /*
        private void ReadMessages() {
            NetIncomingMessage msg;
            Entity player;

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
                                player = _entityWorld.CreateEntity();
                                player.AddComponent(new TransformComponent(0, 0));
                                player.AddComponent(new VelocityComponent());
                                
                                // send message to client with unique id for player creation
                                // every time an entity is created, this is how it'll have to be

                                _players[playerId] = player;

                                break;
                            case NetConnectionStatus.Disconnected:
                                // remove player

                                break;
                        }

                        break;
                    case NetIncomingMessageType.Data:
                        int xPos = msg.ReadInt32();
                        int yPos = msg.ReadInt32();

                        player = _players[msg.SenderConnection.RemoteUniqueIdentifier];
                        TransformComponent transform = player.GetComponent<TransformComponent>();

                        transform.Position = new Vector2(xPos, yPos);

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
                        //NetOutgoingMessage om = _netServer.CreateMessage();

                        //long playerId = otherPlayer.RemoteUniqueIdentifier;

                        // write who this position is for
                        //om.Write(playerId);

                        //Vector3 position = _players[playerId].Position;
                        //om.Write(position.X);
                        //om.Write(position.Y);
                            
                        // send message
                        //_netServer.SendMessage(om, player, NetDeliveryMethod.Unreliable);
					}
				}

				// schedule next update
                _nextSendUpdates += (1.0 / _updatesPerSecond);
			}
        }
        */
    }
}
