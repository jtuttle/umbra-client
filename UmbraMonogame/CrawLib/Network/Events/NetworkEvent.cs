using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace CrawLib.Network.Events {
    public abstract class NetworkEvent {
        protected NetPeer _peer;
        protected int _capacity;

        public NetworkEvent(NetPeer peer) {
            _peer = peer;
        }

        public abstract void Deserialize(NetIncomingMessage msg);
        public abstract NetOutgoingMessage Serialize();
    }
}
