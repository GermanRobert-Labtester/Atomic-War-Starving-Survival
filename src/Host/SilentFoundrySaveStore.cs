using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Foundry;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Silent Foundry save persistence — closes the Setup-without-Save triad gap
    /// found by the audit on Main.cs. Pattern sibling of Combat/Narrative stores.
    /// </summary>
    public static class SilentFoundrySaveStore
    {
        public const string FileName = "silent_foundry_save.json";

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(SilentFoundryState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new SilentFoundrySaveEnvelope { State = state };
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
                GD.PrintErr("[SilentFoundry] save failed: " + e.Message);
                return false;
            }
        }

        public static SilentFoundryState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<SilentFoundrySaveEnvelope>(raw);
                if (envelope == null) return null;
                if (string.IsNullOrEmpty(envelope.Checksum))
                {
                    GD.PrintErr("[SilentFoundry] save envelope missing checksum (corrupt save)");
                    return null;
                }
                string computed = SaveChecksum.Compute(envelope);
                if (!string.Equals(envelope.Checksum, computed, StringComparison.Ordinal))
                {
                    GD.PrintErr("[SilentFoundry] checksum mismatch — possible tampering");
                    return null;
                }
                return envelope.State;
            }
            catch (Exception e)
            {
                GD.PrintErr("[SilentFoundry] load failed: " + e.Message);
                return null;
            }
        }
    }

    [Serializable]
    public sealed class SilentFoundrySaveEnvelope
    {
        public SilentFoundryState State;
        public string Checksum;
    }
}
