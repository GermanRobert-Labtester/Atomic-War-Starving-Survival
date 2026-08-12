using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Ration Conflict System — unequal food/water distribution creates
    /// targeted resentment between survivors, potentially sparking stolen
    /// rations or verbal confrontations.
    ///
    /// Owns: Survivor.PerceivedRationFairness, Survivor.RationResentmentTargetId,
    /// Survivor.RationResentmentLevel.
    /// </summary>
    public class RationConflictSystem
    {
        // ── Constants ──────────────────────────────────────────────────
        public const float FairnessDeviationThreshold = 0.20f;
        public const float ResentmentGainPerDay = 0.10f;
        public const float ResentmentDecayPerDay = 0.03f;
        public const float ConfrontationThreshold = 0.70f;
        public const float TheftThreshold = 0.85f;
        public const float ConfrontationMoraleHit = -10f;
        public const float TheftMoraleHit = -15f;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<Survivor, Survivor, float> OnResentmentBuilt;
        // resenter, target, level
        public event Action<Survivor, Survivor> OnRationConfrontation;
        public event Action<Survivor, Survivor, string> OnRationsStolen;
        // thief, victim, itemId

        // ── Host hooks ─────────────────────────────────────────────────
        public Func<Survivor, float> GetRationAllocation;
        // 0..1 share of total
        public Func<float> GetAverageRationAllocation;
        public Action<Survivor, float> ApplyMoraleDelta;
        public Func<string, string, float> GetAffinity;
        public Action<string, string, float> AdjustAffinity;
        public Func<float> GetDay;
        public System.Random Rng;

        /// <summary>
        /// Tick — update perceived fairness and check conflict thresholds.
        /// </summary>
        public void Tick(Survivor sv, float gameHours, IReadOnlyList<Survivor> survivors)
        {
            if (sv == null || !sv.IsAlive) return;

            float myAllocation = GetRationAllocation?.Invoke(sv) ?? 0.5f;
            float average = GetAverageRationAllocation?.Invoke() ?? 0.5f;

            // Update perceived fairness
            float fairness = 1f - Math.Abs(myAllocation - average);
            sv.PerceivedRationFairness = Mathf.Clamp01(fairness);

            // If getting less than average by the fairness threshold, build resentment.
            float deficit = average - myAllocation;
            if (deficit + 0.001f >= FairnessDeviationThreshold)
            {
                // Find the most over-allocated survivor
                Survivor mostOverAllocated = null;
                float maxAllocation = 0f;
                for (int i = 0; i < survivors.Count; i++)
                {
                    var other = survivors[i];
                    if (other == null || other == sv || !other.IsAlive) continue;
                    float otherAlloc = GetRationAllocation?.Invoke(other) ?? 0.5f;
                    if (otherAlloc > maxAllocation)
                    {
                        maxAllocation = otherAlloc;
                        mostOverAllocated = other;
                    }
                }

                if (mostOverAllocated != null)
                {
                    sv.RationResentmentTargetId = mostOverAllocated.Id;
                    sv.RationResentmentLevel = Math.Min(1f,
                        sv.RationResentmentLevel +
                        ResentmentGainPerDay * (gameHours / 24f));

                    OnResentmentBuilt?.Invoke(sv, mostOverAllocated,
                        sv.RationResentmentLevel);

                    // Check thresholds
                    if (sv.RationResentmentLevel >= TheftThreshold)
                    {
                        AttemptRationTheft(sv, mostOverAllocated);
                    }
                    else if (sv.RationResentmentLevel >= ConfrontationThreshold)
                    {
                        TriggerConfrontation(sv, mostOverAllocated);
                    }
                }
            }
            else
            {
                // Decay resentment when fair
                sv.RationResentmentLevel = Math.Max(0f,
                    sv.RationResentmentLevel -
                    ResentmentDecayPerDay * (gameHours / 24f));
                if (sv.RationResentmentLevel <= 0f)
                    sv.RationResentmentTargetId = null;
            }
        }

        private void TriggerConfrontation(Survivor resenter, Survivor target)
        {
            ApplyMoraleDelta?.Invoke(resenter, ConfrontationMoraleHit);
            ApplyMoraleDelta?.Invoke(target, ConfrontationMoraleHit * 0.5f);
            AdjustAffinity?.Invoke(resenter.Id, target.Id, -20f);
            OnRationConfrontation?.Invoke(resenter, target);
        }

        private void AttemptRationTheft(Survivor thief, Survivor victim)
        {
            float roll = (float)(Rng?.NextDouble() ?? 0.5);
            if (roll < 0.3f) // 30% chance of stealing
            {
                ApplyMoraleDelta?.Invoke(victim, TheftMoraleHit);
                AdjustAffinity?.Invoke(thief.Id, victim.Id, -30f);
                OnRationsStolen?.Invoke(thief, victim, "food_ration");
                thief.RationResentmentLevel = Math.Max(0f,
                    thief.RationResentmentLevel - 0.3f); // release after theft
            }
        }
    }
}
