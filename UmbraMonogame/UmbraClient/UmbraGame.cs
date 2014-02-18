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
#endregion

namespace UmbraClient {
    public class UmbraGame : Game {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        // TODO: move all this client stuff into a helper class, especially the Update logic
        public NetClient client;

        private EntityWorld _entityWorld;
        private Entity _player;

        public UmbraGame()
            : base() {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            NetPeerConfiguration config = new NetPeerConfiguration("Umbra");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.LocalAddress = NetUtility.Resolve("localhost");

            client = new NetClient(config);
        }

        protected override void Initialize() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _entityWorld = new EntityWorld();
            
            EntitySystem.BlackBoard.SetEntry("ContentManager", this.Content);
            EntitySystem.BlackBoard.SetEntry("SpriteBatch", this.spriteBatch);

            _entityWorld.InitializeAll(new[] { GetType().Assembly });

            client.Start();

            client.DiscoverLocalPeers(14242);

            base.Initialize();
        }

        protected override void UnloadContent() {
            
        }

        protected override void Update(GameTime gameTime) {
            // exit if back or esc pressed
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ///////////////////////// TEMP /////////////////////////
            // semi-authoritative setup where client dictates position
            if(_player != null) {
                TransformComponent transform = _player.GetComponent<TransformComponent>();

                if(transform.Dirty) {
                    transform.Dirty = false;

                    NetOutgoingMessage om = client.CreateMessage();
                    om.Write(transform.X); // very inefficient to send a full Int32 (4 bytes) but we'll use this for simplicity
                    om.Write(transform.Y);
                    client.SendMessage(om, NetDeliveryMethod.Unreliable);
                }
            }
            ///////////////////////// TEMP /////////////////////////

            _entityWorld.Update();

            // read messages from server
            NetIncomingMessage msg;

            while((msg = client.ReadMessage()) != null) {
                Console.WriteLine("Received " + msg.MessageType + " message from " + msg.SenderEndpoint);

                switch(msg.MessageType) {
                    case NetIncomingMessageType.DiscoveryResponse:
                        Console.WriteLine("Found server at " + msg.SenderEndpoint);

                        client.Connect(msg.SenderEndpoint);

                        //Vector2 center = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

                        _player = _entityWorld.CreateEntity();
                        _player.AddComponent(new TransformComponent());
                        _player.AddComponent(new VelocityComponent());
                        _player.AddComponent(new SpatialFormComponent("Hero"));
                        _player.Tag = "PLAYER";

                        break;
                    case NetIncomingMessageType.Data:
                        Console.WriteLine("Received server data");

                        //long playerId = msg.ReadInt64();

                        //float xPos = msg.ReadFloat();
                        //float yPos = msg.ReadFloat();

                        //_player.UpdatePosition(xPos, yPos);

                        break;
                    default: break;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            _entityWorld.Draw();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args) {
            client.Shutdown("bye");

            base.OnExiting(sender, args);
        }
    }
}
