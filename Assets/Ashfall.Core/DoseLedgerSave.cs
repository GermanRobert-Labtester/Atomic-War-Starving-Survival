using System;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: THE DOSE — cross-host save envelope for the four dose registers.
    /// Same shape rules as the other expansion save envelopes: checksum recomputed
    /// on encode, hard-reject on decode for tamper/checksumless/newer version.
    ///
    /// v2 adds the Dose questline section. Dose quest progress is owned here —
    /// the Year of Ash envelope is no longer a second owner (its registration was
    /// removed). v1 saves migrate with an empty quest section; a one-time adoption
    /// helper folds any Dose quest progress a pre-v2 save carried inside the Year
    /// of Ash envelope into the Dose envelope (see <see cref="DoseQuestMigration"/>).
    ///
    /// Migration validates the checksum over each version's FROZEN shape (see
    /// <see cref="DoseLedgerSaveV1"/>) because <see cref="SaveChecksum"/> walks
    /// public fields — validating a legacy payload against the current shape would
    /// always mismatch.
    /// </summary>
    [Serializable]
    public class DoseLedgerSave
    {
        public const int CurrentSaveVersion = 2;
        public const int MigrationFromVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public DoseLedgerSystemState doseLedger = new DoseLedgerSystemState();
        public SickListSystemState sickList = new SickListSystemState();
        public CohortSystemState cohort = new CohortSystemState();
        public VoluntaryRegisterSystemState voluntaryRegister = new VoluntaryRegisterSystemState();
        public QuestlineSystemState quests = new QuestlineSystemState();

        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Frozen v1 envelope shape (no quest section). Kept so a v1 file on disk
    /// validates against the field set it was actually hashed with.
    /// Do not add fields here.
    /// </summary>
    [Serializable]
    public class DoseLedgerSaveV1
    {
        public int saveVersion = 1;
        public int simDay;
        public DoseLedgerSystemState doseLedger = new DoseLedgerSystemState();
        public SickListSystemState sickList = new SickListSystemState();
        public CohortSystemState cohort = new CohortSystemState();
        public VoluntaryRegisterSystemState voluntaryRegister = new VoluntaryRegisterSystemState();
        public string Checksum = string.Empty;
    }

    public static class DoseLedgerSaveCodec
    {
        public static DoseLedgerSave Capture(
            int simDay,
            DoseLedgerSystem doseLedger,
            SickListSystem sickList,
            CohortSystem cohort,
            VoluntaryRegisterSystem voluntaryRegister,
            QuestlineSystem quests = null)
        {
            var save = new DoseLedgerSave
            {
                simDay = simDay,
                doseLedger = doseLedger.CaptureState(),
                sickList = sickList.CaptureState(),
                cohort = cohort.CaptureState(),
                voluntaryRegister = voluntaryRegister.CaptureState(),
                quests = quests != null ? quests.CaptureState() : new QuestlineSystemState()
            };
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static string Encode(DoseLedgerSave save, IJsonSerializer json)
        {
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        /// <summary>
        /// Decodes and migrates a Dose save. Legacy versions are parsed as their
        /// FROZEN shapes so the checksum is verified over exactly the fields that
        /// version wrote. Rejects: newer versions, too-old versions, checksumless
        /// payloads, and tampered payloads.
        /// </summary>
        public static DoseLedgerSave Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
                throw new InvalidOperationException("DoseLedgerSave: empty save payload.");

            DoseLedgerSave save;
            try { save = json.Deserialize<DoseLedgerSave>(jsonText); }
            catch (Exception e)
            {
                throw new InvalidOperationException("DoseLedgerSave: malformed save payload: " + e.Message, e);
            }
            if (save == null)
                throw new InvalidOperationException("DoseLedgerSave: empty save payload.");
            if (save.saveVersion > DoseLedgerSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "DoseLedgerSave: saveVersion " + save.saveVersion + " is newer than supported.");
            if (save.saveVersion < DoseLedgerSave.MigrationFromVersion)
                throw new InvalidOperationException("DoseLedgerSave: invalid saveVersion.");

            if (save.saveVersion == 1)
            {
                var v1 = json.Deserialize<DoseLedgerSaveV1>(jsonText);
                if (v1 == null)
                    throw new InvalidOperationException("DoseLedgerSave: v1 deserialization returned null.");
                if (string.IsNullOrEmpty(v1.Checksum))
                    throw new InvalidOperationException("DoseLedgerSave: save carries no checksum (truncated or tampered file).");
                if (!string.Equals(SaveChecksum.Compute(v1), v1.Checksum, StringComparison.Ordinal))
                    throw new InvalidOperationException("DoseLedgerSave: checksum mismatch (corrupt or foreign save).");

                var migrated = new DoseLedgerSave
                {
                    saveVersion = DoseLedgerSave.CurrentSaveVersion,
                    simDay = v1.simDay,
                    doseLedger = v1.doseLedger,
                    sickList = v1.sickList,
                    cohort = v1.cohort,
                    voluntaryRegister = v1.voluntaryRegister
                    // quests stays at its field initialiser (fresh default).
                };
                migrated.Checksum = SaveChecksum.Compute(migrated);
                return migrated;
            }

            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException("DoseLedgerSave: save carries no checksum (truncated or tampered file).");
            string actual = SaveChecksum.Compute(save);
            if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                throw new InvalidOperationException("DoseLedgerSave: checksum mismatch (corrupt or foreign save).");
            return save;
        }

        public static void Restore(
            DoseLedgerSave save,
            DoseLedgerSystem doseLedger,
            SickListSystem sickList,
            CohortSystem cohort,
            VoluntaryRegisterSystem voluntaryRegister,
            QuestlineSystem quests = null)
        {
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            doseLedger?.RestoreState(save.doseLedger);
            sickList?.RestoreState(save.sickList);
            cohort?.RestoreState(save.cohort);
            voluntaryRegister?.RestoreState(save.voluntaryRegister);
            if (quests != null)
                quests.RestoreState(save.quests ?? new QuestlineSystemState());
        }
    }
}
