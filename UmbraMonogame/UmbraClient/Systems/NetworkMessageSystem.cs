﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using Artemis;
using CrawLib.Artemis.Components;
using CrawLib.Network;
using Lidgren.Network;
using CrawLib.Network.Messages;
using CrawLib.Artemis;
using UmbraLib;
using Microsoft.Xna.Framework;
using CrawLib;
using UmbraClient.Components;

namespace UmbraClient.Systems {
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 0)]
    class NetworkMessageSystem : EntitySystem {
        private NetworkAgent _networkAgent;

        public override void LoadContent() {
            _networkAgent = BlackBoard.GetEntry<NetworkAgent>("NetworkAgent");
        }

        // this is a little hacky as there aren't actually any entities to process
        // probably a sign that I'm abusing the entity component system. i think the proper
        // way to do this is to create a simple entity that just has a component to hold
        // a reference to the network agent. that's roughly how it would work in unity.
        // might be best to use a TagSystem like for player input?
        protected override void ProcessEntities(IDictionary<int, Entity> entities) {
            List<NetIncomingMessage> messages = _networkAgent.ReadMessages();

            foreach(NetIncomingMessage netMessage in messages) {
                NetworkMessageType messageType = (NetworkMessageType)Enum.ToObject(typeof(NetworkMessageType), netMessage.ReadByte());

                if(messageType == NetworkMessageType.PlayerConnect) {
                    PlayerConnectMessage<UmbraEntityType> playerConnectMessage = new PlayerConnectMessage<UmbraEntityType>();
                    playerConnectMessage.Decode(netMessage);
                    PlayerConnect(playerConnectMessage);
                }  else if(messageType == NetworkMessageType.EntityAdd) {
                    EntityAddMessage<UmbraEntityType> addMessage = new EntityAddMessage<UmbraEntityType>();
                    addMessage.Decode(netMessage);
                    AddEntity(addMessage);
                } else if(messageType == NetworkMessageType.EntityMove) {
                    EntityMoveMessage moveMessage = new EntityMoveMessage();
                    moveMessage.Decode(netMessage);
                    MoveEntity(moveMessage);
                } else if(messageType == NetworkMessageType.EntityRemove) {
                    EntityRemoveMessage removeMessage = new EntityRemoveMessage();
                    removeMessage.Decode(netMessage);
                    RemoveEntity(removeMessage);
                }
            }
        }

        private void PlayerConnect(PlayerConnectMessage<UmbraEntityType> msg) {
            long entityId = msg.EntityId;
            
            Entity player = CrawEntityManager.Instance.EntityFactory.CreatePlayer((long?)entityId, msg.Position);

            if(msg.IsSelf) {
                player.Tag = "PLAYER";

                // TODO: this probably shouldn't happen here
                CameraComponent camera = BlackBoard.GetEntry<CameraComponent>("Camera");
                camera.Target = player.GetComponent<TransformComponent>();
            }
        }

        private void AddEntity(EntityAddMessage<UmbraEntityType> msg) {
            long entityId = msg.EntityId;
            
            if(msg.EntityType == UmbraEntityType.Player) {
                CrawEntityManager.Instance.EntityFactory.CreatePlayer((long?)entityId, msg.Position);
            } else if(msg.EntityType == UmbraEntityType.NPC) {
                CrawEntityManager.Instance.EntityFactory.CreateNPC((long?)entityId, msg.Position);
            }
        }

        private void MoveEntity(EntityMoveMessage msg) {
            Entity entity = CrawEntityManager.Instance.GetEntity(msg.EntityId);

            if(entity != null && entity.Tag != "PLAYER") {
                TransformComponent transform = entity.GetComponent<TransformComponent>();
                transform.Position = msg.Position;
            }
        }

        private void RemoveEntity(EntityRemoveMessage msg) {
            Entity entity = CrawEntityManager.Instance.GetEntity(msg.EntityId);
            if(entity != null) entity.Delete();
        }
    }
}
