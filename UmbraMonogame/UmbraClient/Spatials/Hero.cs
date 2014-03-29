using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using CrawLib.Artemis.Components;
using CrawLib.Shapes;
using CrawLib;

namespace UmbraClient.Spatials {
    internal static class Hero {
        private static QuadShape _quad;
        private static Texture2D _texture;

        public static void Render(ContentManager content, GraphicsDevice graphicsDevice, TransformComponent transform, BasicEffect effect) {
            if(_quad == null) {
                _texture = content.Load<Texture2D>("Images/OryxChar");
                TextureFrame textureFrame = new TextureFrame(0.292f, 0.031f, 0.042f, 0.031f);
                _quad = new QuadShape(transform.Position, Vector3.Backward, Vector3.Up, textureFrame, 0.5f, 0.5f);
            }

            _quad.Origin = transform.Position + new Vector3(0, _quad.Height / 2.0f, 0);
            _quad.UpdateVertexData();

            effect.Texture = _texture;

            graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, _quad.Vertices, 0, 4, _quad.Indexes, 0, 2);
        }
    }
}
