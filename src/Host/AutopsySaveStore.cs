using System;
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    [Serializable]
    public sealed class AutopsyHostSave
    {
        public string SchemaVersion { get; set; } = "1.0";
        public AutopsyState State { get; set; }
        public string Checksum { get; set; } = string.Empty;
    }

    public static class AutopsySaveStore
    {
        public const string FileName = "autopsy_save.json";
        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath => Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);
        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(AutopsyState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new AutopsyHostSave { State = state };
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
                GD.PrintErr("[Autopsy] save failed: " + e.Message);
                return false;
            }
        }

        public static AutopsyState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var envelope = s_json.Deserialize<AutopsyHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    return envelope.State;
                }

                return s_json.Deserialize<AutopsyState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Autopsy] load failed: " + e.Message);
                return null;
            }
        }
    }
}
