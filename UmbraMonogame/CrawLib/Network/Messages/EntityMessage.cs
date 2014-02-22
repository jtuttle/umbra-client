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

        public EntityMessage(long entityId, NetworkMessageType messageType) {
            EntityId = entityId;
            _messageType = messageType;
        }

        public virtual void Decode(NetIncomingMessage message) { }
        public virtual void Encode(NetOutgoingMessage message) { }
    }
}
