using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrawLib.Artemis.Components {
    public class NetworkComponent {
        public long UniqueId { get; set; }

        public NetworkComponent(long uniqueId) {
            UniqueId = uniqueId;
        }
    }
}
