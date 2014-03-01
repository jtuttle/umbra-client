using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.System;
using UmbraServer.Components;
using Artemis;
using CrawLib.Artemis.Components;
using Artemis.Attributes;
using Artemis.Manager;

namespace UmbraServer.Systems {
    // TODO: probably need to make sure this takes priority over movementsystem
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 1)]
    class AiSystem : EntityComponentProcessingSystem<AiComponent, TransformComponent> {
        private Random _rnd = new Random();

        // temp
        private bool _right = true;

        public override void Process(Entity entity, AiComponent ai, TransformComponent transform) {
            if(_right)
                transform.X += 0.1f;
            else
                transform.X -= 0.1f;

            if(transform.X > 200 || transform.X < 0)
                _right = !_right;
        }
    }
}
