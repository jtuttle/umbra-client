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
#endregion

namespace UmbraClient {
    public class UmbraGameClient : Game {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public NetworkAgent netAgent;

        private EntityWorld _entityWorld;
        private Dictionary<long, Entity> _entities;

        private Entity _player;

        // todo - DRY this stuff up
        private float _updatesPerSecond = 1.0f;
        private double _nextSendUpdates = NetTime.Now;

        public UmbraGameClient()
            : base() {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _entityWorld = new EntityWorld();

            EntitySystem.BlackBoard.SetEntry("ContentManager", this.Content);
            EntitySystem.BlackBoard.SetEntry("SpriteBatch", this.spriteBatch);

            _entityWorld.InitializeAll(new[] { GetType().Assembly });

            _entities = new Dictionary<long, Entity>();
            _entityWorld.EntityManager.AddedEntityEvent += OnEntityAdded;
            _entityWorld.EntityManager.RemovedEntityEvent += OnEntityRemoved;

            netAgent = new NetworkAgent(AgentRole.Client, "Umbra");
            netAgent.Connect("127.0.0.1");

            base.Initialize();
        }

        protected override void UnloadContent() {
            
        }

        protected override void Update(GameTime gameTime) {
            // exit if back or esc pressed
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            List<NetIncomingMessage> messages = netAgent.ReadMessages();

            foreach(NetIncomingMessage netMessage in messages) {
                NetworkMessageType messageType = (NetworkMessageType)Enum.ToObject(typeof(NetworkMessageType), netMessage.ReadByte());
                
                if(messageType == NetworkMessageType.EntityAdd) {
                    EntityAddMessage<UmbraEntityType> msg = new EntityAddMessage<UmbraEntityType>();
                    msg.Decode(netMessage);

                    if(msg.EntityType == UmbraEntityType.Player) {
                        _player = _entityWorld.CreateEntity(msg.EntityId);
                        _player.AddComponent(new UmbraEntityTypeComponent(UmbraEntityType.Player));
                        _player.AddComponent(new TransformComponent(msg.Position));
                        _player.AddComponent(new VelocityComponent());
                        _player.AddComponent(new SpatialFormComponent("Hero"));
                        _player.Tag = "PLAYER";
                    } else if(msg.EntityType == UmbraEntityType.NPC) {
                        Entity npc = _entityWorld.CreateEntity(msg.EntityId);
                        npc.AddComponent(new UmbraEntityTypeComponent(UmbraEntityType.NPC));
                        npc.AddComponent(new TransformComponent(msg.Position));
                        npc.AddComponent(new VelocityComponent());
                        npc.AddComponent(new SpatialFormComponent("NPC"));
                    }
                }
            }

            _entityWorld.Update();

            double now = NetTime.Now;

            if(now > _nextSendUpdates) {
                Console.WriteLine("sending update");

                List<INetworkMessage> outgoingMessages = new List<INetworkMessage>();

                TransformComponent transform = _player.GetComponent<TransformComponent>();
                outgoingMessages.Add(new EntityMoveMessage(_player.UniqueId, transform.Position));

                netAgent.SendMessages(outgoingMessages);

                _nextSendUpdates += (1.0 / _updatesPerSecond);
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
            netAgent.Shutdown();

            base.OnExiting(sender, args);
        }
        
        // todo - DRY this up between server and client
        private void OnEntityAdded(Entity entity) {
            _entities[entity.UniqueId] = entity;
        }

        private void OnEntityRemoved(Entity entity) {
            _entities.Remove(entity.UniqueId);
        }
    }
}
