using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Muster;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Muster (Expansion 06) save persistence — thin pattern sibling of
    /// PhantomMemorySaveStore: user:// path, try/catch, codec serialization.
    /// </summary>
    public static class MusterSaveStore
    {
        public const string FileName = "muster_save.json";

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(MusterState state)
        {
            try
            {
                if (state == null) return false;
                string path = SavePath;
                string dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(path, s_json.Serialize(state));
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Muster] save failed: " + e.Message);
                return false;
            }
        }

        public static MusterState TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                return s_json.Deserialize<MusterState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Muster] load failed: " + e.Message);
                return null;
            }
        }
    }
}
