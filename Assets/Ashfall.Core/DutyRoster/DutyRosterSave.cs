using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Cross-host save envelope for ASHFALL: THE DUTY ROSTER (Exp 02).
    /// Carries the chart rows, the morale marks, the shelter-encounter
    /// counters, the Overflow practice, and the quest runtime ledger.
    /// Written through the IJsonSerializer port so a save written by one
    /// host loads in the other, same as HoldfastSave and YearOfAshSave.
    /// Spec: docs/expansions/expansion_02_the_duty_roster_plan.md §5.
    /// </summary>
    [Serializable]
    public class DutyRosterSave
    {
        /// <summary>
        /// v2 added the bounded Overflow practice state; v3 adds the quest
        /// runtime ledger. v1/v2 saves migrate forward with safe defaults.
        /// </summary>
        public const int CurrentSaveVersion = 3;

        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public DutyRosterSystemState roster = new DutyRosterSystemState();
        public MoraleMarkSystemState marks = new MoraleMarkSystemState();
        public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();
        public DutyRosterOverflowState overflow = new DutyRosterOverflowState();
        public DutyRosterQuestState quests = new DutyRosterQuestState();

        /// <summary>Integrity hash computed over all payload fields.</summary>
        public string Checksum = string.Empty;
    }

    /// <summary>Bounded Overflow practice state (v2+; a small authenticated void, not a district).</summary>
    [Serializable]
    public class DutyRosterOverflowState
    {
        public bool access;
        public List<string> visitedNodes = new List<string>();
    }

    /// <summary>Frozen v1 shape — validates legacy saves against their own checksum.</summary>
    [Serializable]
    public sealed class DutyRosterSaveV1
    {
        public int saveVersion = 1;
        public int simDay;
        public DutyRosterSystemState roster = new DutyRosterSystemState();
        public MoraleMarkSystemState marks = new MoraleMarkSystemState();
        public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();
        public string Checksum = string.Empty;
    }

    /// <summary>Frozen v2 shape (Overflow added; quest ledger did not exist).</summary>
    [Serializable]
    public sealed class DutyRosterSaveV2
    {
        public int saveVersion = 2;
        public int simDay;
        public DutyRosterSystemState roster = new DutyRosterSystemState();
        public MoraleMarkSystemState marks = new MoraleMarkSystemState();
        public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();
        public DutyRosterOverflowState overflow = new DutyRosterOverflowState();
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
            IClock clock,
            DutyRosterQuestRuntime quests = null)
        {
            var save = new DutyRosterSave
            {
                simDay = clock != null ? clock.Day : 0,
                roster = roster.CaptureState(),
                marks = marks.CaptureState(),
                encounters = encounters.CaptureState(),
                overflow = roster.CaptureOverflowState(),
                quests = quests != null ? quests.CaptureState() : new DutyRosterQuestState()
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

            try
            {
                // v1 saves are validated against their FROZEN shape (the Overflow
                // and quest-ledger fields added later are dropped, never blessed)
                // and migrated forward with safe defaults.
                var v1 = json.Deserialize<DutyRosterSaveV1>(jsonText);
                if (v1 != null && v1.saveVersion == 1)
                {
                    ValidateChecksum(v1.Checksum, v1, "v1");
                    var migrated = new DutyRosterSave
                    {
                        saveVersion = DutyRosterSave.CurrentSaveVersion,
                        simDay = v1.simDay,
                        roster = v1.roster ?? new DutyRosterSystemState(),
                        marks = v1.marks ?? new MoraleMarkSystemState(),
                        encounters = v1.encounters ?? new ShelterEncounterSystemState(),
                        overflow = new DutyRosterOverflowState(),
                        quests = new DutyRosterQuestState()
                    };
                    migrated.Checksum = SaveChecksum.Compute(migrated);
                    return migrated;
                }

                // v2 saves carry the Overflow but predate the quest ledger.
                var v2 = json.Deserialize<DutyRosterSaveV2>(jsonText);
                if (v2 != null && v2.saveVersion == 2)
                {
                    ValidateChecksum(v2.Checksum, v2, "v2");
                    var migrated = new DutyRosterSave
                    {
                        saveVersion = DutyRosterSave.CurrentSaveVersion,
                        simDay = v2.simDay,
                        roster = v2.roster ?? new DutyRosterSystemState(),
                        marks = v2.marks ?? new MoraleMarkSystemState(),
                        encounters = v2.encounters ?? new ShelterEncounterSystemState(),
                        overflow = v2.overflow ?? new DutyRosterOverflowState(),
                        quests = new DutyRosterQuestState()
                    };
                    migrated.Checksum = SaveChecksum.Compute(migrated);
                    return migrated;
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    "DutyRosterSave: malformed save payload: " + e.Message, e);
            }

            DutyRosterSave save;
            try
            {
                save = json.Deserialize<DutyRosterSave>(jsonText!);
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

            ValidateChecksum(save.Checksum, save, "v" + save.saveVersion);

            if (save.overflow == null) save.overflow = new DutyRosterOverflowState();
            if (save.overflow.visitedNodes == null) save.overflow.visitedNodes = new List<string>();
            if (save.quests == null) save.quests = new DutyRosterQuestState();
            return save;
        }

        private static void ValidateChecksum(string expected, object payload, string label)
        {
            if (string.IsNullOrEmpty(expected))
                throw new InvalidOperationException(
                    "DutyRosterSave: save carries no checksum (truncated or tampered file).");
            string actual = SaveChecksum.Compute(payload);
            if (!string.Equals(expected, actual, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "DutyRosterSave: checksum mismatch (" + label + ", corrupt or foreign save).");
        }

        /// <summary>Restores all systems and the sim clock. Idempotent.</summary>
        public static void Restore(
            DutyRosterSave save,
            DutyRosterSystem roster,
            MoraleMarkSystem marks,
            ShelterEncounterSystem encounters,
            IClock clock,
            DutyRosterQuestRuntime quests = null)
        {
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            roster.RestoreState(save.roster);
            marks?.RestoreState(save.marks);
            encounters?.RestoreState(save.encounters);
            roster.RestoreOverflowState(save.overflow);
            quests?.RestoreState(save.quests);
            if (clock != null && save.simDay > 0)
                clock.SetDay(save.simDay);
        }
    }
}
