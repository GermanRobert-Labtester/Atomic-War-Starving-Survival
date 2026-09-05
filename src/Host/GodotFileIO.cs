using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
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
        private readonly ILog _log;

        public GodotFileIO(ILog? log = null)
        {
            _log = log ?? new AtomicWar.GodotApp.GodotLog();
        }

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
            if (IsResPath(path)) return Godot.FileAccess.FileExists(path);
            return File.Exists(path);
        }

        public string ReadAllText(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;
            if (IsResPath(path))
            {
                using var f = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
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
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is System.Security.SecurityException)
            {
                GD.PrintErr($"[GodotFileIO] Failed to enumerate files in '{directory}': {ex.Message}");
                return Array.Empty<string>();
            }
        }

        private static void EnumerateResRecursive(string dir, string pattern, SearchOption option, List<string> results)
        {
            using var d = DirAccess.Open(dir);
            if (d == null)
            {
                var err = DirAccess.GetOpenError();
                GD.PrintErr($"[GodotFileIO] Failed to open virtual directory '{dir}': error {err}");
                return;
            }
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

        public string[] GetDirectories(string path, string searchPattern = "*")
        {
            if (string.IsNullOrEmpty(path)) return Array.Empty<string>();
            if (IsResPath(path))
            {
                using var d = DirAccess.Open(path);
                if (d == null) return Array.Empty<string>();
                var list = new List<string>();
                d.ListDirBegin();
                string next = d.GetNext();
                while (!string.IsNullOrEmpty(next))
                {
                    if (next != "." && next != ".." && d.CurrentIsDir())
                    {
                        if (searchPattern == "*" || next.Contains(searchPattern.Trim('*')))
                            list.Add(path.TrimEnd('/') + "/" + next);
                    }
                    next = d.GetNext();
                }
                d.ListDirEnd();
                return list.ToArray();
            }
            if (!Directory.Exists(path)) return Array.Empty<string>();
            try
            {
                return Directory.GetDirectories(path, searchPattern);
            }
            catch
            {
                return Array.Empty<string>();
            }
        }

        public void DeleteDirectory(string path, bool recursive = false)
        {
            if (string.IsNullOrEmpty(path)) return;
            if (IsResPath(path))
            {
                throw new InvalidOperationException($"GodotFileIO: cannot delete res:// directory {path} (read-only PCK)");
            }
            try
            {
                if (Directory.Exists(path))
                    Directory.Delete(path, recursive);
            }
            catch { /* cleanup: best-effort directory deletion */ }
        }
    }
}
