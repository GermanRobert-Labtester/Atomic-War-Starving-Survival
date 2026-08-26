using System;
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="ExpansionHubSave"/> as JSON under
    /// user://expansion_hub_save.json using the core IFileIO / SystemTextJsonSerializer
    /// ports. Shape and validation live in Ashfall.Core.ExpansionHubSaveCodec.
    /// Mirrors the other Godot save stores.
    /// </summary>
    public static class ExpansionHubSaveStore
    {
        public const string FileName = "expansion_hub_save.json";
        public const string SectionName = "expansion_hub";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(ExpansionHubSave state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static ExpansionHubSave? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(ExpansionHubSave state)
    {
        try
        {
            if (state == null) return string.Empty;
            return new SystemTextJsonSerializer().Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[ExpansionHubSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static ExpansionHubSave? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return new SystemTextJsonSerializer().Deserialize<ExpansionHubSave>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[ExpansionHubSaveStore] restore failed: " + e.Message);
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
        public static bool TrySave(ExpansionHubSave save, string pathOverride = null!)
        {
            if (save == null) return false;
            try
            {
                string path = pathOverride ?? SavePath;
                s_files.WriteAllText(path, ExpansionHubSaveCodec.Encode(save, s_json));
                return true;
            }
            catch (Exception e)
            {
                s_log.Error("[ExpansionHubSaveStore] save failed: " + e.Message);
                return false;
            }
        }

        /// <summary>Reads and validates through the codec. Returns null when absent or corrupt.</summary>
        public static ExpansionHubSave? TryLoad(string pathOverride = null!)
        {
            try
            {
                string path = pathOverride ?? SavePath;
                if (!s_files.FileExists(path)) return null;
                return ExpansionHubSaveCodec.Decode(s_files.ReadAllText(path), s_json);
            }
            catch (Exception e)
            {
                s_log.Error("[ExpansionHubSaveStore] load failed: " + e.Message);
                return null;
            }
        }
    }
}
