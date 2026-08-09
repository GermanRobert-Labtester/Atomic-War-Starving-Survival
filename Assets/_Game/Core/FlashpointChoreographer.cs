using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Flashpoint
{
    // -------------------------------------------------------------------
    // Day-30 Flashpoint Choreographer.
    //
    // Two responsibilities:
    //
    // 1) Buildup (days 25-29): subscribe to TimeSystem.OnDayTick. For each
    //    day that has a FlashpointBuildupDay entry, apply its side
    //    effects (audio cue id, economy modifier, world flag). Idempotent:
    //    BuildupDaysProcessed is persisted in save data so save/load
    //    doesn't double-apply.
    //
    // 2) The moment (day 30): subscribe to WorldPhaseSystem.OnNuclearExchange.
    //    A real-time state machine walks the ChoreographyStep list, fires
    //    typed EventBus events at each step, and runs the mechanical EMP /
    //    weather / radiation / morale changes at the right moment. Steps
    //    are timed in real seconds (not game hours) because the flash is
    //    a visual event. Save/load resumes at the last completed step.
    //
    // Construction is plain C# so the class is unit-testable. The
    // GameBootstrap is the only place that wires real systems into it.
    // -------------------------------------------------------------------

    /// <summary>
    /// Plain C# class that orchestrates the Day-30 flashpoint narrative
    /// and mechanics. Owns its own save/load state.
    /// </summary>
    public partial class FlashpointChoreographer
    {
        // -- Config --
        private readonly FlashpointSequenceSO _sequence;
        private readonly Func<bool> _accessibilitySafeMode;
        private readonly FlashpointChoreographerSystems _systems;
        private readonly Func<bool> _hasFlashpointTriggered; // delegate to WorldPhaseSystem.HasTriggeredExchange

        // -- Buildup state --
        private readonly HashSet<int> _buildupDaysProcessed = new HashSet<int>();

        // -- Choreography state --
        private bool _choreographyStarted;
        private int _currentStepIndex = -1;       // last COMPLETED step index; -1 = none yet
        private float _elapsedRealSeconds;       // time accumulated since choreography started
        private float _nextStepDelayRemaining;   // time until the next step fires
        private bool _choreographyCompleted;

        // -- Public state for tests / debug --
        public bool IsChoreographyActive => _choreographyStarted && !_choreographyCompleted;
        public bool IsChoreographyCompleted => _choreographyCompleted;
        public int CurrentStepIndex => _currentStepIndex;
        public IReadOnlyCollection<int> BuildupDaysProcessed => _buildupDaysProcessed;

        /// <summary>Fired the moment the choreography state machine starts.</summary>
        public event Action OnChoreographyStarted;

        /// <summary>Fired the moment the choreography completes its last step.</summary>
        public event Action OnChoreographyCompleted;

        public FlashpointChoreographer(
            FlashpointSequenceSO sequence,
            Func<bool> accessibilitySafeMode,
            FlashpointChoreographerSystems systems,
            Func<bool> hasFlashpointTriggered)
        {
            _sequence = sequence;
            _accessibilitySafeMode = accessibilitySafeMode ?? (() => false);
            _systems = systems;
            _hasFlashpointTriggered = hasFlashpointTriggered ?? (() => false);
        }

        // -----------------------------------------------------------------
        // Public hooks
        // -----------------------------------------------------------------

        /// <summary>
        /// Subscribe to TimeSystem.OnDayTick and WorldPhaseSystem.OnNuclearExchange.
        /// Caller owns the subscription lifetimes (the GameBootstrap will hold
        /// this instance for the lifetime of the session).
        /// </summary>
        public void Attach(Action<Action<int>> subscribeDayTick, Action<Action<int, int>> subscribeHourTick, Action<Action> subscribeExchange)
        {
            if (subscribeDayTick != null) subscribeDayTick(OnDayTick);
            if (subscribeHourTick != null) subscribeHourTick(OnHourTick);
            if (subscribeExchange != null) subscribeExchange(OnNuclearExchange);
        }

        /// <summary>
        /// Fires the choreography state machine. Called by the
        /// WorldPhaseSystem.OnNuclearExchange handler. Idempotent: if the
        /// choreography has already started (e.g. on save/load resume), it
        /// is a no-op.
        /// </summary>
        public void OnNuclearExchange()
        {
            if (_choreographyStarted) return;
            if (_sequence == null || _sequence.steps == null || _sequence.steps.Count == 0)
            {
                // No choreography configured — mark complete so callers can move on.
                _choreographyStarted = true;
                _choreographyCompleted = true;
                return;
            }

            _choreographyStarted = true;
            _currentStepIndex = -1;
            _elapsedRealSeconds = 0f;
            // The first step's delay is the wait AFTER OnNuclearExchange
            // BEFORE the first step fires. Typically 0 (fires immediately).
            _nextStepDelayRemaining = Mathf.Max(0f, _sequence.steps[0].delayFromPreviousSeconds);
            _choreographyCompleted = false;

            string sequenceId = _sequence.sequenceId;
            EventBus.Raise(new FlashpointChoreographyStarted(sequenceId));
            OnChoreographyStarted?.Invoke();
        }

        private void ApplyEconomyModifier(FlashpointEconomyModifier modifier)
        {
            if (modifier == null || _systems == null || _systems.EconomySystem == null) return;

            if (modifier.enableBarterOnlyMode)
            {
                _systems.EconomySystem.SetBarterOnlyMode(
                    enabled: true,
                    acceptedItemIds: modifier.acceptedItemIds);
            }

            if (modifier.demandSpikes != null)
            {
                for (int i = 0; i < modifier.demandSpikes.Count; i++)
                {
                    var spike = modifier.demandSpikes[i];
                    if (spike == null || string.IsNullOrEmpty(spike.itemId)) continue;
                    _systems.EconomySystem.AdjustDemand(spike.itemId, spike.multiplierDelta);
                }
            }
        }

        /// <summary>
        /// Build and publish the typed FlashpointInterceptSignal. Snapshots
        /// the active expeditions at the moment of the EMP. The subscribers
        /// (ExpeditionSystem) are idempotent: re-applying to an already-severed
        /// expedition is a no-op.
        /// </summary>
        private void PublishInterceptSignal(EmpResult empResult)
        {
            var active = _systems?.ExpeditionSystem?.ActiveExpeditions;
            if (active == null || active.Count == 0) return;

            // Copy the list so the signal payload is immutable past publish.
            var snapshot = new List<ExpeditionState>(active.Count);
            for (int i = 0; i < active.Count; i++)
            {
                if (active[i] != null) snapshot.Add(active[i]);
            }
            EventBus.Raise(new FlashpointInterceptSignal(empResult, snapshot));
        }

        private void CompleteChoreography()
        {
            _choreographyCompleted = true;
            EventBus.Raise(FlashpointChoreographyCompleted.Instance);
            OnChoreographyCompleted?.Invoke();
        }

        // -----------------------------------------------------------------
        // Save / load
        // -----------------------------------------------------------------

        public FlashpointChoreographerSave CaptureState()
        {
            var save = new FlashpointChoreographerSave
            {
                ChoreographyStepIndex = _currentStepIndex,
                ElapsedRealSeconds = _elapsedRealSeconds,
                ChoreographyCompleted = _choreographyCompleted
            };
            save.BuildupDaysProcessed.AddRange(_buildupDaysProcessed);
            return save;
        }

        /// <summary>
        /// Restore the choreographer's state from a save. Does NOT replay
        /// buildup days or the choreography — those are gated by the
        /// world flag (buildup) and the WorldPhaseSystem.HasTriggeredExchange
        /// (choreography start). If <paramref name="save"/> is null (V1
        /// save migrated to V2, or fresh launch) and the exchange has
        /// already triggered, the choreography is marked started so the
        /// timeline continues from the right state on next frame.
        /// </summary>
        public void RestoreState(FlashpointChoreographerSave save)
        {
            _buildupDaysProcessed.Clear();
            _choreographyStarted = _hasFlashpointTriggered();
            _choreographyCompleted = false;
            _currentStepIndex = -1;
            _elapsedRealSeconds = 0f;
            _nextStepDelayRemaining = 0f;

            if (save == null) return;

            _buildupDaysProcessed.UnionWith(save.BuildupDaysProcessed);
            _choreographyCompleted = save.ChoreographyCompleted;
            _currentStepIndex = save.ChoreographyStepIndex;
            _elapsedRealSeconds = save.ElapsedRealSeconds;
            // Step delay remaining: start the next step's countdown from
            // zero so the saved step's leftover time is implicitly dropped.
            // A finer-grained per-step resume is a known limitation tracked
            // in the audit report (A-10) — not critical for release.
            _nextStepDelayRemaining = 0f;
        }
    }

    /// <summary>
    /// Bundle of system references the choreographer needs. Lets the
    /// choreographer stay a plain C# class (no MonoBehaviour) while
    /// letting tests pass stubs. No SaveSystem reference: the
    /// Choreographer persists its own state via FlashpointChoreographerSave
    /// (injected through the SaveSystem adapter in GameBootstrap), so it
    /// never has to call SaveSystem.SetWorldFlag directly.
    /// </summary>
    public class FlashpointChoreographerSystems
    {
        public Inventory.Inventory Inventory;
        public Shelter.Shelter Shelter;
        public RadioState RadioState;
        public WeatherSystem WeatherSystem;
        public RadiationSystem RadiationSystem;
        public DynamicEconomySystem EconomySystem;
        public NeedsSystem NeedsSystem;
        public IReadOnlyList<Survivor> Survivors;
        public float ExchangeMoraleHit;

        /// <summary>
        /// Optional: ExpeditionSystem so the EMP step can publish the
        /// "caught outside" intercept signal. When null, the signal is
        /// skipped (no-op; the choreography is mechanical-only).
        /// </summary>
        public ExpeditionSystem ExpeditionSystem;
    }
}
