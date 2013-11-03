using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClientLib
{
    public static class MpUtil
    {
        public static string Join<T>(string separator, IList<T> values)
        {
            return MpUtil.Join(separator, (IEnumerable<T>)values);
        }

        public static string Join<T>(string separator, IEnumerable<T> values)
        {
            List<string> changed = new List<string>();
            foreach (object val in values)
            {
                changed.Add((val != null) ? val.ToString() : null);
            }
            return string.Join(separator, changed.ToArray());
        }

        public static void Log(string fmt, params object[] p)
        {
            string formatted = String.Format(fmt, p);
            Debug.Log("[ClientLib] " + formatted);
        }
    }
}

