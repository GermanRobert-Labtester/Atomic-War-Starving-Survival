using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    [Serializable]
    public sealed class ShelterScheduleHostSave
    {
        public string SchemaVersion { get; set; } = "1.0";
        public ShelterScheduleState State { get; set; }
        public string Checksum { get; set; } = string.Empty;
    }

    public static class ShelterScheduleSaveStore
    {
        public const string FileName = "shelter_schedule_save.json";
        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath => Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);
        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(ShelterScheduleState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new ShelterScheduleHostSave { State = state };
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
                GD.PrintErr("[Schedule] save failed: " + e.Message);
                return false;
            }
        }

        public static ShelterScheduleState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var envelope = s_json.Deserialize<ShelterScheduleHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    return envelope.State;
                }

                return s_json.Deserialize<ShelterScheduleState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Schedule] load failed: " + e.Message);
                return null;
            }
        }
    }
}
