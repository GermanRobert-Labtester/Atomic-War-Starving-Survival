// ============================================================================
// Save Store : ShelterThermalSaveStore
// Core State : Ashfall.Core.ShelterThermalState
// Host Caller: Main.ShelterInfrastructure / ShelterThermalHostSession
// Purpose    : Shelter thermal insulation, heating zones, fuel consumption, and cold exposure
// ============================================================================
using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    [Serializable]
    public sealed class ShelterThermalHostSave
    {
        public string SchemaVersion { get; set; } = "1.0";
        public ShelterThermalState State { get; set; }
        public string Checksum { get; set; } = string.Empty;
    }

    public static class ShelterThermalSaveStore
    {
        public const string FileName = "shelter_thermal_save.json";
        public const string SectionName = "shelter_thermal";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(ShelterThermalState state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static ShelterThermalState? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(ShelterThermalState state)
    {
        try
        {
            if (state == null) return string.Empty;
            return s_json.Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[ShelterThermalSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static ShelterThermalState? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return s_json.Deserialize<ShelterThermalState>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[ShelterThermalSaveStore] restore failed: " + e.Message);
            return null;
        }
    }

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath => SaveSlotRoot.Resolve(FileName);
        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(ShelterThermalState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new ShelterThermalHostSave { State = state };
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
                GD.PrintErr("[Thermal] save failed: " + e.Message);
                return false;
            }
        }

        public static ShelterThermalState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var envelope = s_json.Deserialize<ShelterThermalHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    return envelope.State;
                }

                return s_json.Deserialize<ShelterThermalState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Thermal] load failed: " + e.Message);
                return null;
            }
        }
    }
}
