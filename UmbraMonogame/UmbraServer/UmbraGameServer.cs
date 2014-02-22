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
    }
}
