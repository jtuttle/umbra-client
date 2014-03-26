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

        public CameraComponent(GraphicsDevice graphics, Vector3 position) {
            Position = position;
            Rotation = Matrix.Identity;

            View = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);

            float ratio = (float)graphics.Viewport.Width / graphics.Viewport.Height;
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, ratio, 0.1f, 100.0f);

            MoveSpeed = 1.0f;
        }

        public void MoveCamera(CameraMovement movement) {
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

        public void UpdateViewMatrix() {
            Target = Position + Rotation.Forward;
            View = Matrix.CreateLookAt(Position, Target, Rotation.Up);
        }
    }
}
