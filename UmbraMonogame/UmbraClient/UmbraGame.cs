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
using CrawLib.Entity;
using CrawLib.Entity.Interface;
using UmbraClient.Entities;
#endregion

namespace UmbraMonogame {
    public class UmbraGame : Game, IEntityGame {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        // TODO: move all this client stuff into a helper class, especially the Update logic
        public NetClient client;

        public EntityList Entities { get; private set; }
        public World World { get; private set; }

        // TEMP
        private Player _player;

        public UmbraGame()
            : base() {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            EntityFactory.Game = (IEntityGame)this;

            Entities = new EntityList();
            World = (World)EntityFactory.CreateEntity(typeof(World), "World", null);

            NetPeerConfiguration config = new NetPeerConfiguration("Umbra");
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
            // exit if back or esc pressed
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ///////////////////////// TEMP /////////////////////////
            if(_player != null) {
                int xinput = 0;
                int yinput = 0;

                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                KeyboardState keyState = Keyboard.GetState();

                // use arrows or dpad to move avatar
                if(gamePadState.DPad.Left == ButtonState.Pressed || keyState.IsKeyDown(Keys.Left))
                    xinput = -1;
                if(gamePadState.DPad.Right == ButtonState.Pressed || keyState.IsKeyDown(Keys.Right))
                    xinput = 1;
                if(gamePadState.DPad.Up == ButtonState.Pressed || keyState.IsKeyDown(Keys.Up))
                    yinput = -1;
                if(gamePadState.DPad.Down == ButtonState.Pressed || keyState.IsKeyDown(Keys.Down))
                    yinput = 1;

                if(xinput != 0 || yinput != 0) {
                    NetOutgoingMessage om = client.CreateMessage();
                    om.Write(xinput); // very inefficient to send a full Int32 (4 bytes) but we'll use this for simplicity
                    om.Write(yinput);
                    client.SendMessage(om, NetDeliveryMethod.Unreliable);
                }
            }
            ///////////////////////// TEMP /////////////////////////

            // read messages from server
            NetIncomingMessage msg;

            while((msg = client.ReadMessage()) != null) {
                Console.WriteLine("Received " + msg.MessageType + " message from " + msg.SenderEndpoint);

                switch(msg.MessageType) {
                    case NetIncomingMessageType.DiscoveryResponse:
                        Console.WriteLine("Found server at " + msg.SenderEndpoint);

                        client.Connect(msg.SenderEndpoint);

                        _player = (Player)EntityFactory.CreateEntity(typeof(Player), "Player", World);

                        break;
                    case NetIncomingMessageType.Data:
                        Console.WriteLine("Received server data");

                        long playerId = msg.ReadInt64();

                        float xPos = msg.ReadFloat();
                        float yPos = msg.ReadFloat();

                        _player.UpdatePosition(xPos, yPos);

                        break;
                    default: break;
                }
            }

            Entities.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Entities.Draw();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args) {
            client.Shutdown("bye");

            base.OnExiting(sender, args);
        }
    }
}
