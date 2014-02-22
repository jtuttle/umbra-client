using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Lidgren.Network;
using CrawLib.Artemis.Components;
using Microsoft.Xna.Framework;

namespace CrawLib.Network.Messages {
    public class EntityAddMessage<TEnum> : EntityMessage where TEnum : byte {
        public TEnum EntityType { get; private set; }
        public Vector2 Position { get; private set; }

        public EntityAddMessage(long entityId, TEnum entityType, Vector2 position)
            : base(entityId, NetworkMessageType.EntityAdd) {

            EntityType = entityType;
            Position = position;
        }

        public override void Decode(NetIncomingMessage msg) {
            EntityId = msg.ReadInt64();

            // read entity type

            int x = msg.ReadInt32();
            int y = msg.ReadInt32();
            Position = new Vector2(x, y);
        }

        public override void Encode(NetOutgoingMessage msg) {
            msg.Write(EntityId);

            msg.Write((byte)EntityType);

            msg.Write(Position.X);
            msg.Write(Position.Y);
        }
    }
}
