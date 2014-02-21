using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Lidgren.Network;

namespace CrawLib.Network.Events {
    public class EntityAddEvent : NetworkEvent {
        protected Entity _entity;

        public EntityAddEvent(Entity entity, NetPeer peer)
            : base(peer) {

            _entity = entity;
        }

        public override void Deserialize(NetIncomingMessage msg) {

        }

        public override NetOutgoingMessage Serialize() {
            NetOutgoingMessage msg = _peer.CreateMessage(_capacity);

            // unique id of entity
            // type of entity
            // position of entity

            return msg;
        }
    }
}
