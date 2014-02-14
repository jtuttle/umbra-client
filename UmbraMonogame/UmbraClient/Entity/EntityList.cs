using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UmbraClient.Entity.Interface;

namespace UmbraClient.Entity {
    public class EntityList : IUpdate, IDraw {
        public List<Entity> Entities;

        private List<IUpdate> _updatableEntities;
        private List<IDraw> _drawableEntitites;

        public EntityList() {
            Entities = new List<Entity>();
            _updatableEntities = new List<IUpdate>();
            _drawableEntitites = new List<IDraw>();
        }

        public void Add(Entity entity) {
            Entities.Add(entity);

            IUpdate updatable = entity as IUpdate;

            if(updatable != null)
                _updatableEntities.Add(updatable);

            IDraw drawable = entity as IDraw;

            if(drawable != null)
                _drawableEntitites.Add(drawable);
        }

        public void Remove(Entity entity) {
            Entities.Remove(entity);

            IUpdate updatable = entity as IUpdate;

            if(updatable != null)
                _updatableEntities.Remove(updatable);

            IDraw drawable = entity as IDraw;

            if(drawable != null)
                _drawableEntitites.Remove(drawable);
        }

        public void Update() {
            foreach(IUpdate updatable in _updatableEntities)
                updatable.Update();
        }

        public void Draw() {
            foreach(IDraw drawable in _drawableEntitites)
                drawable.Draw();
        }
    }
}
