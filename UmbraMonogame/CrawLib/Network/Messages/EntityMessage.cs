using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace CrawLib.Network.Messages {
    public abstract class EntityMessage : INetworkMessage {
        private NetworkMessageType _messageType;
        public NetworkMessageType MessageType { get { return _messageType; } }

        public long EntityId { get; protected set; }

        public EntityMessage() { }

        public EntityMessage(long entityId, NetworkMessageType messageType) {
            EntityId = entityId;
            _messageType = messageType;
        }

        public virtual void Decode(NetIncomingMessage msg) {
            EntityId = msg.ReadInt64();
        }

        public virtual void Encode(NetOutgoingMessage msg) {
            msg.Write((long)EntityId);
        }
    }
}
