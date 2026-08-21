using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using static Ashfall.Core.DutyRosterHoldfastBridge;

namespace Ashfall.Core
{
    /// <summary>One quest's runtime progress (save-safe, stage machine).</summary>
    [Serializable]
    public class DutyRosterQuestProgress
    {
        public string questId = string.Empty;
        public int startedDay = -1;
        public int currentStage = -1;
        public int completedDay = -1;
        public int failedDay = -1;
        public bool started;
        public bool completed;
        public bool failed;
        public string chosenChoiceId = string.Empty;
    }

    /// <summary>Durable quest ledger for the Duty Roster expansion.</summary>
    [Serializable]
    public class DutyRosterQuestState
    {
        public string systemId = DutyRosterQuestRuntime.SystemId;
        public List<DutyRosterQuestProgress> quests = new List<DutyRosterQuestProgress>();
        /// <summary>Authored complete/fail mutations recorded when a quest resolves (save-safe flags).</summary>
        public List<string> appliedMutations = new List<string>();
    }

    /// <summary>
    /// Quest runtime for ASHFALL: THE DUTY ROSTER (Exp 02).
    /// Consumes the authored duty_roster_quests.json catalog (single authority)
    /// and tracks stage progress exactly like the Crossing quest runtime —
    /// no parallel quest list, no second journal. Progression is driven by the
    /// player through the real panels; quest completion can set authored
    /// mutations/flags that the owning systems read. Deterministic; no RNG.
    /// </summary>
    public class DutyRosterQuestRuntime
    {
        public const string SystemId = "duty_roster_quest_runtime";

        private readonly DutyRosterQuestState _state;
        private readonly Dictionary<string, DutyRosterQuestProgress> _byId =
            new Dictionary<string, DutyRosterQuestProgress>(StringComparer.Ordinal);
        private readonly HashSet<string> _mutations = new HashSet<string>(StringComparer.Ordinal);
        private DutyRosterCatalog _catalog = new DutyRosterCatalog();

        public event Action<DutyRosterQuestProgress> OnQuestStarted;
        public event Action<DutyRosterQuestProgress> OnQuestStageAdvanced;
        public event Action<DutyRosterQuestProgress> OnQuestCompleted;
        public event Action<DutyRosterQuestProgress> OnQuestFailed;
        public event Action<DutyRosterQuestState> OnStateChanged;

        public DutyRosterQuestState State => _state;
        public int StartedCount => _byId.Count;
        public int CompletedCount
        {
            get
            {
                int n = 0;
                foreach (var p in _byId.Values) if (p.completed) n++;
                return n;
            }
        }

        public DutyRosterQuestRuntime(DutyRosterQuestState state = null!)
        {
            _state = state ?? new DutyRosterQuestState();
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            EnsureList();
            RebuildIndexes();
        }

        public void BindCatalog(DutyRosterCatalog catalog)
        {
            _catalog = catalog ?? new DutyRosterCatalog();
        }

        public DutyRosterQuestProgress? GetProgress(string questId)
        {
            if (string.IsNullOrEmpty(questId)) return null;
            return _byId.TryGetValue(questId, out var p) ? p : null;
        }

        public bool IsStarted(string questId) => GetProgress(questId)?.started == true;
        public bool IsComplete(string questId) => GetProgress(questId)?.completed == true;
        public bool IsFailed(string questId) => GetProgress(questId)?.failed == true;
        public int GetCurrentStage(string questId) => GetProgress(questId)?.currentStage ?? -1;

        /// <summary>Quests the player can begin today: min_day met, prereq complete, not started.</summary>
        public List<DutyRosterQuestEntry> GetAvailableQuests(int day)
        {
            var results = new List<DutyRosterQuestEntry>();
            for (int i = 0; i < _catalog.Quests.Count; i++)
            {
                var q = _catalog.Quests[i];
                if (q == null || string.IsNullOrEmpty(q.id)) continue;
                if (IsStarted(q.id) || IsComplete(q.id) || IsFailed(q.id)) continue;
                if (q.min_day > 0 && day < q.min_day) continue;
                if (!string.IsNullOrEmpty(q.prereq_quest_id) && !IsComplete(q.prereq_quest_id)) continue;
                results.Add(q);
            }
            results.Sort((a, b) => string.CompareOrdinal(a.id, b.id));
            return results;
        }

        /// <summary>Started but unfinished quests (for the panel's in-progress list).</summary>
        public List<DutyRosterQuestEntry> GetActiveQuests()
        {
            var results = new List<DutyRosterQuestEntry>();
            for (int i = 0; i < _catalog.Quests.Count; i++)
            {
                var q = _catalog.Quests[i];
                if (q == null) continue;
                var p = GetProgress(q.id);
                if (p != null && p.started && !p.completed && !p.failed)
                    results.Add(q);
            }
            results.Sort((a, b) => string.CompareOrdinal(a.id, b.id));
            return results;
        }

        /// <summary>Start a quest. Validates the soft gate (day + prereq + not started).</summary>
        public bool StartQuest(string questId, int day)
        {
            if (string.IsNullOrEmpty(questId)) return false;
            var def = _catalog.GetQuest(questId);
            if (def == null) return false;
            if (IsStarted(questId) || IsComplete(questId) || IsFailed(questId)) return false;
            if (def.min_day > 0 && day < def.min_day) return false;
            if (!string.IsNullOrEmpty(def.prereq_quest_id) && !IsComplete(def.prereq_quest_id)) return false;

            var progress = new DutyRosterQuestProgress
            {
                questId = questId,
                startedDay = day,
                currentStage = 0,
                started = true
            };
            _state.quests.Add(progress);
            _byId[questId] = progress;

            OnQuestStarted?.Invoke(progress);
            RaiseChanged();
            return true;
        }

        /// <summary>Advance one stage; completes when the final stage is passed.</summary>
        public bool AdvanceStage(string questId, int day)
        {
            var progress = GetProgress(questId);
            if (progress == null || !progress.started || progress.completed || progress.failed) return false;
            var def = _catalog.GetQuest(questId);
            if (def == null) return false;

            progress.currentStage++;
            if (progress.currentStage >= def.StageCount)
            {
                progress.completed = true;
                progress.completedDay = day;
                RecordMutation(def.complete_mutation);
                OnQuestCompleted?.Invoke(progress);
            }
            else
            {
                OnQuestStageAdvanced?.Invoke(progress);
            }
            RaiseChanged();
            return true;
        }

        /// <summary>Resolve a stage choice; the authored set_flag is recorded and applied.</summary>
        public bool ResolveChoice(string questId, string choiceId)
        {
            var progress = GetProgress(questId);
            if (progress == null || !progress.started || progress.completed || progress.failed) return false;
            var def = _catalog.GetQuest(questId);
            if (def == null || def.choices == null) return false;
            for (int i = 0; i < def.choices.Length; i++)
            {
                if (def.choices[i] != null && def.choices[i].id == choiceId)
                {
                    progress.chosenChoiceId = choiceId;
                    RaiseChanged();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Resolve a stage choice AND apply its authored set_flag to the owning
        /// systems (typed — the flag vocabulary is pinned below, never a free
        /// string). Marks ride MoraleMarkSystem; roster flags ride the roster.
        /// </summary>
        public bool ResolveChoiceWithEffects(
            string questId,
            string choiceId,
            DutyRosterSystem roster,
            MoraleMarkSystem marks,
            int day)
        {
            if (!ResolveChoice(questId, choiceId)) return false;
            var def = _catalog.GetQuest(questId);
            if (def == null || def.choices == null) return false;
            for (int i = 0; i < def.choices.Length; i++)
            {
                var c = def.choices[i];
                if (c == null || c.id != choiceId) continue;
                ApplyChoiceFlag(c.set_flag, roster, marks, day);
                return true;
            }
            return false;
        }

        private void ApplyChoiceFlag(string flag, DutyRosterSystem roster, MoraleMarkSystem marks, int day)
        {
            if (string.IsNullOrEmpty(flag)) return;
            switch (flag)
            {
                case DutyRosterSystem.FlagWaitInk:
                    roster?.ResolveChartChoice(DutyRosterSystem.ChoiceWaitInk, day);
                    break;
                case DutyRosterSystem.MutationFactionBlankRowsAccess:
                    roster?.WithdrawBlankRowsAccessPublic();
                    break;
                case "flag_hadi_hidden":
                    roster?.HideFromNorthCopy(DutyRosterSystem.NpcHadiMorrow);
                    marks?.SetMark(MarkHadiHidden, null!, day);
                    break;
                case "flag_hadi_listed":
                    marks?.SetMark(MarkHadiListed, null!, day);
                    break;
                case "flag_hadi_sent":
                    marks?.SetMark(MarkHadiSent, null!, day);
                    break;
                case "flag_tamsin_waystation":
                    marks?.SetMark(MarkTamsinWatchShort, null!, day);
                    break;
                default:
                    if (flag.StartsWith("mark_", StringComparison.Ordinal)
                        || flag.StartsWith("mmc_", StringComparison.Ordinal))
                    {
                        marks?.SetMark(flag, null!, day);
                    }
                    else if (flag.StartsWith("mutation_", StringComparison.Ordinal))
                    {
                        RecordMutation(flag);
                    }
                    else if (roster != null && flag == DutyRosterSystem.MutationFactionBlankRowsAccess)
                    {
                        // (blank-rows handled above)
                    }
                    break;
            }
        }

        public bool FailQuest(string questId, int day)
        {
            var progress = GetProgress(questId);
            if (progress == null || !progress.started || progress.completed || progress.failed) return false;
            var def = _catalog.GetQuest(questId);
            progress.failed = true;
            progress.failedDay = day;
            RecordMutation(def != null ? def.fail_mutation : string.Empty);
            OnQuestFailed?.Invoke(progress);
            RaiseChanged();
            return true;
        }

        /// <summary>True when an authored complete/fail mutation has been recorded.</summary>
        public bool HasMutation(string mutationId)
        {
            return !string.IsNullOrEmpty(mutationId) && _mutations.Contains(mutationId);
        }

        /// <summary>All recorded mutations (save-safe, for the Holdfast bridge / epilogue reads).</summary>
        public IReadOnlyList<string> AppliedMutations => _state.appliedMutations;

        /// <summary>
        /// quest_roster_window opens the crisis window: more than one shelter
        /// encounter per night is allowed while it is active (spec §5.2 Balance).
        /// </summary>
        public bool IsCrisisQuestActive()
        {
            var p = GetProgress(DutyRosterSystem.QuestWindow);
            return p != null && p.started && !p.completed && !p.failed;
        }

        /// <summary>
        /// Apply the KNOWN authored effects of recorded mutations to the owning
        /// systems (typed — game rules are never hidden in arbitrary strings).
        /// Mutations without an owning field stay recorded flags read by the
        /// Holdfast bridge / epilogue; unknown ids are logged, never silent.
        /// </summary>
        public void ApplyKnownEffects(DutyRosterSystem roster, MoraleMarkSystem marks, int day, ILog log = null!)
        {
            log = log ?? NullLog.Instance;
            if (roster == null) return;

            for (int i = 0; i < _state.appliedMutations.Count; i++)
            {
                string m = _state.appliedMutations[i];
                if (string.IsNullOrEmpty(m)) continue;
                switch (m)
                {
                    case DutyRosterSystem.MutationRosterInUse:
                        roster.MarkRosterInUse();
                        break;
                    case DutyRosterSystem.MutationRosterStillBlank:
                        roster.MarkRosterStillBlank();
                        break;
                    case DutyRosterSystem.MutationRationProtocol:
                        roster.SetRationProtocol(true);
                        if (marks != null) marks.SetMark(MarkRationProtocol, null!, day);
                        break;
                    case DutyRosterSystem.MutationRosterInk:
                        roster.ResolveInkEnding(day);
                        if (marks != null) marks.SetMark(MarkRosterInk, null!, day);
                        break;
                    case DutyRosterSystem.MutationRosterBurned:
                        roster.BurnChart(day);
                        if (marks != null) marks.SetMark(MarkRosterBurned, null!, day);
                        break;
                    case DutyRosterSystem.MutationRosterBlank:
                        if (marks != null) marks.SetMark(MarkRosterBlank, null!, day);
                        break;
                    case DutyRosterSystem.MutationFactionBlankRowsAccess:
                        roster.WithdrawBlankRowsAccessPublic();
                        break;
                    // Bespoke quest outcomes -> authored morale marks (typed map,
                    // Appendix A / §4.1; the marks carry the later prose).
                    case "mutation_bunk_claimed":
                        if (marks != null) marks.SetMark(MarkFourteenthClaimed, null!, day);
                        break;
                    case "mutation_fourteenth_in_ash":
                        if (marks != null) marks.SetMark(MarkFourteenthDenied, null!, day);
                        break;
                    case "mutation_hadi_status":
                        if (marks != null) marks.SetMark(MarkHadiListed, null!, day);
                        break;
                    case "mutation_hadi_never_back":
                        if (marks != null) marks.SetMark(MarkHadiNeverBack, null!, day);
                        break;
                    case "mutation_schedule_living":
                        if (marks != null) marks.SetMark(MarkScheduleLiving, null!, day);
                        break;
                    case "mutation_uncorroborated":
                        if (marks != null) marks.SetMark(MarkUncorroborated, null!, day);
                        break;
                    case "mutation_column_voss":
                        if (marks != null) marks.SetMark(MarkColumnVoss, null!, day);
                        break;
                    case "mutation_column_hidden":
                        if (marks != null) marks.SetMark(MarkColumnHidden, null!, day);
                        break;
                    case "mutation_brass_kept":
                        if (marks != null) marks.SetMark(MarkBrassKept, null!, day);
                        break;
                    case "mutation_plate_on_wall":
                        if (marks != null) marks.SetMark(MarkPlateOnWall, null!, day);
                        break;
                    case "mutation_house_thinned":
                    case "mutation_death_in_stack":
                        if (marks != null) marks.SetMark(MarkHouseThinned, null!, day);
                        break;
                    case "mutation_home_watch":
                        if (marks != null) marks.SetMark(MarkHomeHeld, null!, day);
                        break;
                    default:
                        if (m.StartsWith("mark_", StringComparison.Ordinal)
                            || m.StartsWith("mmc_", StringComparison.Ordinal))
                        {
                            if (marks != null) marks.SetMark(m, null!, day);
                        }
                        else
                        {
                            // Recorded flag only — consumed by the Holdfast bridge
                            // and epilogue reads (e.g. mutation_brass_north).
                            log.Info("[DutyRosterQuest] recorded mutation flag: " + m);
                        }
                        break;
                }
            }
        }

        // ── Save / restore ─────────────────────────────────────────────

        public DutyRosterQuestState CaptureState()
        {
            var copy = new DutyRosterQuestState { systemId = SystemId };
            copy.quests = new List<DutyRosterQuestProgress>();
            if (_state.quests != null)
            {
                for (int i = 0; i < _state.quests.Count; i++)
                {
                    var p = _state.quests[i];
                    if (p == null) continue;
                    copy.quests.Add(Clone(p));
                }
            }
            copy.appliedMutations = _state.appliedMutations != null
                ? new List<string>(_state.appliedMutations)
                : new List<string>();
            return copy;
        }

        public void RestoreState(DutyRosterQuestState saved)
        {
            if (saved == null) return;
            _state.quests = new List<DutyRosterQuestProgress>();
            if (saved.quests != null)
            {
                for (int i = 0; i < saved.quests.Count; i++)
                {
                    if (saved.quests[i] != null)
                        _state.quests.Add(Clone(saved.quests[i]));
                }
            }
            _state.systemId = SystemId;
            _state.appliedMutations = saved.appliedMutations != null
                ? new List<string>(saved.appliedMutations)
                : new List<string>();
            EnsureList();
            RebuildIndexes();
            RaiseChanged();
        }

        private void RecordMutation(string mutationId)
        {
            if (string.IsNullOrEmpty(mutationId)) return;
            if (_mutations.Add(mutationId))
            {
                _state.appliedMutations.Add(mutationId);
            }
        }

        private static DutyRosterQuestProgress Clone(DutyRosterQuestProgress p)
        {
            return new DutyRosterQuestProgress
            {
                questId = p.questId,
                startedDay = p.startedDay,
                currentStage = p.currentStage,
                completedDay = p.completedDay,
                failedDay = p.failedDay,
                started = p.started,
                completed = p.completed,
                failed = p.failed,
                chosenChoiceId = p.chosenChoiceId
            };
        }

        private void EnsureList()
        {
            if (_state.quests == null) _state.quests = new List<DutyRosterQuestProgress>();
            if (_state.appliedMutations == null) _state.appliedMutations = new List<string>();
        }

        private void RebuildIndexes()
        {
            _byId.Clear();
            _mutations.Clear();
            for (int i = 0; i < _state.quests.Count; i++)
            {
                var p = _state.quests[i];
                if (p == null || string.IsNullOrEmpty(p.questId)) continue;
                _byId[p.questId] = p;
            }
            for (int i = 0; i < _state.appliedMutations.Count; i++)
            {
                if (!string.IsNullOrEmpty(_state.appliedMutations[i]))
                    _mutations.Add(_state.appliedMutations[i]);
            }
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
