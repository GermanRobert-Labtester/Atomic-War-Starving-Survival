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
    /// 3. PCK virtual FS via res:// (Godot FileAccess, when JSON is packed)
    /// 4. Current working directory walk (development)
    /// 5. res:// globalized fallback (editor)
    /// </summary>
    public static class CatalogPath
    {
        public static string ResolveDataDir()
        {
            string? env = System.Environment.GetEnvironmentVariable("ASHFALL_DATA");
            if (!string.IsNullOrEmpty(env) && Directory.Exists(env))
                return env;

            // 2. Executable-relative — the exported, self-contained location.
            // Walk parents of the executable file itself.
            string exePath = OS.GetExecutablePath();
            if (!string.IsNullOrEmpty(exePath))
            {
                string exeDir = Path.GetDirectoryName(exePath) ?? string.Empty;
                if (!string.IsNullOrEmpty(exeDir) && CatalogLocator.TryFindDataDirectory(exeDir, out string foundExe))
                    return foundExe;
                // Also try walking from the full executable path (file) — TryFindDataDirectory handles file→parent.
                if (CatalogLocator.TryFindDataDirectory(exePath, out string foundExe2))
                    return foundExe2;
                // Direct executable-adjacent check without walk (fast path for builds/linux layout).
                string direct = Path.Combine(exeDir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(direct))
                    return direct;
            }

            // 3. PCK virtual FS — when Data is packed inside the .pck (future-proof).
            // Use Godot's DirAccess which can see inside the PCK; System.IO cannot.
            const string resData = "res://Assets/StreamingAssets/Data";
            if (Godot.DirAccess.DirExistsAbsolute(resData))
                return ProjectSettings.GlobalizePath(resData);
            // Also try lower-case Godot assets tree (if data migrated to assets/).
            const string resDataLower = "res://assets/StreamingAssets/Data";
            if (Godot.DirAccess.DirExistsAbsolute(resDataLower))
                return ProjectSettings.GlobalizePath(resDataLower);

            // 4. Current working directory walk (development checkout).
            string cwd = Directory.GetCurrentDirectory();
            if (!string.IsNullOrEmpty(cwd) && CatalogLocator.TryFindDataDirectory(cwd, out string foundCwd))
                return foundCwd;

            string resPath = ProjectSettings.GlobalizePath("res://");
            if (!string.IsNullOrEmpty(resPath) && CatalogLocator.TryFindDataDirectory(resPath, out string foundRes))
                return foundRes;

            string fallback = ProjectSettings.GlobalizePath(resData);
            return fallback;
        }

        public static IFileIO CreateFileIOForDataDir(string dataDir)
        {
            if (!string.IsNullOrEmpty(dataDir) && dataDir.StartsWith("res://", StringComparison.Ordinal))
                return new Host.GodotFileIO();
            return new FileSystemIO();
        }
    }
}
