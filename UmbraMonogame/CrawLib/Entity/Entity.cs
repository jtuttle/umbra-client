using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CrawLib.Entity.Interface;

namespace CrawLib.Entity {
    public abstract class Entity {
        public string Name { get; protected set; }
        public Vector3 Position { get; protected set; }
        public Quaternion Orientation { get; protected set; }
        public Entity Parent { get; protected set; }

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
