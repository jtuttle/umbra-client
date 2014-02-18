using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Interface;
using Microsoft.Xna.Framework;

namespace CrawLib.Artemis.Components {
    public class TransformComponent : IComponent {
        private float _x;
        public float X {
            get { return _x; }
            set {
                if(_x != value)
                    Dirty = true;
                _x = value;
            }
        }

        private float _y;
        public float Y {
            get { return _y; }
            set {
                if(_y != value) Dirty = true;
                _y = value;
            }
        }

        public Vector2 Position {
            get { return new Vector2(this.X, this.Y); }
            set {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public bool Dirty { get; set; }

        public TransformComponent()
            : this(Vector2.Zero) {

        }

        public TransformComponent(float x, float y)
            : this(new Vector2(x, y)) {

        }

        public TransformComponent(Vector2 position) {
            Position = position;

            Dirty = false;
        }
    }
}
