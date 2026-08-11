using System;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    [Serializable]
    public class InheritedGeneticsState
    {
        public string traitId = "trait_inherited_genetics";
        public float parentRadThreshold = 500f;
        public float affinityPenaltyWithPure = -0.3f;
    }

    public class Trait_InheritedGenetics
    {
        public event Action<string, string> OnMutationInherited;
        public event Action<string, float> OnAffinityPenalty;

        private readonly InheritedGeneticsState _state;

        public Trait_InheritedGenetics()
        {
            _state = new InheritedGeneticsState();
        }

        public string CheckInheritance(string childId, float parentLifetimeRad, System.Random rng)
        {
            if (parentLifetimeRad < _state.parentRadThreshold)
                return null;

            string mutationType = rng.Next(2) == 0 ? "radiotrophic" : "scales";
            OnMutationInherited?.Invoke(childId, mutationType);
            return mutationType;
        }

        public float GetAffinityPenalty(bool otherIsPure)
        {
            if (!otherIsPure)
                return 0f;

            float penalty = _state.affinityPenaltyWithPure;
            return penalty;
        }

        public void NotifyAffinityPenalty(string childId, bool otherIsPure)
        {
            if (otherIsPure)
                OnAffinityPenalty?.Invoke(childId, _state.affinityPenaltyWithPure);
        }

        public InheritedGeneticsState CaptureState() => _state;

        public void RestoreState(InheritedGeneticsState state)
        {
            _state.traitId = state.traitId;
            _state.parentRadThreshold = state.parentRadThreshold;
            _state.affinityPenaltyWithPure = state.affinityPenaltyWithPure;
        }
    }
}
