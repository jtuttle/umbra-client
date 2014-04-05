#region Using Statements
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using Artemis;
using CrawLib.Artemis.Components;
using Artemis.System;
using CrawLib;
using CrawLib.Network;
using CrawLib.Network.Messages;
using UmbraLib;
using UmbraLib.Components;
using CrawLib.Artemis;
using CrawLib.TileMap;
using UmbraClient.Components;
#endregion

namespace UmbraClient {
    public class UmbraGameClient : Game {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        private NetworkAgent _netAgent;
        
        private EntityWorld _entityWorld;

        public UmbraGameClient()
            : base() {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _netAgent = new NetworkAgent(AgentRole.Client, "Umbra");

            EntitySystem.BlackBoard.SetEntry("Game", this);
            EntitySystem.BlackBoard.SetEntry("ContentManager", Content);
            EntitySystem.BlackBoard.SetEntry("SpriteBatch", spriteBatch);
            EntitySystem.BlackBoard.SetEntry("GraphicsDevice", GraphicsDevice);
            EntitySystem.BlackBoard.SetEntry("ContentManager", Content);
            EntitySystem.BlackBoard.SetEntry("NetworkAgent", _netAgent);

            _entityWorld = new EntityWorld();
            
            // create camera
            Vector3 camPosition = new Vector3(0, 10, 5);
            Matrix camRotation = Matrix.CreateFromAxisAngle(new Vector3(1, 0, 0), MathHelper.ToRadians(-65.0f));
            float aspectRatio = (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;

            CameraComponent cameraComponent = new CameraComponent(camPosition, camRotation, aspectRatio);

            Entity cameraEntity = _entityWorld.CreateEntity();
            cameraEntity.AddComponent(cameraComponent);

            EntitySystem.BlackBoard.SetEntry("Camera", cameraComponent);

            //// TEMP ////
            Map map = new Map(100, 100);
            Entity mapEntity = _entityWorld.CreateEntity();
            mapEntity.AddComponent(new TileMapComponent(map));

            EntitySystem.BlackBoard.SetEntry("Map", map);
            //// TEMP ////

            _entityWorld.InitializeAll(new[] { GetType().Assembly });

            CrawEntityManager.Instance.Initialize(_entityWorld, new ClientEntityFactory(_entityWorld));

            _netAgent.Connect("127.0.0.1");

            base.Initialize();
        }

        protected override void UnloadContent() {
            
        }

        protected override void Update(GameTime gameTime) {
            // exit if back or esc pressed
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _entityWorld.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //spriteBatch.Begin();

            //spriteBatch.Begin(SpriteSortMode.FrontToBack, 
            //                  BlendState.AlphaBlend, 
            //                  SamplerState.LinearClamp, 
            //                  DepthStencilState.None,
            //                  RasterizerState.CullCounterClockwise, 
            //                  null, 
            //                  _camera.Transform);

            _entityWorld.Draw();

            //spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args) {
            _netAgent.Shutdown();

            base.OnExiting(sender, args);
        }
    }
}
