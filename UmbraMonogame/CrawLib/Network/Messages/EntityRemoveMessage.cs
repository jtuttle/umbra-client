using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Lidgren.Network;
using CrawLib.Artemis.Components;
using Microsoft.Xna.Framework;

namespace CrawLib.Network.Messages {
    public class EntityRemoveMessage : EntityMessage {
        public EntityRemoveMessage() : base() { }

        public EntityRemoveMessage(long entityId)
            : base(entityId, NetworkMessageType.EntityRemove) {

        }
    }
}
