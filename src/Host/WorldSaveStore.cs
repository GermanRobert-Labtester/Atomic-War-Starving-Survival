using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// World (weather port) save persistence — thin pattern sibling of the
    /// other host stores: user:// path, try/catch, checksummed envelope.
    /// Legacy bare-state saves (pre-checksum) still load.
    /// </summary>
    public static class WorldSaveStore
    {
        public const string FileName = "world_save.json";
        public const string SectionName = "world";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(WorldHostSave envelope)
    {
        return TryCapture(envelope);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static WorldHostSave? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(WorldHostSave envelope)
    {
        try
        {
            if (envelope == null) return string.Empty;
            return new SystemTextJsonSerializer().Serialize(envelope);
        }
        catch (Exception e)
        {
            GD.PrintErr("[WorldSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static WorldHostSave? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return new SystemTextJsonSerializer().Deserialize<WorldHostSave>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[WorldSaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(
            WorldWeatherState state,
            SkyArmorSaveState skyArmor = null!,
            LocationEvolutionSaveState locationEvolution = null!,
            WildlifeSaveState wildlife = null!,
            LandmarkSaveState landmark = null!)
        {
            try
            {
                if (state == null) return false;
                var envelope = new WorldHostSave
                {
                    State = state,
                    SkyArmor = skyArmor,
                    LocationEvolution = locationEvolution,
                    Wildlife = wildlife,
                    Landmark = landmark
                };
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
                GD.PrintErr("[World] save failed: " + e.Message);
                return false;
            }
        }

        public static WorldHostSave? TryLoadEnvelope()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var envelope = s_json.Deserialize<WorldHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    // A non-empty checksum field is required for any save in the
                    // new envelope format. Empty/null previously slipped past as
                    // "legacy" — a malformed save in the new format must be
                    // rejected, not silently trusted.
                    if (string.IsNullOrEmpty(envelope.Checksum))
                    {
                        GD.PrintErr("[World] load failed: checksum field missing (corrupt save).");
                        return null;
                    }
                    string actual = SaveChecksum.Compute(envelope);
                    if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                    {
                        GD.PrintErr("[World] load failed: checksum mismatch (corrupt or foreign save).");
                        return null;
                    }
                    return envelope;
                }

                // Legacy bare-state save (written before the checksum envelope).
                var legacy = s_json.Deserialize<WorldWeatherState>(raw);
                return legacy == null ? null : new WorldHostSave { State = legacy };
            }
            catch (Exception e)
            {
                GD.PrintErr("[World] load failed: " + e.Message);
                return null;
            }
        }

        public static WorldWeatherState? TryLoad()
        {
            return TryLoadEnvelope()?.State;
        }
    }

    /// <summary>World save envelope: engine state + sky armor + integrity checksum.</summary>
    public class WorldHostSave
    {
        public WorldWeatherState State;
        public SkyArmorSaveState SkyArmor;
        public LocationEvolutionSaveState LocationEvolution;
        public WildlifeSaveState Wildlife;
        public LandmarkSaveState Landmark;
        public string Checksum = string.Empty;
    }
}
