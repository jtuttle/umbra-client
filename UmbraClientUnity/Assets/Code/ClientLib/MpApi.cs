using System;
using System.Net;
using System.Collections.Generic;
using System.IO;

namespace ClientLib
{
    public class MpApi
    {
        public string Host { get; private set; }
        public int Port { get; private set; }

        public MpApi (string host, int port)
        {
            Host = host;
            Port = port;
        }

        public string DoSignIn(string email, string password)
        {
            Json res = Request(RequestMethod.GET, "/auth",
                "email", email,
                "password", password);
            return res["authkey"];
        }

        public void CreateAccount(string email, string displayName, string password)
        {
            Request(RequestMethod.GET, "/account/new",
                "email", email,
                "display_name", displayName,
                "password", password);
        }

        private enum RequestMethod { GET, POST }

        private Json Request(RequestMethod meth, string url, params object[] p)
        {
            List<string> pairs = new List<string>();
            for (int i = 0; i < p.Length; i += 2)
            {
                string key = Uri.EscapeUriString(p[i].ToString());
                string val = Uri.EscapeUriString(p[i+1].ToString());
                pairs.Add(string.Format("{0}={1}", key, val));
            }
            string pdata = MpUtil.Join("&", pairs);
            if (!url.StartsWith("/"))
                url = "/" + url;
            string fullurl = "http://" + Host + ":" + Port.ToString() + url;
            if (meth == RequestMethod.GET && pdata.Length > 0)
                fullurl += "?" + pdata;
            WebRequest request = WebRequest.Create(fullurl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string content = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            Json responseObj = Json.Parse(content);
            if (responseObj["error"] != null)
            {
                throw new UmbraApiException(responseObj["error"]);
            }
            return responseObj;
        }

        public class UmbraApiException : Exception
        {
            public UmbraApiException(string message)
                : base(message)
            { }
        }
    }
}

