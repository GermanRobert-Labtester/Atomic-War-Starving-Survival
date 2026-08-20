using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="PowerGridSave"/> as JSON under
    /// <c>user://power_grid_save.json</c> using the core
    /// <see cref="IFileIO"/> / <see cref="SystemTextJsonSerializer"/> ports.
    /// Shape/checksum live in <see cref="Ashfall.Core.Shelter.PowerGridSaveCodec"/>.
    /// </summary>
    public static class PowerGridSaveStore
    {
        public const string FileName = "power_grid_save.json";

        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(PowerGridSave save)
        {
            if (save == null) return false;
            try
            {
                s_files.WriteAllText(SavePath, PowerGridSaveCodec.EncodeToString(save, s_json));
                return true;
            }
            catch (Exception e)
            {
                s_log.Error("[PowerGridSaveStore] save failed: " + e.Message);
                return false;
            }
        }

        public static PowerGridSave TryLoad()
        {
            try
            {
                if (!s_files.FileExists(SavePath)) return null;
                return PowerGridSaveCodec.Decode(s_files.ReadAllText(SavePath), s_json);
            }
            catch (Exception e)
            {
                s_log.Error("[PowerGridSaveStore] load failed: " + e.Message);
                return null;
            }
        }
    }
}
