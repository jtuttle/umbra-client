using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace CrawLib.Network.Messages {
    public enum NetworkMessageType : byte {
        EntityAdd,
        EntityMove
    }

    public interface INetworkMessage {
        NetworkMessageType MessageType { get; }
        void Decode(NetIncomingMessage message);
        void Encode(NetOutgoingMessage message);
    }
}
