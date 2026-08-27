// ============================================================================
// Save Store : ThirdonarySaveStore
// Core State : Ashfall.Core.Thirdonary.ThirdonarySaveEnvelope
// Host Caller: Main.Quests / ThirdonaryHostSession
// Purpose    : Thirdonary expansion quest narrative trees and faction cipher state
// ============================================================================
using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Thirdonary;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists ThirdonarySaveEnvelope as JSON under user://thirdonary_quest_save.json
    /// using the core IFileIO / SystemTextJsonSerializer ports.
    /// Follows the ExpansionQuestSaveStore pattern exactly.
    /// </summary>
    public static class ThirdonarySaveStore
    {
        public const string FileName = "thirdonary_quest_save.json";
        public const string SectionName = "thirdonary";

        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static void Save(ThirdonarySaveEnvelope envelope)
        {
            if (envelope == null) return;
            try
            {
                s_files.WriteAllText(SavePath, ThirdonarySaveCodec.Encode(envelope, s_json));
            }
            catch (Exception e)
            {
                s_log.Error("[ThirdonarySaveStore] save failed: " + e.Message);
            }
        }

        public static ThirdonarySaveEnvelope? TryLoad(string pathOverride = null!)
        {
            try
            {
                string path = pathOverride ?? SavePath;
                if (!s_files.FileExists(path)) return null;
                return ThirdonarySaveCodec.Decode(s_files.ReadAllText(path), s_json);
            }
            catch (Exception e)
            {
                s_log.Error("[ThirdonarySaveStore] load failed: " + e.Message);
                return null;
            }
        }

        public static string TryCapture(ThirdonarySaveEnvelope envelope)
        {
            try
            {
                if (envelope == null) return string.Empty;
                return s_json.Serialize(envelope);
            }
            catch (Exception e)
            {
                GD.PrintErr("[ThirdonarySaveStore] capture failed: " + e.Message);
                return string.Empty;
            }
        }

        public static ThirdonarySaveEnvelope? TryRestore(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json)) return null;
                return s_json.Deserialize<ThirdonarySaveEnvelope>(json);
            }
            catch (Exception e)
            {
                GD.PrintErr("[ThirdonarySaveStore] restore failed: " + e.Message);
                return null;
            }
        }
    }
}
