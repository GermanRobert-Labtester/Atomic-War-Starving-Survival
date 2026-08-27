// ============================================================================
// Save Store : MemorialSaveStore
// Core State : Ashfall.Core.MemorialSave
// Host Caller: Main.Campaign / MemorialHostSession
// Purpose    : Fallen survivor memorial wall, cause of death records, and shelter grief tallies
// ============================================================================
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
        public const string SectionName = "memorial";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(MemorialSave state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static MemorialSave? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(MemorialSave state)
    {
        try
        {
            if (state == null) return string.Empty;
            return new SystemTextJsonSerializer().Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[MemorialSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static MemorialSave? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return new SystemTextJsonSerializer().Deserialize<MemorialSave>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[MemorialSaveStore] restore failed: " + e.Message);
            return null;
        }
    }

        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

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

        public static MemorialSave? TryLoad()
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
