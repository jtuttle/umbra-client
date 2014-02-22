using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace CrawLib.Network.Messages {
    public enum MessageType : byte {
        EntityAdd
    }

    public interface INetMessage {
        MessageType MessageType { get; }
        void Decode(NetIncomingMessage message);
        void Encode(NetOutgoingMessage message);
    }
}
