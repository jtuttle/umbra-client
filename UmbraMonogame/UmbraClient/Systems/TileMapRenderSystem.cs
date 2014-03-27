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
using CrawLib.Shapes;

namespace UmbraClient.Systems {
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = 0)]
    public class TilemapRenderSystem : EntityComponentProcessingSystem<TileMapComponent> {
        private ContentManager _content;
        private GraphicsDevice _graphicsDevice;

        private Texture2D _texture;
        private BasicEffect _effect;
        //private List<Quad> _quads;
        private List<QuadShape> _quads;

        private CameraComponent _camera;

        public override void LoadContent() {
            _content = BlackBoard.GetEntry<ContentManager>("ContentManager");
            _graphicsDevice = BlackBoard.GetEntry<GraphicsDevice>("GraphicsDevice");
            _camera = BlackBoard.GetEntry<CameraComponent>("Camera");

            _texture = _content.Load<Texture2D>("Images/OryxEnv");

            _effect = new BasicEffect(_graphicsDevice);
            _effect.TextureEnabled = true;
            _effect.Texture = _texture;

            _quads = new List<QuadShape>();

            for(int z = -2; z <= 2; z++) {
                for(int x = -2; x <= 2; x++) {
                    Vector3 quadOrigin = new Vector3(x, 0, z);
                    _quads.Add(new QuadShape(quadOrigin, Vector3.Up, Vector3.Forward));
                }
            }
        }

        public override void Process(Entity entity, TileMapComponent tileMapComponent) {
            _effect.View = _camera.View;
            _effect.Projection = _camera.Projection;

            _effect.CurrentTechnique.Passes[0].Apply();

            for(int i = 0; i < _quads.Count; i++) {
                QuadShape quad = _quads[i];
                _graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, quad.Vertices, 0, 4, quad.Indexes, 0, 2);
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
