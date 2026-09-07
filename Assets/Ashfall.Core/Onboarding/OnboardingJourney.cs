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

        public static readonly OnboardingStage[] FirstHourOrder =
        {
            OnboardingStage.Water,
            OnboardingStage.Power,
            OnboardingStage.Food,
            OnboardingStage.Research,
            OnboardingStage.Expedition,
        };

        public static readonly OnboardingStageDef[] FirstHour =
        {
            new OnboardingStageDef(
                OnboardingStage.Water,
                "Treat the water",
                "Start a real water-treatment batch. The shelter cannot drink intention.",
                "water_treatment",
                ("water.treatment_started", 1)),
            new OnboardingStageDef(
                OnboardingStage.Power,
                "Restore power",
                "Operate a shelter breaker and see the grid state change.",
                "power_grid",
                ("power.breaker_toggled", 1)),
            new OnboardingStageDef(
                OnboardingStage.Food,
                "Use a food ration",
                "Consume a real food ration from the stores.",
                "inventory",
                ("food.ration_consumed", 1)),
            new OnboardingStageDef(
                OnboardingStage.Research,
                "Start research",
                "Start one available research node. Knowledge takes time.",
                "research",
                ("research.started", 1)),
            new OnboardingStageDef(
                OnboardingStage.Expedition,
                "Dispatch an expedition",
                "Dispatch a real expedition. The surface will answer for it.",
                "expeditions",
                ("expedition.dispatched", 1)),
        };

        public static OnboardingStageDef Def(OnboardingStage id) =>
            Order[(int)id];

        public static OnboardingStageDef DefFor(OnboardingProfile profile, OnboardingStage id)
        {
            if (profile == OnboardingProfile.FirstHour)
            {
                for (int i = 0; i < FirstHour.Length; i++)
                    if (FirstHour[i].Id == id) return FirstHour[i];
                throw new ArgumentOutOfRangeException(nameof(id), id, "Not a first-hour onboarding stage.");
            }

            return Def(id);
        }

        public static IReadOnlyList<OnboardingStage> OrderFor(OnboardingProfile profile) =>
            profile == OnboardingProfile.FirstHour ? FirstHourOrder : LegacyOrder;

        private static readonly OnboardingStage[] LegacyOrder =
        {
            OnboardingStage.Protocol,
            OnboardingStage.Inspect,
            OnboardingStage.Rationing,
            OnboardingStage.Assignment,
            OnboardingStage.Weather,
            OnboardingStage.InventoryUse,
            OnboardingStage.DayAdvance,
        };

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
    /// or <see cref="SkipCurrent"/>; <em>journeyComplete</em> is reached only
    /// by the profile's terminal contract (day advance for legacy, expedition
    /// dispatch for first-hour), never by a clock tick alone.
    /// </summary>
    public sealed class OnboardingJourney
    {
        public const int SaveVersion = 1;

        private readonly Dictionary<string, int> _counts =
            new Dictionary<string, int>(StringComparer.Ordinal);

        private OnboardingSaveState _state;

        public OnboardingStage CurrentStage => (OnboardingStage)_state.currentStage;

        public OnboardingProfile Profile => (OnboardingProfile)_state.profile;

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

        public OnboardingJourney(OnboardingProfile profile)
        {
            OnboardingProfile normalized = profile == OnboardingProfile.FirstHour
                ? OnboardingProfile.FirstHour
                : OnboardingProfile.Legacy;
            _state = new OnboardingSaveState
            {
                profile = (int)normalized,
                currentStage = (int)OnboardingCatalog.OrderFor(normalized)[0],
            };
        }

        public static OnboardingJourney CreateFirstHour() =>
            new OnboardingJourney(OnboardingProfile.FirstHour);

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
            OnboardingStage stage = CurrentStage;
            if (IsTerminalStage(stage)) return false;
            int idx = (int)stage;
            if (_state.completedStages.Contains(idx)) return false;

            _state.completedStages.Add(idx);
            OnStageAdvanced?.Invoke((OnboardingStage)idx);
            _state.currentStage = NextIncompleteIndex();
            EmitJourneyChangedIf();
            return true;
        }

        public void SkipAllRemaining()
        {
            var order = OnboardingCatalog.OrderFor(Profile);
            for (int i = CurrentPosition(); i < order.Count; i++)
            {
                int stage = (int)order[i];
                if (IsTerminalStage(order[i])) break;
                if (!_state.completedStages.Contains(stage))
                {
                    _state.completedStages.Add(stage);
                    OnStageAdvanced?.Invoke(order[i]);
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

            if (Profile == OnboardingProfile.FirstHour)
            {
                _state.journeyComplete = false;
                _state.currentStage = (int)OnboardingCatalog.FirstHourOrder[0];
            }
            else if (_state.day >= 2)
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
            OnStageAdvanced?.Invoke(CurrentStage);
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

            if (Profile == OnboardingProfile.Legacy && _state.day >= 2)
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
            OnboardingCatalog.DefFor(Profile, CurrentStage);

        public bool IsStageRequirementsSatisfied(OnboardingStageDef def)
            => AreRequirementsSatisfied(def);

        public IReadOnlyList<OnboardingStage> OutstandingStages()
        {
            var list = new List<OnboardingStage>();
            if (!JourneyComplete)
            {
                var order = OnboardingCatalog.OrderFor(Profile);
                for (int i = 0; i < order.Count; i++)
                    if (!_state.completedStages.Contains((int)order[i]))
                        list.Add(order[i]);
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
            int guard = OnboardingCatalog.OrderFor(Profile).Count;
            while (guard-- > 0)
            {
                OnboardingStage stage = CurrentStage;
                if (Profile == OnboardingProfile.Legacy && IsTerminalStage(stage)) break;
                int idx = (int)stage;
                if (_state.completedStages.Contains(idx)) break;
                if (!AreRequirementsSatisfied(OnboardingCatalog.DefFor(Profile, stage))) break;

                _state.completedStages.Add(idx);
                if (!_suppressEvents) OnStageAdvanced?.Invoke((OnboardingStage)idx);
                anyChange = true;
                _state.currentStage = NextIncompleteIndex();
            }
            if (Profile == OnboardingProfile.FirstHour &&
                OnboardingCatalog.OrderFor(Profile).Count == _state.completedStages.Count &&
                !JourneyComplete)
            {
                _state.journeyComplete = true;
            }
            return anyChange;
        }

        private int NextIncompleteIndex()
        {
            var order = OnboardingCatalog.OrderFor(Profile);
            for (int i = 0; i < order.Count; i++)
            {
                if (!_state.completedStages.Contains((int)order[i]))
                    return (int)order[i];
            }
            return (int)order[order.Count - 1];
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
                profile = _state.profile,
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
                profile = saved.profile == (int)OnboardingProfile.FirstHour
                    ? (int)OnboardingProfile.FirstHour
                    : (int)OnboardingProfile.Legacy,
                currentStage = NormaliseStage(
                    saved.profile == (int)OnboardingProfile.FirstHour
                        ? OnboardingProfile.FirstHour
                        : OnboardingProfile.Legacy,
                    saved.currentStage),
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

            // Reconcile the legacy terminal stage from the real day boundary.
            // First-hour completion is signal-driven and must not be fabricated
            // by a day change during load.
            bool dayRealAdvance = j.Profile == OnboardingProfile.Legacy && j._state.day >= 2;
            if (dayRealAdvance && !j._state.completedStages.Contains((int)OnboardingStage.DayAdvance))
                j._state.completedStages.Add((int)OnboardingStage.DayAdvance);
            if (j.Profile == OnboardingProfile.Legacy)
            {
                j._state.journeyComplete =
                    dayRealAdvance && j._state.completedStages.Contains((int)OnboardingStage.DayAdvance);
            }
            else
            {
                j._state.journeyComplete = OnboardingCatalog.FirstHourOrder
                    .All(stage => j._state.completedStages.Contains((int)stage));
            }

            // Walk forward over any earlier stages that had already met their
            // requirements at save-time so the resume is exactly correct.
            int guard = OnboardingCatalog.OrderFor(j.Profile).Count;
            while (guard-- > 0 &&
                   (j.Profile == OnboardingProfile.FirstHour || !j.IsTerminalStage(j.CurrentStage)) &&
                   j.AreRequirementsSatisfied(j.CurrentStageDef))
            {
                int idx = j._state.currentStage;
                if (j._state.completedStages.Contains(idx)) break;
                j._state.completedStages.Add(idx);
                j._state.currentStage = j.NextIncompleteIndex();
            }

            j._state.currentStage = j.NextIncompleteIndex();

            j._suppressEvents = false;
            j.OnJourneyChanged?.Invoke(j);
            return j;
        }

        private static int NormaliseStage(OnboardingProfile profile, int idx)
        {
            var order = OnboardingCatalog.OrderFor(profile);
            for (int i = 0; i < order.Count; i++)
                if ((int)order[i] == idx) return idx;
            return (int)order[0];
        }

        private int CurrentPosition()
        {
            var order = OnboardingCatalog.OrderFor(Profile);
            for (int i = 0; i < order.Count; i++)
                if (order[i] == CurrentStage) return i;
            return 0;
        }

        private bool IsTerminalStage(OnboardingStage stage) =>
            Profile == OnboardingProfile.FirstHour
                ? stage == OnboardingStage.Expedition
                : stage == OnboardingStage.DayAdvance;

        private void EmitJourneyChangedIf()
        {
            if (_suppressEvents) return;
            OnJourneyChanged?.Invoke(this);
        }
    }
}
