using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Lidgren.Network;
using CrawLib.Artemis.Components;

namespace CrawLib.Network.Messages {
    public class EntityAddEvent : INetMessage {
        public MessageType MessageType { get { return MessageType.EntityAdd; } }

        protected Entity _entity;

        public EntityAddEvent(Entity entity) {
            _entity = entity;
        }

        public void Decode(NetIncomingMessage msg) {
            long entityId = msg.ReadInt64();

            // read type

            int x = msg.ReadInt32();
            int y = msg.ReadInt32();

            
        }

        public void Encode(NetOutgoingMessage msg) {
            msg.Write(_entity.UniqueId);
            
            // type of entity

            TransformComponent transform = _entity.GetComponent<TransformComponent>();
            msg.Write(transform.X);
            msg.Write(transform.Y);
        }
    }
}
