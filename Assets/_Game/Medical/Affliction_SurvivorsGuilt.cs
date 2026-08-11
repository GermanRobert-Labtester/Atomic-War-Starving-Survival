using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Medical
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

    
    [Serializable]
    public class SurvivorsGuiltSystemSave
    {
        public List<string> keys = new List<string>();
        public List<SurvivorsGuiltState> values = new List<SurvivorsGuiltState>();
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
    
        // ── Save / Load ────────────────────────────────────────────────
        public SurvivorsGuiltSystemSave CaptureState()
        {
            var save = new SurvivorsGuiltSystemSave();
            foreach (var kvp in _states)
            {
                save.keys.Add(kvp.Key);
                save.values.Add(kvp.Value);
            }
            return save;
        }

        public void RestoreState(SurvivorsGuiltSystemSave saved)
        {
            _states.Clear();
            if (saved == null || saved.keys == null) return;
            for (int i = 0; i < saved.keys.Count; i++)
            {
                var val = (saved.values != null && i < saved.values.Count) ? saved.values[i] : null;
                if (val != null) _states[saved.keys[i]] = val;
            }
        }

}
}
