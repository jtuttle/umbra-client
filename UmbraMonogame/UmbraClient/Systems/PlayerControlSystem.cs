using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.System;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis;
using CrawLib.Artemis.Components;
using Microsoft.Xna.Framework.Input;
using CrawLib.Network;
using CrawLib.Network.Messages;

namespace UmbraClient.Systems {
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update)]
    public class PlayerControlSystem : TagSystem {
        public PlayerControlSystem()
            : base("PLAYER") {

        }

        public override void Process(Entity entity) {
            TransformComponent transform = entity.GetComponent<TransformComponent>();
            KeyboardState keyboardState = Keyboard.GetState();

            float keyMoveSpeed = 0.3f * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;

            if(keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left)) {
                transform.X -= keyMoveSpeed;
            } 

            if(keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right)) {
                transform.X += keyMoveSpeed;
            }

            if(keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up)) {
                transform.Y -= keyMoveSpeed;
            }

            if(keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down)) {
                transform.Y += keyMoveSpeed;
            }

            // this should perhaps be a different type of message to distinguish it from entities that 
            // the server/other players are moving
            //NetworkAgent.MessageQueue.Enqueue(new EntityMoveMessage(entity.UniqueId, transform.Position));
        }
    }
}
