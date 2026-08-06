using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MascotState
    {
        public string traitId = "trait_mascot";
        public float resilienceBuffMult = 1.5f;
        public bool requiresChildWellFed = true;
        public bool requiresChildHappy = true;
    }

    public class Trait_Mascot
    {
        public event Action<string, float> OnResilienceBuffApplied;
        public event Action OnMascotBuffLost;

        private MascotState _state;

        public Trait_Mascot(MascotState state = null)
        {
            _state = state ?? new MascotState();
        }

        public string TraitId => _state.traitId;

        /// <summary>
        /// Checks whether the mascot buff is active based on the child's condition.
        /// The child must be well-fed and happy for the mascot to provide its buff.
        /// </summary>
        public bool CheckMascotActive(bool childWellFed, bool childHappy)
        {
            bool active = true;

            if (_state.requiresChildWellFed && !childWellFed)
            {
                active = false;
            }

            if (_state.requiresChildHappy && !childHappy)
            {
                active = false;
            }

            return active;
        }

        /// <summary>
        /// Returns the resilience multiplier. 1.5f if mascot is active, 1.0f otherwise.
        /// </summary>
        public float GetResilienceMultiplier(bool mascotActive)
        {
            return mascotActive ? _state.resilienceBuffMult : 1.0f;
        }

        /// <summary>
        /// Applies the mascot resilience buff to an adult. Raises event.
        /// </summary>
        public void ApplyBuff(string adultId)
        {
            if (string.IsNullOrEmpty(adultId))
            {
                Debug.LogWarning("[Trait_Mascot] ApplyBuff called with null/empty adultId.");
                return;
            }

            OnResilienceBuffApplied?.Invoke(adultId, _state.resilienceBuffMult);
        }

        /// <summary>
        /// Call when mascot conditions are no longer met. Raises buff-lost event.
        /// </summary>
        public void RemoveBuff()
        {
            OnMascotBuffLost?.Invoke();
        }

        public MascotState CaptureState()
        {
            return new MascotState
            {
                traitId = _state.traitId,
                resilienceBuffMult = _state.resilienceBuffMult,
                requiresChildWellFed = _state.requiresChildWellFed,
                requiresChildHappy = _state.requiresChildHappy
            };
        }

        public void RestoreState(MascotState state)
        {
            _state = state ?? new MascotState();
        }
    }
}
