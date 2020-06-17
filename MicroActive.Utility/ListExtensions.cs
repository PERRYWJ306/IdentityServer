using System;
using System.Collections.Generic;

namespace System.Collections.Generic
{
	public static class ListExtensions
	{
		public static bool IsEmpty<T>(this IList<T> source)
		{
			if (source == null || source.Count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool IsNotEmpty<T>(this IList<T> source)
		{
			if (source != null && source.Count > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
