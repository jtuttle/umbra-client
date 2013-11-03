using System;
using System.Collections.Generic;

namespace ClientLib
{
	public static class Util
	{
		public static string Join<T>(string separator, IList<T> values)
		{
			return Util.Join(separator, (IEnumerable<T>)values);
		}
		
		public static string Join<T>(string separator, IEnumerable<T> values)
		{
			List<string> changed = new List<string>();
			foreach (object val in values)
			{
				changed.Add((val != null) ? val.ToString() : null);
			}
			return string.Join (separator, changed.ToArray());
		}
	}
}

