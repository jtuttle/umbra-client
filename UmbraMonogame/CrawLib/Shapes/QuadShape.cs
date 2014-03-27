using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawLib.Shapes {
    public class QuadShape {
        public Vector3 Origin { get; private set; }
        public Vector3 Normal { get; private set; }
        public Vector3 Up { get; private set; }

        public float Width { get; private set; }
        public float Height { get; private set; }

        public VertexPositionNormalTexture[] Vertices;
        public short[] Indexes;

        public QuadShape(Vector3 origin, Vector3 normal, Vector3 up, float width = 1.0f, float height = 1.0f) {
            Origin = origin;
            Normal = normal;
            Up = up;

            Width = width;
            Height = height;

            UpdateVertexData();
        }

        public void UpdateVertexData() {
            Vertices = new VertexPositionNormalTexture[4];

            Vector3 left = Vector3.Cross(Normal, Up);
            Vector3 upperCenter = (Up * Height / 2) + Origin;

            // vertex coordinates
            Vector3 upperLeft = upperCenter + (left * Width / 2);
            Vector3 upperRight = upperCenter - (left * Width / 2);
            Vector3 lowerLeft = upperLeft - (Up * Height);
            Vector3 lowerRight = upperRight - (Up * Height);

            // texture coordinates
            Vector2 textureUpperLeft = new Vector2(0, 0);
            Vector2 textureUpperRight = new Vector2(0, 1);
            Vector2 textureLowerLeft = new Vector2(1, 0);
            Vector2 textureLowerRight = new Vector2(1, 1);

            Vertices = new VertexPositionNormalTexture[4] {
                new VertexPositionNormalTexture(lowerLeft, Normal, textureLowerLeft),
                new VertexPositionNormalTexture(upperLeft, Normal, textureUpperLeft),
                new VertexPositionNormalTexture(lowerRight, Normal, textureLowerRight),
                new VertexPositionNormalTexture(upperRight, Normal, textureUpperRight)
            };

            Indexes = new short[6] { 0, 1, 2, 2, 1, 3 };
        }
    }
}
