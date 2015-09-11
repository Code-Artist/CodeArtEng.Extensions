using System.Collections.Generic;
using System.Linq;

namespace System.IO
{
    public static class PathEx
    {
        public static string GetShortPath(string inputPath, int maxLength)
        {
            if (maxLength == -1) return inputPath;
            if (inputPath.Length < maxLength) return inputPath;

            List<string> result = new List<string>();
            string[] tItem = inputPath.Split('\\');
            if (tItem.Count() < 2) return inputPath;

            result.Add(tItem[0]);
            result.Add("...");
            result.Add(tItem.Last());

            //Minimum Requirement
            int totalLength = 0;
            foreach (string item in result) totalLength += item.Length;
            totalLength += result.Count - 1; //Including [\] between each item

            for (int x = tItem.Count() - 2; x > 0; x--)
            {
                if (totalLength + tItem[x].Length < maxLength)
                {
                    result.Insert(2, tItem[x]);
                    totalLength += tItem[x].Length + 1; //Including [\] symbol
                }
                else break;
            }

            string resultString = string.Join("\\", result.ToArray());
            return resultString;
        }

    }
}
