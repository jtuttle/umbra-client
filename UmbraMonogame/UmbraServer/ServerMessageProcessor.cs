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

namespace UmbraServer {
    public class ServerMessageProcessor {
        public ServerMessageProcessor(EntityWorld entityWorld) {

        }

        public void ProcessIncomingMessages(List<NetIncomingMessage> messages) {
            foreach(NetIncomingMessage netMessage in messages) {
                NetworkMessageType messageType = (NetworkMessageType)Enum.ToObject(typeof(NetworkMessageType), netMessage.ReadByte());

                if(messageType == NetworkMessageType.EntityMove) {
                    EntityMoveMessage moveMessage = new EntityMoveMessage();
                    moveMessage.Decode(netMessage);
                    MoveEntity(moveMessage);
                }
            }
        }

        private void MoveEntity(EntityMoveMessage msg) {
            Entity entity = EntityManager.Instance.GetEntity(msg.EntityId);

            if(entity.Tag == "PLAYER") {
                TransformComponent transform = entity.GetComponent<TransformComponent>();
                transform.Position = msg.Position;
            }
        }
    }
}
