using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Moral Branching System — as moral choices pile up across a campaign,
    /// survivors lean toward Numbed Resilience (immune to death morale loss,
    /// unable to boost others) or Burdened Compassion (boosts bunker morale
    /// when helping others, takes severe stress penalties on tragedy).
    ///
    /// Branch is decided at 5+ moral choices and locked in for the campaign.
    ///
    /// Plain C#, leaf assembly. Host injects choice-category callbacks.
    /// </summary>
    public class MoralBranchingSystem
    {
        // ── Constants ──────────────────────────────────────────────────
        public const int ChoicesToBranch = 5;
        public const float NumbedBaseLevel = 0.6f;
        public const float CompassionBaseLevel = 0.6f;
        public const float BranchStrengthPerChoice = 0.08f;
        public const float BurdenedCompassionShelterMoraleBuff = 5f;
        public const float BurdenedCompassionTragedyPenalty = -15f;
        public const float NumbedResilienceDeathImmuneChance = 1f;
        public const float NumbedResilienceComfortDisabled = 1f;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<Survivor, MoralBranchDirection> OnBranchDecided;
        public event Action<Survivor, float> OnBurdenedCompassionActivated;
        // sv, shelterMoraleDelta
        public event Action<Survivor> OnNumbedComfortBlocked;

        // ── Host hooks ─────────────────────────────────────────────────
        public Action<Survivor, float> ApplyMoraleDelta;
        public Action<float> ApplyShelterMoraleDelta;

        /// <summary>
        /// Register a moral choice made by the player that affects a survivor.
        /// Choices are categorized as empathy-driven or pragmatism-driven.
        /// </summary>
        public void RegisterMoralChoice(Survivor sv, bool isEmpathyChoice)
        {
            if (sv == null || !sv.IsAlive) return;
            if (sv.HasMoralBranch) return; // already branched, locked in

            sv.MoralChoiceCount++;

            if (sv.MoralChoiceCount >= ChoicesToBranch)
                DecideBranch(sv, isEmpathyChoice);
        }

        private void DecideBranch(Survivor sv, bool lastChoiceIsEmpathy)
        {
            // Count empathy vs pragmatism choices (simplified: use last choice direction)
            if (lastChoiceIsEmpathy)
            {
                sv.BranchDirection = MoralBranchDirection.BurdenedCompassion;
                sv.BurdenedCompassionLevel = CompassionBaseLevel;
            }
            else
            {
                sv.BranchDirection = MoralBranchDirection.NumbedResilience;
                sv.NumbedResilienceLevel = NumbedBaseLevel;
            }
            OnBranchDecided?.Invoke(sv, sv.BranchDirection);
        }

        /// <summary>
        /// Apply the Burdened Compassion shelter-wide morale buff.
        /// Call when a burdened-compassion survivor helps another.
        /// </summary>
        public void OnHelpedOthers(Survivor sv)
        {
            if (sv == null || sv.BranchDirection != MoralBranchDirection.BurdenedCompassion)
                return;
            ApplyShelterMoraleDelta?.Invoke(BurdenedCompassionShelterMoraleBuff);
            OnBurdenedCompassionActivated?.Invoke(sv, BurdenedCompassionShelterMoraleBuff);
        }

        /// <summary>
        /// Apply the Burdened Compassion tragedy penalty.
        /// Call when a tragedy occurs (death, raid loss, etc.).
        /// </summary>
        public void OnTragedyWitnessed(Survivor sv)
        {
            if (sv == null || !sv.IsAlive) return;
            if (sv.BranchDirection == MoralBranchDirection.BurdenedCompassion)
            {
                ApplyMoraleDelta?.Invoke(sv, BurdenedCompassionTragedyPenalty);
            }
            // Numbed: no effect — immune
        }

        /// <summary>
        /// Check if a numbed survivor blocks a comfort action.
        /// </summary>
        public bool IsComfortBlocked(Survivor sv)
        {
            if (sv == null) return false;
            if (sv.BranchDirection == MoralBranchDirection.NumbedResilience)
            {
                OnNumbedComfortBlocked?.Invoke(sv);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if a numbed survivor is immune to death morale loss.
        /// </summary>
        public bool IsDeathMoraleImmune(Survivor sv)
        {
            return sv != null &&
                sv.BranchDirection == MoralBranchDirection.NumbedResilience;
        }

        /// <summary>
        /// Get the effective shelter morale modifier from all burdened-compassion
        /// survivors currently helping others.
        /// </summary>
        public float GetShelterMoraleBuff(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return 0f;
            float buff = 0f;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv != null && sv.IsAlive &&
                    sv.BranchDirection == MoralBranchDirection.BurdenedCompassion)
                {
                    buff += BurdenedCompassionShelterMoraleBuff *
                        sv.BurdenedCompassionLevel;
                }
            }
            return buff;
        }
    }
}
