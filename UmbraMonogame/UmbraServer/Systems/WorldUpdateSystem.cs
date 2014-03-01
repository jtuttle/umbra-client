using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using CrawLib.Artemis.Components;
using Artemis;
using Lidgren.Network;
using CrawLib.Network.Messages;
using CrawLib.Network;

namespace UmbraServer.Systems {
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update)]
    class WorldUpdateSystem : EntityComponentProcessingSystem<TransformComponent> {
        private NetworkAgent _netAgent;

        private float _updatesPerSecond;
        private double _nextSendUpdates;
        private bool _sendUpdates;

        public override void LoadContent() {
            _netAgent = BlackBoard.GetEntry<NetworkAgent>("NetworkAgent");

            _updatesPerSecond = 10.0f;
            _nextSendUpdates = NetTime.Now;
            _sendUpdates = false;
        }

        protected override void ProcessEntities(IDictionary<int, Entity> entities) {
            if(NetTime.Now > _nextSendUpdates) {
                Console.WriteLine("sending world update");
                _sendUpdates = true;
                _nextSendUpdates += (1.0 / _updatesPerSecond);
            }

            base.ProcessEntities(entities);

            _sendUpdates = false;
        }

        public override void Process(Entity entity, TransformComponent transform) {
            if(_sendUpdates) {
                _netAgent.BroadcastMessage(new EntityMoveMessage(entity.UniqueId, transform.Position));
            }
        }
    }
}
