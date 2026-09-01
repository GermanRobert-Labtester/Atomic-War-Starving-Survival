using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Onboarding
{
    /// <summary>
    /// Authoritative definition of one onboarding stage: a stable id, the
    /// observable sigils that satisfy it, and a short label + objective text.
    /// No hint copy lives here — diegetic hint text is composed by the host to
    /// keep Core free of presentation prose.
    /// </summary>
    public readonly struct OnboardingStageDef
    {
        public readonly OnboardingStage Id;
        public readonly string Title;
        public readonly string Objective;
        public readonly string ShowMeWhereRoute;
        public readonly (string Sigil, int Threshold)[] Requirements;

        public OnboardingStageDef(
            OnboardingStage id,
            string title,
            string objective,
            string showMeWhereRoute,
            params (string, int)[] requirements)
        {
            Id = id;
            Title = title ?? string.Empty;
            Objective = objective ?? string.Empty;
            ShowMeWhereRoute = showMeWhereRoute ?? string.Empty;
            Requirements = requirements ?? Array.Empty<(string, int)>();
        }
    }

    /// <summary>
    /// Authoritative catalog of onboarding stages. Order is fixed; the journey
    /// machine iterates in declared order. Sigil names are stable across saves.
    /// </summary>
    public static class OnboardingCatalog
    {
        public const string DaySentinel = "day.at_least";

        public static readonly OnboardingStageDef[] Order =
        {
            new OnboardingStageDef(
                OnboardingStage.Protocol,
                "Resolve the Day 1 protocol",
                "Walk the opening directives: ration, maintenance, then radio. Each choice has a cost.",
                "protocol",
                ("protocol.ration", 1),
                ("protocol.maintenance", 1),
                ("protocol.radio", 1)),
            new OnboardingStageDef(
                OnboardingStage.Inspect,
                "Inspect three bunker rooms",
                "Open the shelter and inspect rooms until three have confirming notes.",
                "shelter",
                ("inspect.room", 3)),
            new OnboardingStageDef(
                OnboardingStage.Rationing,
                "Open the stores and read them",
                "Open the inventory and look at the food and water you are rationing.",
                "inventory",
                ("store.opened", 1)),
            new OnboardingStageDef(
                OnboardingStage.Assignment,
                "Assign a survivor to a duty",
                "Open the duty roster and assign one survivor to a shift. Survivors cannot work without one.",
                "duty_roster",
                ("duty.assigned", 1)),
            new OnboardingStageDef(
                OnboardingStage.Weather,
                "Read the weather",
                "Open the weather forecast or panel to learn what tomorrow will bring.",
                "weather",
                ("weather.read", 1)),
            new OnboardingStageDef(
                OnboardingStage.InventoryUse,
                "Use an item from the stores",
                "Equip a protective item or consume something real from the ledger. Both are real commands.",
                "inventory",
                ("inventory.used", 1)),
            new OnboardingStageDef(
                OnboardingStage.DayAdvance,
                "End Day 1",
                "Press the Advance Day confirm. The first night ticks; the morning briefing returns.",
                "dashboard",
                (DaySentinel, 2)),
        };

        public static OnboardingStageDef Def(OnboardingStage id) =>
            Order[(int)id];

        public static int LastDataStageIndex => (int)OnboardingStage.DayAdvance;
    }

    /// <summary>
    /// Stateful onboarding journey. Deterministic engine-agnostic machine; the
    /// host supplies real observed signals via <see cref="RecordSigil"/> and
    /// updates time via <see cref="SetDay"/>. The persistent sigil store is
    /// an ordinal-stable list so <c>SaveChecksum</c>'s recursive walk yields the
    /// same hash across runtime vs built-with-separate-process builds; the
    /// runtime counter is a private Dictionary used for fast requirements
    /// checks.
    ///
    /// Two completion surfaces coexist: <em>stageComplete</em> from real signals
    /// or <see cref="SkipCurrent"/>; <em>journeyComplete</em> ONLY when the real
    /// first-day advance has actually happened — never auto-skipped.
    /// </summary>
    public sealed class OnboardingJourney
    {
        public const int SaveVersion = 1;

        private readonly Dictionary<string, int> _counts =
            new Dictionary<string, int>(StringComparer.Ordinal);

        private OnboardingSaveState _state;

        public OnboardingStage CurrentStage => (OnboardingStage)_state.currentStage;

        public bool JourneyComplete => _state.journeyComplete;

        public bool IsStageComplete(OnboardingStage stage)
            => _state.completedStages.Contains((int)stage);

        public OnboardingAssistance Assistance => (OnboardingAssistance)_state.assistance;

        public int Day => _state.day;

        public IReadOnlyDictionary<string, int> Sigils => _counts;

        public IReadOnlyList<int> CompletedStages =>
            _state.completedStages ?? new List<int>();

        public IReadOnlyList<string> DismissedHints =>
            _state.dismissedHints ?? new List<string>();

        public event Action<OnboardingStage>? OnStageAdvanced;
        public event Action<OnboardingJourney>? OnJourneyChanged;

        public OnboardingJourney()
        {
            _state = new OnboardingSaveState();
        }

        public OnboardingSignalResult RecordSigil(string sigilName, int delta = 1)
        {
            if (string.IsNullOrWhiteSpace(sigilName) || delta <= 0)
                return OnboardingSignalResult.Ignored;
            if (string.Equals(sigilName, OnboardingCatalog.DaySentinel, StringComparison.Ordinal))
                return OnboardingSignalResult.Ignored;

            bool progressed = false;
            int prior = _counts.TryGetValue(sigilName, out int prev) ? prev : 0;
            int next = prior + delta;
            _counts[sigilName] = next;
            if (next != prior) progressed = true;

            bool newlySatisfied = AdvanceStagesAccountingForNonTerminal();
            EmitJourneyChangedIf();

            if (newlySatisfied) return OnboardingSignalResult.Advanced;
            if (progressed) return OnboardingSignalResult.Progressed;
            return OnboardingSignalResult.Ignored;
        }

        public bool SkipCurrent()
        {
            int idx = _state.currentStage;
            if (idx >= (int)OnboardingStage.DayAdvance) return false;
            if (_state.completedStages.Contains(idx)) return false;

            _state.completedStages.Add(idx);
            OnStageAdvanced?.Invoke((OnboardingStage)idx);
            _state.currentStage = NextIncompleteIndex();
            EmitJourneyChangedIf();
            return true;
        }

        public void SkipAllRemaining()
        {
            for (int i = _state.currentStage; i < (int)OnboardingStage.DayAdvance; i++)
            {
                if (!_state.completedStages.Contains(i))
                {
                    _state.completedStages.Add(i);
                    OnStageAdvanced?.Invoke((OnboardingStage)i);
                }
            }
            _state.currentStage = NextIncompleteIndex();
            EmitJourneyChangedIf();
        }

        public void Replay()
        {
            _state.completedStages.Clear();
            _state.stagesGuided.Clear();
            _state.dismissedHints.Clear();
            _counts.Clear();

            if (_state.day >= 2)
            {
                _state.completedStages.Add((int)OnboardingStage.DayAdvance);
                _state.journeyComplete = true;
                _state.currentStage = (int)OnboardingStage.DayAdvance;
            }
            else
            {
                _state.journeyComplete = false;
                _state.currentStage = (int)OnboardingStage.Protocol;
            }
            OnStageAdvanced?.Invoke(Category(_state.currentStage));
            EmitJourneyChangedIf();
        }

        public void SetAssistance(OnboardingAssistance level)
        {
            if (_state.assistance == (int)level) return;
            _state.assistance = (int)level;
            EmitJourneyChangedIf();
        }

        public void SetDay(int day)
        {
            if (day <= _state.day) return;
            _state.day = day;

            if (_state.day >= 2)
            {
                if (!_state.completedStages.Contains((int)OnboardingStage.DayAdvance))
                {
                    _state.completedStages.Add((int)OnboardingStage.DayAdvance);
                    OnStageAdvanced?.Invoke(OnboardingStage.DayAdvance);
                }
                if (!_state.journeyComplete)
                {
                    _state.journeyComplete = true;
                    OnStageAdvanced?.Invoke(OnboardingStage.DayAdvance);
                }
            }
            _state.currentStage = NextIncompleteIndex();
            EmitJourneyChangedIf();
        }

        public void DismissHint(string hintKey)
        {
            if (string.IsNullOrWhiteSpace(hintKey)) return;
            if (_state.dismissedHints.Contains(hintKey)) return;
            _state.dismissedHints.Add(hintKey);
            EmitJourneyChangedIf();
        }

        public bool IsHintDismissed(string hintKey)
            => !string.IsNullOrWhiteSpace(hintKey) &&
               _state.dismissedHints != null &&
               _state.dismissedHints.Contains(hintKey);

        public void RecordShowMeWhere(OnboardingStage stage)
        {
            if (!_state.stagesGuided.Contains((int)stage))
                _state.stagesGuided.Add((int)stage);
        }

        public bool HasShownShowMeWhere(OnboardingStage stage)
            => _state.stagesGuided.Contains((int)stage);

        public OnboardingStageDef CurrentStageDef =>
            OnboardingCatalog.Def(CurrentStage);

        public bool IsStageRequirementsSatisfied(OnboardingStageDef def)
            => AreRequirementsSatisfied(def);

        public IReadOnlyList<OnboardingStage> OutstandingStages()
        {
            var list = new List<OnboardingStage>();
            bool pastTerminal = _state.currentStage >= OnboardingCatalog.LastDataStageIndex
                                  && _state.completedStages.Contains(OnboardingCatalog.LastDataStageIndex);
            if (!pastTerminal)
            {
                for (int i = 0; i <= OnboardingCatalog.LastDataStageIndex; i++)
                    if (!_state.completedStages.Contains(i))
                        list.Add((OnboardingStage)i);
            }
            return list;
        }

        private bool AreRequirementsSatisfied(OnboardingStageDef def)
        {
            foreach (var req in def.Requirements)
            {
                if (string.Equals(req.Sigil, OnboardingCatalog.DaySentinel, StringComparison.Ordinal))
                {
                    if (_state.day < req.Threshold) return false;
                    continue;
                }
                if (!_counts.TryGetValue(req.Sigil, out int count) || count < req.Threshold)
                    return false;
            }
            return true;
        }

        private bool _suppressEvents;

        private bool AdvanceStagesAccountingForNonTerminal()
        {
            bool anyChange = false;
            int guard = OnboardingCatalog.LastDataStageIndex + 1;
            while (guard-- > 0)
            {
                int idx = _state.currentStage;
                if (idx >= (int)OnboardingStage.DayAdvance) break;
                if (_state.completedStages.Contains(idx)) break;
                if (!AreRequirementsSatisfied(OnboardingCatalog.Def((OnboardingStage)idx))) break;

                _state.completedStages.Add(idx);
                if (!_suppressEvents) OnStageAdvanced?.Invoke((OnboardingStage)idx);
                anyChange = true;
                _state.currentStage = NextIncompleteIndex();
            }
            return anyChange;
        }

        private int NextIncompleteIndex()
        {
            int lastData = (int)OnboardingStage.DayAdvance;
            for (int i = 0; i < lastData; i++)
            {
                if (!_state.completedStages.Contains(i))
                    return i;
            }
            return lastData;
        }

        public OnboardingSaveState CaptureState()
        {
            // Build the persistent sigil list with strictly sorted ordinal-stable
            // ordering so SaveChecksum's recursive walk is deterministic and
            // byte-identical across runs.
            var persistedSigils = _counts
                .Where(pair => pair.Value > 0)
                .OrderBy(pair => pair.Key, StringComparer.Ordinal)
                .Select(pair => new OnboardingSigilRecord { key = pair.Key, count = pair.Value })
                .ToList();

            return new OnboardingSaveState
            {
                schemaVersion = _state.schemaVersion,
                day = _state.day,
                sigils = persistedSigils,
                currentStage = _state.currentStage,
                completedStages = new List<int>(_state.completedStages),
                journeyComplete = _state.journeyComplete,
                assistance = _state.assistance,
                dismissedHints = new List<string>(_state.dismissedHints ?? new List<string>()),
                stagesGuided = new List<int>(_state.stagesGuided ?? new List<int>()),
            };
        }

        public static OnboardingJourney Restore(OnboardingSaveState saved)
        {
            var j = new OnboardingJourney();
            if (saved == null) return j;

            int futureVersion = saved.schemaVersion;
            if (futureVersion <= 0)
                futureVersion = SaveVersion;
            if (futureVersion > SaveVersion)
                throw new InvalidOperationException(
                    $"Onboarding save version {futureVersion} is not supported by this build (max v{SaveVersion}).");

            // Reconstruction is idempotent — suppress per-step events so
            // listeners attached after Restore don't see a flood, but emit
            // one journey-changed after Restore completes.
            j._suppressEvents = true;

            j._state = new OnboardingSaveState
            {
                schemaVersion = futureVersion,
                day = saved.day == 0 ? 1 : saved.day,
                currentStage = NormaliseStage(saved.currentStage),
                completedStages = saved.completedStages != null
                    ? new List<int>(saved.completedStages)
                    : new List<int>(),
                journeyComplete = saved.journeyComplete,
                assistance = saved.assistance,
                dismissedHints = saved.dismissedHints != null
                    ? new List<string>(saved.dismissedHints)
                    : new List<string>(),
                stagesGuided = saved.stagesGuided != null
                    ? new List<int>(saved.stagesGuided)
                    : new List<int>(),
            };

            // Reconstruct runtime counter map from the persisted ordinal list.
            if (saved.sigils != null)
            {
                for (int i = 0; i < saved.sigils.Count; i++)
                {
                    var rec = saved.sigils[i];
                    if (rec == null || rec.count <= 0 || string.IsNullOrEmpty(rec.key)) continue;
                    j._counts[rec.key] = rec.count;
                }
            }

            // Reconcile: the terminal DayAdvance stage completes iff the real
            // day has reached 2.
            bool dayRealAdvance = j._state.day >= 2;
            if (dayRealAdvance && !j._state.completedStages.Contains((int)OnboardingStage.DayAdvance))
                j._state.completedStages.Add((int)OnboardingStage.DayAdvance);
            j._state.journeyComplete =
                dayRealAdvance && j._state.completedStages.Contains((int)OnboardingStage.DayAdvance);

            // Walk forward over any earlier stages that had already met their
            // requirements at save-time so the resume is exactly correct.
            while (j._state.currentStage < (int)OnboardingStage.DayAdvance &&
                   j.AreRequirementsSatisfied(OnboardingCatalog.Def(
                       (OnboardingStage)j._state.currentStage)))
            {
                int idx = j._state.currentStage;
                if (j._state.completedStages.Contains(idx)) break;
                j._state.completedStages.Add(idx);
                if (j._state.currentStage < (int)OnboardingStage.DayAdvance - 1)
                    j._state.currentStage++;
                else
                    break;
            }

            j._state.currentStage = j.NextIncompleteIndex();

            j._suppressEvents = false;
            j.OnJourneyChanged?.Invoke(j);
            return j;
        }

        private static int NormaliseStage(int idx)
        {
            if (idx < 0 || idx >= OnboardingCatalog.Order.Length)
                return (int)OnboardingStage.Protocol;
            return idx;
        }

        private static OnboardingStage Category(int idx)
            => (OnboardingStage)Math.Clamp(idx, 0, OnboardingCatalog.Order.Length - 1);

        private void EmitJourneyChangedIf()
        {
            if (_suppressEvents) return;
            OnJourneyChanged?.Invoke(this);
        }
    }
}
