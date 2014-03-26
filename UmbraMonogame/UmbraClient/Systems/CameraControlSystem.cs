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

namespace UmbraClient.Systems {
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 0)]
    class CameraControlSystem : EntityComponentProcessingSystem<CameraComponent> {
        private float _speed = 1.0f;

        public override void LoadContent() {
            
        }

        public override void Process(Entity entity, CameraComponent cameraComponent) {
            KeyboardState keyboardState = Keyboard.GetState();

            if(keyboardState.IsKeyDown(Keys.W))
                cameraComponent.MoveCamera(CameraMovement.Forward);
            if(keyboardState.IsKeyDown(Keys.S))
                cameraComponent.MoveCamera(CameraMovement.Backward);
            if(keyboardState.IsKeyDown(Keys.A))
                cameraComponent.MoveCamera(CameraMovement.Left);
            if(keyboardState.IsKeyDown(Keys.D))
                cameraComponent.MoveCamera(CameraMovement.Right);
            if(keyboardState.IsKeyDown(Keys.PageUp))
                cameraComponent.MoveCamera(CameraMovement.Up);
            if(keyboardState.IsKeyDown(Keys.PageDown))
                cameraComponent.MoveCamera(CameraMovement.Down);

            cameraComponent.UpdateViewMatrix();
        }
    }
}
