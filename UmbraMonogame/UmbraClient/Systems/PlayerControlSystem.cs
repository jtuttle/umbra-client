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
using Microsoft.Xna.Framework;

namespace UmbraClient.Systems {
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 1)]
    class PlayerControlSystem : TagSystem {
        private NetworkAgent _netAgent;

        private int _updatesPerSecond = 10;
        private double _nextSendUpdates = NetTime.Now;

        public PlayerControlSystem()
            : base("PLAYER") {

        }

        public override void LoadContent() {
            _netAgent = BlackBoard.GetEntry<NetworkAgent>("NetworkAgent");
        }

        public override void Process(Entity entity) {
            TransformComponent transform = entity.GetComponent<TransformComponent>();

            Console.WriteLine(transform.Position);

            KeyboardState keyboardState = Keyboard.GetState();

            float keyMoveSpeed = 0.005f * TimeSpan.FromTicks(EntityWorld.Delta).Milliseconds;

            if(keyboardState.IsKeyDown(Keys.A)) {// || keyboardState.IsKeyDown(Keys.Left)) {
                transform.X -= keyMoveSpeed;
            }

            if(keyboardState.IsKeyDown(Keys.D)) {// || keyboardState.IsKeyDown(Keys.Right)) {
                transform.X += keyMoveSpeed;
            }

            if(keyboardState.IsKeyDown(Keys.W)) {// || keyboardState.IsKeyDown(Keys.Up)) {
                transform.Z -= keyMoveSpeed;
            }

            if(keyboardState.IsKeyDown(Keys.S)) {// || keyboardState.IsKeyDown(Keys.Down)) {
                transform.Z += keyMoveSpeed;
            }

            // send position update message to server at set interval
            // might be better to add this to a queue and send them all at once
            // TODO - delta compression, only send if it changes
            if(NetTime.Now > _nextSendUpdates) {
                Console.WriteLine("sending player position update");

                List<INetworkMessage> outgoingMessages = new List<INetworkMessage>();

                Vector2 position = new Vector2(transform.Position.X, transform.Position.Y);
                outgoingMessages.Add(new EntityMoveMessage(entity.UniqueId, position));

                _netAgent.SendMessages(outgoingMessages);
                
                _nextSendUpdates += (1.0f / _updatesPerSecond);
            }
        }
    }
}
