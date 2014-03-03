using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using CrawLib.Artemis.Components;
using Artemis;
using CrawLib.TileMap;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace UmbraClient.Systems {
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = 0)]
    public class TilemapRenderSystem : EntityComponentProcessingSystem<TileMapComponent> {
        private ContentManager _content;
        private SpriteBatch _spriteBatch;

        private Texture2D _sprite;
        private float _scale;

        // todo - move these
        private const int TILE_WIDTH = 32;
        private const int TILE_HEIGHT = 32;

        public override void LoadContent() {
            _content = BlackBoard.GetEntry<ContentManager>("ContentManager");
            _spriteBatch = BlackBoard.GetEntry<SpriteBatch>("SpriteBatch");

            // get camera from blackboard
        }
        
        public override void Process(Entity entity, TileMapComponent tileMapComponent) {
            if(_sprite == null) {
                _sprite = _content.Load<Texture2D>("Images/OryxEnv");
                //_bounds = new Rectangle(169, 25, 22, 22); // per tile
                _scale = 2.0f;
            }

            MapTile[] tiles = tileMapComponent.TileMap.Tiles;

            for(int i = 0; i < tiles.Length; i++) {
                float x = i % tileMapComponent.TileMap.Width * TILE_WIDTH * _scale;
                float y = (float)(int)(i / tileMapComponent.TileMap.Width) * TILE_HEIGHT * _scale;

                Rectangle bounds = new Rectangle(32, 224, TILE_WIDTH, TILE_HEIGHT);

                _spriteBatch.Draw(_sprite, new Vector2(x, y), bounds, Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
            }
        }
    }
}
