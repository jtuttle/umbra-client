using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrawLib.Entity;
using CrawLib.Entity.Interface;
using UmbraMonogame;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UmbraClient.Entities {
    public class Player : Entity, IUpdate, IDraw {
        private SpriteBatch _spriteBatch;

        private Texture2D _hero;

        public Player(string name, Entity parent, Vector3 position, Quaternion orientation, Game game)
            : base(name, parent, position, orientation, game) {

            _spriteBatch = (_game as UmbraGame).spriteBatch;

            _hero = (_game as UmbraGame).Content.Load<Texture2D>("Images/Hero");
        }

        public void Update() {

        }

        public void Draw() {
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            _spriteBatch.Draw(_hero, new Vector2(Position.X, Position.Y), Color.White);
            _spriteBatch.End();
        }

        public void UpdatePosition(float xPos, float yPos) {
            Position = new Vector3(xPos, yPos, 0);
        }
    }
}
