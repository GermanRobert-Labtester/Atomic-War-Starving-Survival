using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp.Host
{
    /// <summary>
    /// Godot-aware file IO: handles both loose filesystem and res:// PCK virtual FS.
    /// For res:// paths, uses Godot.FileAccess/DirAccess; otherwise falls back to System.IO.
    /// Keeps Ashfall.Core pure (no Godot reference in Core) while enabling PCK-native Data.
    /// </summary>
    public sealed class GodotFileIO : IFileIO
    {
        private static bool IsResPath(string path) => !string.IsNullOrEmpty(path) && path.StartsWith("res://", StringComparison.Ordinal);

        public bool DirectoryExists(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            if (IsResPath(path)) return DirAccess.DirExistsAbsolute(path);
            return Directory.Exists(path);
        }

        public bool FileExists(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            if (IsResPath(path)) return FileAccess.FileExists(path);
            return File.Exists(path);
        }

        public string ReadAllText(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;
            if (IsResPath(path))
            {
                using var f = FileAccess.Open(path, FileAccess.ModeFlags.Read);
                if (f == null) throw new FileNotFoundException($"GodotFileIO: res file not found {path}", path);
                return f.GetAsText();
            }
            return File.ReadAllText(path, Encoding.UTF8);
        }

        public void WriteAllText(string path, string contents)
        {
            if (string.IsNullOrEmpty(path)) return;
            if (IsResPath(path))
            {
                // res:// is read-only in PCK/export; writes should go to user://
                // Fall back to System.IO for res:// writes (will fail visibly if attempted)
                throw new InvalidOperationException($"GodotFileIO: cannot write to res:// path {path} (read-only PCK)");
            }
            string? dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(path, contents ?? string.Empty, Encoding.UTF8);
        }

        public string Combine(params string[] parts) => Path.Combine(parts);

        public string[] EnumerateFiles(string directory, string searchPattern, SearchOption searchOption)
        {
            if (string.IsNullOrEmpty(directory)) return new string[0];
            if (IsResPath(directory))
            {
                var results = new List<string>();
                EnumerateResRecursive(directory, searchPattern, searchOption, results);
                return results.ToArray();
            }
            if (!Directory.Exists(directory)) return new string[0];
            try
            {
                return Directory.GetFiles(directory, searchPattern, searchOption);
            }
            catch
            {
                return new string[0];
            }
        }

        private static void EnumerateResRecursive(string dir, string pattern, SearchOption option, List<string> results)
        {
            using var d = DirAccess.Open(dir);
            if (d == null) return;
            d.ListDirBegin();
            string fileName = d.GetNext();
            // Extract extension from pattern like "*.json"
            string ext = pattern.StartsWith("*.") ? pattern.Substring(1) : string.Empty;
            while (fileName != string.Empty)
            {
                if (fileName == "." || fileName == "..")
                {
                    fileName = d.GetNext();
                    continue;
                }
                string fullPath = dir.TrimEnd('/') + "/" + fileName;
                if (d.CurrentIsDir())
                {
                    if (option == SearchOption.AllDirectories)
                        EnumerateResRecursive(fullPath, pattern, option, results);
                }
                else
                {
                    if (string.IsNullOrEmpty(ext) || fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                        results.Add(fullPath);
                }
                fileName = d.GetNext();
            }
            d.ListDirEnd();
        }
    }
}
