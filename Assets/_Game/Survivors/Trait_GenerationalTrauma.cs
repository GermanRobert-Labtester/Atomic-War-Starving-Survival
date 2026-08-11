using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    [Serializable]
    public class GenerationalTraumaState
    {
        public string traitId = "trait_generational_trauma";
        public float moraleCapPenalty = 0.10f;
        public List<string> witnessedChildIds = new List<string>();
        public List<string> witnessedParentIds = new List<string>();
        public List<string> appliedTeenIds = new List<string>();
    }

    public class Trait_GenerationalTrauma
    {
        public event Action<string, string> OnTraumaWitnessed;
        public event Action<string, float> OnMoraleCapped;

        private readonly GenerationalTraumaState _state;

        public Trait_GenerationalTrauma()
        {
            _state = new GenerationalTraumaState();
        }

        public void RecordParentBreak(string childId, string parentId)
        {
            _state.witnessedChildIds.Add(childId);
            _state.witnessedParentIds.Add(parentId);
            OnTraumaWitnessed?.Invoke(childId, parentId);
        }

        public void ApplyOnTransition(string teenId)
        {
            if (!_state.witnessedChildIds.Contains(teenId))
                return;
            if (_state.appliedTeenIds.Contains(teenId))
                return;

            _state.appliedTeenIds.Add(teenId);
            OnMoraleCapped?.Invoke(teenId, _state.moraleCapPenalty);
        }

        public float GetMoraleCapMultiplier()
        {
            return 1f - _state.moraleCapPenalty;
        }

        public GenerationalTraumaState CaptureState() => _state;

        public void RestoreState(GenerationalTraumaState state)
        {
            _state.traitId = state.traitId;
            _state.moraleCapPenalty = state.moraleCapPenalty;
            _state.witnessedChildIds = new List<string>(state.witnessedChildIds);
            _state.witnessedParentIds = new List<string>(state.witnessedParentIds);
            _state.appliedTeenIds = new List<string>(state.appliedTeenIds);
        }
    }
}
