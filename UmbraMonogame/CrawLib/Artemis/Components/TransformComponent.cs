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

        private float _z;
        public float Z {
            get { return _z; }
            set {
                if(_z != value) Dirty = true;
                _z = value;
            }
        }

        public Vector3 Position {
            get { return new Vector3(this.X, this.Y, this.Z); }
            set {
                this.X = value.X;
                this.Y = value.Y;
                this.Z = value.Z;
            }
        }

        public bool Dirty { get; set; }

        public TransformComponent()
            : this(Vector3.Zero) {

        }

        public TransformComponent(float x, float y, float z)
            : this(new Vector3(x, y, z)) {

        }

        public TransformComponent(Vector3 position) {
            Position = position;

            Dirty = false;
        }
    }
}
