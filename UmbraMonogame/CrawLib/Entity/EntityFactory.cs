using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CrawLib.Entity.Interface;

namespace CrawLib.Entity {
    public class EntityFactory {
        public static IEntityGame Game;

        public static Entity CreateEntity(Type entityType, string name, Entity parent, Vector3 position, Quaternion orientation) {
            Entity entity = (Entity)Activator.CreateInstance(entityType, name, parent, position, orientation, Game);

            if(Game != null)
                Game.Entities.Add(entity);

            return entity;
        }

        public static Entity CreateEntity(Type entityType, string name, Entity parent) {
            return CreateEntity(entityType, name, parent, Vector3.Zero, Quaternion.Identity);
        }
    }
}
