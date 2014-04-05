using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Lidgren.Network;
using CrawLib.Artemis.Components;
using Microsoft.Xna.Framework;

namespace CrawLib.Network.Messages {
    public class EntityAddMessage<TEnum> : EntityMessage {
        public TEnum EntityType { get; private set; }
        public Vector3 Position { get; private set; }

        public EntityAddMessage() : base() { }

        public EntityAddMessage(long entityId, TEnum entityType, Vector3 position)
            : base(entityId, NetworkMessageType.EntityAdd) {

            EntityType = entityType;
            Position = position;
        }

        public override void Decode(NetIncomingMessage msg) {
            base.Decode(msg);

            EntityType = (TEnum)Enum.ToObject(typeof(TEnum), msg.ReadByte());

            float x = msg.ReadFloat();
            float y = msg.ReadFloat();
            float z = msg.ReadFloat();
            Position = new Vector3(x, y, z);
        }

        public override void Encode(NetOutgoingMessage msg) {
            base.Encode(msg);

            msg.Write(Convert.ToByte(EntityType));

            msg.Write(Position.X);
            msg.Write(Position.Y);
            msg.Write(Position.Z);
        }
    }
}
