using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Muster;

namespace AtomicWar.GodotApp
{
    /// <summary>Combined Muster envelope: escalation state + coalition camp + the six
    /// Section V current state machines + Hydro-Barons.</summary>
    public class MusterHostSave
    {
        public MusterState Muster;
        public CoalitionCampState Camp;
        public ColdCountState ColdCount;
        public ProvisionedState Provisioned;
        public LongWalkState LongWalk;
        public ScavengerGuildState ScavengerGuild;
        public IronRaidersState IronRaiders;
        public HydroBaronsState HydroBarons;
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Muster (Expansion 06) save persistence — thin pattern sibling of
    /// PhantomMemorySaveStore: user:// path, try/catch, codec serialization.
    /// </summary>
    public static class MusterSaveStore
    {
        public const string FileName = "muster_save.json";

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(MusterHostSave save)
        {
            try
            {
                if (save == null) return false;
                string path = SavePath;
                string dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);
                // Recompute so a mutated envelope cannot persist a stale hash.
                save.Checksum = SaveChecksum.Compute(save);
                System.IO.File.WriteAllText(path, s_json.Serialize(save));
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Muster] save failed: " + e.Message);
                return false;
            }
        }

        public static MusterHostSave TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var save = s_json.Deserialize<MusterHostSave>(raw);
                if (save == null) return null;
                // The checksummed envelope is the current Muster format; an empty
                // checksum means a malformed new-format save, not "legacy". A
                // pre-envelope bare-state file yields a null-shaped envelope and
                // falls through to a fresh state (no partial restore).
                if (string.IsNullOrEmpty(save.Checksum))
                {
                    GD.PrintErr("[Muster] load failed: checksum field missing (corrupt save).");
                    return null;
                }
                string actual = SaveChecksum.Compute(save);
                if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                {
                    GD.PrintErr("[Muster] load failed: checksum mismatch (corrupt or foreign save).");
                    return null;
                }
                return save;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Muster] load failed: " + e.Message);
                return null;
            }
        }
    }
}
