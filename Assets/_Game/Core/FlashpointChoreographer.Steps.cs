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
using Ashfall.Core;

namespace AtomicWar._Game.Flashpoint
{
    public partial class FlashpointChoreographer
    {
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
                case "weather_event_trigger":
                    // Prompts #319–#325 — Section X new weather events.
                    // The bridge in GameBootstrap.Weather.NewContent.cs listens
                    // for this typed event and calls the right Trigger() on the
                    // new Weather_* systems.
                    EventBus.Raise(new FlashpointWeatherEventTriggered(step.weatherEventId));
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
            EmpResult empResult;
            if (_systems == null)
            {
                empResult = new EmpResult();
                EventBus.Raise(new FlashpointEmptiedDevices(0, 0, false, 0f));
                return;
            }

            empResult = EMPEvent.ApplyGlobal(
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
                    if (_systems.NeedsSystem != null)
                        _systems.NeedsSystem.Modify(sv, NeedKind.Morale, -_systems.ExchangeMoraleHit);
                    else
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

            // Publish the "caught outside" intercept signal AFTER the EMP
            // mechanical work so subscribers can read the post-EMP state
            // (radio destroyed, weather forced to Ashfall, etc.). The
            // ExpeditionSystem listens and severs comms on every active
            // expedition, applies trait-driven behavior, and queues the
            // hatch dilemma for when the survivor returns.
            PublishInterceptSignal(empResult);
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

        /// <summary>
        /// Advance the choreography by <paramref name="realDeltaSeconds"/>.
        /// Called from GameBootstrap.Update with Time.deltaTime. The
        /// choreography runs in real time, not game time.
        ///
        /// Semantics: each step's <c>delayFromPreviousSeconds</c> is the wait
        /// time BEFORE that step fires, measured from the previous step (or
        /// from OnNuclearExchange for the first step). A step with delay 0
        /// fires on the next Tick. After a step fires, the wait time for the
        /// NEXT step is set to that next step's delay — never to the current
        /// step's, which would cause the next step to fire on the same tick
        /// when the current step's delay is 0.
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
                ExecuteStep(step);

                if (step.actionId == "complete")
                {
                    CompleteChoreography();
                    break;
                }

                // Set the wait for the NEXT step. If there is no next step,
                // park the timer at MaxValue so the while loop exits.
                int followingIndex = _currentStepIndex + 1;
                if (followingIndex < _sequence.steps.Count)
                {
                    _nextStepDelayRemaining = Mathf.Max(0f,
                        _sequence.steps[followingIndex].delayFromPreviousSeconds);
                }
                else
                {
                    _nextStepDelayRemaining = float.MaxValue;
                }
            }
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

    }
}
