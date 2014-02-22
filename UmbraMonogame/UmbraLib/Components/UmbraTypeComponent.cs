using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UmbraLib;

namespace UmbraLib.Components {
    public class UmbraTypeComponent {
        public UmbraEntityType EntityType { get; set; }

        public UmbraTypeComponent(UmbraEntityType entityType) {
            EntityType = entityType;
        }
    }
}
