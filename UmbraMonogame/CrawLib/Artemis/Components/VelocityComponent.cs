using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Interface;

namespace CrawLib.Artemis.Components {
    public class VelocityComponent : IComponent {
        public float Speed { get; set; }
        public float Angle { get; set; }

        public VelocityComponent() 
            : this(0, 0) {

        }

        public VelocityComponent(float speed) 
            : this(speed, 0) {

        }

        public VelocityComponent(float speed, float angle) {
            Speed = speed;
            Angle = angle;
        }
    }
}
