using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrawLib.Artemis;
using Artemis;
using Microsoft.Xna.Framework;
using CrawLib.Artemis.Components;
using UmbraLib.Components;
using UmbraLib;
using UmbraServer.Components;

namespace UmbraServer {
    public class ServerEntityFactory : IEntityFactory {
        private EntityWorld _entityWorld;

        public ServerEntityFactory(EntityWorld entityWorld) {
            _entityWorld = entityWorld;
        }

        public Entity CreatePlayer(long? entityId, Vector2 position) {
            Entity player = _entityWorld.CreateEntity(entityId);

            player.AddComponent(new TransformComponent(position));
            player.AddComponent(new VelocityComponent());
            player.AddComponent(new UmbraEntityTypeComponent(UmbraEntityType.Player));

            return player;
        }

        public Entity CreateNPC(long? entityId, Vector2 position) {
            Entity npc = _entityWorld.CreateEntity();

            npc.AddComponent(new UmbraEntityTypeComponent(UmbraEntityType.NPC));
            npc.AddComponent(new TransformComponent(position));
            npc.AddComponent(new VelocityComponent());
            npc.AddComponent(new AiComponent());

            return npc;
        }
    }
}
