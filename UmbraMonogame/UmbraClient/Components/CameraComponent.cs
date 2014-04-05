using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Interface;
using CrawLib.Artemis.Components;
using Microsoft.Xna.Framework;

namespace UmbraClient.Components {
    public class CameraComponent : IComponent {
        public Vector3 Position { get; set; }
        public Matrix Rotation { get; set; }

        public TransformComponent Target { get; set; }

        public Matrix View { get; private set; }
        public Matrix Projection { get; private set; }

        public CameraComponent(Vector3 position, Matrix rotation, float aspectRatio) {
            Position = position;
            Rotation = rotation;

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 100.0f);
        }

        public void UpdateViewMatrix() {
            if(Target == null) return;

            View = Matrix.CreateLookAt(Position, Target.Position, Rotation.Up);
        }
    }
}
