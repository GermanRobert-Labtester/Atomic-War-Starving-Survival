using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BrainwashedState
    {
        public string survivorId = string.Empty;
        public int hoursOfPropaganda = 0;
        public int brainwashThresholdHours = 48;
        public bool isBrainwashed = false;
        public float defectChance = 0.10f;
        public string lastFrequencyId = string.Empty;
    }

    /// <summary>
    /// Prompt #637: Affliction: Brainwashed.
    /// Listening to Cult/Warlord radio propaganda for too long causes the survivor
    /// to adopt the ideology. Affinity with bunker-mates drops to zero. The survivor
    /// may attempt to defect from the group.
    /// </summary>
    public class Affliction_Brainwashed
    {
        private BrainwashedState _state = new BrainwashedState();

        public event Action<BrainwashedState> OnBrainwashed;
        public event Action<BrainwashedState, bool> OnDefectionAttempt;
        public event Action<BrainwashedState> OnCured;

        public BrainwashedState State => _state;

        public Affliction_Brainwashed(string survivorId)
        {
            _state.survivorId = survivorId;
        }

        public void TickHour(float radioListeningHours, string frequencyId)
        {
            if (_state.isBrainwashed) return;

            _state.lastFrequencyId = frequencyId ?? string.Empty;
            _state.hoursOfPropaganda += Mathf.CeilToInt(radioListeningHours);

            if (_state.hoursOfPropaganda >= _state.brainwashThresholdHours)
            {
                _state.isBrainwashed = true;
                OnBrainwashed?.Invoke(_state);
            }
        }

        public bool CheckDefection(System.Random rng)
        {
            if (!_state.isBrainwashed) return false;

            bool defects = (float)rng.NextDouble() < _state.defectChance;
            OnDefectionAttempt?.Invoke(_state, defects);
            return defects;
        }

        public void Cure()
        {
            _state.isBrainwashed = false;
            _state.hoursOfPropaganda = 0;
            _state.lastFrequencyId = string.Empty;
            OnCured?.Invoke(_state);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public BrainwashedState CaptureState() => _state;

        public void RestoreState(BrainwashedState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
