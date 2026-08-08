using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GlowingStagState
    {
        public string id = "encounter_glowing_stag";
        public string displayName = "The Glowing Stag";
        public bool isPassive = true;
        public bool requiresSniper = true;
        public int radiotrophicMeatYield = 1;
        public float maxHealthBonus = 15f;
        public float radResistanceBonus = 10f;
        public float moraleKillPenalty = -30f;
    }

    /// <summary>
    /// Prompt #554: Encounter: The Glowing Stag.
    /// Rare, beautifully serene mutant animal. Passive.
    /// If hunted (requires Sniper), yields RadiotrophicMeat.
    /// Eating it permanently increases max Health and RadiationResistance.
    /// Causes massive Morale drop from killing something so pure.
    /// </summary>
    public class Encounter_GlowingStag
    {
        private GlowingStagState _state = new GlowingStagState();

        public event Action<GlowingStagState> OnStagSpotted;
        public event Action<GlowingStagState> OnStagHunted;
        public event Action<GlowingStagState, float, float, float> OnRadiotrophicMeatConsumed;

        public GlowingStagState State => _state;

        public bool TryHunt(bool hasSniper, float shooterSkill)
        {
            OnStagSpotted?.Invoke(_state);

            if (!hasSniper)
            {
                return false;
            }

            float successChance = Mathf.Clamp01(shooterSkill);
            System.Random rng = AtomicWar._Game.Utilities.SeededRandom.CreateFixed("encounter_glowingstag");
            bool success = rng.NextDouble() < successChance;

            if (success)
            {
                OnStagHunted?.Invoke(_state);
            }

            return success;
        }

        public (float newMaxHealth, float newRadResistance, float newMorale) ApplyConsumptionEffects(
            float currentMaxHealth,
            float currentRadResistance,
            float currentMorale)
        {
            float newMaxHealth = currentMaxHealth + _state.maxHealthBonus;
            float newRadResistance = currentRadResistance + _state.radResistanceBonus;
            float newMorale = currentMorale + _state.moraleKillPenalty;

            OnRadiotrophicMeatConsumed?.Invoke(_state, newMaxHealth, newRadResistance, newMorale);

            return (newMaxHealth, newRadResistance, newMorale);
        }

        public GlowingStagState CaptureState() => _state;

        public void RestoreState(GlowingStagState saved)
        {
            _state = saved ?? new GlowingStagState();
        }
    }
}
