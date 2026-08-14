using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Economy (market port) save persistence — thin pattern sibling of the
    /// other host stores: user:// path, try/catch, codec.
    /// </summary>
    public static class EconomySaveStore
    {
        public const string FileName = "economy_save.json";

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(MarketState state)
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
                GD.PrintErr("[Economy] save failed: " + e.Message);
                return false;
            }
        }

        public static MarketState TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                return s_json.Deserialize<MarketState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Economy] load failed: " + e.Message);
                return null;
            }
        }
    }
}
