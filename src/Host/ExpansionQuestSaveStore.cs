// ============================================================================
// Save Store : ExpansionQuestSaveStore
// Core State : Ashfall.Core.ExpansionQuestSaveEnvelope
// Host Caller: Main.Quests / ExpansionQuestHostSession
// Purpose    : Expansion quest graph runtime states, objective progression, and quest flags
// ============================================================================
using System;
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="ExpansionQuestSaveEnvelope"/> as JSON under user://expansion_quest_save.json
    /// using the core IFileIO / SystemTextJsonSerializer ports.
    /// </summary>
    public static class ExpansionQuestSaveStore
    {
        public const string FileName = "expansion_quest_save.json";
        public const string SectionName = "expansion_quest";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(ExpansionQuestSaveEnvelope envelope)
    {
        return TryCapture(envelope);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static ExpansionQuestSaveEnvelope? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(ExpansionQuestSaveEnvelope envelope)
    {
        try
        {
            if (envelope == null) return string.Empty;
            return new SystemTextJsonSerializer().Serialize(envelope);
        }
        catch (Exception e)
        {
            GD.PrintErr("[ExpansionQuestSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static ExpansionQuestSaveEnvelope? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return new SystemTextJsonSerializer().Deserialize<ExpansionQuestSaveEnvelope>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[ExpansionQuestSaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        /// <summary>Writes through the codec (checksum stamped).</summary>
        public static void Save(ExpansionQuestSaveEnvelope envelope)
        {
            if (envelope == null) return;
            try
            {
                s_files.WriteAllText(SavePath, ExpansionQuestSaveCodec.Encode(envelope, s_json));
            }
            catch (Exception e)
            {
                s_log.Error("[ExpansionQuestSaveStore] save failed: " + e.Message);
            }
        }

        /// <summary>Reads and validates through the codec. Returns null when absent or corrupt.</summary>
        public static ExpansionQuestSaveEnvelope TryLoad(string pathOverride = null!)
        {
            try
            {
                string path = pathOverride ?? SavePath;
                if (!s_files.FileExists(path)) return null;
                return ExpansionQuestSaveCodec.Decode(s_files.ReadAllText(path), s_json);
            }
            catch (Exception e)
            {
                s_log.Error("[ExpansionQuestSaveStore] load failed: " + e.Message);
                return null;
            }
        }
    }
}
