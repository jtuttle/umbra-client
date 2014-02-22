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
        private int _nextEntityId;
        public int NextEntityId {
            get { return _nextEntityId++; }
        }
        
        private NetworkAgent _netAgent;

        private EntityWorld _entityWorld;
        private Dictionary<long, Entity> _entities;

        private float _updatesPerSecond = 30.0f;
        private double _nextSendUpdates = NetTime.Now;

        // temp
        private Entity _player;

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

            _netAgent.SendMessages();
        }

        private void OnPlayerConnect() {
            Entity player = _entityWorld.CreateEntity();

            TransformComponent transformComponent = new TransformComponent(100, 100);
            player.AddComponent(transformComponent);
            player.AddComponent(new VelocityComponent());

            EntityAddMessage<UmbraEntityType> msg = new EntityAddMessage<UmbraEntityType>(player.UniqueId, UmbraEntityType.Player, transformComponent.Position);
            _netAgent.BroadcastMessage(msg, true);

            // temp
            _player = player;
        }

        private void OnEntityAdded(Entity entity) {
            _entities[entity.UniqueId] = entity;
        }

        private void OnEntityRemoved(Entity entity) {
            _entities.Remove(entity.UniqueId);
        }
    }
}
