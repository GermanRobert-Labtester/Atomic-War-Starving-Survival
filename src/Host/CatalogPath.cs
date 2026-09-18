// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Resolves the shared Unity StreamingAssets/Data folder on disk.
    /// Precedence (deterministic, self-contained, CI-friendly, Linux-safe):
    /// 1. ASHFALL_DATA env override (explicit)
    /// 2. Executable-relative deployment (exported build: builds/linux/Assets/StreamingAssets/Data)
    /// 3. Project/Development directory on disk (globalized res:// or CWD walk)
    /// 4. PCK virtual FS via res:// (Godot FileAccess, when JSON is packed inside .pck)
    /// </summary>
    public static class CatalogPath
    {
        /// <summary>Plan 26A — how <see cref="ResolveDataDir"/> resolved the
        /// last call (boot diagnostics). One of: env, executable, project, cwd,
        /// pck, fallback.</summary>
        public static string LastResolutionSource { get; private set; } = "unresolved";

        public static string ResolveDataDir()
        {
            string? env = System.Environment.GetEnvironmentVariable("ASHFALL_DATA");
            if (!string.IsNullOrEmpty(env) && Directory.Exists(env))
            {
                LastResolutionSource = "env";
                return env;
            }

            // 2. Executable-relative — the exported, self-contained location.
            string exePath = OS.GetExecutablePath();
            if (!string.IsNullOrEmpty(exePath))
            {
                string exeDir = Path.GetDirectoryName(exePath) ?? string.Empty;
                if (!string.IsNullOrEmpty(exeDir) && CatalogLocator.TryFindDataDirectory(exeDir, out string foundExe))
                    return foundExe;
                if (CatalogLocator.TryFindDataDirectory(exePath, out string foundExe2))
                    return foundExe2;
                string direct = Path.Combine(exeDir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(direct))
                {
                    LastResolutionSource = "executable";
                    return direct;
                }
            }

            // 3. Project root on disk via globalized res:// (development & editor).
            string resPath = ProjectSettings.GlobalizePath("res://");
            if (!string.IsNullOrEmpty(resPath) && CatalogLocator.TryFindDataDirectory(resPath, out string foundRes))
            {
                LastResolutionSource = "project";
                return foundRes;
            }

            // 4. Current working directory walk (development checkout).
            string cwd = Directory.GetCurrentDirectory();
            if (!string.IsNullOrEmpty(cwd) && CatalogLocator.TryFindDataDirectory(cwd, out string foundCwd))
            {
                LastResolutionSource = "cwd";
                return foundCwd;
            }

            // 5. PCK virtual FS — when Data is packed inside the .pck only.
            const string resData = "res://Assets/StreamingAssets/Data";
            if (Godot.DirAccess.DirExistsAbsolute(resData))
            {
                LastResolutionSource = "pck";
                return resData;
            }
            const string resDataLower = "res://assets/StreamingAssets/Data";
            if (Godot.DirAccess.DirExistsAbsolute(resDataLower))
            {
                LastResolutionSource = "pck";
                return resDataLower;
            }
            const string resDataRoot = "res://StreamingAssets/Data";
            if (Godot.DirAccess.DirExistsAbsolute(resDataRoot))
            {
                LastResolutionSource = "pck";
                return resDataRoot;
            }

            string fallback = ProjectSettings.GlobalizePath(resData);
            LastResolutionSource = "fallback";
            return fallback;
        }

        /// <summary>Plan 26A — resolve a bare catalog file name under the one
        /// data root. Rejects traversal/absolute/separator input.</summary>
        public static string ResolveCatalog(string fileName)
        {
            if (!IsSafeSegment(fileName))
                throw new ArgumentException($"unsafe catalog file name: '{fileName}'", nameof(fileName));
            return Combine(ResolveDataDir(), fileName);
        }

        /// <summary>Plan 26A — resolve a file inside a single subdirectory of the
        /// data root (e.g. <c>ResolveSub("narrative", "x.json")</c>).</summary>
        public static string ResolveSub(string dir, string fileName)
        {
            if (!IsSafeSegment(dir))
                throw new ArgumentException($"unsafe catalog subdirectory: '{dir}'", nameof(dir));
            if (!IsSafeSegment(fileName))
                throw new ArgumentException($"unsafe catalog file name: '{fileName}'", nameof(fileName));
            return Combine(Combine(ResolveDataDir(), dir), fileName);
        }

        private static bool IsSafeSegment(string? segment)
        {
            if (string.IsNullOrEmpty(segment)) return false;
            if (segment.Contains("..")) return false;
            if (segment.IndexOf('/') >= 0 || segment.IndexOf('\\') >= 0) return false;
            if (segment.IndexOf(':') >= 0) return false;
            if (Path.IsPathRooted(segment)) return false;
            return true;
        }

        private static string Combine(string root, string leaf)
            => root.StartsWith("res://", StringComparison.Ordinal)
                ? root.TrimEnd('/') + "/" + leaf
                : Path.Combine(root, leaf);


        public static IFileIO CreateFileIOForDataDir(string dataDir)
        {
            if (!string.IsNullOrEmpty(dataDir) && dataDir.StartsWith("res://", StringComparison.Ordinal))
                return new Host.GodotFileIO();
            return new FileSystemIO();
        }

        /// <summary>
        /// Resolves the repository root directory on disk.
        /// Walks up from the resolved data directory, project setting root, or executable path.
        /// </summary>
        public static string ResolveRepoRoot()
        {
            string res = ProjectSettings.GlobalizePath("res://");
            if (!string.IsNullOrEmpty(res) && Directory.Exists(Path.Combine(res, "src")))
                return res.TrimEnd('/', '\\');

            string data = ResolveDataDir();
            if (!string.IsNullOrEmpty(data) && !data.StartsWith("res://", StringComparison.Ordinal))
            {
                var dir = new DirectoryInfo(data);
                while (dir != null)
                {
                    if (Directory.Exists(Path.Combine(dir.FullName, "src")))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }

            string exe = OS.GetExecutablePath();
            if (!string.IsNullOrEmpty(exe))
            {
                var dir = new DirectoryInfo(Path.GetDirectoryName(exe) ?? "");
                while (dir != null)
                {
                    if (Directory.Exists(Path.Combine(dir.FullName, "src")))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }

            return Directory.GetCurrentDirectory();
        }
    }
}
