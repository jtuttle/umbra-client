using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Interface;
using Microsoft.Xna.Framework;

namespace CrawLib.Artemis.Components {
    public class TransformComponent : IComponent {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2 Position {
            get { return new Vector2(this.X, this.Y); }
            set {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public TransformComponent()
            : this(Vector2.Zero) {

        }

        public TransformComponent(float x, float y)
            : this(new Vector2(x, y)) {
        
        }

        public TransformComponent(Vector2 position) {
            Position = position;
        }
    }
}
