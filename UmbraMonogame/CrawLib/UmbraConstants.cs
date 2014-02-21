using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrawLib {
    public class UmbraConstants {
        public enum NetworkEventType {
            EntityAdd,
            EntityRemove,
            EntityMove
        }

        public enum EntityType {
            Player,
            Dog,
            Cactus
        }
    }
}
