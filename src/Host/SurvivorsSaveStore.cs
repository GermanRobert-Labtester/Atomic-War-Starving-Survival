using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Survivors (needs + radiation) save persistence — checksummed envelope,
    /// thin pattern sibling of InventorySaveStore.
    /// </summary>
    public static class SurvivorsSaveStore
    {
        public const string FileName = "survivors_save.json";

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(SurvivorsSaveState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new SurvivorsHostSave { State = state };
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
                GD.PrintErr("[Survivors] save failed: " + e.Message);
                return false;
            }
        }

        public static SurvivorsSaveState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<SurvivorsHostSave>(raw);
                if (envelope == null || envelope.State == null) return null;
                // The checksummed envelope is the current Survivors format; an
                // empty checksum means a malformed new-format save, not "legacy".
                if (string.IsNullOrEmpty(envelope.Checksum))
                {
                    GD.PrintErr("[Survivors] load failed: checksum field missing (corrupt save).");
                    return null;
                }
                string actual = SaveChecksum.Compute(envelope);
                if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                {
                    GD.PrintErr("[Survivors] load failed: checksum mismatch (corrupt or foreign save).");
                    return null;
                }
                return envelope.State;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Survivors] load failed: " + e.Message);
                return null;
            }
        }
    }

    /// <summary>Survivors envelope: engine state + integrity checksum.</summary>
    public class SurvivorsHostSave
    {
        public SurvivorsSaveState State;
        public string Checksum = string.Empty;
    }
}
