// ============================================================================
// Save Store : EncounterChoiceSaveStore
// Core State : Ashfall.Core.EncounterChoiceState
// Host Caller: Main.Expeditions / EncounterChoiceHostSession
// Purpose    : Expedition encounter branch decisions, player choices, and consequence flags
// ============================================================================
using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// EncounterChoice resolver save persistence — closes the
    /// Setup-without-Save triad gap found by the audit on Main.cs.
    /// Pattern sibling of Combat/Narrative stores.
    /// </summary>
    public static class EncounterChoiceSaveStore
    {
        public const string FileName = "encounter_choice_save.json";
        public const string SectionName = "encounter_choice";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(EncounterChoiceState state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static EncounterChoiceState? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(EncounterChoiceState state)
    {
        try
        {
            if (state == null) return string.Empty;
            return s_json.Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[EncounterChoiceSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static EncounterChoiceState? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return s_json.Deserialize<EncounterChoiceState>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[EncounterChoiceSaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(EncounterChoiceState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new EncounterChoiceSaveEnvelope { State = state };
                envelope.Checksum = SaveChecksum.Compute(envelope);
                string path = SavePath;
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(path, s_json.Serialize(envelope));
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr("[EncounterChoice] save failed: " + e.Message);
                return false;
            }
        }

        public static EncounterChoiceState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<EncounterChoiceSaveEnvelope>(raw);
                if (envelope == null) return null;
                if (string.IsNullOrEmpty(envelope.Checksum))
                {
                    GD.PrintErr("[EncounterChoice] save envelope missing checksum (corrupt save)");
                    return null;
                }
                string computed = SaveChecksum.Compute(envelope);
                if (!string.Equals(envelope.Checksum, computed, StringComparison.Ordinal))
                {
                    GD.PrintErr("[EncounterChoice] checksum mismatch — possible tampering");
                    return null;
                }
                return envelope.State;
            }
            catch (Exception e)
            {
                GD.PrintErr("[EncounterChoice] load failed: " + e.Message);
                return null;
            }
        }
    }

    [Serializable]
    public sealed class EncounterChoiceSaveEnvelope
    {
        public EncounterChoiceState State;
        public string Checksum;
    }
}
