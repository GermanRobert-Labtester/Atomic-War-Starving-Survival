using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Resolves the shared Unity StreamingAssets/Data folder on disk.
    /// JSON is not copied; both engines read the same files.
    /// </summary>
    public static class CatalogPath
    {
        public static string ResolveDataDir()
        {
            string? env = System.Environment.GetEnvironmentVariable("ASHFALL_DATA");
            if (!string.IsNullOrEmpty(env) && Directory.Exists(env))
                return env;

            string[] starts =
            {
                Directory.GetCurrentDirectory(),
                ProjectSettings.GlobalizePath("res://"),
                OS.GetExecutablePath()
            };

            for (int i = 0; i < starts.Length; i++)
            {
                if (string.IsNullOrEmpty(starts[i])) continue;
                if (CatalogLocator.TryFindDataDirectory(starts[i], out string found))
                    return found;
            }

            string fallback = ProjectSettings.GlobalizePath("res://Assets/StreamingAssets/Data");
            return fallback;
        }
    }
}
