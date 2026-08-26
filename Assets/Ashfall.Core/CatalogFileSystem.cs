// SPDX-License-Identifier: MIT
// ASHFALL Core: JSON-catalog file enumeration utility.

using System.IO;

namespace Ashfall.Core
{
    /// <summary>
    /// Static catalogue-enumeration helper. Keeps JSON-listing off the
    /// baseline <see cref="IFileIO"/> contract so that hosts which only
    /// implement the minimum (Unity, for example) still compile against
    /// Core. The BCL adapter provides a direct implementation; the helper
    /// dispatches through whichever adapter is at hand.
    /// </summary>
    public static class CatalogFileSystem
    {
        /// <summary>
        /// Enumerate every <c>*.json</c> path under a directory.
        /// </summary>
        /// <param name="files">Host file port.</param>
        /// <param name="dataDirectory">Root to search.</param>
        /// <param name="searchOption">Recursive or top-level only.</param>
        public static string[] EnumerateJsonFiles(IFileIO files, string dataDirectory, SearchOption searchOption)
        {
            if (files == null) return new string[0];
            if (!files.DirectoryExists(dataDirectory)) return new string[0];
            // Use the host's enumeration (FileSystemIO or GodotFileIO) — handles res:// PCK via DirAccess.
            try
            {
                return files.EnumerateFiles(dataDirectory, "*.json", searchOption);
            }
            catch
            {
                return new string[0];
            }
        }
    }
}
