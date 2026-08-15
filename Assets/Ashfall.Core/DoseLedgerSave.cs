using System;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: THE DOSE — cross-host save envelope for the four dose registers.
    /// Same shape rules as the other expansion save envelopes: checksum recomputed
    /// on encode, hard-reject on decode for tamper/checksumless/newer version.
    /// </summary>
    [Serializable]
    public class DoseLedgerSave
    {
        public const int CurrentSaveVersion = 1;

        public int saveVersion = CurrentSaveVersion;
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
            VoluntaryRegisterSystem voluntaryRegister)
        {
            var save = new DoseLedgerSave
            {
                simDay = simDay,
                doseLedger = doseLedger.CaptureState(),
                sickList = sickList.CaptureState(),
                cohort = cohort.CaptureState(),
                voluntaryRegister = voluntaryRegister.CaptureState()
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
            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException("DoseLedgerSave: save carries no checksum (truncated or tampered file).");
            if (save.saveVersion > DoseLedgerSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "DoseLedgerSave: saveVersion " + save.saveVersion + " is newer than supported.");
            if (save.saveVersion < 1)
                throw new InvalidOperationException("DoseLedgerSave: invalid saveVersion.");
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
            VoluntaryRegisterSystem voluntaryRegister)
        {
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            doseLedger?.RestoreState(save.doseLedger);
            sickList?.RestoreState(save.sickList);
            cohort?.RestoreState(save.cohort);
            voluntaryRegister?.RestoreState(save.voluntaryRegister);
        }
    }
}