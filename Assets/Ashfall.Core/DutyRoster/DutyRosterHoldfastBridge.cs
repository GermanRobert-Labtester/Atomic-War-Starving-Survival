using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Snapshot of the Duty Roster's Holdfast-facing read model (Appendix A.2).
    /// Hosts present it; the owning Core systems are the authority.
    /// </summary>
    public sealed class DutyRosterHoldfastSnapshot
    {
        /// <summary>chart script: blank | pencil | ink | burned.</summary>
        public string ChartScript = string.Empty;
        /// <summary>North copies carry these rows (blank rows and hidden names omitted).</summary>
        public List<DutyRosterRow> NorthRows = new List<DutyRosterRow>();
        /// <summary>Names on the roster that must exist as census/levy rows when in use.</summary>
        public List<string> LevyNames = new List<string>();
        /// <summary>Blank Rows hide access currently granted?</summary>
        public bool BlankRowsAccess = true;
        /// <summary>Overflow practice currently open?</summary>
        public bool OverflowAccess = false;
        /// <summary>Roster mutation flag for Holdfast dialogue (Appendix A.2 row 8-10).</summary>
        public string Mutation = string.Empty;
        /// <summary>Hadi status: "" | "listed" | "hidden" | "never_back".</summary>
        public string HadiStatus = string.Empty;
        /// <summary>
        /// Quest-driven mutations (e.g. mutation_schedule_living, mutation_quieter_room,
        /// mutation_column_voss, mutation_brass_kept) — Holdfast dialogue/epilogue reads.
        /// </summary>
        public List<string> QuestMutations = new List<string>();
        /// <summary>Morale marks currently held (later-prose flags; never a score).</summary>
        public List<string> MarkIds = new List<string>();
    }

    /// <summary>
    /// Duty Roster ↔ Holdfast two-way integration bridge (plan Appendix A).
    /// Routes consequences through the owning Core systems — never from UI code.
    ///
    /// Holdfast → Duty: levy honour/substitute/refuse, membrane strip/drop,
    /// waystation staffing, ice-road dark, 12-C live, Sela clinic/stay.
    /// Duty → Holdfast: north roster copy for the levy, chart-script mutations,
    /// Hadi listed/hidden/never-back, Blank Rows access, Overflow access.
    ///
    /// The bridge owns no persistent state: everything it reads and writes lives
    /// in the owning systems' save-safe state. Deterministic; no RNG.
    /// </summary>
    public static class DutyRosterHoldfastBridge
    {
        public const string MarkThreeAway = "mark_three_away";
        public const string MarkLadleDefault = "mark_ladle_default";
        public const string MarkEdorStool = "mark_edor_stool";
        public const string MarkFilterWho = "mmc_filter_who";
        public const string MarkTamsinWatchShort = "mark_tamsin_watch_short";
        public const string MarkHouseThinned = "mark_house_thinned";
        public const string MarkHadiListed = "mark_hadi_listed";
        public const string MarkHadiHidden = "mark_hadi_hidden";
        public const string MarkHadiSent = "mark_hadi_sent";
        public const string MarkHadiNeverBack = "mark_hadi_never_back";
        public const string MarkRosterInk = "mark_roster_ink";
        public const string MarkRosterPencil = "mark_roster_pencil";
        public const string MarkRosterBlank = "mark_roster_blank";
        public const string MarkRosterBurned = "mark_roster_burned";
        public const string MarkHomeHeld = "mark_home_held";
        public const string MarkUncorroborated = "mark_uncorroborated";
        public const string MarkRationProtocol = "mark_ration_protocol";
        public const string MarkFourteenthClaimed = "mark_fourteenth_claimed";
        public const string MarkFourteenthDenied = "mark_fourteenth_denied";
        public const string MarkScheduleLiving = "mark_schedule_living";
        public const string MarkColumnVoss = "mark_column_voss";
        public const string MarkColumnHidden = "mark_column_hidden";
        public const string MarkBrassKept = "mark_brass_kept";
        public const string MarkPlateOnWall = "mark_plate_on_wall";

        /// <summary>Max rows flipped to levy by a single levy-honour order.</summary>
        public const int MaxLevyRows = 3;

        /// <summary>
        /// Apply Holdfast state to the Duty Roster (Appendix A.1). Deterministic;
        /// marks ride the owning MoraleMarkSystem so they persist and reload.
        /// </summary>
        public static void SyncFromHoldfast(
            DutyRosterSystem roster,
            MoraleMarkSystem marks,
            ShelterEncounterSystem encounters,
            CensusClaimSystem census,
            IceRoadSystem iceRoad,
            WaystationSystem waystation,
            BrineWaterSystem brine,
            int day)
        {
            if (roster == null || !roster.IsUnlocked) return;

            // ── A.1 levy honour: three rows become levy; the ladle goes short. ──
            if (census != null && census.LevyHonour)
            {
                int flipped = 0;
                for (int i = 0; i < roster.Rows.Count && flipped < MaxLevyRows; i++)
                {
                    var row = roster.Rows[i];
                    if (row == null || string.IsNullOrEmpty(row.survivorId)) continue;
                    if (row.status == DutyRosterSystem.StatusHome)
                    {
                        roster.SetStatus(row.survivorId, DutyRosterSystem.StatusLevy);
                        flipped++;
                    }
                }
                if (flipped > 0 && marks != null)
                    marks.SetMark(MarkThreeAway, flipped.ToString(), day);
            }

            // ── A.1 levy substitute: Kess marks it irregular; the ladle defaults. ──
            if (census != null && census.LevySubstitute && marks != null)
                marks.SetMark(MarkLadleDefault, null, day);

            // ── A.1 levy refuse: Edor's stool; the road may run dark (11-day lamps). ──
            if (census != null && census.LevyRefuse)
            {
                if (marks != null) marks.SetMark(MarkEdorStool, null, day);
                if (encounters != null && encounters.IsUnlocked)
                    encounters.StartEncounter("se_edor_stool_levy_refuse", ShelterEncounterSystem.KindEdorStool, day,
                        ShelterEncounterSystem.VisitorEdor, "levy refused");
            }

            // ── A.1 membrane strip: iodine/filters/brass short; filtration cough. ──
            if (brine != null && brine.State != null && brine.State.membraneSector4Strip && marks != null)
                marks.SetMark(MarkFilterWho, null, day);

            // ── A.1 waystation staffed: home watch is short. ──
            if (waystation != null
                && waystation.State != null
                && waystation.State.watchSurvivorIds != null
                && waystation.State.watchSurvivorIds.Length > 0
                && marks != null)
                marks.SetMark(MarkTamsinWatchShort, null, day);

            // ── A.1 ice road dark (Yara withdrew): everyone home, crowd at the hatch. ──
            if (iceRoad != null && iceRoad.IsUnlocked && !iceRoad.IsOpen)
            {
                if (marks != null) marks.SetMark(MarkHouseThinned, null, day);
                if (encounters != null && encounters.IsUnlocked)
                    encounters.StartEncounter("se_road_dark_crowd", ShelterEncounterSystem.KindRoadDarkCrowd, day,
                        payload: "ice road dark");
            }
        }

        /// <summary>
        /// Duty Roster → Holdfast read model (Appendix A.2). Never mutates
        /// Holdfast systems from here — the host routes the snapshot into the
        /// owning Holdfast/Census systems' existing seams.
        /// </summary>
        public static DutyRosterHoldfastSnapshot SnapshotForHoldfast(DutyRosterSystem roster)
        {
            return SnapshotForHoldfast(roster, null, null);
        }

        /// <summary>
        /// Duty Roster → Holdfast read model, including the quest-driven
        /// mutations and held morale marks (Holdfast dialogue/epilogue reads).
        /// </summary>
        public static DutyRosterHoldfastSnapshot SnapshotForHoldfast(
            DutyRosterSystem roster,
            DutyRosterQuestRuntime quests,
            MoraleMarkSystem marks)
        {
            var snap = new DutyRosterHoldfastSnapshot();
            if (roster == null) return snap;

            snap.ChartScript = roster.ChartScript;
            snap.NorthRows = roster.CopyForNorth();
            snap.BlankRowsAccess = roster.BlankRowsAccess;
            snap.OverflowAccess = roster.OverflowAccess;

            if (quests != null)
            {
                snap.QuestMutations = new List<string>(quests.AppliedMutations);
                snap.QuestMutations.Sort(string.CompareOrdinal);
            }
            if (marks != null)
            {
                snap.MarkIds = new List<string>();
                var state = marks.State;
                if (state != null && state.marks != null)
                {
                    for (int i = 0; i < state.marks.Count; i++)
                    {
                        if (state.marks[i] != null && !string.IsNullOrEmpty(state.marks[i].id))
                            snap.MarkIds.Add(state.marks[i].id);
                    }
                }
                snap.MarkIds.Sort(string.CompareOrdinal);
            }

            if (roster.LevyRequiresRows)
            {
                for (int i = 0; i < roster.Rows.Count; i++)
                {
                    var row = roster.Rows[i];
                    if (row != null && row.script != DutyRosterSystem.ScriptBlank)
                        snap.LevyNames.Add(row.survivorId);
                }
                snap.LevyNames.Sort(string.CompareOrdinal);
            }

            switch (roster.ChartScript)
            {
                case DutyRosterSystem.ScriptInk: snap.Mutation = "mutation_roster_ink"; break;
                case DutyRosterSystem.ScriptPencil: snap.Mutation = "mutation_roster_pencil"; break;
                case DutyRosterSystem.ScriptBurned: snap.Mutation = "mutation_roster_burned"; break;
                default: snap.Mutation = "mutation_roster_blank"; break;
            }

            var hadi = roster.GetRow(DutyRosterSystem.NpcHadiMorrow);
            if (hadi != null)
            {
                if (hadi.status == DutyRosterSystem.StatusMissing || hadi.status == DutyRosterSystem.StatusDead)
                    snap.HadiStatus = "never_back";
                else if (roster.IsValidLevyName(DutyRosterSystem.NpcHadiMorrow))
                    snap.HadiStatus = "listed";
                else
                    snap.HadiStatus = "hidden";
            }

            return snap;
        }

        /// <summary>
        /// Census/12-C names must exist as roster rows when the chart is in use
        /// (Appendix A.2, mutation_roster_in_use). Returns names that fail the
        /// check so the host can surface actionable diagnostics.
        /// </summary>
        public static List<string> ValidateLevyNamesAgainstRoster(DutyRosterSystem roster, IReadOnlyList<string> censusLevyNames)
        {
            var failures = new List<string>();
            if (censusLevyNames == null) return failures;
            if (roster == null || !roster.LevyRequiresRows) return failures;

            for (int i = 0; i < censusLevyNames.Count; i++)
            {
                string name = censusLevyNames[i];
                if (!roster.IsValidLevyName(name))
                    failures.Add(name);
            }
            return failures;
        }

        /// <summary>
        /// Register a Duty-side mark as the "true thing / lie / child's version"
        /// payload variant (MoraleMarkSystem spec §5.3). Marks never become a
        /// numerical morale meter — this only attaches later prose.
        /// </summary>
        public static void NoteMark(MoraleMarkSystem marks, string id, string payload, int day)
        {
            if (marks == null || string.IsNullOrEmpty(id)) return;
            marks.SetMark(id, payload, day);
        }
    }
}
