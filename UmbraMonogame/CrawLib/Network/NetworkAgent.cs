﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using CrawLib.Network.Messages;

namespace CrawLib.Network {
    public enum AgentRole { 
        Client, 
        Server
    }

    public class NetworkAgent {
        public delegate void PlayerConnectDelegate(NetConnection connection);
        public event PlayerConnectDelegate OnPlayerConnect = delegate { };

        public delegate void PlayerDisconnectDelegate(NetConnection connection);
        public event PlayerDisconnectDelegate OnPlayerDisconnect = delegate { };

        private AgentRole _role;
        private NetPeer _peer;
        private NetPeerConfiguration _config;
        private int _port = 14242;
        private float _pingInterval = 2.0f;

        private List<NetIncomingMessage> _incomingMessages;

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
                _config.PingInterval = _pingInterval;
                _config.ConnectionTimeout = _pingInterval + 1;
                _peer = new NetServer(_config);
                
                Log("Server starting on " + _config.LocalAddress + ":" + _config.Port);
            } else if(_role == AgentRole.Client) {
                _config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
                _config.PingInterval = _pingInterval;
                _config.ConnectionTimeout = _pingInterval + 1;
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
            _peer.Shutdown("Closing connection.");
        }

        public List<NetIncomingMessage> ReadMessages() {
            _incomingMessages.Clear();
            
            NetIncomingMessage msg;

            while((msg = _peer.ReadMessage()) != null) {
                //Log("Received " + msg.MessageType + " message");

                switch(msg.MessageType) {
                    case NetIncomingMessageType.DiscoveryRequest:
                        _peer.SendDiscoveryResponse(null, msg.SenderEndpoint);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        if(_role == AgentRole.Server)
                            Log(msg.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                        Log("Status message: " + msg.ReadString());

                        if(status == NetConnectionStatus.Connected)
                            OnPlayerConnect(msg.SenderConnection);
                        else if(status == NetConnectionStatus.Disconnected)
                            OnPlayerDisconnect(msg.SenderConnection);
                        break;
                    case NetIncomingMessageType.Data:
                        _incomingMessages.Add(msg);
                        break;
                    default:
                        Log("ERROR: Unknown message type");
                        break;
                }
            }

            return _incomingMessages;
        }

        public void SendMessages(List<INetworkMessage> outgoingMessages) {
            foreach(INetworkMessage outgoingMessage in outgoingMessages) {
                BroadcastMessage(outgoingMessage);
            }
        }

        public void SendMessage(INetworkMessage msg, NetConnection recipient) {
            SendMessage(msg, recipient, false);
        }

        public void SendMessage(INetworkMessage msg, NetConnection recipient, bool guaranteed) {
            NetDeliveryMethod method = (guaranteed ? NetDeliveryMethod.ReliableOrdered : NetDeliveryMethod.UnreliableSequenced);

            // opt - tell the peer the message size
            NetOutgoingMessage outgoing = _peer.CreateMessage();
            outgoing.Write((byte)msg.MessageType);
            msg.Encode(outgoing);

            _peer.SendMessage(outgoing, recipient, method);
        }

        public void BroadcastMessage(INetworkMessage msg) {
            BroadcastMessage(msg, false);
        }

        public void BroadcastMessage(INetworkMessage msg, bool guaranteed) {
            foreach(NetConnection connection in _peer.Connections)
                SendMessage(msg, connection, guaranteed);
        }

        private void Log(string output) {
            Console.WriteLine(output);
        }
    }
}
