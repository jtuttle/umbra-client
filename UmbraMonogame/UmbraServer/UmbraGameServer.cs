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

        private Dictionary<NetConnection, long> _playerEntityIds;
        
        public UmbraGameServer() {
            _playerEntityIds = new Dictionary<NetConnection, long>();
        }

        public void Initialize() {
            _networkAgent = new NetworkAgent(AgentRole.Server, "Umbra");
            _networkAgent.OnPlayerConnect += OnPlayerConnect;
            _networkAgent.OnPlayerDisconnect += OnPlayerDisconnect;

            EntitySystem.BlackBoard.SetEntry("NetworkAgent", _networkAgent);

            _entityWorld = new EntityWorld();
            _entityWorld.InitializeAll(new[] { GetType().Assembly });

            CrawEntityManager.Instance.Initialize(_entityWorld, new ServerEntityFactory(_entityWorld));
        }

        public void Start() {
            //// TEMP ////
            Vector3 position = new Vector3(0, 0, 0);
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

        private void OnPlayerConnect(NetConnection playerConnection) {
            EntityAddMessage<UmbraEntityType> entityAddMessage;

            Bag<Entity> entities = _entityWorld.EntityManager.GetEntities(Aspect.All(typeof(UmbraEntityTypeComponent)));
            
            // send message to connecting player about existing entities
            foreach(Entity entity in entities) {
                UmbraEntityTypeComponent entityType = entity.GetComponent<UmbraEntityTypeComponent>();
                TransformComponent transform = entity.GetComponent<TransformComponent>();

                entityAddMessage = new EntityAddMessage<UmbraEntityType>(entity.UniqueId, entityType.EntityType, transform.Position);
                _networkAgent.SendMessage(entityAddMessage, playerConnection);
            }

            // send message confirming player's connection
            PlayerConnectMessage<UmbraEntityType> playerConnectMessage;

            Vector3 startPos = new Vector3(10, 0, 10);
            Entity player = CrawEntityManager.Instance.EntityFactory.CreatePlayer(null, startPos);

            playerConnectMessage = new PlayerConnectMessage<UmbraEntityType>(player.UniqueId, UmbraEntityType.Player, startPos, true);
            _networkAgent.SendMessage(playerConnectMessage, playerConnection);

            // message other clients about player's connection
            playerConnectMessage = new PlayerConnectMessage<UmbraEntityType>(player.UniqueId, UmbraEntityType.Player, startPos, false);
            
            foreach(NetConnection connection in _networkAgent.Connections) {
                if(connection != playerConnection) {
                    _networkAgent.SendMessage(playerConnectMessage, connection);
                }
            }

            _playerEntityIds[playerConnection] = player.UniqueId;
        }

        private void OnPlayerDisconnect(NetConnection playerConnection) {
            long playerEntityId = _playerEntityIds[playerConnection];
            Entity playerEntity = CrawEntityManager.Instance.GetEntity(playerEntityId);

            playerEntity.Delete();
            _playerEntityIds.Remove(playerConnection);

            EntityRemoveMessage msg = new EntityRemoveMessage(playerEntityId);
            _networkAgent.BroadcastMessage(msg);
        }
    }
}
