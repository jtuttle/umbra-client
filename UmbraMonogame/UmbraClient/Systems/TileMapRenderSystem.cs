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
using UmbraClient.Components;

namespace UmbraClient.Systems {
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = 0)]
    public class TilemapRenderSystem : EntityComponentProcessingSystem<TileMapComponent> {
        private ContentManager _content;
        private GraphicsDevice _graphicsDevice;

        private Texture2D _texture;
        private BasicEffect _effect;
        private List<Quad> _quads;

        private CameraComponent _camera;

        public override void LoadContent() {
            _content = BlackBoard.GetEntry<ContentManager>("ContentManager");
            _graphicsDevice = BlackBoard.GetEntry<GraphicsDevice>("GraphicsDevice");
            _camera = BlackBoard.GetEntry<CameraComponent>("Camera");

            _texture = _content.Load<Texture2D>("Images/OryxEnv");

            _effect = new BasicEffect(_graphicsDevice);
            _effect.TextureEnabled = true;
            _effect.Texture = _texture;

            _quads = new List<Quad>();

            for(int y = 0; y < 5; y++) {
                for(int x = 0; x < 5; x++) {
                    Vector3 pos = new Vector3(x, y, 0);
                    _quads.Add(new Quad(pos, Vector3.Backward, Vector3.Up, 1, 1));
                }
            }
        }

        public override void Process(Entity entity, TileMapComponent tileMapComponent) {
            //Matrix View = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            //Matrix Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 4.0f / 3.0f, 1, 500);

            //_effect.View = View;
            //_effect.Projection = Projection;

            _effect.View = _camera.View;
            _effect.Projection = _camera.Projection;

            _effect.CurrentTechnique.Passes[0].Apply();

            for(int i = 0; i < _quads.Count; i++) {
                int x = i % 5;
                int y = (int)Math.Floor(i / 5.0f);

                _graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, _quads[i].Vertices, 0, 4, _quads[i].Indexes, 0, 2);
            }
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
