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

namespace UmbraServer {
    public class UmbraGameServer {
        private int _nextEntityId;
        public int NextEntityId {
            get { return _nextEntityId++; }
        }
        
        private NetworkAgent _netAgent;

        private EntityWorld _entityWorld;
        private Dictionary<long, Entity> _entities;

        // todo - DRY this stuff up
        private float _updatesPerSecond = 1.0f;
        private double _nextSendUpdates = NetTime.Now;

        public UmbraGameServer() {
            
        }

        public void Initialize() {
            _entityWorld = new EntityWorld();
            _entityWorld.InitializeAll(new[] { GetType().Assembly });
            _entityWorld.EntityManager.AddedEntityEvent += OnEntityAdded;
            _entityWorld.EntityManager.RemovedEntityEvent += OnEntityRemoved;

            _entities = new Dictionary<long, Entity>();

            _netAgent = new NetworkAgent(AgentRole.Server, "Umbra");
            _netAgent.OnPlayerConnect += OnPlayerConnect;
        }

        public void Start() {
            //// TEMP ////
            Entity npc = _entityWorld.CreateEntity();

            TransformComponent transformComponent = new TransformComponent(200, 200);
            npc.AddComponent(transformComponent);
            npc.AddComponent(new VelocityComponent());
            npc.AddComponent(new AiComponent());

            EntityAddMessage<UmbraEntityType> msg = new EntityAddMessage<UmbraEntityType>(npc.UniqueId, UmbraEntityType.NPC, transformComponent.Position);
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

                    if(_entities.ContainsKey(msg.EntityId)) {
                        Entity entity = _entities[msg.EntityId];

                        TransformComponent transform = entity.GetComponent<TransformComponent>();
                        transform.Position = msg.Position;
                    }
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

        private void OnPlayerConnect() {
            Entity player = _entityWorld.CreateEntity();

            TransformComponent transformComponent = new TransformComponent(100, 100);
            player.AddComponent(transformComponent);
            player.AddComponent(new VelocityComponent());

            EntityAddMessage<UmbraEntityType> msg = new EntityAddMessage<UmbraEntityType>(player.UniqueId, UmbraEntityType.Player, transformComponent.Position);
            _netAgent.BroadcastMessage(msg, true);
        }

        private void OnEntityAdded(Entity entity) {
            _entities[entity.UniqueId] = entity;
        }

        private void OnEntityRemoved(Entity entity) {
            _entities.Remove(entity.UniqueId);
        }
    }
}
