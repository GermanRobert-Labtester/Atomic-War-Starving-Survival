using System;

namespace Ashfall.Core.Factions
{
    [Serializable]
    public class IndependentBranchSave
    {
        public const int CurrentSaveVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public IndependentBranchSystemState branchSystem = new IndependentBranchSystemState();

        /// <summary>Integrity hash computed over all payload fields.</summary>
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Serialization codec for the Independent branch save section.
    /// Structural mirror of MilitaryBranchSaveCodec/RebelBranchSaveCodec.
    /// </summary>
    public static class IndependentBranchSaveCodec
    {
        public static IndependentBranchSave Capture(IndependentBranchSystem system)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));

            var save = new IndependentBranchSave
            {
                branchSystem = system.CaptureState()
            };
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static void Restore(IndependentBranchSave save, IndependentBranchSystem system)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (system == null) throw new ArgumentNullException(nameof(system));
            system.RestoreState(save.branchSystem);
        }

        public static string Encode(IndependentBranchSave save, IJsonSerializer json)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (json == null) throw new ArgumentNullException(nameof(json));
            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        public static IndependentBranchSave Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrEmpty(jsonText))
                throw new InvalidOperationException("IndependentBranchSave: empty save payload.");
            if (json == null) throw new ArgumentNullException(nameof(json));

            var save = json.Deserialize<IndependentBranchSave>(jsonText);
            if (save == null)
                throw new InvalidOperationException("IndependentBranchSave: deserialization returned null.");

            if (save.saveVersion > IndependentBranchSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    $"IndependentBranchSave: saveVersion {save.saveVersion} is newer than supported ({IndependentBranchSave.CurrentSaveVersion}).");

            if (!string.IsNullOrEmpty(save.Checksum))
            {
                string actual = SaveChecksum.Compute(save);
                if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                    throw new InvalidOperationException("IndependentBranchSave: checksum mismatch (corrupted or tampered save).");
            }

            return save;
        }
    }
}
