using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using CrawLib.Artemis.Components;

namespace UmbraClient.Spatials {
    internal static class NPC {
        private static Texture2D _sprite;
        private static Rectangle _bounds;
        private static float _scale;
        
        public static void Render(SpriteBatch spriteBatch, ContentManager content, TransformComponent transform) {
            if(_sprite == null) {
                _sprite = content.Load<Texture2D>("Images/OryxChar");
                _bounds = new Rectangle(193, 25, 22, 22);
                _scale = 2.0f;
            }

            Vector2 position = new Vector2(transform.X - (_bounds.Width * 0.5f), transform.Y - (_bounds.Height * 0.5f));

            spriteBatch.Draw(_sprite, position, _bounds, Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
        }
    }
}
