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
        //private float _scale;

        public override void LoadContent() {
            _content = BlackBoard.GetEntry<ContentManager>("ContentManager");
            _spriteBatch = BlackBoard.GetEntry<SpriteBatch>("SpriteBatch");
            _camera = BlackBoard.GetEntry<Camera2D>("Camera");
        }
        
        public override void Process(Entity entity, TileMapComponent tileMapComponent) {
            if(_sprite == null) {
                _sprite = _content.Load<Texture2D>("Images/OryxEnv");
            }

            float scale = _camera.Scale;

            MapTile[] tiles = tileMapComponent.TileMap.Tiles;

            RectangleF viewBounds = _camera.ViewBounds;

            int tileWidth = TileConfig.TILE_WIDTH;
            int tileHeight = TileConfig.TILE_HEIGHT;

            Coord2D mapDimensions = new Coord2D(tileMapComponent.TileMap.Width, tileMapComponent.TileMap.Height);
            Coord2D topLeft = WorldPositionToTileCoords(new Vector2(viewBounds.X, viewBounds.Y) - new Vector2(tileWidth, tileHeight), mapDimensions, scale);
            Coord2D bottomRight = WorldPositionToTileCoords(new Vector2(viewBounds.X + viewBounds.Width, viewBounds.Y + viewBounds.Height) + new Vector2(tileWidth, tileHeight), mapDimensions, scale);

            for(int y = topLeft.Y; y < bottomRight.Y; y++) {
                for(int x = topLeft.X; x < bottomRight.X; x++) {
                    int index = TileCoordsToArrayIndex(new Coord2D(x, y), tileMapComponent.TileMap.Width);

                    float drawX = index % tileMapComponent.TileMap.Width * tileWidth * scale;
                    float drawY = (float)(int)(index / tileMapComponent.TileMap.Width) * tileHeight * scale;

                    _spriteBatch.Draw(_sprite, new Vector2(drawX, drawY), new Rectangle(32, 224, tileWidth, tileHeight), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                }
            }


            /*
            Viewport viewport = _spriteBatch.GraphicsDevice.Viewport;
            float left = _camera.Position.X - (viewport.Width / 2.0f) - TILE_WIDTH * scale;
            float right = _camera.Position.X + (viewport.Width / 2.0f) + TILE_WIDTH * scale;
            float top = _camera.Position.Y - (viewport.Height / 2.0f) - TILE_HEIGHT * scale;
            float bottom = _camera.Position.Y + (viewport.Height / 2.0f) + TILE_HEIGHT * scale;

            Coord2D mapDimensions = new Coord2D(tileMapComponent.TileMap.Width, tileMapComponent.TileMap.Height);

            Coord2D topLeft = WorldPositionToTileCoords(new Vector2(left, top), mapDimensions, scale);
            Coord2D bottomRight = WorldPositionToTileCoords(new Vector2(right, bottom), mapDimensions, scale);

            for(int y = topLeft.Y; y < bottomRight.Y; y++) {
                for(int x = topLeft.X; x < bottomRight.X; x++) {
                    int index = TileCoordsToArrayIndex(new Coord2D(x, y), tileMapComponent.TileMap.Width);

                    float drawX = index % tileMapComponent.TileMap.Width * TILE_WIDTH * scale;
                    float drawY = (float)(int)(index / tileMapComponent.TileMap.Width) * TILE_HEIGHT * scale;

                    _spriteBatch.Draw(_sprite, new Vector2(drawX, drawY), new Rectangle(32, 224, TILE_WIDTH, TILE_HEIGHT), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                }
            }
            */
        }

        // TODO - so much useless casting, might need to roll my own Floor and Clamp
        private Coord2D WorldPositionToTileCoords(Vector2 worldPos, Coord2D mapDimensions, float scale) {
            int xCoord = (int)(worldPos.X / (TileConfig.TILE_WIDTH * scale));
            int yCoord = (int)(worldPos.Y / (TileConfig.TILE_HEIGHT * scale));

            int x = (int)MathHelper.Clamp((float)xCoord, 0, (float)mapDimensions.X);
            int y = (int)MathHelper.Clamp((float)yCoord, 0, (float)mapDimensions.Y);

            return new Coord2D(x, y);
        }

        private int TileCoordsToArrayIndex(Coord2D tileCoords, int mapWidth) {
            return tileCoords.Y * mapWidth + tileCoords.X;
        }
    }
}
