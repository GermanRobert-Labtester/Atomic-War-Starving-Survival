// ============================================================================
// Save Store : RegionalTreatySaveStore
// Core State : Ashfall.Core.RegionalTreatyState
// Host Caller: Main.ShelterSocial / RegionalTreatyHostSession
// Purpose    : Regional faction pacts, non-aggression treaties, and border agreements
// ============================================================================
using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    [Serializable]
    public sealed class RegionalTreatyHostSave
    {
        public string SchemaVersion { get; set; } = "1.0";
        public RegionalTreatyState State { get; set; }
        public string Checksum { get; set; } = string.Empty;
    }

    public static class RegionalTreatySaveStore
    {
        public const string FileName = "regional_treaty_save.json";
        public const string SectionName = "regional_treaty";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(RegionalTreatyState state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static RegionalTreatyState? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(RegionalTreatyState state)
    {
        try
        {
            if (state == null) return string.Empty;
            return s_json.Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[RegionalTreatySaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static RegionalTreatyState? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return s_json.Deserialize<RegionalTreatyState>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[RegionalTreatySaveStore] restore failed: " + e.Message);
            return null;
        }
    }

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath => SaveSlotRoot.Resolve(FileName);
        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(RegionalTreatyState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new RegionalTreatyHostSave { State = state };
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
                GD.PrintErr("[Treaty] save failed: " + e.Message);
                return false;
            }
        }

        public static RegionalTreatyState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var envelope = s_json.Deserialize<RegionalTreatyHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    return envelope.State;
                }

                return s_json.Deserialize<RegionalTreatyState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Treaty] load failed: " + e.Message);
                return null;
            }
        }
    }
}
