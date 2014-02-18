using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using CrawLib.Artemis.Components;

namespace UmbraClient.Spatials {
    internal static class Hero {
        private static Texture2D _sprite;

        public static void Render(SpriteBatch spriteBatch, ContentManager content, TransformComponent transform) {
            if(_sprite == null)
                _sprite = content.Load<Texture2D>("Images/Hero");

            Vector2 position = new Vector2(transform.X - (_sprite.Width * 0.5f), transform.Y - (_sprite.Height * 0.5f));

            spriteBatch.Draw(_sprite, position, _sprite.Bounds, Color.White);
        }
    }
}
