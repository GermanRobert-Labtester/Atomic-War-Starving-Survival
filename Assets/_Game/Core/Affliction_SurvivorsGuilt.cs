using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SurvivorsGuiltState
    {
        public string survivorId;
        public string deceasedLovedOneName;
        public float moralePenaltyOnGoodEvent = -10f;
        public bool isAfflicted;
        public bool requiresTherapist = true;
    }

    public class SurvivorsGuiltSystem
    {
        private readonly Dictionary<string, SurvivorsGuiltState> _states = new Dictionary<string, SurvivorsGuiltState>();

        public IReadOnlyDictionary<string, SurvivorsGuiltState> States => _states;

        public event Action<string, string> OnGuiltAfflicted;  // survivorId, deceasedName
        public event Action<string, float, string> OnMoraleLostFromGoodEvent;  // survivorId, moraleDelta, goodEventType
        public event Action<string> OnGuiltCured;  // survivorId

        private SurvivorsGuiltState GetOrCreate(string survivorId)
        {
            if (!_states.TryGetValue(survivorId, out var state))
            {
                state = new SurvivorsGuiltState
                {
                    survivorId = survivorId,
                    deceasedLovedOneName = string.Empty,
                    moralePenaltyOnGoodEvent = -10f,
                    isAfflicted = false,
                    requiresTherapist = true
                };
                _states[survivorId] = state;
            }
            return state;
        }

        public void Afflict(string survivorId, string deceasedName)
        {
            var state = GetOrCreate(survivorId);
            if (state.isAfflicted)
                return;

            state.isAfflicted = true;
            state.deceasedLovedOneName = deceasedName;
            OnGuiltAfflicted?.Invoke(survivorId, deceasedName);
        }

        /// <summary>
        /// Applies morale penalty when a good event occurs. Returns morale delta (negative).
        /// Returns 0 if survivor is not afflicted.
        /// </summary>
        public float ApplyGoodEventPenalty(string survivorId, string goodEventType)
        {
            if (!_states.TryGetValue(survivorId, out var state) || !state.isAfflicted)
                return 0f;

            float delta = state.moralePenaltyOnGoodEvent;
            OnMoraleLostFromGoodEvent?.Invoke(survivorId, delta, goodEventType);
            return delta;
        }

        /// <summary>
        /// Attempts to cure the guilt. Only succeeds if a therapist is available.
        /// Returns true if cured.
        /// </summary>
        public bool TryCure(string survivorId, bool hasTherapist)
        {
            if (!_states.TryGetValue(survivorId, out var state) || !state.isAfflicted)
                return false;

            if (state.requiresTherapist && !hasTherapist)
                return false;

            state.isAfflicted = false;
            state.deceasedLovedOneName = string.Empty;
            OnGuiltCured?.Invoke(survivorId);
            return true;
        }
    }
}
