using System;

namespace Ashfall.Core.Factions
{
    [Serializable]
    public class MilitaryBranchSave
    {
        public const int CurrentSaveVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public MilitaryBranchSystemState branchSystem = new MilitaryBranchSystemState();

        /// <summary>Integrity hash computed over all payload fields.</summary>
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Serialization codec for the Military branch save section. Follows the
    /// same shape as YearOfAshSaveCodec: Capture/Restore talk to the live
    /// system, Encode/Decode talk to JSON text, and a stale checksum is
    /// always recomputed before encoding.
    /// </summary>
    public static class MilitaryBranchSaveCodec
    {
        public static MilitaryBranchSave Capture(MilitaryBranchSystem system)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));

            var save = new MilitaryBranchSave
            {
                branchSystem = system.CaptureState()
            };
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static void Restore(MilitaryBranchSave save, MilitaryBranchSystem system)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (system == null) throw new ArgumentNullException(nameof(system));
            system.RestoreState(save.branchSystem);
        }

        public static string Encode(MilitaryBranchSave save, IJsonSerializer json)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (json == null) throw new ArgumentNullException(nameof(json));
            // Always recompute: a caller may have mutated a captured save after
            // Capture() stamped it, and a stale checksum would poison the file.
            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        public static MilitaryBranchSave Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrEmpty(jsonText))
                throw new InvalidOperationException("MilitaryBranchSave: empty save payload.");
            if (json == null) throw new ArgumentNullException(nameof(json));

            var save = json.Deserialize<MilitaryBranchSave>(jsonText);
            if (save == null)
                throw new InvalidOperationException("MilitaryBranchSave: deserialization returned null.");

            if (save.saveVersion > MilitaryBranchSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    $"MilitaryBranchSave: saveVersion {save.saveVersion} is newer than supported ({MilitaryBranchSave.CurrentSaveVersion}).");

            // Only one schema version exists so far; no migration path is needed
            // yet. When a v2 shape is introduced, freeze this v1 shape exactly
            // as YearOfAshSaveV1 does, and add a MigrateToCurrent branch here.

            if (!string.IsNullOrEmpty(save.Checksum))
            {
                string actual = SaveChecksum.Compute(save);
                if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                    throw new InvalidOperationException("MilitaryBranchSave: checksum mismatch (corrupted or tampered save).");
            }

            return save;
        }
    }
}
