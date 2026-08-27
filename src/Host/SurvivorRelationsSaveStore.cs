// ============================================================================
// Save Store : SurvivorRelationsSaveStore
// Core State : Ashfall.Core.SurvivorRelationsState
// Host Caller: Main.ShelterSocial / SurvivorRelationsHostSession
// Purpose    : Interpersonal survivor affinities, rivalries, trust bonds, and social friction
// ============================================================================
using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    [Serializable]
    public sealed class SurvivorRelationsHostSave
    {
        public string SchemaVersion { get; set; } = "1.0";
        public SurvivorRelationsState State { get; set; }
        public string Checksum { get; set; } = string.Empty;
    }

    public static class SurvivorRelationsSaveStore
    {
        public const string FileName = "survivor_relations_save.json";
        public const string SectionName = "survivor_relations";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(SurvivorRelationsState state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static SurvivorRelationsState? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(SurvivorRelationsState state)
    {
        try
        {
            if (state == null) return string.Empty;
            return s_json.Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[SurvivorRelationsSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static SurvivorRelationsState? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return s_json.Deserialize<SurvivorRelationsState>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[SurvivorRelationsSaveStore] restore failed: " + e.Message);
            return null;
        }
    }

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath => SaveSlotRoot.Resolve(FileName);
        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(SurvivorRelationsState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new SurvivorRelationsHostSave { State = state };
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
                GD.PrintErr("[Relations] save failed: " + e.Message);
                return false;
            }
        }

        public static SurvivorRelationsState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var envelope = s_json.Deserialize<SurvivorRelationsHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    return envelope.State;
                }

                return s_json.Deserialize<SurvivorRelationsState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Relations] load failed: " + e.Message);
                return null;
            }
        }
    }
}
