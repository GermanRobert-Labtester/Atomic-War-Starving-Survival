using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GriefCascadeState
    {
        public string eventId = "event_grief_cascade";
        public string triggerDeathId;
        public float belovedRating;
        public bool cascadeActive;
        public int cascadeDay;
        public List<string> deathChain = new List<string>();
        public float moraleFloor;
        public int startDay;
    }

    /// <summary>
    /// Prompt #848: Grief Cascades — Beloved survivor dies, whole bunker
    /// morale drops, triggering Mental Breaks, more deaths, and potential
    /// total collapse within 48 hours.
    /// </summary>
    public class Event_GriefCascade
    {
        /// <summary>
        /// MISC-005: seeded stream backing the default <c>randomFloat</c>. The
        /// parameter exists so hosts can pass a campaign rng for deterministic
        /// replay; the old default reached for wall-clock UnityEngine.Random, so
        /// every caller that omitted it silently opted out of determinism.
        /// </summary>
    private static System.Random FallbackRng =>
        AtomicWar._Game.Utilities.SeededRandom.Stream("event_griefcascade");

        private GriefCascadeState _state = new GriefCascadeState();

        private const float BelovedThreshold = 0.8f;
        private const float MoraleDropMultiplier = 50f;
        private const float MentalBreakMoraleThreshold = 0.3f;
        private const float BreakChance = 0.20f;

        public event Action<string, float> OnBelovedDied;              // survivorId, belovedRating
        public event Action<string> OnCascadeStarted;                  // triggerId
        public event Action<float> OnMoraleCrashed;                    // amount
        public event Action<string, string> OnMentalBreakTriggered;    // survivorId, breakType
        public event Action<string, string> OnSecondaryDeath;          // survivorId, cause
        public event Action<int> OnCascadeEnded;                       // totalDeaths

        public GriefCascadeState CaptureState() => _state;

        public void RestoreState(GriefCascadeState state)
        {
            _state = state ?? new GriefCascadeState();
            if (_state.deathChain == null)
                _state.deathChain = new List<string>();
        }

        /// <summary>
        /// Called when a beloved survivor dies. Evaluates whether a grief cascade triggers.
        /// belovedRating = average affinity of the deceased with all other survivors (0-1).
        /// </summary>
        public bool OnBelovedDeath(string survivorId, float belovedRating)
        {
            if (belovedRating < BelovedThreshold) return false;

            _state.triggerDeathId = survivorId;
            _state.belovedRating = belovedRating;
            _state.deathChain.Add(survivorId);

            OnBelovedDied?.Invoke(survivorId, belovedRating);
            return true;
        }

        /// <summary>
        /// Starts the grief cascade on the given day. Applies initial morale crash.
        /// </summary>
        public float StartCascade(int day)
        {
            _state.cascadeActive = true;
            _state.cascadeDay = 0;
            _state.startDay = day;

            float moraleDrop = _state.belovedRating * MoraleDropMultiplier;
            _state.moraleFloor = -moraleDrop;

            OnCascadeStarted?.Invoke(_state.triggerDeathId);
            OnMoraleCrashed?.Invoke(moraleDrop);

            return moraleDrop;
        }

        /// <summary>
        /// Daily tick during cascade. Each survivor with Morale below threshold
        /// has a chance of a mental break. Caller provides random function
        /// and morale query for deterministic replay.
        /// </summary>
        public List<(string survivorId, string breakType)> TickDay(
            List<string> aliveSurvivors,
            Func<string, float> getMorale,
            Func<float> randomFloat = null)
        {
            if (!_state.cascadeActive) return new List<(string, string)>();

            Func<float> rng = randomFloat ?? (() => (float)FallbackRng.NextDouble());
            _state.cascadeDay++;

            var breaks = new List<(string, string)>();

            foreach (var survivorId in aliveSurvivors)
            {
                float morale = getMorale(survivorId);
                if (morale < MentalBreakMoraleThreshold && rng() < BreakChance)
                {
                    string breakType = ResolveBreakType(rng);
                    breaks.Add((survivorId, breakType));

                    OnMentalBreakTriggered?.Invoke(survivorId, breakType);

                    // If break is fatal (suicide, murder, flee), add to death chain
                    if (breakType == "suicide" || breakType == "murder" || breakType == "flee")
                    {
                        _state.deathChain.Add(survivorId);
                        OnSecondaryDeath?.Invoke(survivorId, breakType);
                    }
                }
            }

            // Check if cascade should end: morale stabilized (no breaks this day)
            if (breaks.Count == 0)
            {
                _state.cascadeActive = false;
                OnCascadeEnded?.Invoke(_state.deathChain.Count);
            }

            return breaks;
        }

        /// <summary>
        /// Determines the type of mental break based on random value.
        /// </summary>
        private string ResolveBreakType(Func<float> rng)
        {
            float roll = rng();
            if (roll < 0.33f) return "suicide";
            if (roll < 0.66f) return "murder";
            return "flee";
        }

        /// <summary>
        /// Forces the cascade to end (e.g., player intervention or morale stabilized).
        /// </summary>
        public void ForceEnd()
        {
            if (!_state.cascadeActive) return;
            _state.cascadeActive = false;
            OnCascadeEnded?.Invoke(_state.deathChain.Count);
        }

        /// <summary>
        /// Returns true if the grief cascade is currently active.
        /// </summary>
        public bool IsCascading() => _state.cascadeActive;

        /// <summary>
        /// Returns the current cascade day (0-based from start).
        /// </summary>
        public int GetCascadeDay() => _state.cascadeDay;

        /// <summary>
        /// Returns the total number of deaths in the cascade chain.
        /// </summary>
        public int GetDeathCount() => _state.deathChain.Count;
    }
}
