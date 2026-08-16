using System;
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

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(WorldWeatherState state, SkyArmorSaveState skyArmor = null)
        {
            try
            {
                if (state == null) return false;
                var envelope = new WorldHostSave { State = state, SkyArmor = skyArmor };
                envelope.Checksum = SaveChecksum.Compute(envelope);
                string path = SavePath;
                string dir = Path.GetDirectoryName(path);
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

        public static WorldHostSave TryLoadEnvelope()
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
                    if (!string.IsNullOrEmpty(envelope.Checksum))
                    {
                        string actual = SaveChecksum.Compute(envelope);
                        if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                        {
                            GD.PrintErr("[World] load failed: checksum mismatch (corrupt or foreign save).");
                            return null;
                        }
                    }
                    return envelope;
                }

                // Legacy bare-state save (written before the checksum envelope).
                var legacy = s_json.Deserialize<WorldWeatherState>(raw);
                return legacy != null ? new WorldHostSave { State = legacy } : null;
            }
            catch (Exception e)
            {
                GD.PrintErr("[World] load failed: " + e.Message);
                return null;
            }
        }

        public static WorldWeatherState TryLoad()
        {
            return TryLoadEnvelope()?.State;
        }
    }

    /// <summary>World save envelope: engine state + sky armor + integrity checksum.</summary>
    public class WorldHostSave
    {
        public WorldWeatherState State;
        public SkyArmorSaveState SkyArmor;
        public string Checksum = string.Empty;
    }
}
