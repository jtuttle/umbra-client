using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrawLib.Entity;
using CrawLib.Entity.Interface;
using Microsoft.Xna.Framework;

namespace UmbraServer.Entities {
    public class Player : Entity {
        public Player(string name, Entity parent, Vector3 position, Quaternion orientation, Game game)
            : base(name, parent, position, orientation, game) {

        }

        public void UpdatePosition(int xInput, int yInput) {
            Position = new Vector3(Position.X + xInput, Position.Y + yInput, 0);
        }
    }
}
