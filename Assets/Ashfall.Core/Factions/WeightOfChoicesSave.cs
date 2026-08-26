using System;

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Combined save envelope for "The Weight of Choices" branching system.
    /// A playthrough commits to at most ONE of Military, Rebel, or
    /// Independent (never more than one — the three branch systems are
    /// mutually exclusive per design), but PRPF standing/alignment is always
    /// present regardless of which base faction the player picked, or even
    /// if they picked none yet. All three per-faction sections are always
    /// written so the envelope shape stays constant; whichever ones the
    /// player never committed to simply stay at their fresh-system defaults
    /// (uncommitted, unlocked) and are harmless dead weight in the file.
    ///
    /// v2 adds the Independent branch section. v1 files (Military + Rebel +
    /// PRPF only) migrate with a fresh, uncommitted Independent section.
    /// </summary>
    [Serializable]
    public class WeightOfChoicesSave
    {
        public const int CurrentSaveVersion = 2;

        public int saveVersion = CurrentSaveVersion;
        public MilitaryBranchSystemState militaryBranch = new MilitaryBranchSystemState();
        public RebelBranchSystemState rebelBranch = new RebelBranchSystemState();
        public IndependentBranchSystemState independentBranch = new IndependentBranchSystemState();
        public PrpfSystemState prpf = new PrpfSystemState();

        /// <summary>Integrity hash computed over all payload fields.</summary>
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Frozen v1 envelope shape (Military + Rebel + PRPF only, no Independent
    /// section). Kept so a v1 file on disk validates against the field set
    /// it was actually hashed with — SaveChecksum walks public fields, so
    /// validating a v1 payload against the v2 shape would always mismatch.
    /// Do not add fields here.
    /// </summary>
    [Serializable]
    public class WeightOfChoicesSaveV1
    {
        public int saveVersion = 1;
        public MilitaryBranchSystemState militaryBranch = new MilitaryBranchSystemState();
        public RebelBranchSystemState rebelBranch = new RebelBranchSystemState();
        public PrpfSystemState prpf = new PrpfSystemState();
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Serialization codec for the combined Military + Rebel + Independent +
    /// PRPF envelope. Follows the same Capture/Restore/Encode/Decode shape as
    /// YearOfAshSaveCodec and the individual per-faction codecs, but composes
    /// all four live systems in one call so a host session only needs to
    /// hold one save section for the whole branching system.
    /// </summary>
    public static class WeightOfChoicesSaveCodec
    {
        public static WeightOfChoicesSave Capture(
            MilitaryBranchSystem militaryBranch,
            RebelBranchSystem rebelBranch,
            IndependentBranchSystem independentBranch,
            PrpfStandingSystem prpf)
        {
            if (militaryBranch == null) throw new ArgumentNullException(nameof(militaryBranch));
            if (rebelBranch == null) throw new ArgumentNullException(nameof(rebelBranch));
            if (independentBranch == null) throw new ArgumentNullException(nameof(independentBranch));
            if (prpf == null) throw new ArgumentNullException(nameof(prpf));

            var save = new WeightOfChoicesSave
            {
                militaryBranch = militaryBranch.CaptureState(),
                rebelBranch = rebelBranch.CaptureState(),
                independentBranch = independentBranch.CaptureState(),
                prpf = prpf.CaptureState()
            };
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static void Restore(
            WeightOfChoicesSave save,
            MilitaryBranchSystem militaryBranch,
            RebelBranchSystem rebelBranch,
            IndependentBranchSystem independentBranch,
            PrpfStandingSystem prpf)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (militaryBranch == null) throw new ArgumentNullException(nameof(militaryBranch));
            if (rebelBranch == null) throw new ArgumentNullException(nameof(rebelBranch));
            if (independentBranch == null) throw new ArgumentNullException(nameof(independentBranch));
            if (prpf == null) throw new ArgumentNullException(nameof(prpf));

            militaryBranch.RestoreState(save.militaryBranch);
            rebelBranch.RestoreState(save.rebelBranch);
            independentBranch.RestoreState(save.independentBranch);
            prpf.RestoreState(save.prpf);
        }

        public static string Encode(WeightOfChoicesSave save, IJsonSerializer json)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (json == null) throw new ArgumentNullException(nameof(json));
            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        public static WeightOfChoicesSave Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrEmpty(jsonText))
                throw new InvalidOperationException("WeightOfChoicesSave: empty save payload.");
            if (json == null) throw new ArgumentNullException(nameof(json));

            var save = json.Deserialize<WeightOfChoicesSave>(jsonText);
            if (save == null)
                throw new InvalidOperationException("WeightOfChoicesSave: deserialization returned null.");

            if (save.saveVersion > WeightOfChoicesSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    $"WeightOfChoicesSave: saveVersion {save.saveVersion} is newer than supported ({WeightOfChoicesSave.CurrentSaveVersion}).");

            // A v1 file was hashed over the v1 field set. Validate it against the frozen
            // v1 shape and upgrade in place; the Independent section starts fresh.
            if (save.saveVersion < WeightOfChoicesSave.CurrentSaveVersion)
                return MigrateToCurrent(jsonText, json, save.saveVersion);

            if (!string.IsNullOrEmpty(save.Checksum))
            {
                string actual = SaveChecksum.Compute(save);
                if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                    throw new InvalidOperationException("WeightOfChoicesSave: checksum mismatch (corrupted or tampered save).");
            }

            return save;
        }

        private static WeightOfChoicesSave MigrateToCurrent(string jsonText, IJsonSerializer json, int version)
        {
            if (version == 1)
            {
                var v1 = json.Deserialize<WeightOfChoicesSaveV1>(jsonText);
                if (v1 == null)
                    throw new InvalidOperationException("WeightOfChoicesSave: v1 deserialization returned null.");

                if (!string.IsNullOrEmpty(v1.Checksum))
                {
                    string actual = SaveChecksum.Compute(v1);
                    if (!string.Equals(v1.Checksum, actual, StringComparison.Ordinal))
                        throw new InvalidOperationException("WeightOfChoicesSave: checksum mismatch (corrupted or tampered save).");
                }

                var upgraded = new WeightOfChoicesSave
                {
                    saveVersion = WeightOfChoicesSave.CurrentSaveVersion,
                    militaryBranch = v1.militaryBranch,
                    rebelBranch = v1.rebelBranch,
                    prpf = v1.prpf
                    // independentBranch stays at its field initialiser (fresh, uncommitted).
                };
                upgraded.Checksum = SaveChecksum.Compute(upgraded);
                return upgraded;
            }

            throw new InvalidOperationException(
                $"WeightOfChoicesSave: no migration path from saveVersion {version}.");
        }

        /// <summary>
        /// True if the player has committed to more than one of
        /// Military/Rebel/Independent simultaneously — a data-integrity
        /// invariant violation, since the three branch systems are mutually
        /// exclusive by design. Hosts should check this after Restore and
        /// treat a true result as a corrupt save, not a valid multi-faction
        /// state.
        /// </summary>
        public static bool HasConflictingFactionCommitment(WeightOfChoicesSave save)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            int committedCount = 0;
            if (save.militaryBranch?.branch?.committed == true) committedCount++;
            if (save.rebelBranch?.branch?.committed == true) committedCount++;
            if (save.independentBranch?.branch?.committed == true) committedCount++;
            return committedCount > 1;
        }
    }
}
