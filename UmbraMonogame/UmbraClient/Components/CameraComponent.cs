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

    public class CameraComponent : IComponent {
        public Vector3 Position { get; private set; }
        public Matrix Rotation { get; private set; }

        public Matrix Projection { get; private set; }
        public Matrix View { get; private set; }

        public Vector3 Target { get; private set; }

        public float MoveSpeed { get; private set; }

        private float _yaw;
        private float _pitch;
        private float _roll;

        public CameraComponent(GraphicsDevice graphics, Vector3 position, Matrix rotation) {
            Position = position;
            Rotation = rotation;

            View = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);

            float ratio = (float)graphics.Viewport.Width / graphics.Viewport.Height;
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, ratio, 0.1f, 100.0f);

            MoveSpeed = 0.2f;
        }

        public void TranslateCamera(CameraMovement movement) {
            switch(movement) {
                case CameraMovement.Forward:
                    Position += Rotation.Forward * MoveSpeed;
                    break;
                case CameraMovement.Backward:
                    Position += Rotation.Backward * MoveSpeed;
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
    }
}
