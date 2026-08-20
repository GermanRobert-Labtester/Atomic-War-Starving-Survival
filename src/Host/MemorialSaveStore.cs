using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Memorial;

namespace AtomicWar.GodotApp
{
    /// <summary>Persists MemorialState under user://memorial_save.json.</summary>
    public static class MemorialSaveStore
    {
        public const string FileName = "memorial_save.json";
        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool TrySave(MemorialSave save)
        {
            if (save == null) return false;
            try
            {
                save.Checksum = SaveChecksum.Compute(save);
                s_files.WriteAllText(SavePath, s_json.Serialize(save));
                return true;
            }
            catch (Exception e)
            {
                s_log.Error("[MemorialSaveStore] save failed: " + e.Message);
                return false;
            }
        }

        public static MemorialSave TryLoad()
        {
            try
            {
                if (!s_files.FileExists(SavePath)) return null;
                var save = s_json.Deserialize<MemorialSave>(s_files.ReadAllText(SavePath));
                if (save == null) return null;
                if (string.IsNullOrEmpty(save.Checksum))
                    throw new InvalidOperationException("MemorialSave: empty checksum");
                if (!string.Equals(save.Checksum, SaveChecksum.Compute(save), StringComparison.Ordinal))
                    throw new InvalidOperationException("MemorialSave: checksum mismatch");
                return save;
            }
            catch (Exception e)
            {
                s_log.Error("[MemorialSaveStore] load failed: " + e.Message);
                return null;
            }
        }
    }
}
