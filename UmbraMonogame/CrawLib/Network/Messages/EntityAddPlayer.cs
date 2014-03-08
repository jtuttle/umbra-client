using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Lidgren.Network;
using CrawLib.Artemis.Components;
using Microsoft.Xna.Framework;

namespace CrawLib.Network.Messages {
    public class EntityAddPlayer<TEnum> : EntityMessage {
        public TEnum EntityType { get; private set; }
        public Vector2 Position { get; private set; }

        public EntityAddPlayer() : base() { }

        public EntityAddPlayer(long entityId, TEnum entityType, Vector2 position)
            : base(entityId, NetworkMessageType.EntityAddPlayer) {

            EntityType = entityType;
            Position = position;
        }

        public override void Decode(NetIncomingMessage msg) {
            base.Decode(msg);

            EntityType = (TEnum)Enum.ToObject(typeof(TEnum), msg.ReadByte());

            float x = msg.ReadFloat();
            float y = msg.ReadFloat();
            Position = new Vector2(x, y);
        }

        public override void Encode(NetOutgoingMessage msg) {
            base.Encode(msg);

            msg.Write(Convert.ToByte(EntityType));

            msg.Write(Position.X);
            msg.Write(Position.Y);
        }
    }
}
