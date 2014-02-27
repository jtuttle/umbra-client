using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.System;
using UmbraServer.Components;
using Artemis;
using CrawLib.Artemis.Components;

namespace UmbraServer.Systems {
    class AiSystem : EntityComponentProcessingSystem<AiComponent, TransformComponent> {
        public override void Process(Entity entity, AiComponent ai, TransformComponent transform) {
            Random rnd = new Random();

            transform.X += rnd.Next() * 5;
            transform.Y += rnd.Next() * 5;
        }
    }
}
