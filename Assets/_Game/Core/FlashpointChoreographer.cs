using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
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
    public class FlashpointChoreographer
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
        /// Day-tick handler. Applies the buildup day entry for this day if
        /// one exists and hasn't been applied yet. Idempotent across save/load.
        /// </summary>
        public void OnDayTick(int day)
        {
            if (_sequence == null) return;
            var entry = _sequence.FindBuildupDay(day);
            if (entry == null) return;
            if (_buildupDaysProcessed.Contains(day)) return;

            ApplyBuildupDay(entry);
            _buildupDaysProcessed.Add(day);
        }

        /// <summary>
        /// Hour-tick handler. Currently unused by the choreography itself, but
        /// exposed so the GameBootstrap can wire a programmatic "false calm"
        /// narrative event that fires during the morning of day 30 (a few
        /// hours before OnNuclearExchange if the exchange is configured to
        /// fire mid-day).
        /// </summary>
        public void OnHourTick(int day, int hour)
        {
            // No-op by design. Subclasses or future revisions can use this
            // hook for time-of-day buildup beats (e.g. an audio cue that
            // ramps over hours, or the false-calm morning).
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
            _nextStepDelayRemaining = 0f;
            _choreographyCompleted = false;

            string sequenceId = _sequence.sequenceId;
            EventBus.Raise(new FlashpointChoreographyStarted(sequenceId));
            OnChoreographyStarted?.Invoke();
        }

        /// <summary>
        /// Advance the choreography by <paramref name="realDeltaSeconds"/>.
        /// Called from GameBootstrap.Update with Time.deltaTime. The
        /// choreography runs in real time, not game time.
        /// </summary>
        public void Tick(float realDeltaSeconds)
        {
            if (!_choreographyStarted || _choreographyCompleted) return;
            if (realDeltaSeconds <= 0f) return;
            if (_sequence == null || _sequence.steps == null) return;

            _elapsedRealSeconds += realDeltaSeconds;
            _nextStepDelayRemaining -= realDeltaSeconds;

            // Process every step whose delay has elapsed. This allows a single
            // tick() to catch up if the frame rate is low.
            while (!_choreographyCompleted && _nextStepDelayRemaining <= 0f)
            {
                int nextIndex = _currentStepIndex + 1;
                if (nextIndex >= _sequence.steps.Count)
                {
                    CompleteChoreography();
                    break;
                }

                var step = _sequence.steps[nextIndex];
                _currentStepIndex = nextIndex;
                _nextStepDelayRemaining = Mathf.Max(0f, step.delayFromPreviousSeconds);
                ExecuteStep(step);

                if (step.actionId == "complete")
                {
                    CompleteChoreography();
                    break;
                }
            }
        }

        // -----------------------------------------------------------------
        // Buildup side effects
        // -----------------------------------------------------------------

        private void ApplyBuildupDay(FlashpointBuildupDay entry)
        {
            if (entry == null) return;

            // Audio cue: emit a typed event with the cue id. The audio layer
            // is responsible for the actual mix swap; we don't import the
            // AudioMixer here (no engine reference).
            // Economy modifier: applies a demand spike and (optionally) sets
            // barter-only mode so the trader panic is diegetic.
            string economyModifierId = entry.economyModifierId;
            FlashpointEconomyModifier economyModifier = null;
            if (!string.IsNullOrEmpty(economyModifierId))
            {
                economyModifier = _sequence.FindEconomyModifier(economyModifierId);
                if (economyModifier != null)
                {
                    ApplyEconomyModifier(economyModifier);
                }
            }

            // The worldFlagKey is informational only: the Choreographer's own
            // _buildupDaysProcessed set is the authoritative idempotency
            // guard (persisted via FlashpointChoreographerSave). Other
            // systems that need to know whether a buildup day has applied
            // can check that set, or subscribe to FlashpointBuildupDayEntered.
            EventBus.Raise(new FlashpointBuildupDayEntered(
                day: entry.day,
                audioCueId: entry.audioCueId,
                economyModifierId: economyModifierId,
                worldFlagKey: entry.worldFlagKey));
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

        // -----------------------------------------------------------------
        // Choreography step execution
        // -----------------------------------------------------------------

        private void ExecuteStep(FlashpointChoreographyStep step)
        {
            if (step == null || string.IsNullOrEmpty(step.actionId)) return;

            // The narrative side: raise a typed event for the UI / audio / VFX
            // layer. The mechanical side is handled inline where appropriate.
            switch (step.actionId)
            {
                case "flash":
                    ExecuteFlashStep(step);
                    break;
                case "emp":
                    ExecuteEmpStep(step);
                    break;
                case "shockwave":
                    ExecuteShockwaveStep(step);
                    break;
                case "sirens":
                    EventBus.Raise(new FlashpointSirensSpooling(muffled: true));
                    break;
                case "weather_shift":
                    EventBus.Raise(new FlashpointWeatherShifted("Ashfall"));
                    break;
                case "radiation_hud_unlock":
                    EventBus.Raise(FlashpointRadiationHudUnlocked.Instance);
                    break;
                case "complete":
                    // Handled by Tick() after the step.
                    break;
                default:
                    Debug.LogWarning($"[FlashpointChoreographer] Unknown action id '{step.actionId}'");
                    break;
            }
        }

        private void ExecuteFlashStep(FlashpointChoreographyStep step)
        {
            bool safe = _accessibilitySafeMode();
            float duration = safe && _sequence.accessibility != null
                ? _sequence.accessibility.safeFlashSeconds
                : _sequence.accessibility != null
                    ? _sequence.accessibility.defaultFlashSeconds
                    : 4f;

            EventBus.Raise(new FlashpointFlashStarted(duration, safe));
        }

        private void ExecuteEmpStep(FlashpointChoreographyStep step)
        {
            if (_systems == null)
            {
                EventBus.Raise(new FlashpointEmptiedDevices(0, 0, false, 0f));
                return;
            }

            var empResult = EMPEvent.ApplyGlobal(
                _systems.Inventory,
                _systems.Shelter,
                _systems.RadioState);

            float moraleHit = 0f;
            if (_systems.Survivors != null && _systems.ExchangeMoraleHit > 0f)
            {
                for (int i = 0; i < _systems.Survivors.Count; i++)
                {
                    var sv = _systems.Survivors[i];
                    if (sv == null || !sv.IsAlive) continue;
                    sv.Needs.Morale = Mathf.Clamp(sv.Needs.Morale - _systems.ExchangeMoraleHit, 0f, 100f);
                    moraleHit = _systems.ExchangeMoraleHit;
                }
            }

            if (_systems.WeatherSystem != null)
            {
                _systems.WeatherSystem.RestrictToNonHazardWeather = false;
                _systems.WeatherSystem.ForceWeather(WeatherKind.Ashfall);
            }

            if (_systems.RadiationSystem != null)
            {
                _systems.RadiationSystem.IsPaused = false;
            }

            EventBus.Raise(new FlashpointEmptiedDevices(
                empResult.DevicesBroken,
                empResult.ModulesDisabled,
                empResult.RadioDestroyed,
                moraleHit));
        }

        private void ExecuteShockwaveStep(FlashpointChoreographyStep step)
        {
            bool safe = _accessibilitySafeMode();
            float shakeAmp = step.cameraShakeAmplitude;
            if (safe && _sequence.accessibility != null)
            {
                shakeAmp *= _sequence.accessibility.safeShakeMultiplier;
            }
            // Approximate the rumble as 6s of camera shake. The audio layer
            // supplies the actual sub-bass; we just signal the visual.
            float duration = 6f;
            EventBus.Raise(new FlashpointShockwaveHit(shakeAmp, duration));
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
            // A finer-grained per-step resume is a known limitation; see
            // WorldPhaseSave.ChoreographyStepIndex TODO.
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
        public IReadOnlyList<Survivor> Survivors;
        public float ExchangeMoraleHit;
    }
}
