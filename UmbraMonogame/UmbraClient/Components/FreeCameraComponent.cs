using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbraClient.Components {
    public enum CameraMovement {
        Left, Right, Forward, Backward, Up, Down
    }

    public class FreeCameraComponent : IComponent {
        public Vector3 Position { get; set; }
        public Matrix Rotation { get; private set; }

        public Matrix Projection { get; private set; }
        public Matrix View { get; private set; }

        public Vector3 Target { get; private set; }

        public float MoveSpeed { get; private set; }

        private float _yaw;
        private float _pitch;
        private float _roll;

        public FreeCameraComponent(Vector3 position, Matrix rotation, GraphicsDevice graphics) {
            Position = position;
            Rotation = rotation;

            View = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);

            float ratio = (float)graphics.Viewport.Width / graphics.Viewport.Height;
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, ratio, 0.1f, 100.0f);

            MoveSpeed = 0.1f;
        }

        public void TranslateCamera(CameraMovement movement) {
            switch(movement) {
                case CameraMovement.Forward:
                    //Position += Rotation.Forward * MoveSpeed;
                    Position -= new Vector3(0, 0, MoveSpeed);
                    break;
                case CameraMovement.Backward:
                    //Position += Rotation.Backward * MoveSpeed;
                    Position += new Vector3(0, 0, MoveSpeed);
                    break;
                case CameraMovement.Left:
                    Position += Rotation.Left * MoveSpeed;
                    break;
                case CameraMovement.Right:
                    Position += Rotation.Right * MoveSpeed;
                    break;
                case CameraMovement.Up:
                    Position += Rotation.Up * MoveSpeed;
                    break;
                case CameraMovement.Down:
                    Position += Rotation.Down * MoveSpeed;
                    break;
                default: break;
            }
        }

        public void RotateCamera(float yaw, float pitch, float roll) {
            _yaw += yaw;
            _pitch += pitch;
            _roll += roll;
        }

        public void UpdateViewMatrix() {
            Rotation.Forward.Normalize();
            Rotation.Up.Normalize();
            Rotation.Right.Normalize();

            Rotation *= Matrix.CreateFromAxisAngle(Rotation.Right, _pitch);
            Rotation *= Matrix.CreateFromAxisAngle(Rotation.Up, _yaw);
            Rotation *= Matrix.CreateFromAxisAngle(Rotation.Forward, _roll);

            _yaw = 0.0f;
            _pitch = 0.0f;
            _roll = 0.0f;

            Target = Position + Rotation.Forward;

            View = Matrix.CreateLookAt(Position, Target, Rotation.Up);
        }

        /* from the related system: */
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
