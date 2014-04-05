using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrawLib.Artemis;
using CrawLib.Network.Messages;
using Artemis;
using Microsoft.Xna.Framework;
using UmbraLib;
using CrawLib.Artemis.Components;
using UmbraLib.Components;

namespace UmbraClient {
    public class ClientEntityFactory : IEntityFactory {
        private EntityWorld _entityWorld;

        public ClientEntityFactory(EntityWorld entityWorld) {
            _entityWorld = entityWorld;
        }

        public Entity CreatePlayer(long? entityId, Vector3 position) {
            Entity player = _entityWorld.CreateEntity(entityId);

            player.AddComponent(new UmbraEntityTypeComponent(UmbraEntityType.Player));
            player.AddComponent(new TransformComponent(position));
            player.AddComponent(new VelocityComponent());
            player.AddComponent(new SpatialFormComponent("Hero"));

            return player;
        }

        public Entity CreateNPC(long? entityId, Vector3 position) {
            Entity npc = _entityWorld.CreateEntity(entityId);

            npc.AddComponent(new UmbraEntityTypeComponent(UmbraEntityType.NPC));
            npc.AddComponent(new TransformComponent(position));
            npc.AddComponent(new VelocityComponent());
            npc.AddComponent(new SpatialFormComponent("NPC"));

            return npc;
        }
    }
}
