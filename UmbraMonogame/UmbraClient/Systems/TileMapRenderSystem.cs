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
using CrawLib;

namespace UmbraClient.Systems {
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = 0)]
    public class TilemapRenderSystem : EntityComponentProcessingSystem<TileMapComponent> {
        private ContentManager _content;
        private SpriteBatch _spriteBatch;
        private Camera2D _camera;

        private Texture2D _sprite;
        private float _scale;

        // todo - move these
        private const int TILE_WIDTH = 32;
        private const int TILE_HEIGHT = 32;

        public override void LoadContent() {
            _content = BlackBoard.GetEntry<ContentManager>("ContentManager");
            _spriteBatch = BlackBoard.GetEntry<SpriteBatch>("SpriteBatch");
            _camera = BlackBoard.GetEntry<Camera2D>("Camera");
        }
        
        public override void Process(Entity entity, TileMapComponent tileMapComponent) {
            if(_sprite == null) {
                _sprite = _content.Load<Texture2D>("Images/OryxEnv");
                //_bounds = new Rectangle(169, 25, 22, 22); // per tile
                _scale = 2.0f;
            }

            MapTile[] tiles = tileMapComponent.TileMap.Tiles;

            Rectangle bounds = new Rectangle(32, 224, TILE_WIDTH, TILE_HEIGHT);

            // opt - tons of repeated calculations here

            Viewport viewport = _spriteBatch.GraphicsDevice.Viewport;
            float left = _camera.Position.X - (viewport.Width / 2.0f) - TILE_WIDTH * _scale;
            float right = _camera.Position.X + (viewport.Width / 2.0f) + TILE_WIDTH * _scale;
            float top = _camera.Position.Y - (viewport.Height / 2.0f) - TILE_HEIGHT * _scale;
            float bottom = _camera.Position.Y + (viewport.Height / 2.0f) + TILE_HEIGHT * _scale;

            Coord2D mapDimensions = new Coord2D(tileMapComponent.TileMap.Width, tileMapComponent.TileMap.Height);

            Coord2D topLeft = WorldPositionToTileCoords(new Vector2(left, top), mapDimensions, _scale);
            Coord2D bottomRight = WorldPositionToTileCoords(new Vector2(right, bottom), mapDimensions, _scale);

            for(int y = topLeft.Y; y < bottomRight.Y; y++) {
                for(int x = topLeft.X; x < bottomRight.X; x++) {
                    int index = TileCoordsToArrayIndex(new Coord2D(x, y), tileMapComponent.TileMap.Width);

                    float drawX = index % tileMapComponent.TileMap.Width * TILE_WIDTH * _scale;
                    float drawY = (float)(int)(index / tileMapComponent.TileMap.Width) * TILE_HEIGHT * _scale;

                    _spriteBatch.Draw(_sprite, new Vector2(drawX, drawY), bounds, Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
                }
            }

            //for(int i = 0; i < tiles.Length; i++) {
            //    float x = i % tileMapComponent.TileMap.Width * TILE_WIDTH * _scale;
            //    float y = (float)(int)(i / tileMapComponent.TileMap.Width) * TILE_HEIGHT * _scale;

            //    _spriteBatch.Draw(_sprite, new Vector2(x, y), bounds, Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
            //}
        }

        // TODO - so much useless casting, might need to roll my own Floor and Clamp
        private Coord2D WorldPositionToTileCoords(Vector2 worldPos, Coord2D mapDimensions, float scale) {
            int xCoord = (int)(worldPos.X / (TILE_WIDTH * scale));
            int yCoord = (int)(worldPos.Y / (TILE_HEIGHT * scale));

            int x = (int)MathHelper.Clamp((float)xCoord, 0, (float)mapDimensions.X);
            int y = (int)MathHelper.Clamp((float)yCoord, 0, (float)mapDimensions.Y);

            return new Coord2D(x, y);
        }

        private int TileCoordsToArrayIndex(Coord2D tileCoords, int mapWidth) {
            return tileCoords.Y * mapWidth + tileCoords.X;
        }
    }
}
