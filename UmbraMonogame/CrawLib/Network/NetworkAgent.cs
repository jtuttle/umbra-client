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

        private List<NetIncomingMessage> _incomingMessages;
        private NetOutgoingMessage _outgoingMessage;

        public List<NetConnection> Connections {
            get { return _peer.Connections; }
        }

        public NetworkAgent(AgentRole role, string tag) {
            _role = role;
            _config = new NetPeerConfiguration(tag);

            _incomingMessages = new List<NetIncomingMessage>();
    
            Initialize();
        }

        private void Initialize() {
            if(_role == AgentRole.Server) {
                _config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
                _config.Port = _port;
                _peer = new NetServer(_config);
                Log("Server starting on " + _config.LocalAddress + ":" + _config.Port);
            } else if(_role == AgentRole.Client) {
                _config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
                _peer = new NetClient(_config);
            }

            _peer.Start();
        }

        public void Connect(string ip) {
            if(_role == AgentRole.Client) {
                Log("Connecting to " + ip + ":" + _port);
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

        public List<NetIncomingMessage> ReadMessages() {
            _incomingMessages.Clear();

            NetIncomingMessage msg;
            string output = "";

            while((msg = _peer.ReadMessage()) != null) {
                Log("Received " + msg.MessageType + " message");

                switch(msg.MessageType) {
                    case NetIncomingMessageType.DiscoveryRequest:
                        _peer.SendDiscoveryResponse(null, msg.SenderEndpoint);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        if(_role == AgentRole.Server)
                            output += msg.ReadString() + "\n";
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();

                        if(_role == AgentRole.Server)
                            output += "Status message: " + msg.ReadString() + "\n";

                        if(status == NetConnectionStatus.Connected) {
                            // PLAYER CONNECTED
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        _incomingMessages.Add(msg);
                        break;
                    default:
                        output += "ERROR: Unknown message type";
                        break;
                }
            }

            if(_role == AgentRole.Server)
                Log(output);

            return _incomingMessages;
        }

        private void Log(string output) {
            Console.Write(output);
        }
    }
}
