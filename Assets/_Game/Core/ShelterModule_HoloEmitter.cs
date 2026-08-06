using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class HoloEmitterState
    {
        public string moduleId = "shelter_module_holo_emitter";
        public float raidReductionPercent = 0.8f;
        public bool isActive = false;
    }

    /// <summary>
    /// Holographic Emitters — installed in the airlock, these project the
    /// illusion of armed guards to deter human raiders. Reduces raid frequency
    /// by 80% against human attackers, but is completely useless against
    /// mutants and animals that cannot be fooled.
    /// Prompt #795: ShelterModule_HoloEmitter
    /// </summary>
    public class ShelterModule_HoloEmitter
    {
        // -- Constants --
        public const float RaidReductionPercent = 0.8f;
        public const float HumanRaidMultiplier = 0.2f;   // 1 - 0.8 = 0.2
        public const float NonHumanRaidMultiplier = 1.0f;

        // -- Events --
        public event Action OnEmittersActivated;
        public event Action OnEmittersDeactivated;

        // -- State --
        private bool _isActive = false;

        // -- Public API --

        /// <summary>
        /// Activates the holographic emitters, projecting armed guard illusions
        /// in the airlock to deter human raiders.
        /// </summary>
        public void Activate()
        {
            _isActive = true;
            OnEmittersActivated?.Invoke();
        }

        /// <summary>
        /// Deactivates the holographic emitters.
        /// </summary>
        public void Deactivate()
        {
            _isActive = false;
            OnEmittersDeactivated?.Invoke();
        }

        /// <summary>
        /// Returns the raid frequency multiplier based on the raider type.
        /// Human raiders are deterred (0.2x frequency). Mutants and animals
        /// are unaffected (1.0x frequency).
        /// </summary>
        /// <param name="raiderType">
        /// Identifier for the raider faction. Expected values include
        /// "human", "raider", "bandit" for human raiders;
        /// "mutant", "animal", "creature" for non-human threats.
        /// </param>
        public float GetRaidFrequencyMultiplier(string raiderType)
        {
            if (!_isActive) return NonHumanRaidMultiplier;
            if (string.IsNullOrEmpty(raiderType)) return NonHumanRaidMultiplier;

            string lower = raiderType.ToLowerInvariant();
            // Human-type raiders are fooled by holograms
            if (lower == "human" || lower == "raider" || lower == "bandit" || lower == "scavenger")
            {
                return HumanRaidMultiplier;
            }
            // Mutants and animals cannot be deceived
            return NonHumanRaidMultiplier;
        }

        /// <summary>Returns true if the emitters are currently active.</summary>
        public bool IsActive() => _isActive;

        // -- Save / Load --

        public HoloEmitterState CaptureState()
        {
            return new HoloEmitterState
            {
                moduleId = "shelter_module_holo_emitter",
                raidReductionPercent = RaidReductionPercent,
                isActive = _isActive
            };
        }

        public void RestoreState(HoloEmitterState saved)
        {
            if (saved == null) return;
            _isActive = saved.isActive;
        }
    }
}
