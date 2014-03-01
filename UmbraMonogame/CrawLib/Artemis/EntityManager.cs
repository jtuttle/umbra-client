using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;

namespace CrawLib.Artemis {
    public class EntityManager {
        private static EntityManager _instance;

        private EntityWorld _entityWorld;
        private Dictionary<long, Entity> _entities;

        private EntityManager() {
            _entities = new Dictionary<long, Entity>();
        }

        public static EntityManager Instance {
            get {
                if(_instance == null)
                    _instance = new EntityManager();

                return _instance;
            }
        }

        public void Initialize(EntityWorld entityWorld) {
            _entityWorld = entityWorld;

            _entityWorld.EntityManager.AddedEntityEvent += OnEntityAdded;
            _entityWorld.EntityManager.RemovedEntityEvent += OnEntityRemoved;
        }

        private void OnEntityAdded(Entity entity) {
            _entities[entity.UniqueId] = entity;
        }

        private void OnEntityRemoved(Entity entity) {
            _entities.Remove(entity.UniqueId);
        }
    }
}
