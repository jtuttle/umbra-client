using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Lidgren.Network;
using CrawLib.Network.Messages;
using UmbraLib;
using Microsoft.Xna.Framework;
using CrawLib.Artemis;
using CrawLib.Artemis.Components;

namespace UmbraClient {
    public class ClientMessageProcessor {
        public ClientMessageProcessor(EntityWorld entityWorld) {

        }

        public void ProcessIncomingMessages(List<NetIncomingMessage> messages) {
            foreach(NetIncomingMessage netMessage in messages) {
                NetworkMessageType messageType = (NetworkMessageType)Enum.ToObject(typeof(NetworkMessageType), netMessage.ReadByte());

                if(messageType == NetworkMessageType.EntityAdd) {
                    EntityAddMessage<UmbraEntityType> addMessage = new EntityAddMessage<UmbraEntityType>();
                    addMessage.Decode(netMessage);
                    AddEntity(addMessage);
                } else if(messageType == NetworkMessageType.EntityMove) {
                    EntityMoveMessage moveMessage = new EntityMoveMessage();
                    moveMessage.Decode(netMessage);
                    MoveEntity(moveMessage);
                }
            }
        }

        private void AddEntity(EntityAddMessage<UmbraEntityType> msg) {
            long entityId = msg.EntityId;
            Vector2 position = msg.Position;

            if(msg.EntityType == UmbraEntityType.Player) {
                EntityManager.Instance.EntityFactory.CreatePlayer((long?)entityId, position);
            } else if(msg.EntityType == UmbraEntityType.NPC) {
                EntityManager.Instance.EntityFactory.CreateNPC((long?)entityId, position);
            }
        }

        private void MoveEntity(EntityMoveMessage msg) {
            Entity entity = EntityManager.Instance.GetEntity(msg.EntityId);

            if(entity.Tag != "PLAYER") {
                TransformComponent transform = entity.GetComponent<TransformComponent>();
                transform.Position = msg.Position;
            }
        }
    }
}
