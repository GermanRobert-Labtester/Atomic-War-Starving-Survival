using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.PlayerCommand;

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

        public ActionResult AssignWithResult(string role, string survivorId)
        {
            return _assignments.AssignWithResult(role, survivorId);
        }

        /// <summary>
        /// Side-effect-free preview of a duty assignment command.
        /// Shares the same validation path as <see cref="AssignWithResult"/>.
        /// </summary>
        public CommandPreview PreviewAssign(string role, string survivorId, long stateVersion = 0)
        {
            var validation = _assignments.ValidateAssign(role, survivorId);
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
        public CommandResult ExecuteAssign(string role, string survivorId, long expectedStateVersion = 0, long currentStateVersion = 0)
        {
            var preview = PreviewAssign(role, survivorId, expectedStateVersion);
            if (!preview.IsAvailable)
                return CommandResult.FromPreview(preview);

            if (preview.StateVersion != currentStateVersion)
                return CommandResult.StalePreview(PlayerCommandCode.AssignRole, preview.StateVersion, currentStateVersion);

            var result = AssignWithResult(role, survivorId);
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
            if (_state.hiddenFromNorth == null) _state.hiddenFromNorth = new List<string>();
            if (_state.blankRowsLivingNames == null) _state.blankRowsLivingNames = new List<string>();
            if (_state.overflowVisited == null) _state.overflowVisited = new List<string>();
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
                        survivorId = a.survivorId
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
