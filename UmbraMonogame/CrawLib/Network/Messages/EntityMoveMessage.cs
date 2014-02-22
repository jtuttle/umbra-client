using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Artemis;
using CrawLib.Artemis.Components;
using Microsoft.Xna.Framework;

namespace CrawLib.Network.Messages {
    public class EntityMoveMessage : EntityMessage {
        public Vector2 Position { get; set; }

        // TODO: instead of passing an entity in, maybe stick to primitive types?
        // breaks coupling with Artemis
        public EntityMoveMessage(long entityId, Vector2 position)
            : base(entityId, NetworkMessageType.EntityMove) {

            Position = position;
        }

        public override void Decode(NetIncomingMessage msg) {
            long entityId = msg.ReadInt64();

            // read entity type

            int x = msg.ReadInt32();
            int y = msg.ReadInt32();
            Position = new Vector2(x, y);
        }

        public override void Encode(NetOutgoingMessage msg) {
            msg.Write(EntityId);

            // type of entity

            msg.Write(Position.X);
            msg.Write(Position.Y);
        }
    }
}
