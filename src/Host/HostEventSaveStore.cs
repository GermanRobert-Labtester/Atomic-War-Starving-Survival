using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using AtomicWar.GodotApp.Host;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="HostEventState"/> as JSON under
    /// <c>user://host_event_save.json</c> using the core
    /// <see cref="IFileIO"/> / <see cref="SystemTextJsonSerializer"/> ports.
    /// Checksummed envelope (pattern sibling of ExpeditionSaveStore);
    /// legacy bare-state saves (pre-checksum) still load.
    /// </summary>
    public static class HostEventSaveStore
    {
        public const string FileName = "host_event_save.json";
        public const string SectionName = "host_event";

        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static string TryCapture(HostEventState state)
        {
            try
            {
                if (state == null) return string.Empty;
                return s_json.Serialize(state);
            }
            catch (Exception e)
            {
                GD.PrintErr("[HostEventSaveStore] capture failed: " + e.Message);
                return string.Empty;
            }
        }

        public static HostEventState? TryRestore(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json)) return null;
                return s_json.Deserialize<HostEventState>(json);
            }
            catch (Exception e)
            {
                GD.PrintErr("[HostEventSaveStore] restore failed: " + e.Message);
                return null;
            }
        }

        public static bool TrySave(HostEventState state)
        {
            if (state == null) return false;
            try
            {
                var envelope = new HostEventHostSave { State = state };
                envelope.Checksum = SaveChecksum.Compute(envelope);
                s_files.WriteAllText(SavePath, s_json.Serialize(envelope));
                return true;
            }
            catch (Exception e)
            {
                s_log.Error("[HostEventSaveStore] save failed: " + e.Message);
                return false;
            }
        }

        public static HostEventState? TryLoad()
        {
            try
            {
                if (!s_files.FileExists(SavePath)) return null;
                string raw = s_files.ReadAllText(SavePath);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var envelope = s_json.Deserialize<HostEventHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    // A non-empty checksum field is required for any save in the
                    // new envelope format. Empty/null is a malformed new-format
                    // save — reject it, do not silently trust it.
                    if (string.IsNullOrEmpty(envelope.Checksum))
                    {
                        s_log.Error("[HostEventSaveStore] load failed: checksum field missing (corrupt save).");
                        return null;
                    }
                    string actual = SaveChecksum.Compute(envelope);
                    if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                    {
                        s_log.Error("[HostEventSaveStore] load failed: checksum mismatch (corrupt or foreign save).");
                        return null;
                    }
                    return envelope.State;
                }

                // Legacy bare-state save (written before the checksum envelope).
                return s_json.Deserialize<HostEventState>(raw);
            }
            catch (Exception e)
            {
                s_log.Error("[HostEventSaveStore] load failed: " + e.Message);
                return null;
            }
        }
    }

    /// <summary>HostEvent save envelope: engine state + integrity checksum.</summary>
    public class HostEventHostSave
    {
        public HostEventState State;
        public string Checksum = string.Empty;
    }
}
