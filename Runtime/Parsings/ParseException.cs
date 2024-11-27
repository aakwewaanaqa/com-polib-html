using System;
using UnityEngine;

namespace Parsings
{
    public class ParseException : Exception
    {
        public string Source;

        public ParseException(string src)
        {
            Source = src;
            Debug.LogError($"{src}不能被解析。");
        }
    }
}