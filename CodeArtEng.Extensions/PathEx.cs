using System.Collections.Generic;
using System.Linq;

namespace System.IO
{
    /// <summary>
    /// Provides utility methods for manipulating and formatting file system paths.
    /// </summary>
    /// <remarks>The PathEx class contains static methods that extend or supplement the functionality of the
    /// standard System.IO.Path class. All members are static and the class cannot be instantiated.</remarks>
    public static class PathEx
    {
        /// <summary>
        /// Generates a shortened version of the specified file path, preserving the first and last segments and
        /// inserting an ellipsis if the path exceeds the specified maximum length.
        /// </summary>
        /// <remarks>The shortened path retains the first and last segments of the original path, with an
        /// ellipsis ('...') inserted between them. Additional segments may be included if space allows. This method is
        /// useful for displaying long paths in user interfaces where space is limited.</remarks>
        /// <param name="inputPath">The full file path to be shortened. Cannot be null.</param>
        /// <param name="maxLength">The maximum allowed length of the returned path. Specify -1 to return the original path without shortening.</param>
        /// <returns>A shortened path string that does not exceed the specified maximum length. If the original path is shorter
        /// than or equal to the maximum length, or if the path has fewer than two segments, the original path is
        /// returned.</returns>
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
