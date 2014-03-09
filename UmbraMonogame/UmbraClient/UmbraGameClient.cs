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
#endregion

namespace UmbraClient {
    public class UmbraGameClient : Game {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        private NetworkAgent _netAgent;
        
        private EntityWorld _entityWorld;

        private Camera2D _camera;

        public UmbraGameClient()
            : base() {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _netAgent = new NetworkAgent(AgentRole.Client, "Umbra");

            _camera = new Camera2D(this);
            Components.Add(_camera);

            EntitySystem.BlackBoard.SetEntry("ContentManager", Content);
            EntitySystem.BlackBoard.SetEntry("SpriteBatch", spriteBatch);
            EntitySystem.BlackBoard.SetEntry("NetworkAgent", _netAgent);
            EntitySystem.BlackBoard.SetEntry("Camera", _camera);

            _entityWorld = new EntityWorld();
            _entityWorld.InitializeAll(new[] { GetType().Assembly });

            CrawEntityManager.Instance.Initialize(_entityWorld, new ClientEntityFactory(_entityWorld));

            //// TEMP ////
            Map map = new Map(30, 30);
            Entity mapEntity = _entityWorld.CreateEntity();
            mapEntity.AddComponent(new TileMapComponent(map));

            _camera.MoveBounds = new Rectangle(0, 0, map.Width * TileConfig.TILE_WIDTH, map.Height * TileConfig.TILE_HEIGHT);
            //// TEMP ////
            
            _netAgent.Connect("127.0.0.1");

            base.Initialize();
            //_camera.Scale = 2.0f;
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

            spriteBatch.Begin(SpriteSortMode.FrontToBack, 
                              BlendState.AlphaBlend, 
                              SamplerState.LinearClamp, 
                              DepthStencilState.None,
                              RasterizerState.CullCounterClockwise, 
                              null, 
                              _camera.Transform);

            _entityWorld.Draw();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args) {
            _netAgent.Shutdown();

            base.OnExiting(sender, args);
        }
    }
}
