using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists ChemicalDependencyLedgerState under user://chemical_dependency_save.json.
    /// Checksummed envelope (pattern sibling of ExpeditionSaveStore);
    /// legacy bare-state saves (pre-checksum) still load.
    /// </summary>
    public static class ChemicalDependencySaveStore
    {
        public const string FileName = "chemical_dependency_save.json";
        public const string SectionName = "chemical_dependency";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(ChemicalDependencyLedgerState state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static ChemicalDependencyLedgerState? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(ChemicalDependencyLedgerState state)
    {
        try
        {
            if (state == null) return string.Empty;
            return new SystemTextJsonSerializer().Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[ChemicalDependencySaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static ChemicalDependencyLedgerState? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return new SystemTextJsonSerializer().Deserialize<ChemicalDependencyLedgerState>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[ChemicalDependencySaveStore] restore failed: " + e.Message);
            return null;
        }
    }

        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool TrySave(ChemicalDependencyLedgerState state)
        {
            if (state == null) return false;
            try
            {
                var envelope = new ChemicalDependencyHostSave { State = state };
                envelope.Checksum = SaveChecksum.Compute(envelope);
                s_files.WriteAllText(SavePath, s_json.Serialize(envelope));
                return true;
            }
            catch (Exception e)
            {
                s_log.Error("[ChemicalDependencySaveStore] save failed: " + e.Message);
                return false;
            }
        }

        public static ChemicalDependencyLedgerState? TryLoad()
        {
            try
            {
                if (!s_files.FileExists(SavePath)) return null;
                string raw = s_files.ReadAllText(SavePath);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var envelope = s_json.Deserialize<ChemicalDependencyHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    // A non-empty checksum field is required for any save in the
                    // new envelope format. Empty/null is a malformed new-format
                    // save — reject it, do not silently trust it.
                    if (string.IsNullOrEmpty(envelope.Checksum))
                    {
                        s_log.Error("[ChemicalDependencySaveStore] load failed: checksum field missing (corrupt save).");
                        return null;
                    }
                    string actual = SaveChecksum.Compute(envelope);
                    if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                    {
                        s_log.Error("[ChemicalDependencySaveStore] load failed: checksum mismatch (corrupt or foreign save).");
                        return null;
                    }
                    return envelope.State;
                }

                // Legacy bare-state save (written before the checksum envelope).
                return s_json.Deserialize<ChemicalDependencyLedgerState>(raw);
            }
            catch (Exception e)
            {
                s_log.Error("[ChemicalDependencySaveStore] load failed: " + e.Message);
                return null;
            }
        }
    }

    /// <summary>ChemicalDependency save envelope: engine state + integrity checksum.</summary>
    public class ChemicalDependencyHostSave
    {
        public ChemicalDependencyLedgerState State;
        public string Checksum = string.Empty;
    }
}
