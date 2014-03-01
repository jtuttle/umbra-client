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
using Lidgren.Network;

namespace UmbraClient.Systems {
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update)]
    public class PlayerControlSystem : TagSystem {
        private NetworkAgent _netAgent;

        private float _updatesPerSecond = 1.0f;
        private double _nextSendUpdates = NetTime.Now;

        public PlayerControlSystem()
            : base("PLAYER") {

        }

        public override void LoadContent() {
            _netAgent = BlackBoard.GetEntry<NetworkAgent>("NetworkAgent");
        }

        public override void Process(Entity entity) {
            TransformComponent transform = entity.GetComponent<TransformComponent>();

            KeyboardState keyboardState = Keyboard.GetState();

            float keyMoveSpeed = 0.3f * TimeSpan.FromTicks(EntityWorld.Delta).Milliseconds;

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

            // send position update message to server at set interval
            // might be better to add this to a queue and send them all at once
            // TODO - delta compression, only send if it changes
            if(NetTime.Now > _nextSendUpdates) {
                Console.WriteLine("sending player position update");

                List<INetworkMessage> outgoingMessages = new List<INetworkMessage>();

                outgoingMessages.Add(new EntityMoveMessage(entity.UniqueId, transform.Position));

                _netAgent.SendMessages(outgoingMessages);
                
                _nextSendUpdates += (1.0 / _updatesPerSecond);
            }
        }
    }
}
