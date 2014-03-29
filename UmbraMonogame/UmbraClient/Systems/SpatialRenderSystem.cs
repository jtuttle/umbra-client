using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using CrawLib.Artemis.Components;
using Artemis;
using UmbraClient.Spatials;
using Microsoft.Xna.Framework;
using CrawLib;
using UmbraClient.Components;

namespace UmbraClient.Systems {
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = 1)]
    public class SpatialRenderSystem : EntityComponentProcessingSystem<SpatialFormComponent, TransformComponent> {
        private ContentManager _content;
        private GraphicsDevice _graphicsDevice;
        private CameraComponent _camera;

        private BasicEffect _effect;

        private string _spatialName;

        public override void LoadContent() {
            _content = BlackBoard.GetEntry<ContentManager>("ContentManager");
            _graphicsDevice = BlackBoard.GetEntry<GraphicsDevice>("GraphicsDevice");
            _camera = BlackBoard.GetEntry<CameraComponent>("Camera");

            _effect = new BasicEffect(_graphicsDevice);
            _effect.TextureEnabled = true;
        }

        public override void Process(Entity entity, SpatialFormComponent spatialFormComponent, TransformComponent transformComponent) {
            _spatialName = spatialFormComponent.SpatialFormFile;



            // this ain't quite workin'
            //Matrix bboard = Matrix.CreateConstrainedBillboard(transformComponent.Position, _camera.Position, new Vector3(0, 1, 0), _camera.Rotation.Up, _camera.Rotation.Forward);
            //bboard.Right = -bboard.Right; // this had the wrong sign for some reason
            //_effect.World = bboard;



            _effect.View = _camera.View;
            _effect.Projection = _camera.Projection;

            _effect.CurrentTechnique.Passes[0].Apply();

            if(string.Compare("Hero", _spatialName, StringComparison.InvariantCultureIgnoreCase) == 0) {
                Hero.Render(_content, _graphicsDevice, transformComponent, _effect);
            }

            if(string.Compare("NPC", _spatialName, StringComparison.InvariantCultureIgnoreCase) == 0) {
                //NPC.Render(_content, _graphicsDevice, transformComponent, _effect);
            }

            //if(spatialFormComponent != null) {
            //    _spatialName = spatialFormComponent.SpatialFormFile;

                // previously commented out
                //if(transformComponent.X >= 0 && transformComponent.Y >= 0 &&
                //    transformComponent.X < _spriteBatch.GraphicsDevice.Viewport.Width &&
                //    transformComponent.Y < _spriteBatch.GraphicsDevice.Viewport.Height)
                //{
                    //if(string.Compare("Hero", _spatialName, StringComparison.InvariantCultureIgnoreCase) == 0) {
                    //    Hero.Render(_spriteBatch, _content, transformComponent);
                    //}

                    //if(string.Compare("NPC", _spatialName, StringComparison.InvariantCultureIgnoreCase) == 0) {
                    //    NPC.Render(_spriteBatch, _content, transformComponent);
                    //}



                //}
            //}
        }
    }
}
