using System;
using System.Collections.Generic;

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
        public string systemId = DutyRosterSystem.SystemId;
        public bool expansionUnlocked;
        public bool wallInspected;
        public string chartScript = DutyRosterSystem.ScriptBlank;
        public bool kessPencilAllowed;
        public bool waitInk;
        public bool blankRowsAccess = true;
        public bool mutationRosterInUse;
        public bool mutationRosterStillBlank;
        public bool mutationRosterBurned;
        public bool mutationRationProtocol;
        public string endingId;
        public bool secondWinterActive;
        public int seedSalt = DutyRosterSystem.SeedUtilityOffset;
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
        public const string SystemId = "duty_roster_system";
        public const string ExpansionId = "expansion_the_duty_roster";
        public const string FlagExpUnlocked = "exp_duty_roster_unlocked";

        public const string LocStackRosterWall = "loc_stack_roster_wall";
        public const string LocStackSleeping = "loc_stack_sleeping";
        public const string LocStackMess = "loc_stack_mess";
        public const string LocStackFiltration = "loc_stack_filtration";
        public const string LocStackAirlock = "loc_stack_airlock";
        public const string LocStackClinicAlcove = "loc_stack_clinic_alcove";

        public const string QuestTheChart = "quest_roster_the_chart";
        public const string QuestWhoEats = "quest_roster_who_eats";
        public const string QuestFourteenth = "quest_roster_fourteenth";
        public const string QuestCaretaker = "quest_roster_caretaker";
        public const string QuestTheColumn = "quest_roster_the_column";
        public const string QuestTheTin = "quest_roster_the_tin";
        public const string QuestQuiet = "quest_roster_quiet";
        public const string QuestSole = "quest_roster_sole";
        public const string QuestWindow = "quest_roster_window";
        public const string QuestInk = "quest_roster_ink";

        public const string NpcKessAdler = "npc_kess_adler";
        public const string NpcAnselDuth = "npc_ansel_duth";
        public const string NpcHadiMorrow = "npc_hadi_morrow";
        public const string NpcTamsinRook = "npc_tamsin_rook";
        public const string NpcLenQuill = "npc_len_quill";
        public const string NpcNilaBrant = "npc_nila_brant";

        public const string ChoiceWritePencil = "roster_write_pencil";
        public const string ChoiceLeaveBlank = "roster_leave_blank";
        public const string ChoiceWaitInk = "roster_wait_ink";
        public const string ChoiceLadleChild = "roster_ladle_child";
        public const string ChoiceLadleHatch = "roster_ladle_hatch";
        public const string ChoiceLadleLeave = "roster_ladle_leave";
        public const string ChoiceLadleProtocol = "roster_ladle_protocol";

        public const string ScriptBlank = "blank";
        public const string ScriptPencil = "pencil";
        public const string ScriptInk = "ink";
        public const string ScriptBurned = "burned";

        public const string StatusHome = "home";
        public const string StatusLevy = "levy";
        public const string StatusWaystation = "waystation";
        public const string StatusQuiet = "quiet";
        public const string StatusMissing = "missing";
        public const string StatusDead = "dead";

        public const string RoleNightWatch = "night_watch";
        public const string RoleMess = "mess";
        public const string RoleHatchOpener = "hatch_opener";
        public const string RoleIntakeSleeper = "intake_sleeper";
        public const string RoleExpedition = "expedition";

        public const string MutationRosterInUse = "mutation_roster_in_use";
        public const string MutationRosterStillBlank = "mutation_roster_still_blank";
        public const string MutationRationProtocol = "mutation_ration_protocol";
        public const string MutationRosterBurned = "mutation_roster_burned";
        public const string MutationRosterInk = "mutation_roster_ink";
        public const string MutationRosterBlank = "mutation_roster_blank";
        public const string MutationFactionBlankRowsAccess = "faction_blank_rows_access";
        public const string FlagWaitInk = "flag_wait_ink";

        // Endings (spec §3 Endings — the game does not rank them)
        public const string EndingInk = "ending_roster_ink";
        public const string EndingPencil = "ending_roster_pencil";
        public const string EndingBlank = "ending_roster_blank";
        public const string EndingBurned = "ending_roster_burned";
        public const string EndingSecondWinter = "ending_roster_second_winter";

        // Second Winter (spec §5.4 — data profile, not a 4th simulation class)
        public const string SeasonSecondWinter = "season_second_winter";
        public const int SecondWinterWindowMinDays = 8;
        public const int SecondWinterWindowMaxDays = 12;
        public const float SecondWinterEncounterWeight = 1.6f;

        /// <summary>Printed manifest cap. Over-occupancy is the fourteenth-bunk quest, not a UI cheat.</summary>
        public const int ManifestCap = 14;
        public const int SoftGateDay = 60;
        public const int StillBlankDays = 40;
        /// <summary>Utility AI salt. Spec: _worldSeed + 1208.</summary>
        public const int SeedUtilityOffset = 1208;

        public static readonly string[] StackWingIds =
        {
            LocStackRosterWall, LocStackSleeping, LocStackMess, LocStackFiltration, LocStackAirlock, LocStackClinicAlcove
        };

        /// <summary>
        /// The Overflow is a small authenticated void practice — four bounded
        /// nodes, not a district. Allocation 11 and 13 are dark; the pump hatch
        /// and the blank cellar are reachable through them (spec §2.4).
        /// </summary>
        public const string LocOverflowAlloc11 = "loc_overflow_alloc_11";
        public const string LocOverflowAlloc13 = "loc_overflow_alloc_13";
        public const string LocOverflowPumpHatch = "loc_overflow_pump_hatch";
        public const string LocOverflowBlankCellar = "loc_overflow_blank_cellar";

        public static readonly string[] OverflowNodeIds =
        {
            LocOverflowAlloc11, LocOverflowAlloc13, LocOverflowPumpHatch, LocOverflowBlankCellar
        };

        public static readonly string[] AssignmentRoles =
        {
            RoleNightWatch, RoleMess, RoleHatchOpener, RoleIntakeSleeper, RoleExpedition
        };

        private DutyRosterSystemState _state = new DutyRosterSystemState();
        private readonly Dictionary<string, DutyRosterRow> _byId = new Dictionary<string, DutyRosterRow>();
        private readonly Dictionary<string, string> _assignmentByRole = new Dictionary<string, string>();
        private readonly HashSet<string> _hiddenFromNorth = new HashSet<string>();
        private readonly HashSet<string> _blankRowsLiving = new HashSet<string>();

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

        public DutyRosterSystem() : this(SeedUtilityOffset)
        {
        }

        public DutyRosterSystem(int seedSalt)
        {
            _state.seedSalt = seedSalt;
            EnsureLists();
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
            if (!_state.expansionUnlocked) return false;
            if (_state.chartScript == ScriptBurned || _state.mutationRosterBurned) return false;
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (script != ScriptPencil && script != ScriptInk) return false;

            if (_blankRowsLiving.Contains(survivorId))
            {
                // Kess will not pencil a Blank Rows name. Ink is how the hatch at 11 goes dark.
                if (script != ScriptInk) return false;
            }

            if (!sleptHere && script == ScriptPencil)
                return false;

            DutyRosterRow existing = GetRow(survivorId)!;
            if (existing == null && OccupiedRowCount >= ManifestCap)
                return false;

            if (existing == null)
            {
                existing = new DutyRosterRow { survivorId = survivorId, status = StatusHome };
                _state.rows.Add(existing);
                _byId[survivorId] = existing;
            }

            existing.displayName = displayName ?? string.Empty;
            existing.occupationObserved = occupationObserved ?? string.Empty;
            existing.script = script;
            if (sleptHere) existing.lastSleptDay = day;
            if (string.IsNullOrEmpty(existing.status)) existing.status = StatusHome;

            if (script == ScriptInk && _blankRowsLiving.Contains(survivorId))
                WithdrawBlankRowsAccess();

            OnNameWritten?.Invoke(survivorId);
            RaiseUpdated();
            return true;
        }

        public bool EraseName(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            DutyRosterRow row = GetRow(survivorId)!;
            if (row == null) return false;

            _state.rows.Remove(row);
            _byId.Remove(survivorId);
            RemoveAssignmentsFor(survivorId);
            OnNameErased?.Invoke(survivorId);
            RaiseUpdated();
            return true;
        }

        public bool BurnChart(int day)
        {
            if (_state.mutationRosterBurned) return false;
            _state.chartScript = ScriptBurned;
            _state.mutationRosterBurned = true;
            _state.mutationRosterInUse = false;
            _state.kessPencilAllowed = false;
            _state.waitInk = false;
            _state.lastBurnDay = day;
            _state.rows.Clear();
            _byId.Clear();
            _assignmentByRole.Clear();
            _state.assignments.Clear();
            OnRosterBurned?.Invoke();
            RaiseUpdated();
            return true;
        }

        /// <summary>Morning tick. Kess fills pencil if allowed. Ink never auto-fills.</summary>
        public void TickMorning(int day, IReadOnlyList<DutyRosterOccupant> occupants)
        {
            if (!_state.expansionUnlocked) return;
            if (_state.chartScript == ScriptBurned) return;

            if (_state.kessPencilAllowed && _state.chartScript == ScriptPencil)
            {
                if (occupants != null)
                {
                    for (int i = 0; i < occupants.Count; i++)
                    {
                        DutyRosterOccupant occ = occupants[i];
                        if (occ == null || string.IsNullOrEmpty(occ.survivorId) || !occ.sleptHere)
                            continue;
                        DutyRosterRow row = GetRow(occ.survivorId)!;
                        if (row == null)
                            WriteName(occ.survivorId, occ.displayName, occ.occupationObserved, ScriptPencil, day, true);
                        else
                            row.lastSleptDay = day;
                    }
                }
            }
            else if (_state.chartScript == ScriptBlank && !_state.waitInk && !_state.kessPencilAllowed)
            {
                _state.daysLeftBlank++;
                if (_state.daysLeftBlank >= StillBlankDays)
                    _state.mutationRosterStillBlank = true;
            }

            _state.lastMorningDay = day;
            RaiseUpdated();
        }

        public bool ResolveChartChoice(string choiceId, int day)
        {
            if (!_state.expansionUnlocked) return false;
            if (_state.chartScript == ScriptBurned) return false;
            if (string.IsNullOrEmpty(choiceId)) return false;

            if (choiceId == ChoiceWritePencil)
            {
                _state.kessPencilAllowed = true;
                _state.waitInk = false;
                _state.chartScript = ScriptPencil;
                _state.mutationRosterInUse = true;
                _state.mutationRosterStillBlank = false;
                _state.daysLeftBlank = 0;
                RaiseUpdated();
                return true;
            }

            if (choiceId == ChoiceLeaveBlank)
            {
                _state.kessPencilAllowed = false;
                _state.waitInk = false;
                _state.chartScript = ScriptBlank;
                _state.daysLeftBlank = 0;
                RaiseUpdated();
                return true;
            }

            if (choiceId == ChoiceWaitInk)
            {
                _state.kessPencilAllowed = false;
                _state.waitInk = true;
                _state.chartScript = ScriptBlank;
                RaiseUpdated();
                return true;
            }

            return false;
        }

        public bool ResolveLadleChoice(string choiceId, int day)
        {
            if (!_state.expansionUnlocked) return false;
            if (string.IsNullOrEmpty(choiceId)) return false;
            if (choiceId == ChoiceLadleProtocol)
            {
                _state.mutationRationProtocol = true;
                RaiseUpdated();
                return true;
            }

            if (choiceId == ChoiceLadleChild || choiceId == ChoiceLadleHatch || choiceId == ChoiceLadleLeave)
            {
                RaiseUpdated();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Ink ending resolution (spec §3 Endings + §4.1 quest_roster_ink).
        /// The wall has names that do not come off in the morning. Edor's return
        /// is current. 11 goes dark if their living is on it. The hatch reversed
        /// reads your list.
        /// </summary>
        public bool ResolveInkEnding(int day)
        {
            if (!_state.expansionUnlocked) return false;
            if (_state.mutationRosterBurned) return false;
            if (_state.chartScript == ScriptBurned) return false;

            _state.chartScript = ScriptInk;
            _state.mutationRosterInUse = true;
            _state.mutationRosterStillBlank = false;
            _state.daysLeftBlank = 0;
            _state.kessPencilAllowed = false;
            _state.waitInk = false;
            _state.endingId = EndingInk;
            RaiseUpdated();
            return true;
        }

        /// <summary>Second Winter season profile active (data, not a 4th sim class).</summary>
        public void SetSecondWinterActive(bool active)
        {
            if (_state.secondWinterActive == active) return;
            _state.secondWinterActive = active;
            RaiseChanged();
        }

        public bool IsSecondWinterActive => _state.secondWinterActive;

        public bool SetStatus(string survivorId, string status)
        {
            DutyRosterRow row = GetRow(survivorId);
            if (row == null) return false;
            if (!IsKnownStatus(status)) return false;
            row.status = status;
            if (status == StatusDead || status == StatusQuiet || status == StatusMissing
                || status == StatusLevy || status == StatusWaystation)
            {
                RemoveAssignmentsFor(survivorId);
            }

            RaiseUpdated();
            return true;
        }

        public bool SetRowScript(string survivorId, string script)
        {
            DutyRosterRow row = GetRow(survivorId);
            if (row == null) return false;
            if (script != ScriptPencil && script != ScriptInk && script != ScriptBlank) return false;
            if (_blankRowsLiving.Contains(survivorId) && script == ScriptPencil)
                return false;
            row.script = script;
            if (script == ScriptInk)
            {
                _state.chartScript = ScriptInk;
                if (_blankRowsLiving.Contains(survivorId))
                    WithdrawBlankRowsAccess();
            }

            RaiseUpdated();
            return true;
        }

        public bool Assign(string role, string survivorId)
        {
            if (!IsKnownRole(role)) return false;

            // Duplicate-role rule: one survivor holds at most one assignment.
            if (string.IsNullOrEmpty(survivorId))
            {
                // Clearing: no duplicate concern.
                return AssignInternal(role, null!);
            }
            DutyRosterRow row = GetRow(survivorId);
            if (row == null) return false;
            if (!CanAssign(row)) return false;

            string currentRole = GetRoleOf(survivorId)!;
            if (currentRole != null && currentRole != role)
                return false; // already assigned elsewhere

            return AssignInternal(role, survivorId);
        }

        private bool AssignInternal(string role, string survivorId)
        {
            if (!IsKnownRole(role)) return false;
            if (!string.IsNullOrEmpty(survivorId))
            {
                DutyRosterRow row = GetRow(survivorId);
                if (row == null) return false;
                if (!CanAssign(row)) return false;
            }

            if (string.IsNullOrEmpty(survivorId))
            {
                _assignmentByRole.Remove(role);
            }
            else
            {
                _assignmentByRole[role] = survivorId;
            }
            SyncAssignmentList();
            OnAssignmentChanged?.Invoke(role, survivorId);
            RaiseUpdated();
            return true;
        }

        /// <summary>The role a survivor currently holds, or null.</summary>
        public string GetRoleOf(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            foreach (var kv in _assignmentByRole)
                if (kv.Value == survivorId) return kv.Key;
            return null;
        }

        public string GetAssignment(string role)
        {
            if (string.IsNullOrEmpty(role)) return null;
            _assignmentByRole.TryGetValue(role, out string id);
            return id;
        }

        /// <summary>
        /// Player skipped the night slate. Deterministic Utility AI among home rows.
        /// Same seed + day => same picks. Does not use string.GetHashCode.
        /// </summary>
        public int AutoAssignDefaults(int day)
        {
            var eligible = new List<string>();
            for (int i = 0; i < _state.rows.Count; i++)
            {
                DutyRosterRow row = _state.rows[i];
                if (row != null && CanAssign(row) && row.status == StatusHome)
                    eligible.Add(row.survivorId);
            }

            eligible.Sort(string.CompareOrdinal);
            int assigned = 0;
            var used = new HashSet<string>();
            for (int r = 0; r < AssignmentRoles.Length; r++)
            {
                string role = AssignmentRoles[r];
                if (_assignmentByRole.ContainsKey(role)) continue;
                string pick = PickEligible(eligible, used, day, role)!;
                if (pick == null) continue;
                if (Assign(role, pick))
                {
                    used.Add(pick);
                    assigned++;
                }
            }

            return assigned;
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

        public bool OverflowAccess => _state.overflowAccess;
        public IReadOnlyList<string> OverflowVisited => _state.overflowVisited;

        public bool GrantOverflowAccess()
        {
            if (_state.overflowAccess) return false;
            _state.overflowAccess = true;
            RaiseChanged();
            return true;
        }

        public bool WithdrawOverflowAccess()
        {
            if (!_state.overflowAccess) return false;
            _state.overflowAccess = false;
            RaiseChanged();
            return true;
        }

        /// <summary>Register a visit to one of the four authenticated Overflow nodes.</summary>
        public bool RegisterOverflowVisit(string nodeId)
        {
            if (!_state.overflowAccess) return false;
            if (!IsOverflowNode(nodeId)) return false;
            if (_state.overflowVisited.Contains(nodeId)) return false;
            _state.overflowVisited.Add(nodeId);
            RaiseChanged();
            return true;
        }

        public bool HasVisitedOverflow(string nodeId)
        {
            return !string.IsNullOrEmpty(nodeId) && _state.overflowVisited != null
                && _state.overflowVisited.Contains(nodeId);
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
            var copy = new DutyRosterOverflowState
            {
                access = _state.overflowAccess,
                visitedNodes = _state.overflowVisited != null
                    ? new List<string>(_state.overflowVisited)
                    : new List<string>()
            };
            return copy;
        }

        /// <summary>Restore the Overflow practice state. Missing state defaults to closed/empty.</summary>
        public void RestoreOverflowState(DutyRosterOverflowState saved)
        {
            if (saved == null) return;
            _state.overflowAccess = saved.access;
            _state.overflowVisited = saved.visitedNodes != null
                ? new List<string>(saved.visitedNodes)
                : new List<string>();
            // Never bless an unauthenticated node into the visited ledger.
            if (_state.overflowVisited != null)
                _state.overflowVisited.RemoveAll(n => !IsOverflowNode(n));
            RaiseChanged();
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

        private bool CanAssign(DutyRosterRow row)
        {
            if (row == null) return false;
            if (row.status == StatusDead || row.status == StatusQuiet || row.status == StatusMissing)
                return false;
            if (row.status == StatusLevy || row.status == StatusWaystation)
                return false;
            return true;
        }

        private string PickEligible(List<string> eligible, HashSet<string> used, int day, string role)
        {
            var pool = new List<string>();
            for (int i = 0; i < eligible.Count; i++)
            {
                if (!used.Contains(eligible[i]))
                    pool.Add(eligible[i]);
            }

            if (pool.Count == 0) return null;
            int salt = _state.seedSalt + SeedUtilityOffset + day * 17 + StableHash.Of(role);
            int n = (int)(((long)salt & 0x7FFFFFFF));
            return pool[n % pool.Count];
        }

        private static bool IsKnownRole(string role)
        {
            for (int i = 0; i < AssignmentRoles.Length; i++)
                if (AssignmentRoles[i] == role) return true;
            return false;
        }

        private static bool IsKnownStatus(string status)
        {
            return status == StatusHome || status == StatusLevy || status == StatusWaystation
                || status == StatusQuiet || status == StatusMissing || status == StatusDead;
        }

        private void RemoveAssignmentsFor(string survivorId)
        {
            var drop = new List<string>();
            foreach (var kv in _assignmentByRole)
            {
                if (kv.Value == survivorId)
                    drop.Add(kv.Key);
            }

            for (int i = 0; i < drop.Count; i++)
                _assignmentByRole.Remove(drop[i]);
            if (drop.Count > 0)
                SyncAssignmentList();
        }

        private void SyncAssignmentList()
        {
            // Emit in ordinal role order: dictionary iteration order is not a
            // cross-host guarantee, and the assignments list is part of the save.
            _state.assignments.Clear();
            var roles = new List<string>(_assignmentByRole.Count);
            foreach (var kv in _assignmentByRole) roles.Add(kv.Key);
            roles.Sort(string.CompareOrdinal);
            for (int i = 0; i < roles.Count; i++)
            {
                _state.assignments.Add(new DutyRosterAssignmentEntry
                {
                    role = roles[i],
                    survivorId = _assignmentByRole[roles[i]]
                });
            }
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
