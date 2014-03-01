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
using UmbraServer.Components;
using UmbraLib.Components;
using Artemis.Utils;
using CrawLib.Artemis;

namespace UmbraServer {
    public class UmbraGameServer {
        private EntityWorld _entityWorld;

        private NetworkAgent _netAgent;

        // todo - DRY this stuff up
        private float _updatesPerSecond = 10.0f;
        private double _nextSendUpdates = NetTime.Now;

        public UmbraGameServer() {
            
        }

        public void Initialize() {
            _entityWorld = new EntityWorld();
            _entityWorld.InitializeAll(new[] { GetType().Assembly });

            EntityManager.Instance.Initialize(_entityWorld, new ServerEntityFactory(_entityWorld));

            _netAgent = new NetworkAgent(AgentRole.Server, "Umbra");
            _netAgent.OnPlayerConnect += OnPlayerConnect;
        }

        public void Start() {
            //// TEMP ////
            Vector2 position = new Vector2(200, 200);
            Entity npc = EntityManager.Instance.EntityFactory.CreateNPC(null, position);

            EntityAddMessage<UmbraEntityType> msg = new EntityAddMessage<UmbraEntityType>(npc.UniqueId, UmbraEntityType.NPC, position);
            _netAgent.BroadcastMessage(msg, true);
            //// TEMP ////
        }
        
        public void Shutdown() {
            _netAgent.Shutdown();
        }

        public void Update() {
            List<NetIncomingMessage> messages = _netAgent.ReadMessages();

            foreach(NetIncomingMessage netMessage in messages) {
                NetworkMessageType messageType = (NetworkMessageType)Enum.ToObject(typeof(NetworkMessageType), netMessage.ReadByte());

                if(messageType == NetworkMessageType.EntityMove) {
                    EntityMoveMessage msg = new EntityMoveMessage();
                    msg.Decode(netMessage);

                    //if(_entities.ContainsKey(msg.EntityId)) {
                    //    Entity entity = _entities[msg.EntityId];

                    //    TransformComponent transform = entity.GetComponent<TransformComponent>();
                    //    transform.Position = msg.Position;
                    //}
                }
            }

            _entityWorld.Update();

            double now = NetTime.Now;

            if(now > _nextSendUpdates) {
                Console.WriteLine("sending update");

                List<INetworkMessage> outgoingMessages = new List<INetworkMessage>();

                foreach(Entity entity in _entityWorld.EntityManager.GetEntities(Aspect.All(typeof(TransformComponent)))) {
                    TransformComponent transform = entity.GetComponent<TransformComponent>();
                    outgoingMessages.Add(new EntityMoveMessage(entity.UniqueId, transform.Position));
                }

                _netAgent.SendMessages(outgoingMessages);

                _nextSendUpdates += (1.0 / _updatesPerSecond);
            }
        }

        private void OnPlayerConnect(NetConnection connection) {
            EntityAddMessage<UmbraEntityType> msg;

            Bag<Entity> entities = _entityWorld.EntityManager.GetEntities(Aspect.All(typeof(UmbraEntityTypeComponent)));
            
            // signal addition of all other entities
            foreach(Entity entity in entities) {
                UmbraEntityTypeComponent entityType = entity.GetComponent<UmbraEntityTypeComponent>();
                TransformComponent transform = entity.GetComponent<TransformComponent>();

                msg = new EntityAddMessage<UmbraEntityType>(entity.UniqueId, entityType.EntityType, transform.Position);
                _netAgent.SendMessage(msg, connection);
            }

            // create and signal addition of player entity
            Vector2 position = new Vector2(100, 100);
            Entity player = EntityManager.Instance.EntityFactory.CreatePlayer(null, position);

            msg = new EntityAddMessage<UmbraEntityType>(player.UniqueId, UmbraEntityType.Player, position);
            _netAgent.BroadcastMessage(msg, true);
        }
    }
}
