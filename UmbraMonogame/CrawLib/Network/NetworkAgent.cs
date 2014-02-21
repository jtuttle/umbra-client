using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace CrawLib.Network {
    public enum AgentRole { 
        Client, 
        Server
    }

    public class NetworkAgent {
        private AgentRole _role;
        private NetPeer _peer;
        private NetPeerConfiguration _config;
        private int _port = 14242;

        private NetOutgoingMessage _outgoingMessage;

        public NetworkAgent(AgentRole role, string tag) {
            _role = role;
            _config = new NetPeerConfiguration(tag);
            
            Initialize();
        }

        private void Initialize() {
            if(_role == AgentRole.Server) {
                _config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
                _config.Port = _port;
                _peer = new NetServer(_config);
            } else if(_role == AgentRole.Client) {
                _config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
                _peer = new NetClient(_config);
            }

            _peer.Start();
        }

        public void Connect(string ip) {
            if(_role == AgentRole.Client) {
                _peer.Connect(ip, _port);
            } else {
                throw new SystemException("Attempted to connect as server. Only clients should connect.");
            }
        }

        public void Shutdown() {
            _peer.Shutdown("Closing conection.");
        }

        public void SendMessage(NetConnection recipient) {
            SendMessage(recipient, false);   
        }

        public void SendMessage(NetConnection recipient, bool guaranteed) {
            NetDeliveryMethod method = (guaranteed ? NetDeliveryMethod.ReliableOrdered : NetDeliveryMethod.UnreliableSequenced);

            // one optimization trick for Lidgren is to call CreateMessage with a size argument to prevent dynamic resizing 
            // of the message, so this is probably not very smart since you have no idea about the size this early on
            _peer.SendMessage(_outgoingMessage, recipient, method);
            _outgoingMessage = _peer.CreateMessage();
        }
    }
}
