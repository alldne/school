using System;
using System.Collections.Generic;

namespace School
{
    public static class EnumerationExtensions
    {
        public static IEnumerable<T> Intersperse<T>(this IEnumerable<T> source, T element)
        {
            bool first = true;
            foreach (T value in source)
            {
                if (!first) yield return element;
                yield return value;
                first = false;
            }
        }
    }
}

