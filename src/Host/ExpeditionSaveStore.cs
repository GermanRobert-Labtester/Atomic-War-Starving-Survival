using System;
using System.IO;
using System.Collections.Generic;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Expedition (Encounters port) save persistence — thin pattern sibling of
    /// the other host stores: user:// path, try/catch, checksummed envelope.
    /// Legacy bare-state saves (pre-checksum) still load.
    /// </summary>
    public static class ExpeditionSaveStore
    {
        public const string FileName = "expedition_save.json";
        public const string SectionName = "expedition";
    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(List<ExpeditionState> state)
    {
        try
        {
            if (state == null) return string.Empty;
            return new SystemTextJsonSerializer().Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[ExpeditionSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static List<ExpeditionState>? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return new SystemTextJsonSerializer().Deserialize<List<ExpeditionState>>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[ExpeditionSaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(List<ExpeditionState> state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new ExpeditionHostSave { State = state };
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
                GD.PrintErr("[Expedition] save failed: " + e.Message);
                return false;
            }
        }

        public static List<ExpeditionState>? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var envelope = s_json.Deserialize<ExpeditionHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    // A non-empty checksum field is required for any save in the
                    // new envelope format. Empty/null previously slipped past as
                    // "legacy" — a malformed save in the new format must be
                    // rejected, not silently trusted.
                    if (string.IsNullOrEmpty(envelope.Checksum))
                    {
                        GD.PrintErr("[Expedition] load failed: checksum field missing (corrupt save).");
                        return null;
                    }
                    string actual = SaveChecksum.Compute(envelope);
                    if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                    {
                        GD.PrintErr("[Expedition] load failed: checksum mismatch (corrupt or foreign save).");
                        return null;
                    }
                    return envelope.State;
                }

                // Legacy bare-state save (written before the checksum envelope).
                return s_json.Deserialize<List<ExpeditionState>>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Expedition] load failed: " + e.Message);
                return null;
            }
        }
    }

    /// <summary>Expedition save envelope: engine state + integrity checksum.</summary>
    public class ExpeditionHostSave
    {
        public List<ExpeditionState> State;
        public string Checksum = string.Empty;
    }
}
