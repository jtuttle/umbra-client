using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

namespace ClientLib
{
    public class UmbraClient
    {
        public string Host { get; private set; }
        public int Port { get; private set; }

        private Thread ReadThread { get; set; }
        private bool ContinueReading { get; set; }
        private TcpClient Tcp { get; set; }
        private NetworkStream Stream { get; set; }

        public UmbraClient(string host, int port)
        {
            Tcp = new TcpClient();
            Host = host;
            Port = port;
        }

        public void Start()
        {
            Tcp.Connect(Host, Port);
            Stream = Tcp.GetStream();
            ContinueReading = true;
            ReadThread = new Thread(new ThreadStart(ReadFromServer));
            ReadThread.Start();
        }

        public void Stop()
        {
            ContinueReading = false;
            ReadThread.Join();
        }

        public void SendAuth(string authkey, int x, int y, int z) { Send("auth", authkey, x, y, z); }
        public void SendSetPosition(string room, float px, float py, float pz, float vx, float vy, float vz) { Send("set-position", room, px, py, pz, vx, vy, vz); }
        public void SendGetMyPosition() { Send("get-position"); }
        public void SendGetWholeStateUpdate() { Send("get-state"); }
        public void SendSay(string message) { Send("say", message); }

        public delegate void PositionUpdateHandler(string cid, string room, float px, float py, float pz, float vx, float vy, float vz);
        public delegate void SomeoneJoinedHandler(string cid, string displayName, float x, float y, float z);
        public delegate void SomeoneDroppedHandler(string cid);
        public delegate void SomeoneSpokeHandler(string cid, string message);
        public delegate void ErrorHandler(string message);

        public event PositionUpdateHandler PositionUpdate;
        public event SomeoneJoinedHandler SomeoneJoined;
        public event SomeoneDroppedHandler SomeoneDropped;
        public event SomeoneSpokeHandler SomeoneSpoke;
        public event ErrorHandler ErrorOccured;

        public void Send(params object[] p)
        {
            string msg = Util.Join(" ", p);
            if (!msg.EndsWith("\n"))
            {
                msg += "\n";
            }
            byte[] encoded = Encoding.ASCII.GetBytes(msg);
            Stream.Write(encoded, 0, encoded.Length);
            Stream.Flush();
        }

        private void ReadFromServer()
        {
            string accum = "";
            byte[] buff = new byte[512];
            while (ContinueReading)
            {
                IAsyncResult ar = Stream.BeginRead(buff, 0, buff.Length, null, null);
                while (!ar.IsCompleted && ContinueReading)
                {
                    Thread.Sleep(200);
                }
                if (!ContinueReading)
                {
                    return;
                }
                int read = Stream.EndRead(ar);
                string decoded = Encoding.ASCII.GetString(buff, 0, read);
                accum += decoded;
                while (accum.Contains("\n"))
                {
                    string nextLine = accum.Substring(0, accum.IndexOf("\n"));
                    accum = accum.Substring(accum.IndexOf("\n") + 1);
                    ParseAndDoleOut(nextLine);
                }
            }
        }

        private void ParseAndDoleOut(string feed)
        {
            List<string> words = new List<string>();
            string curWord = "";
            bool inQuote = false;
            bool inEscape = false;
            foreach (char ch in feed)
            {
                if (inEscape)
                {
                    inEscape = false;
                    curWord += ch;
                }
                else if (ch == '\\')
                {
                    inEscape = true;
                }
                else if (ch == '\'')
                {
                    if (inQuote)
                    {
                        inQuote = false;
                        if (curWord.Length > 0)
                        {
                            words.Add(curWord);
                            curWord = "";
                        }
                    }
                    else
                    {
                        inQuote = true;
                    }
                }
                else if (ch == ' ')
                {
                    if (curWord.Length > 0)
                    {
                        words.Add(curWord);
                        curWord = "";
                    }
                }
                else
                {
                    curWord += ch;
                }
            }
            if (curWord.Length > 0)
            {
                words.Add(curWord);
            }

            string cid = words[1];
            string cmd = words[0];
            string[] p = new string[words.Count - 2];
            for (int i = 2; i < words.Count; i++)
            {
                p[i - 2] = words[i];
            }
            if (cmd == "position")
            {
                PositionUpdate(cid, p[0], float.Parse(p[1]), float.Parse(p[2]), float.Parse(p[3]),
                    float.Parse(p[4]), float.Parse(p[5]), float.Parse(p[6]));
            }
            else if (cmd == "say")
            {
                SomeoneSpoke(cid, String.Join(" ", p));
            }
            else if (cmd == "join")
            {
                SomeoneJoined(cid, p[0], float.Parse(p[1]), float.Parse(p[2]), float.Parse(p[3]));
            }
            else if (cmd == "drop")
            {
                SomeoneDropped(cid);
            }
            else
            {
                ErrorOccured("no-handler " + feed);
            }
        }
    }
}

