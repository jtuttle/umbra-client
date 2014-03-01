using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Interface;

namespace UmbraLib.Components {
    public class UmbraEntityTypeComponent : IComponent {
        public UmbraEntityType EntityType { get; private set; }

        public UmbraEntityTypeComponent(UmbraEntityType entityType) {
            EntityType = entityType;
        }
    }
}
