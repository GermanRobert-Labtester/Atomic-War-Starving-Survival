using System;

namespace Ashfall.Core.Factions
{
    [Serializable]
    public class RebelBranchSave
    {
        public const int CurrentSaveVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public RebelBranchSystemState branchSystem = new RebelBranchSystemState();

        /// <summary>Integrity hash computed over all payload fields.</summary>
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Serialization codec for the Rebel branch save section. Structural
    /// mirror of MilitaryBranchSaveCodec: Capture/Restore talk to the live
    /// system, Encode/Decode talk to JSON text, and a stale checksum is
    /// always recomputed before encoding.
    /// </summary>
    public static class RebelBranchSaveCodec
    {
        public static RebelBranchSave Capture(RebelBranchSystem system)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));

            var save = new RebelBranchSave
            {
                branchSystem = system.CaptureState()
            };
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static void Restore(RebelBranchSave save, RebelBranchSystem system)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (system == null) throw new ArgumentNullException(nameof(system));
            system.RestoreState(save.branchSystem);
        }

        public static string Encode(RebelBranchSave save, IJsonSerializer json)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (json == null) throw new ArgumentNullException(nameof(json));
            // Always recompute: a caller may have mutated a captured save after
            // Capture() stamped it, and a stale checksum would poison the file.
            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        public static RebelBranchSave Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrEmpty(jsonText))
                throw new InvalidOperationException("RebelBranchSave: empty save payload.");
            if (json == null) throw new ArgumentNullException(nameof(json));

            var save = json.Deserialize<RebelBranchSave>(jsonText);
            if (save == null)
                throw new InvalidOperationException("RebelBranchSave: deserialization returned null.");

            if (save.saveVersion > RebelBranchSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    $"RebelBranchSave: saveVersion {save.saveVersion} is newer than supported ({RebelBranchSave.CurrentSaveVersion}).");

            // Only one schema version exists so far; no migration path is needed
            // yet. When a v2 shape is introduced, freeze this v1 shape exactly
            // as MilitaryBranchSave/YearOfAshSaveV1 do, and add a
            // MigrateToCurrent branch here.

            if (!string.IsNullOrEmpty(save.Checksum))
            {
                string actual = SaveChecksum.Compute(save);
                if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                    throw new InvalidOperationException("RebelBranchSave: checksum mismatch (corrupted or tampered save).");
            }

            return save;
        }
    }
}
