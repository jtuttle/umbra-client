using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using Artemis;
using CrawLib;
using CrawLib.Artemis.Components;
using CrawLib.Shapes;
using CrawLib.TileMap;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using UmbraClient.Components;

namespace UmbraClient.Systems {
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = 0)]
    public class TilemapRenderSystem : EntityComponentProcessingSystem<TileMapComponent> {
        private ContentManager _content;
        private GraphicsDevice _graphicsDevice;
        private CameraComponent _camera;
        private Map _map;

        private Texture2D _texture;
        private BasicEffect _effect;
        private List<QuadShape> _quads;

        private Rectangle _renderBounds;

        public override void LoadContent() {
            _content = BlackBoard.GetEntry<ContentManager>("ContentManager");
            _graphicsDevice = BlackBoard.GetEntry<GraphicsDevice>("GraphicsDevice");
            _camera = BlackBoard.GetEntry<CameraComponent>("Camera");
            _map = BlackBoard.GetEntry<Map>("Map");

            _texture = _content.Load<Texture2D>("Images/OryxEnv");

            _effect = new BasicEffect(_graphicsDevice);
            _effect.TextureEnabled = true;
            _effect.Texture = _texture;

            _quads = new List<QuadShape>();

            for(int z = 0; z < _map.Height; z++) {
                for(int x = 0; x < _map.Width; x++) {
                    Vector3 quadOrigin = new Vector3(x, 0, z);
                    TextureFrame textureFrame = new TextureFrame(0.375f, 0, 0.0625f, 0.0625f);
                    QuadShape quad = new QuadShape(quadOrigin, Vector3.Up, Vector3.Forward, textureFrame);
                    _quads.Add(quad);
                }
            }
        }

        public override void Process(Entity entity, TileMapComponent tileMapComponent) {
            _effect.View = _camera.View;
            _effect.Projection = _camera.Projection;

            _effect.CurrentTechnique.Passes[0].Apply();

            UpdateRenderBounds();

            for(int y = _renderBounds.Y; y < _renderBounds.Height; y++) {
                for(int x = _renderBounds.X; x < _renderBounds.Width; x++) {
                    int index = y * _map.Width + x;

                    _graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList,
                                                                                           _quads[index].Vertices, 0, 4,
                                                                                           _quads[index].Indexes, 0, 2);
                }
            }
        }

        // this should probably be computed from the camera position but for now hard-coding
        private void UpdateRenderBounds() {
            _renderBounds.X = Math.Max(0, (int)_camera.Position.X - 10);
            _renderBounds.Y = Math.Max(0, (int)_camera.Position.Z - 11);
            _renderBounds.Width = Math.Min((int)_camera.Position.X + 11, _map.Width);
            _renderBounds.Height = Math.Min((int)_camera.Position.Z + 1, _map.Height);

            //Console.WriteLine(_renderBounds);
        }


        // TODO - so much useless casting, might need to roll my own Floor and Clamp
        /*
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
        */
    }
}
