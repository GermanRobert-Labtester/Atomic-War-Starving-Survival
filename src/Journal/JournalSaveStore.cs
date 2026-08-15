using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Journal;

namespace AtomicWar.Journal
{
    /// <summary>
    /// Persists JournalSave as JSON under user://journal_save.json via the
    /// engine-agnostic core IJsonSerializer (cross-host portable shape) inside a
    /// checksummed envelope, matching every other host save store.
    /// Legacy bare-state saves (pre-checksum) still load.
    /// </summary>
    public static class JournalSaveStore
    {
        public const string FileName = "journal_save.json";

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static void Save(JournalSave save, string? pathOverride = null)
        {
            if (save == null) return;
            try
            {
                var envelope = new JournalHostSave { State = save };
                envelope.Checksum = SaveChecksum.Compute(envelope);
                string path = pathOverride ?? SavePath;
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(path, s_json.Serialize(envelope));
            }
            catch (Exception e)
            {
                GD.PrintErr($"[JournalSaveStore] save failed: {e.Message}");
            }
        }

        public static JournalSave? Load(string? pathOverride = null)
        {
            try
            {
                string path = pathOverride ?? SavePath;
                if (!s_files.FileExists(path)) return null;
                string json = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json)) return null;

                var envelope = s_json.Deserialize<JournalHostSave>(json);
                if (envelope != null && envelope.State != null)
                {
                    if (!string.IsNullOrEmpty(envelope.Checksum))
                    {
                        string actual = SaveChecksum.Compute(envelope);
                        if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                        {
                            GD.PrintErr("[JournalSaveStore] load failed: checksum mismatch (corrupt or foreign save).");
                            return null;
                        }
                    }
                    return envelope.State;
                }

                // Legacy bare-state save (written before the checksum envelope).
                return s_json.Deserialize<JournalSave>(json);
            }
            catch (Exception e)
            {
                GD.PrintErr($"[JournalSaveStore] load failed: {e.Message}");
                return null;
            }
        }
    }

    /// <summary>Journal save envelope: engine state + integrity checksum.</summary>
    public class JournalHostSave
    {
        public JournalSave State;
        public string Checksum = string.Empty;
    }
}
