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
using UmbraClient.Entity;
using UmbraClient.Entity.Interface;
#endregion

namespace UmbraMonogame {
    public class UmbraClient : Game, IEntityGame {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // TODO: move all this client stuff into a helper class, especially the Update logic
        NetClient client;

        // TODO: move this too
        private Texture2D _hero;

        public EntityList Entities { get; private set; }
        public World World { get; private set; }

        public UmbraClient()
            : base() {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            EntityFactory.Game = (IEntityGame)this;

            Entities = new EntityList();
            World = (World)EntityFactory.CreateEntity(typeof(World), "World", null);

            NetPeerConfiguration config = new NetPeerConfiguration("MyExampleName");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.LocalAddress = NetUtility.Resolve("localhost");

            client = new NetClient(config);
        }

        protected override void Initialize() {
            client.Start();

            client.DiscoverLocalPeers(14242);

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _hero = Content.Load<Texture2D>("Images/Hero");
        }

        protected override void UnloadContent() {
        
        }

        protected override void Update(GameTime gameTime) {
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            NetIncomingMessage msg;

            while((msg = client.ReadMessage()) != null) {
                Console.WriteLine("Received " + msg.MessageType + " message from " + msg.SenderEndpoint);

                switch(msg.MessageType) {
                    case NetIncomingMessageType.DiscoveryResponse:
                        Console.WriteLine("Found server at " + msg.SenderEndpoint);

                        client.Connect(msg.SenderEndpoint);

                        break;
                    case NetIncomingMessageType.Data:
                        Console.WriteLine("Received server data");

                        // process data from server

                        break;
                    default: break;
                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Draw(_hero, Vector2.Zero, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args) {
            client.Shutdown("bye");

            base.OnExiting(sender, args);
        }
    }
}
