using System;
using System.Collections.Generic;

namespace Tools
{
    public static class Extensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static string Join<T>(this IEnumerable<T> str, string separator)
        {
            return string.Join(separator, str);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
