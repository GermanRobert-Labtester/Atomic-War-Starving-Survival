using System;

namespace Ashfall.Core
{
    /// <summary>
    /// Cross-host save envelope for ASHFALL: THE DUTY ROSTER (Exp 02).
    /// Carries the chart rows, the morale marks, and the shelter-encounter
    /// counters. Written through the IJsonSerializer port so a save written by
    /// one host loads in the other, same as HoldfastSave and YearOfAshSave.
    /// Spec: docs/expansions/expansion_02_the_duty_roster_plan.md §5.
    /// </summary>
    [Serializable]
    public class DutyRosterSave
    {
        public const int CurrentSaveVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public DutyRosterSystemState roster = new DutyRosterSystemState();
        public MoraleMarkSystemState marks = new MoraleMarkSystemState();
        public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();

        /// <summary>Integrity hash computed over all payload fields.</summary>
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Serialization codec for the Duty Roster expansion state. Same rules as
    /// HoldfastSaveCodec: the checksum is recomputed on encode, and decode
    /// hard-rejects an empty payload, a missing/mismatched checksum (tamper or
    /// foreign file), or a saveVersion this build cannot read.
    /// </summary>
    public static class DutyRosterSaveCodec
    {
        public static DutyRosterSave Capture(
            DutyRosterSystem roster,
            MoraleMarkSystem marks,
            ShelterEncounterSystem encounters,
            IClock clock)
        {
            var save = new DutyRosterSave
            {
                simDay = clock != null ? clock.Day : 0,
                roster = roster.CaptureState(),
                marks = marks.CaptureState(),
                encounters = encounters.CaptureState()
            };
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static string Encode(DutyRosterSave save, IJsonSerializer json)
        {
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            // Always recompute: a caller may have mutated a captured save after
            // Capture() stamped it, and a stale checksum would poison the file.
            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        public static DutyRosterSave Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
                throw new InvalidOperationException("DutyRosterSave: empty save payload.");

            DutyRosterSave save;
            try
            {
                save = json.Deserialize<DutyRosterSave>(jsonText);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    "DutyRosterSave: malformed save payload: " + e.Message, e);
            }

            if (save == null)
                throw new InvalidOperationException("DutyRosterSave: empty save payload.");

            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException(
                    "DutyRosterSave: save carries no checksum (truncated or tampered file).");
            if (save.saveVersion > DutyRosterSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "DutyRosterSave: saveVersion " + save.saveVersion
                    + " is newer than this build supports (" + DutyRosterSave.CurrentSaveVersion + ").");
            if (save.saveVersion < 1)
                throw new InvalidOperationException(
                    "DutyRosterSave: saveVersion " + save.saveVersion + " is not a valid version.");

            string actual = SaveChecksum.Compute(save);
            if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "DutyRosterSave: checksum mismatch (corrupt or foreign save).");

            return save;
        }

        /// <summary>Restores all systems and the sim clock. Idempotent.</summary>
        public static void Restore(
            DutyRosterSave save,
            DutyRosterSystem roster,
            MoraleMarkSystem marks,
            ShelterEncounterSystem encounters,
            IClock clock)
        {
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            roster.RestoreState(save.roster);
            marks?.RestoreState(save.marks);
            encounters?.RestoreState(save.encounters);
            if (clock != null && save.simDay > 0)
                clock.SetDay(save.simDay);
        }
    }
}
