using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Combat save persistence — thin pattern sibling of the other host stores:
    /// user:// path, try/catch, checksummed envelope. Legacy bare-state saves
    /// (pre-checksum) still load and are migrated.
    /// </summary>
    public static class CombatSaveStore
    {
        public const string FileName = "combat_save.json";
        public const string SectionName = "combat";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(CombatState state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static CombatState? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(CombatState state)
    {
        try
        {
            if (state == null) return string.Empty;
            return s_json.Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[CombatSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static CombatState? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return s_json.Deserialize<CombatState>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[CombatSaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(CombatState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new CombatHostSave { State = state };
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
                GD.PrintErr("[Combat] save failed: " + e.Message);
                return false;
            }
        }

        public static CombatState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var envelope = s_json.Deserialize<CombatHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    // A non-empty checksum is required for any save in the new
                    // envelope format. Empty/null previously slipped past as
                    // "legacy" — a malformed save in the new format must be
                    // rejected, not silently trusted.
                    if (string.IsNullOrEmpty(envelope.Checksum))
                    {
                        GD.PrintErr("[Combat] load failed: checksum field missing (corrupt save).");
                        return null;
                    }
                    string actual = SaveChecksum.Compute(envelope);
                    if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                    {
                        GD.PrintErr("[Combat] load failed: checksum mismatch (corrupt or foreign save).");
                        return null;
                    }
                    return envelope.State;
                }

                // Legacy bare-state save (written before the checksum envelope).
                return s_json.Deserialize<CombatState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Combat] load failed: " + e.Message);
                return null;
            }
        }
    }

    /// <summary>Combat save envelope: engine state + integrity checksum.</summary>
    public class CombatHostSave
    {
        public CombatState State;
        public string Checksum = string.Empty;
    }
}
