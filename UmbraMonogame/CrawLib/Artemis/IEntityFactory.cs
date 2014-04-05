using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Microsoft.Xna.Framework;

namespace CrawLib.Artemis {
    public interface IEntityFactory {
        Entity CreatePlayer(long? entityId, Vector3 position);
        Entity CreateNPC(long? entityId, Vector3 position);
    }
}
