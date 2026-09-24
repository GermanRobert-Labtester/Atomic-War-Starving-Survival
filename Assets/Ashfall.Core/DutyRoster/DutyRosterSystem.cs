// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.DutyRoster;
using Ashfall.Core.Survivors;
#pragma warning disable CS8618
using Ashfall.Core.PlayerCommand;
using Ashfall.Core.World;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: THE DUTY ROSTER — the chart as save-safe occupancy.
    /// Not a job minigame. A document that other systems read.
    /// Spec: docs/expansions/expansion_02_the_duty_roster_plan.md §5.1 (Sprint 1).
    /// Engine-agnostic; no UnityEngine / Godot / JsonUtility.
    ///
    /// Hatch dilemma magnitudes are owned by ExpeditionSystem (Prompt #26).
    /// Do not retune. Read-only reminder: let-in 50 rads/h, force-decon 10,
    /// deny-entry morale 20 on every other living survivor.
    /// </summary>
    [Serializable]
    public class DutyRosterRow
    {
        public string survivorId;
        public string displayName;
        public string occupationObserved;
        public string status;
        public string script;
        public int lastSleptDay = -1;

        public DutyRosterRow Clone()
        {
            return new DutyRosterRow
            {
                survivorId = survivorId,
                displayName = displayName,
                occupationObserved = occupationObserved,
                status = status,
                script = script,
                lastSleptDay = lastSleptDay
            };
        }
    }

    [Serializable]
    public class DutyRosterAssignmentEntry
    {
        public string role;
        public string survivorId;
        public bool fitnessWarningAcknowledged;
        public int fitnessWarningDay = -1;
        public List<string> fitnessWarningReasons = new List<string>();
    }

    /// <summary>
    /// Expansion 36 watch-specific shift record. General duty assignments stay
    /// in <see cref="DutyRosterAssignmentEntry"/>; this bounded sub-object only
    /// records which survivor stood which authored watch post and when.
    /// </summary>
    [Serializable]
    public class DutyRosterWatchShift
    {
        public string shift_id = string.Empty;
        public string post_id = string.Empty;
        public string survivorId = string.Empty;
        public int day = -1;
        public int start_hour = 0;
        public int duration_hours = 1;
        public int fatigue_before_permille = 0;
        public int fatigue_after_permille = 0;
        public string fatigue_tier = "Alert";
        public bool completed = false;
    }

    [Serializable]
    public class DutyRosterPneumaticMemo
    {
        public string memoId;
        public string targetRoomId;
        public int deliveredDay;
        public int expiresDay;
        public float shiftEfficiencyBonus;
    }

    /// <summary>Home occupant hint for the morning tick. Host supplies who slept.</summary>
    public class DutyRosterOccupant
    {
        public string survivorId;
        public string displayName;
        public string occupationObserved;
        public bool sleptHere;
    }

    [Serializable]
    public class DutyRosterSystemState
    {
        public string systemId = DutyRosterIds.SystemId;
        public bool expansionUnlocked;
        public bool wallInspected;
        public string chartScript = DutyRosterIds.ScriptBlank;
        public bool kessPencilAllowed;
        public bool waitInk;
        public bool blankRowsAccess = true;
        public bool mutationRosterInUse;
        public bool mutationRosterStillBlank;
        public bool mutationRosterBurned;
        public bool mutationRationProtocol;
        public string endingId;
        public bool secondWinterActive;
        public int seedSalt = DutyRosterIds.SeedUtilityOffset;
        public int lastMorningDay = -1;
        public int daysLeftBlank;
        public int lastBurnDay = -1;
        public bool overflowAccess;
        public List<string> overflowVisited = new List<string>();
        public List<DutyRosterRow> rows = new List<DutyRosterRow>();
        public List<DutyRosterAssignmentEntry> assignments = new List<DutyRosterAssignmentEntry>();
        public List<DutyRosterWatchShift> watch_shifts = new List<DutyRosterWatchShift>();
        public int watch_schema_version = 1;
        public List<DutyRosterPneumaticMemo> pneumaticMemos = new List<DutyRosterPneumaticMemo>();
        public List<string> hiddenFromNorth = new List<string>();
        public List<string> blankRowsLivingNames = new List<string>();
    }

    public class DutyRosterSystem
    {
        public const string SystemId = DutyRosterIds.SystemId;
        public const string ExpansionId = DutyRosterIds.ExpansionId;
        public const string FlagExpUnlocked = DutyRosterIds.FlagExpUnlocked;

        public const string LocStackRosterWall = DutyRosterIds.LocStackRosterWall;
        public const string LocStackSleeping = DutyRosterIds.LocStackSleeping;
        public const string LocStackMess = DutyRosterIds.LocStackMess;
        public const string LocStackFiltration = DutyRosterIds.LocStackFiltration;
        public const string LocStackAirlock = DutyRosterIds.LocStackAirlock;
        public const string LocStackClinicAlcove = DutyRosterIds.LocStackClinicAlcove;

        public const string QuestTheChart = DutyRosterIds.QuestTheChart;
        public const string QuestWhoEats = DutyRosterIds.QuestWhoEats;
        public const string QuestFourteenth = DutyRosterIds.QuestFourteenth;
        public const string QuestCaretaker = DutyRosterIds.QuestCaretaker;
        public const string QuestTheColumn = DutyRosterIds.QuestTheColumn;
        public const string QuestTheTin = DutyRosterIds.QuestTheTin;
        public const string QuestQuiet = DutyRosterIds.QuestQuiet;
        public const string QuestSole = DutyRosterIds.QuestSole;
        public const string QuestWindow = DutyRosterIds.QuestWindow;
        public const string QuestInk = DutyRosterIds.QuestInk;

        public const string NpcKessAdler = DutyRosterIds.NpcKessAdler;
        public const string NpcAnselDuth = DutyRosterIds.NpcAnselDuth;
        public const string NpcHadiMorrow = DutyRosterIds.NpcHadiMorrow;
        public const string NpcTamsinRook = DutyRosterIds.NpcTamsinRook;
        public const string NpcLenQuill = DutyRosterIds.NpcLenQuill;
        public const string NpcNilaBrant = DutyRosterIds.NpcNilaBrant;

        public const string ChoiceWritePencil = DutyRosterIds.ChoiceWritePencil;
        public const string ChoiceLeaveBlank = DutyRosterIds.ChoiceLeaveBlank;
        public const string ChoiceWaitInk = DutyRosterIds.ChoiceWaitInk;
        public const string ChoiceLadleChild = DutyRosterIds.ChoiceLadleChild;
        public const string ChoiceLadleHatch = DutyRosterIds.ChoiceLadleHatch;
        public const string ChoiceLadleLeave = DutyRosterIds.ChoiceLadleLeave;
        public const string ChoiceLadleProtocol = DutyRosterIds.ChoiceLadleProtocol;

        public const string ScriptBlank = DutyRosterIds.ScriptBlank;
        public const string ScriptPencil = DutyRosterIds.ScriptPencil;
        public const string ScriptInk = DutyRosterIds.ScriptInk;
        public const string ScriptBurned = DutyRosterIds.ScriptBurned;

        public const string StatusHome = DutyRosterIds.StatusHome;
        public const string StatusLevy = DutyRosterIds.StatusLevy;
        public const string StatusWaystation = DutyRosterIds.StatusWaystation;
        public const string StatusQuiet = DutyRosterIds.StatusQuiet;
        public const string StatusMissing = DutyRosterIds.StatusMissing;
        public const string StatusDead = DutyRosterIds.StatusDead;

        public const string RoleNightWatch = DutyRosterIds.RoleNightWatch;
        public const string RoleMess = DutyRosterIds.RoleMess;
        public const string RoleHatchOpener = DutyRosterIds.RoleHatchOpener;
        public const string RoleIntakeSleeper = DutyRosterIds.RoleIntakeSleeper;
        public const string RoleExpedition = DutyRosterIds.RoleExpedition;

        public const string MutationRosterInUse = DutyRosterIds.MutationRosterInUse;
        public const string MutationRosterStillBlank = DutyRosterIds.MutationRosterStillBlank;
        public const string MutationRationProtocol = DutyRosterIds.MutationRationProtocol;
        public const string MutationRosterBurned = DutyRosterIds.MutationRosterBurned;
        public const string MutationRosterInk = DutyRosterIds.MutationRosterInk;
        public const string MutationRosterBlank = DutyRosterIds.MutationRosterBlank;
        public const string MutationFactionBlankRowsAccess = DutyRosterIds.MutationFactionBlankRowsAccess;
        public const string FlagWaitInk = DutyRosterIds.FlagWaitInk;

        // Endings (spec §3 Endings — the game does not rank them)
        public const string EndingInk = DutyRosterIds.EndingInk;
        public const string EndingPencil = DutyRosterIds.EndingPencil;
        public const string EndingBlank = DutyRosterIds.EndingBlank;
        public const string EndingBurned = DutyRosterIds.EndingBurned;
        public const string EndingSecondWinter = DutyRosterIds.EndingSecondWinter;

        // Second Winter (spec §5.4 — data profile, not a 4th simulation class)
        public const string SeasonSecondWinter = DutyRosterIds.SeasonSecondWinter;
        public const int SecondWinterWindowMinDays = DutyRosterIds.SecondWinterWindowMinDays;
        public const int SecondWinterWindowMaxDays = DutyRosterIds.SecondWinterWindowMaxDays;
        public const float SecondWinterEncounterWeight = DutyRosterIds.SecondWinterEncounterWeight;

        /// <summary>Printed manifest cap. Over-occupancy is the fourteenth-bunk quest, not a UI cheat.</summary>
        public const int ManifestCap = DutyRosterIds.ManifestCap;
        public const int SoftGateDay = DutyRosterIds.SoftGateDay;
        public const int StillBlankDays = DutyRosterIds.StillBlankDays;
        /// <summary>Utility AI salt. Spec: _worldSeed + 1208.</summary>
        public const int SeedUtilityOffset = DutyRosterIds.SeedUtilityOffset;

        public static readonly string[] StackWingIds = DutyRosterIds.StackWingIds;

        /// <summary>
        /// The Overflow is a small authenticated void practice — four bounded
        /// nodes, not a district. Allocation 11 and 13 are dark; the pump hatch
        /// and the blank cellar are reachable through them (spec §2.4).
        /// </summary>
        public const string LocOverflowAlloc11 = DutyRosterIds.LocOverflowAlloc11;
        public const string LocOverflowAlloc13 = DutyRosterIds.LocOverflowAlloc13;
        public const string LocOverflowPumpHatch = DutyRosterIds.LocOverflowPumpHatch;
        public const string LocOverflowBlankCellar = DutyRosterIds.LocOverflowBlankCellar;

        public static readonly string[] OverflowNodeIds = DutyRosterIds.OverflowNodeIds;

        public static readonly string[] AssignmentRoles = DutyRosterIds.AssignmentRoles;

        private DutyRosterSystemState _state = new DutyRosterSystemState();
        private readonly Dictionary<string, DutyRosterRow> _byId = new Dictionary<string, DutyRosterRow>();
        private readonly Dictionary<string, string> _assignmentByRole = new Dictionary<string, string>();
        private readonly HashSet<string> _hiddenFromNorth = new HashSet<string>();
        private readonly HashSet<string> _blankRowsLiving = new HashSet<string>();
        private readonly DutyRosterAssignmentEngine _assignments;
        private readonly DutyRosterOverflowEngine _overflow;
        private readonly DutyRosterChartEngine _chart;

        public event Action OnRosterUpdated;
        public event Action<string> OnNameWritten;
        public event Action<string> OnNameErased;
        public event Action OnRosterBurned;
        public event Action<string, string> OnAssignmentChanged;
        /// <summary>Raised when an existing labor assignment is vacated.</summary>
        public event Action<string, string> OnDutyVacated;
        public event Action<DutyRosterSystemState> OnStateChanged;

        public DutyRosterSystemState State => _state;
        public bool IsUnlocked => _state.expansionUnlocked;
        public string ChartScript => _state.chartScript;
        public bool BlankRowsAccess => _state.blankRowsAccess;
        public bool MutationInUse => _state.mutationRosterInUse;
        public int OccupiedRowCount => _state.rows != null ? _state.rows.Count : 0;
        public IReadOnlyList<DutyRosterRow> Rows => _state.rows;
        public Func<string, bool>? IsSurvivorReservedExternally
        {
            get => _assignments.IsExternalReserved;
            set => _assignments.IsExternalReserved = value;
        }
        public Func<string, bool>? IsCandidateEligible
        {
            get => _assignments.IsCandidateEligible;
            set
            {
                _assignments.IsCandidateEligible = value;
                _chart.IsCandidateEligible = value;
            }
        }
        public Func<string, string, RoleFitnessVerdict>? EvaluateRoleFitness
        {
            get => _assignments.EvaluateRoleFitness;
            set => _assignments.EvaluateRoleFitness = value;
        }

        /// <summary>
        /// Plan 43 / C1[13]: Optional crew consent/refusal evaluator.
        /// </summary>
        public Func<string, string, CrewConsentVerdict>? EvaluateCrewConsent
        {
            get => _assignments.EvaluateCrewConsent;
            set => _assignments.EvaluateCrewConsent = value;
        }

        /// <summary>
        /// Read-only role preview used by host UI and command validation. It
        /// delegates to the same evaluator enforced during assignment commit.
        /// </summary>
        public RoleFitnessVerdict? PreviewRoleFitness(string survivorId, string role)
        {
            return _assignments.EvaluateRoleFitness?.Invoke(survivorId, role);
        }

        /// <summary>
        /// Plan 43 / C1[13]: Read-only preview of crew consent before assignment.
        /// </summary>
        public CrewConsentVerdict? PreviewCrewConsent(string survivorId, string role)
        {
            return _assignments.EvaluateCrewConsent?.Invoke(survivorId, role);
        }

        public DutyRosterSystem() : this(SeedUtilityOffset)
        {
        }

        public DutyRosterSystem(int seedSalt)
        {
            _state.seedSalt = seedSalt;
            EnsureLists();
            _assignments = new DutyRosterAssignmentEngine(
                _assignmentByRole,
                _state.assignments,
                GetRow,
                RaiseUpdated,
                (r, s) => OnAssignmentChanged?.Invoke(r, s),
                (r, s) => OnDutyVacated?.Invoke(r, s),
                () => _state.seedSalt,
                () => _state.rows);
            _overflow = new DutyRosterOverflowEngine(RaiseChanged);
            _overflow.Bind(_state);
            _chart = new DutyRosterChartEngine(
                _byId,
                _blankRowsLiving,
                _assignments,
                RaiseUpdated,
                id => OnNameWritten?.Invoke(id),
                id => OnNameErased?.Invoke(id),
                () => OnRosterBurned?.Invoke(),
                WithdrawBlankRowsAccess);
            _chart.Bind(_state);
        }

        public void Initialise(int seedSalt)
        {
            _state.seedSalt = seedSalt;
        }

        /// <summary>Old saves: wall stays blank until the chart quest.</summary>
        public void Unlock(int day)
        {
            if (_state.expansionUnlocked) return;
            _state.expansionUnlocked = true;
            RaiseChanged();
        }

        public void NotifyWallInspected()
        {
            _state.wallInspected = true;
            RaiseChanged();
        }

        /// <summary>
        /// Soft gate: Day 60+, lore_allocation_wrongness, inspect the wall,
        /// or Edor's census started (Holdfast flag, passed in — this system does not read IceRoad).
        /// </summary>
        public bool CanBeginChart(int day, bool loreAllocationWrongness, bool holdfastClerkStarted)
        {
            if (!_state.expansionUnlocked) return false;
            return day >= SoftGateDay
                || loreAllocationWrongness
                || _state.wallInspected
                || holdfastClerkStarted;
        }

        public DutyRosterRow GetRow(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            _byId.TryGetValue(survivorId, out DutyRosterRow row);
            return row;
        }

        public bool WriteName(
            string survivorId,
            string displayName,
            string occupationObserved,
            string script,
            int day,
            bool sleptHere)
        {
            return _chart.WriteName(survivorId, displayName, occupationObserved, script, day, sleptHere);
        }

        public bool EraseName(string survivorId)
        {
            return _chart.EraseName(survivorId);
        }

        public bool BurnChart(int day)
        {
            return _chart.BurnChart(day);
        }

        /// <summary>Morning tick. Kess fills pencil if allowed. Ink never auto-fills.</summary>
        public void TickMorning(int day, IReadOnlyList<DutyRosterOccupant> occupants)
        {
            _chart.TickMorning(day, occupants);
        }

        public bool ResolveChartChoice(string choiceId, int day)
        {
            return _chart.ResolveChartChoice(choiceId, day);
        }

        public bool ResolveLadleChoice(string choiceId, int day)
        {
            return _chart.ResolveLadleChoice(choiceId, day);
        }

        /// <summary>
        /// Ink ending resolution (spec §3 Endings + §4.1 quest_roster_ink).
        /// The wall has names that do not come off in the morning. Edor's return
        /// is current. 11 goes dark if their living is on it. The hatch reversed
        /// reads your list.
        /// </summary>
        public bool ResolveInkEnding(int day)
        {
            return _chart.ResolveInkEnding(day);
        }

        public bool SetStatus(string survivorId, string status)
        {
            return _chart.SetStatus(survivorId, status);
        }

        public bool SetRowScript(string survivorId, string script)
        {
            return _chart.SetRowScript(survivorId, script);
        }

        /// <summary>Second Winter season profile active (data, not a 4th sim class).</summary>
        public void SetSecondWinterActive(bool active)
        {
            if (_state.secondWinterActive == active) return;
            _state.secondWinterActive = active;
            RaiseChanged();
        }

        public bool IsSecondWinterActive => _state.secondWinterActive;

        public bool Assign(string role, string survivorId)
        {
            return _assignments.Assign(role, survivorId);
        }

        /// <summary>
        /// Expansion 36 watch-shift command. It validates the survivor through
        /// the same canonical duty/fitness gate, but stores only a watch-specific
        /// shift sub-object; it does not create a second survivor roster.
        /// </summary>
        public ActionResult AssignWatchShift(
            string shiftId,
            string postId,
            string survivorId,
            int day,
            int startHour,
            int durationHours,
            bool confirmFitnessWarning = false,
            int? fatigueBeforePermille = null)
        {
            EnsureLists();
            if (string.IsNullOrWhiteSpace(shiftId) || string.IsNullOrWhiteSpace(postId) || string.IsNullOrWhiteSpace(survivorId))
                return ActionResult.Failed("invalid_watch_shift", "watch.invalid_shift");
            if (day < 0 || startHour < 0 || startHour > 23 || durationHours < 1 || durationHours > 12 || startHour + durationHours > 24)
                return ActionResult.Failed("invalid_watch_shift_time", "watch.invalid_shift_time");

            string id = shiftId.Trim();
            if (_state.watch_shifts.Exists(x => x != null && string.Equals(x.shift_id, id, StringComparison.OrdinalIgnoreCase)))
                return ActionResult.Blocked("watch_shift_exists", "watch.shift_exists");

            var validation = _assignments.ValidateAssign(DutyRosterIds.RoleNightWatch, survivorId, confirmFitnessWarning);
            if (!validation.IsSuccess) return validation;

            int end = startHour + durationHours;
            for (int i = 0; i < _state.watch_shifts.Count; i++)
            {
                var existing = _state.watch_shifts[i];
                if (existing == null || existing.completed || existing.day != day || !string.Equals(existing.survivorId, survivorId, StringComparison.OrdinalIgnoreCase))
                    continue;
                int existingEnd = existing.start_hour + existing.duration_hours;
                if (startHour < existingEnd && existing.start_hour < end)
                    return ActionResult.Blocked("watch_shift_overlap", "watch.shift_overlap");
            }

            int before = Math.Clamp(fatigueBeforePermille ?? 0, 0, 1000);
            _state.watch_shifts.Add(new DutyRosterWatchShift
            {
                shift_id = id,
                post_id = postId.Trim(),
                survivorId = survivorId.Trim(),
                day = day,
                start_hour = startHour,
                duration_hours = durationHours,
                fatigue_before_permille = Math.Clamp(before, 0, 1000),
                fatigue_after_permille = Math.Clamp(before, 0, 1000),
                fatigue_tier = WatchFatigueTier.Alert.ToString(),
                completed = false
            });
            PruneWatchShifts();
            RaiseChanged();
            return ActionResult.Success("watch.shift_assigned");
        }

        public ActionResult CompleteWatchShift(string shiftId, int fatigueAfterPermille, int? restQualityPermille = null)
        {
            EnsureLists();
            for (int i = 0; i < _state.watch_shifts.Count; i++)
            {
                var shift = _state.watch_shifts[i];
                if (shift == null || !string.Equals(shift.shift_id, shiftId?.Trim(), StringComparison.OrdinalIgnoreCase))
                    continue;
                if (shift.completed) return ActionResult.Blocked("watch_shift_complete", "watch.shift_complete");
                int after = Math.Clamp(fatigueAfterPermille, 0, 1000);
                // If the caller did not supply a measured post-shift value, the
                // signed pure engine supplies the deterministic watch calculation.
                if (restQualityPermille.HasValue)
                    after = NightWatchPatrolReadinessEngine.AdvanceWatchFatigue(
                        shift.fatigue_before_permille, shift.duration_hours, restQualityPermille.Value);
                shift.fatigue_after_permille = after;
                shift.fatigue_tier = NightWatchPatrolReadinessEngine.ClassifyFatigue(after).ToString();
                shift.completed = true;
                RaiseChanged();
                return ActionResult.Success("watch.shift_completed");
            }
            return ActionResult.Failed("unknown_watch_shift", "watch.unknown_shift");
        }

        public IReadOnlyList<DutyRosterWatchShift> GetWatchShifts(int? day = null)
        {
            EnsureLists();
            var result = new List<DutyRosterWatchShift>();
            for (int i = 0; i < _state.watch_shifts.Count; i++)
            {
                var shift = _state.watch_shifts[i];
                if (shift == null || (day.HasValue && shift.day != day.Value)) continue;
                result.Add(CopyWatchShift(shift));
            }
            return result;
        }

        public int GetWatchShiftCount(string postId, int day, bool completedOnly = false)
        {
            EnsureLists();
            if (string.IsNullOrWhiteSpace(postId)) return 0;
            int count = 0;
            for (int i = 0; i < _state.watch_shifts.Count; i++)
            {
                var shift = _state.watch_shifts[i];
                if (shift == null || shift.day != day || !string.Equals(shift.post_id, postId.Trim(), StringComparison.OrdinalIgnoreCase)) continue;
                if (completedOnly && !shift.completed) continue;
                count++;
            }
            return count;
        }

        public int GetAverageWatchFatigue(string postId, int day)
        {
            EnsureLists();
            int total = 0;
            int count = 0;
            for (int i = 0; i < _state.watch_shifts.Count; i++)
            {
                var shift = _state.watch_shifts[i];
                if (shift == null || shift.day != day || !string.Equals(shift.post_id, postId?.Trim(), StringComparison.OrdinalIgnoreCase)) continue;
                total += shift.fatigue_after_permille;
                count++;
            }
            return count == 0 ? 0 : total / count;
        }

        public DutyRosterWatchShift? FindWatchShift(string shiftId)
        {
            EnsureLists();
            for (int i = 0; i < _state.watch_shifts.Count; i++)
            {
                if (_state.watch_shifts[i] != null && string.Equals(_state.watch_shifts[i].shift_id, shiftId?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return CopyWatchShift(_state.watch_shifts[i]);
            }
            return null;
        }

        private void PruneWatchShifts()
        {
            const int cap = 128;
            if (_state.watch_shifts.Count <= cap) return;
            _state.watch_shifts.Sort((a, b) =>
            {
                int day = a.day.CompareTo(b.day);
                if (day != 0) return day;
                return string.CompareOrdinal(a.shift_id, b.shift_id);
            });
            _state.watch_shifts.RemoveRange(0, _state.watch_shifts.Count - cap);
        }

        private static DutyRosterWatchShift CopyWatchShift(DutyRosterWatchShift shift) => new DutyRosterWatchShift
        {
            shift_id = shift.shift_id ?? string.Empty,
            post_id = shift.post_id ?? string.Empty,
            survivorId = shift.survivorId ?? string.Empty,
            day = shift.day,
            start_hour = shift.start_hour,
            duration_hours = shift.duration_hours,
            fatigue_before_permille = Math.Clamp(shift.fatigue_before_permille, 0, 1000),
            fatigue_after_permille = Math.Clamp(shift.fatigue_after_permille, 0, 1000),
            fatigue_tier = shift.fatigue_tier ?? string.Empty,
            completed = shift.completed
        };

        /// <summary>Plan 24B A2 — optional duty-hour projection (bound by the
        /// host to its derived ledger). Null ⇒ the hours surface is unavailable
        /// and panels omit it; never persisted, never a second assignment
        /// authority.</summary>
        public Func<string, DutyHourSnapshot>? DutyHourResolver { get; set; }

        /// <summary>Read-only hours snapshot for the duty detail UI, or null
        /// when no ledger is bound (legacy paths).</summary>
        public DutyHourSnapshot? PreviewDutyHours(string survivorId)
            => DutyHourResolver?.Invoke(survivorId);

        /// <summary>
        /// Canonical consumer for a delivered pneumatic duty memo. Delivery is
        /// idempotent by memo ID and the bonus is time-bounded campaign state.
        /// </summary>
        public bool ReceivePneumaticMemo(
            string memoId,
            string targetRoomId,
            int deliveredDay,
            int durationDays = 1,
            float shiftEfficiencyBonus = 0.05f)
        {
            if (string.IsNullOrWhiteSpace(memoId)) return false;
            EnsureLists();
            for (int i = 0; i < _state.pneumaticMemos.Count; i++)
                if (_state.pneumaticMemos[i].memoId == memoId) return false;
            _state.pneumaticMemos.Add(new DutyRosterPneumaticMemo
            {
                memoId = memoId,
                targetRoomId = targetRoomId ?? string.Empty,
                deliveredDay = deliveredDay,
                expiresDay = deliveredDay + Math.Max(0, durationDays),
                shiftEfficiencyBonus = Math.Clamp(shiftEfficiencyBonus, 0f, 0.25f)
            });
            RaiseChanged();
            return true;
        }

        public float GetPneumaticMemoBonus(string targetRoomId, int day)
        {
            float total = 0f;
            if (_state.pneumaticMemos == null) return total;
            for (int i = 0; i < _state.pneumaticMemos.Count; i++)
            {
                var memo = _state.pneumaticMemos[i];
                if (memo == null || memo.targetRoomId != targetRoomId) continue;
                if (day >= memo.deliveredDay && day <= memo.expiresDay)
                    total += memo.shiftEfficiencyBonus;
            }
            return Math.Clamp(total, 0f, 0.5f);
        }

        /// <summary>
        /// Optional Plan 137 hook: query work speed / throughput multiplier for an assigned worker.
        /// </summary>
        public Func<string, float>? WorkSpeedMultiplierLookup { get; set; }

        public float GetEffectiveWorkSpeed(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return 1.0f;
            return WorkSpeedMultiplierLookup != null ? Math.Max(0.1f, WorkSpeedMultiplierLookup(survivorId)) : 1.0f;
        }

        public ActionResult AssignWithResult(
            string role,
            string survivorId,
            bool confirmFitnessWarning = false)
        {
            return _assignments.AssignWithResult(role, survivorId, confirmFitnessWarning);
        }

        /// <summary>
        /// Side-effect-free preview of a duty assignment command.
        /// Shares the same validation path as <see cref="AssignWithResult"/>.
        /// </summary>
        public CommandPreview PreviewAssign(
            string role,
            string survivorId,
            long stateVersion = 0,
            bool confirmFitnessWarning = false)
        {
            var validation = _assignments.ValidateAssign(role, survivorId, confirmFitnessWarning);
            if (!validation.IsSuccess)
                return CommandPreview.Unavailable(PlayerCommandCode.AssignRole, validation.FailureCode, validation.MessageKey, stateVersion);

            var deltas = new Dictionary<string, double>();
            if (!string.IsNullOrEmpty(survivorId))
            {
                deltas["assignment"] = 1;
                deltas["role"] = role.Length;
            }

            return CommandPreview.Available(
                PlayerCommandCode.AssignRole,
                stateVersion,
                deltas,
                isIrreversible: false,
                messageKey: "duty_roster.preview_assign");
        }

        /// <summary>
        /// Execute a duty assignment using the same validation path as <see cref="PreviewAssign"/>.
        /// Stale previews (state version mismatch) are rejected without mutation.
        /// </summary>
        public CommandResult ExecuteAssign(
            string role,
            string survivorId,
            long expectedStateVersion = 0,
            long currentStateVersion = 0,
            bool confirmFitnessWarning = false)
        {
            var preview = PreviewAssign(role, survivorId, expectedStateVersion, confirmFitnessWarning);
            if (!preview.IsAvailable)
                return CommandResult.FromPreview(preview);

            if (preview.StateVersion != currentStateVersion)
                return CommandResult.StalePreview(PlayerCommandCode.AssignRole, preview.StateVersion, currentStateVersion);

            var result = AssignWithResult(role, survivorId, confirmFitnessWarning);
            if (!result.IsSuccess)
                return new CommandResult(
                    PlayerCommandCode.AssignRole,
                    result,
                    expectedStateVersion,
                    currentStateVersion);

            return CommandResult.FromSuccess(
                PlayerCommandCode.AssignRole,
                result,
                expectedStateVersion,
                currentStateVersion + 1);
        }

        /// <summary>The role a survivor currently holds, or null.</summary>
        public string GetRoleOf(string survivorId)
        {
            return _assignments.GetRoleOf(survivorId);
        }

        public string GetAssignment(string role)
        {
            return _assignments.GetAssignment(role);
        }

        /// <summary>
        /// Records the player's explicit acceptance of an impaired fitness
        /// warning on the existing roster assignment. The verdict remains
        /// derived; only the acknowledgement belongs in the duty save.
        /// </summary>
        public bool AcknowledgeFitnessWarning(string role, string survivorId, int day)
        {
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(survivorId)) return false;
            if (GetAssignment(role) != survivorId) return false;
            var verdict = PreviewRoleFitness(survivorId, role);
            if (verdict == null || !verdict.RequiresConfirmation) return false;

            for (int i = 0; i < _state.assignments.Count; i++)
            {
                var assignment = _state.assignments[i];
                if (assignment == null || assignment.role != role || assignment.survivorId != survivorId)
                    continue;
                assignment.fitnessWarningAcknowledged = true;
                assignment.fitnessWarningDay = day;
                assignment.fitnessWarningReasons = new List<string>(verdict.WarningReasons);
                RaiseUpdated();
                return true;
            }
            return false;
        }

        /// <summary>Drop every role assignment held by a survivor (death, departure).</summary>
        public void RemoveAssignmentsFor(string survivorId)
        {
            _assignments.RemoveAssignmentsFor(survivorId);
        }

        /// <summary>
        /// Player skipped the night slate. Deterministic Utility AI among home rows.
        /// Same seed + day => same picks. Does not use string.GetHashCode.
        /// </summary>
        public int AutoAssignDefaults(int day)
        {
            return _assignments.AutoAssignDefaults(day);
        }

        /// <summary>
        /// Levy / CensusClaim hook: when the chart is in use and not blank,
        /// named IDs must exist as rows. Hidden names are omitted from north copies.
        /// Does not call CensusClaimSystem — host wires that.
        /// </summary>
        public bool LevyRequiresRows =>
            _state.mutationRosterInUse
            && _state.chartScript != ScriptBlank
            && _state.chartScript != ScriptBurned;

        public bool IsValidLevyName(string survivorId)
        {
            if (!LevyRequiresRows) return true;
            DutyRosterRow row = GetRow(survivorId);
            return row != null && row.script != ScriptBlank && !_hiddenFromNorth.Contains(survivorId);
        }

        public void HideFromNorthCopy(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (_hiddenFromNorth.Add(survivorId))
            {
                _state.hiddenFromNorth.Add(survivorId);
                RaiseChanged();
            }
        }

        public List<DutyRosterRow> CopyForNorth()
        {
            var copy = new List<DutyRosterRow>();
            for (int i = 0; i < _state.rows.Count; i++)
            {
                DutyRosterRow row = _state.rows[i];
                if (row == null || string.IsNullOrEmpty(row.survivorId)) continue;
                if (_hiddenFromNorth.Contains(row.survivorId)) continue;
                if (row.script == ScriptBlank) continue;
                copy.Add(row.Clone());
            }

            return copy;
        }

        public void RegisterBlankRowsLivingName(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (_blankRowsLiving.Add(survivorId))
            {
                _state.blankRowsLivingNames.Add(survivorId);
                RaiseChanged();
            }
        }

        /// <summary>Blank Rows access can be restored only by an authored practice (quest_roster_blank_access).</summary>
        public bool GrantBlankRowsAccess()
        {
            if (_state.blankRowsAccess) return false;
            _state.blankRowsAccess = true;
            RaiseChanged();
            return true;
        }

        /// <summary>Public withdrawal (authored practice / census listing).</summary>
        public bool WithdrawBlankRowsAccessPublic()
        {
            if (!_state.blankRowsAccess) return false;
            _state.blankRowsAccess = false;
            RaiseChanged();
            return true;
        }

        // ── Authored quest-mutation targets (typed; the quest runtime maps ids) ──

        /// <summary>The chart is in use (quest_roster_the_chart completes).</summary>
        public bool MarkRosterInUse()
        {
            if (_state.mutationRosterInUse) return false;
            _state.mutationRosterInUse = true;
            RaiseChanged();
            return true;
        }

        /// <summary>The chart was left blank long enough (quest fail path).</summary>
        public bool MarkRosterStillBlank()
        {
            if (_state.mutationRosterStillBlank) return false;
            _state.mutationRosterStillBlank = true;
            RaiseChanged();
            return true;
        }

        public bool SetRationProtocol(bool active)
        {
            if (_state.mutationRationProtocol == active) return false;
            _state.mutationRationProtocol = active;
            RaiseChanged();
            return true;
        }

        // ── Overflow practice (bounded void, spec §2.4) ────────────────

        public bool OverflowAccess => _overflow.Access;
        public IReadOnlyList<string> OverflowVisited => _overflow.Visited;

        public bool GrantOverflowAccess()
        {
            return _overflow.GrantOverflowAccess();
        }

        public bool WithdrawOverflowAccess()
        {
            return _overflow.WithdrawOverflowAccess();
        }

        /// <summary>Register a visit to one of the four authenticated Overflow nodes.</summary>
        public bool RegisterOverflowVisit(string nodeId)
        {
            return _overflow.RegisterOverflowVisit(nodeId);
        }

        public bool HasVisitedOverflow(string nodeId)
        {
            return _overflow.HasVisitedOverflow(nodeId);
        }

        public static bool IsOverflowNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            for (int i = 0; i < OverflowNodeIds.Length; i++)
                if (OverflowNodeIds[i] == nodeId) return true;
            return false;
        }

        public void NotifyListedOnCensusOr12C(string survivorId)
        {
            if (!string.IsNullOrEmpty(survivorId) && _blankRowsLiving.Contains(survivorId))
                WithdrawBlankRowsAccess();
        }

        public DutyRosterSystemState CaptureState()
        {
            var copy = new DutyRosterSystemState();
            CopyState(_state, copy);
            return copy;
        }

        /// <summary>Capture the bounded Overflow practice state (v2 envelope field).</summary>
        public DutyRosterOverflowState CaptureOverflowState()
        {
            return _overflow.Capture();
        }

        /// <summary>Restore the Overflow practice state. Missing state defaults to closed/empty.</summary>
        public void RestoreOverflowState(DutyRosterOverflowState saved)
        {
            _overflow.Restore(saved);
        }

        public void RestoreState(DutyRosterSystemState saved)
        {
            // Deep-copy: the deserialized DTO must not become the live state.
            // Otherwise the caller's save object and the running system alias
            // the same lists and a later mutation corrupts the envelope.
            if (saved == null) _state = new DutyRosterSystemState();
            else
            {
                _state = new DutyRosterSystemState();
                CopyState(saved, _state);
            }
            _overflow.Bind(_state);
            _chart.Bind(_state);
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            EnsureLists();
            RebuildIndexes();
            RaiseChanged();
        }

        private void WithdrawBlankRowsAccess()
        {
            if (!_state.blankRowsAccess) return;
            _state.blankRowsAccess = false;
            RaiseChanged();
        }

        private static bool IsKnownStatus(string status)
        {
            return status == StatusHome || status == StatusLevy || status == StatusWaystation
                || status == StatusQuiet || status == StatusMissing || status == StatusDead;
        }

        private void EnsureLists()
        {
            if (_state.rows == null) _state.rows = new List<DutyRosterRow>();
            if (_state.assignments == null) _state.assignments = new List<DutyRosterAssignmentEntry>();
            if (_state.watch_schema_version <= 0) _state.watch_schema_version = 1;
            if (_state.watch_schema_version > 1)
                throw new InvalidOperationException($"duty roster watch schema {_state.watch_schema_version} is newer than supported 1.");
            if (_state.watch_shifts == null) _state.watch_shifts = new List<DutyRosterWatchShift>();
            NormalizeWatchShifts();
            if (_state.pneumaticMemos == null) _state.pneumaticMemos = new List<DutyRosterPneumaticMemo>();
            if (_state.hiddenFromNorth == null) _state.hiddenFromNorth = new List<string>();
            if (_state.blankRowsLivingNames == null) _state.blankRowsLivingNames = new List<string>();
            if (_state.overflowVisited == null) _state.overflowVisited = new List<string>();
        }

        private void NormalizeWatchShifts()
        {
            const int cap = 128;
            var normalized = new List<DutyRosterWatchShift>();
            var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var shift in _state.watch_shifts ?? new List<DutyRosterWatchShift>())
            {
                if (shift == null || string.IsNullOrWhiteSpace(shift.shift_id) ||
                    string.IsNullOrWhiteSpace(shift.post_id) || string.IsNullOrWhiteSpace(shift.survivorId) ||
                    shift.day < 0 || shift.start_hour < 0 || shift.start_hour > 23 ||
                    shift.duration_hours < 1 || shift.duration_hours > 12 ||
                    shift.start_hour + shift.duration_hours > 24) continue;
                var copy = CopyWatchShift(shift);
                copy.shift_id = copy.shift_id.Trim();
                copy.post_id = copy.post_id.Trim();
                copy.survivorId = copy.survivorId.Trim();
                if (!ids.Add(copy.shift_id)) continue;
                copy.fatigue_tier = NightWatchPatrolReadinessEngine.ClassifyFatigue(copy.fatigue_after_permille).ToString();
                normalized.Add(copy);
            }

            normalized.Sort((a, b) =>
            {
                int day = a.day.CompareTo(b.day);
                if (day != 0) return day;
                int start = a.start_hour.CompareTo(b.start_hour);
                if (start != 0) return start;
                int post = string.CompareOrdinal(a.post_id, b.post_id);
                if (post != 0) return post;
                return string.CompareOrdinal(a.shift_id, b.shift_id);
            });
            if (normalized.Count > cap) normalized.RemoveRange(cap, normalized.Count - cap);
            _state.watch_shifts = normalized;
        }

        private void RebuildIndexes()
        {
            _byId.Clear();
            _assignmentByRole.Clear();
            _hiddenFromNorth.Clear();
            _blankRowsLiving.Clear();
            for (int i = 0; i < _state.rows.Count; i++)
            {
                DutyRosterRow row = _state.rows[i];
                if (row == null || string.IsNullOrEmpty(row.survivorId)) continue;
                _byId[row.survivorId] = row;
            }

            for (int i = 0; i < _state.assignments.Count; i++)
            {
                DutyRosterAssignmentEntry a = _state.assignments[i];
                if (a == null || string.IsNullOrEmpty(a.role) || string.IsNullOrEmpty(a.survivorId))
                    continue;
                _assignmentByRole[a.role] = a.survivorId;
            }

            for (int i = 0; i < _state.hiddenFromNorth.Count; i++)
            {
                if (!string.IsNullOrEmpty(_state.hiddenFromNorth[i]))
                    _hiddenFromNorth.Add(_state.hiddenFromNorth[i]);
            }

            for (int i = 0; i < _state.blankRowsLivingNames.Count; i++)
            {
                if (!string.IsNullOrEmpty(_state.blankRowsLivingNames[i]))
                    _blankRowsLiving.Add(_state.blankRowsLivingNames[i]);
            }
        }

        private void RaiseUpdated()
        {
            OnRosterUpdated?.Invoke();
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);

        private static void CopyState(DutyRosterSystemState from, DutyRosterSystemState to)
        {
            to.systemId = from.systemId;
            to.watch_schema_version = from.watch_schema_version <= 0 ? 1 : from.watch_schema_version;
            to.expansionUnlocked = from.expansionUnlocked;
            to.wallInspected = from.wallInspected;
            to.chartScript = from.chartScript;
            to.kessPencilAllowed = from.kessPencilAllowed;
            to.waitInk = from.waitInk;
            to.blankRowsAccess = from.blankRowsAccess;
            to.mutationRosterInUse = from.mutationRosterInUse;
            to.mutationRosterStillBlank = from.mutationRosterStillBlank;
            to.mutationRosterBurned = from.mutationRosterBurned;
            to.mutationRationProtocol = from.mutationRationProtocol;
            to.endingId = from.endingId;
            to.secondWinterActive = from.secondWinterActive;
            to.seedSalt = from.seedSalt;
            to.lastMorningDay = from.lastMorningDay;
            to.daysLeftBlank = from.daysLeftBlank;
            to.lastBurnDay = from.lastBurnDay;
            to.overflowAccess = from.overflowAccess;
            to.overflowVisited = from.overflowVisited != null
                ? new List<string>(from.overflowVisited)
                : new List<string>();
            to.rows = new List<DutyRosterRow>();
            if (from.rows != null)
            {
                for (int i = 0; i < from.rows.Count; i++)
                {
                    if (from.rows[i] != null)
                        to.rows.Add(from.rows[i].Clone());
                }
            }

            to.assignments = new List<DutyRosterAssignmentEntry>();
            if (from.assignments != null)
            {
                for (int i = 0; i < from.assignments.Count; i++)
                {
                    DutyRosterAssignmentEntry a = from.assignments[i];
                    if (a == null) continue;
                    to.assignments.Add(new DutyRosterAssignmentEntry
                    {
                        role = a.role,
                        survivorId = a.survivorId,
                        fitnessWarningAcknowledged = a.fitnessWarningAcknowledged,
                        fitnessWarningDay = a.fitnessWarningDay,
                        fitnessWarningReasons = a.fitnessWarningReasons != null
                            ? new List<string>(a.fitnessWarningReasons)
                            : new List<string>()
                    });
                }
            }

            to.watch_shifts = new List<DutyRosterWatchShift>();
            if (from.watch_shifts != null)
            {
                for (int i = 0; i < from.watch_shifts.Count; i++)
                {
                    if (from.watch_shifts[i] != null)
                        to.watch_shifts.Add(CopyWatchShift(from.watch_shifts[i]));
                }
            }

            to.pneumaticMemos = new List<DutyRosterPneumaticMemo>();
            if (from.pneumaticMemos != null)
            {
                for (int i = 0; i < from.pneumaticMemos.Count; i++)
                {
                    var memo = from.pneumaticMemos[i];
                    if (memo == null) continue;
                    to.pneumaticMemos.Add(new DutyRosterPneumaticMemo
                    {
                        memoId = memo.memoId,
                        targetRoomId = memo.targetRoomId,
                        deliveredDay = memo.deliveredDay,
                        expiresDay = memo.expiresDay,
                        shiftEfficiencyBonus = memo.shiftEfficiencyBonus
                    });
                }
            }

            to.hiddenFromNorth = from.hiddenFromNorth != null
                ? new List<string>(from.hiddenFromNorth)
                : new List<string>();
            to.blankRowsLivingNames = from.blankRowsLivingNames != null
                ? new List<string>(from.blankRowsLivingNames)
                : new List<string>();
        }
    }
}
