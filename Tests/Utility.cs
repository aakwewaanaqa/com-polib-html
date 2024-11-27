using System.IO;
using UnityEngine;

namespace Tests
{
    internal static class Utility
    {
        public const string FOLDER = "Packages/com.polib.html/Tests";

        public static FileInfo GetTestedFile(string subPath)
        {
            var          dp     = Application.dataPath;
            var          path   = Path.Combine(dp, "..", FOLDER, subPath);
            return new FileInfo(path);
        }

        public static string ReadString(string subPath)
        {
            var file = GetTestedFile(subPath);
            if (!file.Exists) return string.Empty;
            using var stream = file.OpenText();
            return stream.ReadToEnd();
        }
    }
}