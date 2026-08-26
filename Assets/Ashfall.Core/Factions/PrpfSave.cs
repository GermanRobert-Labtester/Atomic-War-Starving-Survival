using System;

namespace Ashfall.Core.Factions
{
    [Serializable]
    public class PrpfSave
    {
        public const int CurrentSaveVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public PrpfSystemState prpf = new PrpfSystemState();

        /// <summary>Integrity hash computed over all payload fields.</summary>
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Serialization codec for the PRPF standing/alignment save section.
    /// Structural mirror of MilitaryBranchSaveCodec/RebelBranchSaveCodec.
    /// </summary>
    public static class PrpfSaveCodec
    {
        public static PrpfSave Capture(PrpfStandingSystem system)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));

            var save = new PrpfSave
            {
                prpf = system.CaptureState()
            };
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static void Restore(PrpfSave save, PrpfStandingSystem system)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (system == null) throw new ArgumentNullException(nameof(system));
            system.RestoreState(save.prpf);
        }

        public static string Encode(PrpfSave save, IJsonSerializer json)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (json == null) throw new ArgumentNullException(nameof(json));
            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        public static PrpfSave Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrEmpty(jsonText))
                throw new InvalidOperationException("PrpfSave: empty save payload.");
            if (json == null) throw new ArgumentNullException(nameof(json));

            var save = json.Deserialize<PrpfSave>(jsonText);
            if (save == null)
                throw new InvalidOperationException("PrpfSave: deserialization returned null.");

            if (save.saveVersion > PrpfSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    $"PrpfSave: saveVersion {save.saveVersion} is newer than supported ({PrpfSave.CurrentSaveVersion}).");

            if (!string.IsNullOrEmpty(save.Checksum))
            {
                string actual = SaveChecksum.Compute(save);
                if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                    throw new InvalidOperationException("PrpfSave: checksum mismatch (corrupted or tampered save).");
            }

            return save;
        }
    }
}
