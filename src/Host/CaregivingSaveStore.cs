using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    [Serializable]
    public sealed class CaregivingHostSave
    {
        public string SchemaVersion { get; set; } = "1.0";
        public CaregivingSaveState State { get; set; }
        public string Checksum { get; set; } = string.Empty;
    }

    public static class CaregivingSaveStore
    {
        public const string FileName = "caregiving_save.json";
        public const string SectionName = "caregiving";

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(CaregivingSaveState state)
        {
            return TryCapture(state);
        }

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static CaregivingSaveState? TryRestoreDirect(string json)
        {
            return TryRestore(json);
        }

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(CaregivingSaveState state)
        {
            try
            {
                if (state == null) return string.Empty;
                return s_json.Serialize(state);
            }
            catch (Exception e)
            {
                GD.PrintErr("[CaregivingSaveStore] capture failed: " + e.Message);
                return string.Empty;
            }
        }

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static CaregivingSaveState? TryRestore(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json)) return null;
                return s_json.Deserialize<CaregivingSaveState>(json);
            }
            catch (Exception e)
            {
                GD.PrintErr("[CaregivingSaveStore] restore failed: " + e.Message);
                return null;
            }
        }

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath => SaveSlotRoot.Resolve(FileName);
        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(CaregivingSaveState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new CaregivingHostSave { State = state };
                envelope.Checksum = SaveChecksum.Compute(envelope);
                string path = SavePath;
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllText(path, s_json.Serialize(envelope));
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Caregiving] save failed: " + e.Message);
                return false;
            }
        }

        public static CaregivingSaveState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var envelope = s_json.Deserialize<CaregivingHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    return envelope.State;
                }

                return s_json.Deserialize<CaregivingSaveState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Caregiving] load failed: " + e.Message);
                return null;
            }
        }
    }
}
