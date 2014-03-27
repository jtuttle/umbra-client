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

namespace UmbraClient.Systems {
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = 1)]
    public class RenderSystem : EntityComponentProcessingSystem<SpatialFormComponent, TransformComponent> {
        public override void LoadContent() {
            
        }

        public override void Process(Entity entity, SpatialFormComponent spatialFormComponent, TransformComponent transformComponent) {


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
