using System;
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="HoldfastSave"/> as JSON under user://holdfast_s1_save.json
    /// using the core IFileIO / SystemTextJsonSerializer ports. The save shape and all
    /// validation live in Ashfall.Core.HoldfastSaveCodec — this type only picks the
    /// Godot path and the log. Mirrors src/Journal/JournalSaveStore.cs.
    /// </summary>
    public static class HoldfastSaveStore
    {
        public const string FileName = "holdfast_s1_save.json";
        public const string SectionName = "holdfast_s1";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(HoldfastSave state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static HoldfastSave? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(HoldfastSave state)
    {
        try
        {
            if (state == null) return string.Empty;
            return new SystemTextJsonSerializer().Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[HoldfastSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static HoldfastSave? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return new SystemTextJsonSerializer().Deserialize<HoldfastSave>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[HoldfastSaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        /// <summary>Writes through the codec (checksum stamped). Returns false on failure.</summary>
        public static bool TrySave(HoldfastSave save, string pathOverride = null!)
        {
            try
            {
                string path = pathOverride ?? SavePath;
                s_files.WriteAllText(path, HoldfastSaveCodec.Encode(save, s_json));
                return true;
            }
            catch (Exception e)
            {
                s_log.Error("[HoldfastSaveStore] save failed: " + e.Message);
                return false;
            }
        }

        /// <summary>Reads and validates through the codec. Returns null when absent or corrupt.</summary>
        public static HoldfastSave? TryLoad(string pathOverride = null!)
        {
            try
            {
                string path = pathOverride ?? SavePath;
                if (!s_files.FileExists(path)) return null;
                return HoldfastSaveCodec.Decode(s_files.ReadAllText(path), s_json);
            }
            catch (Exception e)
            {
                s_log.Error("[HoldfastSaveStore] load failed: " + e.Message);
                return null;
            }
        }
    }
}
