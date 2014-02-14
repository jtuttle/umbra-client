using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UmbraClient.Entity.Interface;
using Microsoft.Xna.Framework;

namespace UmbraClient.Entity {
    public class World : Entity {
        public World(string name, Entity parent, Vector3 position, Quaternion orientation, Game game) 
            : base(name, parent, position, orientation, game) {
        
        }
    }
}
