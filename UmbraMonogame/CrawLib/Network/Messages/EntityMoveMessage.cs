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

        public EntityMoveMessage() : base() { }

        public EntityMoveMessage(long entityId, Vector2 position)
            : base(entityId, NetworkMessageType.EntityMove) {

            Position = position;
        }

        public override void Decode(NetIncomingMessage msg) {
            base.Decode(msg);

            // opt - should probably use ints here
            float x = msg.ReadFloat();
            float y = msg.ReadFloat(); 
            Position = new Vector2(x, y);
        }

        public override void Encode(NetOutgoingMessage msg) {
            base.Encode(msg);

            // opt - should probably use ints here
            msg.Write(Position.X);
            msg.Write(Position.Y);
        }
    }
}
