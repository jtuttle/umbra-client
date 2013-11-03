using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ClientLib
{
    public class Json : IEnumerable<Json>
    {
        public Json(object data)
        {
            if (data == null)
                throw new ArgumentException();
            Data = data;
        }

        public object Data { get; private set; }
        public Type DataType { get { return Data.GetType(); } }

        public Json this[object key]
        {
            set { ((JsonHash)Data)[key] = value; }
            get
            {
                if (DataType != typeof(JsonHash))
                    return null;
                return ((JsonHash)Data)[key];
            }
        }

        public IEnumerator<Json> GetEnumerator()
        {
            return ((JsonList)Data).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((JsonList)Data).GetEnumerator();
        }

        static public implicit operator string(Json json)
        {
            return (string)json.Data;
        }

        static public implicit operator int(Json json)
        {
            return (int)json.Data;
        }

        static public implicit operator double(Json json)
        {
            return (double)json.Data;
        }

        public override string ToString()
        {
            if (Data == null) return null;
            return Data.ToString();
        }

        public static Json Parse(string content)
        {
            return JsonParser.ParseNext(new JsonTokenizer(content));
        }
    }

    public class JsonHash : Dictionary<object, Json>
    {
        new public Json this[object key]
        {
            set { base[key] = value; }
            get
            {
                if (!ContainsKey(key))
                    return null;
                return base[key];
            }
        }

        public override string ToString()
        {
            List<string> pairs = new List<string>();
            foreach (object key in Keys)
                pairs.Add(string.Format("{0}: {1}", key, this[key]));
            return "{" + Util.Join(", ", pairs) + "}";
        }
    }

    public class JsonList : List<Json>
    {
        public override string ToString()
        {
            return "[" + Util.Join(", ", this) + "]";
        }
    }

    internal class JsonParser
    {
        public static Json Wrap(object obj)
        {
            if (obj == null)
                return null;
            else if (obj.GetType() == typeof(Json))
                return (Json)obj;
            else
                return new Json(obj);
        }

        public static Json ParseNext(JsonTokenizer parser, string token = null)
        {
            if (token == null)
                token = parser.NextToken();
            if (token == "[")
                return Wrap(ParseNextList(parser));
            else if (token == "{")
                return Wrap(ParseNextHash(parser));
            else
                return ParseNextValue(parser, token);
        }

        private static Json ParseNextValue(JsonTokenizer parser, string token = null)
        {
            if (token == null)
                token = parser.NextToken();
            if (Regex.IsMatch(token, "^-?[0-9]+(.[0-9]+)?$") ||
                Regex.IsMatch(token, "^-?.[0-9]+$"))
            {
                if (token.IndexOf(".") == -1)
                    return Wrap(int.Parse(token));
                else
                    return Wrap(double.Parse(token));
            }
            else
                return Wrap(token);
        }

        private static JsonList ParseNextList(JsonTokenizer parser, JsonList list = null)
        {
            if (list == null)
                list = new JsonList();
            string token = parser.NextToken();
            while (token != null)
            {
                if (token == "]")
                    return list;
                else if (token == ",")
                { /* do nothing */ }
                else
                    list.Add(ParseNext(parser, token));
                token = parser.NextToken();
            }
            return list;
        }

        private static JsonHash ParseNextHash(JsonTokenizer parser, JsonHash hash = null)
        {
            if (hash == null)
                hash = new JsonHash();
            string token = parser.NextToken();
            while (token != null)
            {
                if (token == "}")
                    return hash;
                object key = ParseNextValue(parser, token).Data;
                if (parser.NextToken() != ":")
                    throw new ArgumentException();
                Json value = ParseNext(parser);
                hash[key] = value;
                token = parser.NextToken();
                if (token == ",")
                    token = parser.NextToken();
            }
            return hash;
        }
    }

    internal class JsonTokenizer
    {
        public string Content { get; set; }
        public int Scan { get; set; }

        public JsonTokenizer(string content)
        {
            Content = content;
            Scan = 0;
        }

        private static readonly string SingleCharTokens = "[]{},:";
        public string NextToken()
        {
            while (Scan < Content.Length && char.IsWhiteSpace(Content[Scan]))
                Scan++;
            if (Scan >= Content.Length)
                return null;
            char ch = Content[Scan];
            if (SingleCharTokens.IndexOf(ch) != -1)
            {
                Scan++;
                return "" + ch;
            }
            else if (ch == '\"' || ch == '\'')
            {
                char term = ch;
                string token = "";
                bool inEscape = false;
                for (Scan += 1; Scan < Content.Length; Scan++)
                {
                    ch = Content[Scan];
                    if (inEscape)
                    {
                        inEscape = false;
                        token += ch;
                    }
                    else if (ch == '\\')
                        inEscape = true;
                    else if (ch == term)
                    {
                        Scan++;
                        return token;
                    }
                    else
                        token += ch;
                }
                throw new ArgumentException();
            }
            else
            {
                string token = "";
                for (; Scan < Content.Length; Scan++)
                {
                    ch = Content[Scan];
                    if (char.IsWhiteSpace(ch) || SingleCharTokens.IndexOf(ch) != -1)
                        return token;
                    else
                        token += ch;
                }
                return token;
            }
        }
    }
}
