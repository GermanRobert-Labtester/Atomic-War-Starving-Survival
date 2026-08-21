using System;
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    [Serializable]
    public sealed class VinylMoraleHostSave
    {
        public string SchemaVersion { get; set; } = "1.0";
        public VinylMoraleState State { get; set; }
        public string Checksum { get; set; } = string.Empty;
    }

    public static class VinylMoraleSaveStore
    {
        public const string FileName = "vinyl_morale_save.json";
        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath => Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);
        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(VinylMoraleState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new VinylMoraleHostSave { State = state };
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
                GD.PrintErr("[Vinyl] save failed: " + e.Message);
                return false;
            }
        }

        public static VinylMoraleState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var envelope = s_json.Deserialize<VinylMoraleHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    return envelope.State;
                }

                return s_json.Deserialize<VinylMoraleState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Vinyl] load failed: " + e.Message);
                return null;
            }
        }
    }
}
