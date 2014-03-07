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
using Artemis.System;

namespace UmbraServer {
    public class UmbraGameServer {
        private EntityWorld _entityWorld;

        private NetworkAgent _networkAgent;
        
        public UmbraGameServer() {
            
        }

        public void Initialize() {
            _networkAgent = new NetworkAgent(AgentRole.Server, "Umbra");
            _networkAgent.OnPlayerConnect += OnPlayerConnect;

            EntitySystem.BlackBoard.SetEntry("NetworkAgent", _networkAgent);

            _entityWorld = new EntityWorld();
            _entityWorld.InitializeAll(new[] { GetType().Assembly });

            CrawEntityManager.Instance.Initialize(_entityWorld, new ServerEntityFactory(_entityWorld));
        }

        public void Start() {
            //// TEMP ////
            Vector2 position = new Vector2(600, 600);
            Entity npc = CrawEntityManager.Instance.EntityFactory.CreateNPC(null, position);

            EntityAddMessage<UmbraEntityType> msg = new EntityAddMessage<UmbraEntityType>(npc.UniqueId, UmbraEntityType.NPC, position);
            _networkAgent.BroadcastMessage(msg, true);
            //// TEMP ////
        }
        
        public void Shutdown() {
            _networkAgent.Shutdown();
        }

        public void Update() {
            _entityWorld.Update();
        }

        private void OnPlayerConnect(NetConnection connection) {
            EntityAddMessage<UmbraEntityType> msg;

            Bag<Entity> entities = _entityWorld.EntityManager.GetEntities(Aspect.All(typeof(UmbraEntityTypeComponent)));
            
            // signal addition of all other entities
            foreach(Entity entity in entities) {
                UmbraEntityTypeComponent entityType = entity.GetComponent<UmbraEntityTypeComponent>();
                TransformComponent transform = entity.GetComponent<TransformComponent>();

                msg = new EntityAddMessage<UmbraEntityType>(entity.UniqueId, entityType.EntityType, transform.Position);
                _networkAgent.SendMessage(msg, connection);
            }

            // create and signal addition of player entity
            Vector2 position = new Vector2(0, 0);
            Entity player = CrawEntityManager.Instance.EntityFactory.CreatePlayer(null, position);

            msg = new EntityAddMessage<UmbraEntityType>(player.UniqueId, UmbraEntityType.Player, position);
            _networkAgent.BroadcastMessage(msg, true);
        }
    }
}
