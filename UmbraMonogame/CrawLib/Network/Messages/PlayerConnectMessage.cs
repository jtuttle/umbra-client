using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Lidgren.Network;
using CrawLib.Artemis.Components;
using Microsoft.Xna.Framework;

namespace CrawLib.Network.Messages {
    public class PlayerConnectMessage<TEnum> : EntityMessage {
        public TEnum EntityType { get; private set; }
        public Vector2 Position { get; private set; }
        public bool IsSelf { get; private set; }

        public PlayerConnectMessage() : base() { }

        public PlayerConnectMessage(long entityId, TEnum entityType, Vector2 position, bool isSelf)
            : base(entityId, NetworkMessageType.PlayerConnect) {

            EntityType = entityType;
            Position = position;
            IsSelf = isSelf;
        }

        public override void Decode(NetIncomingMessage msg) {
            base.Decode(msg);

            EntityType = (TEnum)Enum.ToObject(typeof(TEnum), msg.ReadByte());

            float x = msg.ReadFloat();
            float y = msg.ReadFloat();
            Position = new Vector2(x, y);

            IsSelf = msg.ReadBoolean();
        }

        public override void Encode(NetOutgoingMessage msg) {
            base.Encode(msg);

            msg.Write(Convert.ToByte(EntityType));

            msg.Write(Position.X);
            msg.Write(Position.Y);

            msg.Write(IsSelf);
        }
    }
}
