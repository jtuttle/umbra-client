using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.System;
using Artemis;
using Microsoft.Xna.Framework;
using CrawLib.Artemis.Components;
using CrawLib.Network;
using CrawLib.Network.Messages;

namespace UmbraServer.Systems {
    public class MovementSystem : EntityComponentProcessingSystem<TransformComponent, VelocityComponent> {
        public override void Process(Entity entity, TransformComponent transform, VelocityComponent velocity) {
            if(transform != null && velocity != null) {
                float ms = TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;

                transform.X += (float)(Math.Cos(MathHelper.ToRadians(velocity.Angle)) * velocity.Speed * ms);
                transform.Y += (float)(Math.Sin(MathHelper.ToRadians(velocity.Angle)) * velocity.Speed * ms);

                NetworkAgent.MessageQueue.Enqueue(new EntityMoveMessage(entity.UniqueId, transform.Position));
            }
        }
    }
}
