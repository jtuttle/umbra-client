using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using Artemis;
using UmbraClient.Components;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using CrawLib.Artemis.Components;

namespace UmbraClient.Systems {
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 2)]
    class CameraUpdateSystem : EntityComponentProcessingSystem<CameraComponent> {
        private Game _game;

        private MouseState _lastMouseState;

        public override void LoadContent() {
            _game = BlackBoard.GetEntry<Game>("Game");
        }

        public override void Process(Entity entity, CameraComponent cameraComponent) {
            TransformComponent transform = cameraComponent.Target;

            cameraComponent.Position = new Vector3(transform.X, transform.Y + 10, transform.Z + 5);
            cameraComponent.UpdateViewMatrix();

            //KeyboardState keyboardState = Keyboard.GetState();
            //MouseState mouseState = Mouse.GetState();

            /*
            if(keyboardState.IsKeyDown(Keys.Up))
                cameraComponent.TranslateCamera(CameraMovement.Forward);
            if(keyboardState.IsKeyDown(Keys.Down))
                cameraComponent.TranslateCamera(CameraMovement.Backward);
            if(keyboardState.IsKeyDown(Keys.Left))
                cameraComponent.TranslateCamera(CameraMovement.Left);
            if(keyboardState.IsKeyDown(Keys.Right))
                cameraComponent.TranslateCamera(CameraMovement.Right);
            if(keyboardState.IsKeyDown(Keys.PageUp))
                cameraComponent.TranslateCamera(CameraMovement.Up);
            if(keyboardState.IsKeyDown(Keys.PageDown))
                cameraComponent.TranslateCamera(CameraMovement.Down);

            int centerX = _game.Window.ClientBounds.Width / 2;
            int centerY = _game.Window.ClientBounds.Height / 2;

            if(mouseState.LeftButton == ButtonState.Pressed) {
                float yaw = MathHelper.ToRadians((mouseState.X - _lastMouseState.X) * 0.05f);
                float pitch = MathHelper.ToRadians((mouseState.Y - _lastMouseState.Y) * 0.05f);

                cameraComponent.RotateCamera(yaw, pitch, 0);
            }

            cameraComponent.UpdateViewMatrix();

            _lastMouseState = mouseState;
            */
        }
    }
}
