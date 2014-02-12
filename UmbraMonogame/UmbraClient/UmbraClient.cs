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
#endregion

namespace UmbraMonogame {
    public class UmbraClient : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // TODO: move all this client stuff into a helper class, especially the Update logic
        NetClient client;

        public UmbraClient()
            : base() {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

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

            

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args) {
            client.Shutdown("bye");

            base.OnExiting(sender, args);
        }
    }
}
