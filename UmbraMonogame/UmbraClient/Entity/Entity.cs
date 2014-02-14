using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using UmbraClient.Entity.Interface;

namespace UmbraClient.Entity {
    public abstract class Entity {
        public string Name { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Orientation { get; private set; }
        public Entity Parent { get; private set; }

        protected Game _game;

        public Entity() {
            throw new InvalidOperationException("Entities should be created through EntityFactory");
        }

        public Entity(string name, Entity parent, Vector3 position, Quaternion orientation, Game game) {
            Name = name;
            Parent = parent;
            Position = position;
            Orientation = orientation;
            _game = game;
        }

        // public void ProcessMessage
    }
}
