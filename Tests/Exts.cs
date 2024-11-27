using System;
using System.Collections.Generic;

namespace Tests
{
    public static class Exts
    {
        public static void Iter<T>(this IEnumerable<T> ie, Action<T> act)
        {
            foreach (var i in ie) act(i);
        }
    }
}