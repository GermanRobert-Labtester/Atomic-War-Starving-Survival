using UnityEngine;
using AtomicWar._Game.Survivors;
using Ashfall.Core.Journal;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Desperation fallback: drink straight from the bunker's dirty/irradiated
    /// catchment pool when dehydrated and no clean water is available. Heavily
    /// weighted by RiskBiasTrait (#19) — Reckless/Denialist survivors reach for
    /// it far sooner than Paranoid/Cautious ones. Irradiated water adds a rad
    /// dose via RadiationSystem; dirty water carries a bacterial-illness chance
    /// (Prompt #24) applied as a direct Health hit.
    /// </summary>
    [CreateAssetMenu(fileName = "Action_DrinkContaminatedWater", menuName = "ASHFALL/AI/Drink Contaminated Water Action")]
    public class DrinkContaminatedWaterActionSO : SurvivorAction
    {
        /// <summary>
        /// MISC-005: seeded last-resort stream. Callers should inject a campaign rng;
        /// without this, an un-injected host silently fell back to wall-clock
        /// UnityEngine.Random and made this roll unreplayable across loads.
        /// </summary>
    private static System.Random FallbackRng =>
        AtomicWar._Game.Utilities.SeededRandom.Stream("drink_contaminated_action");

        [Tooltip("Thirst restored per drink from the dirty/irradiated pool.")]
        public float ThirstRestore = 35f;
        [Tooltip("Only considered once thirst is at or above this (0..100) — a last resort, not a habit.")]
        public float MinThirstToConsider = 60f;
        [Tooltip("Radiation dose added (rads) when drinking from the irradiated pool.")]
        public float IrradiatedDoseAmount = 25f;
        [Tooltip("Chance per drink of a bacterial-illness health hit from dirty water.")]
        [Range(0f, 1f)] public float DirtyWaterIllnessChance = 0.35f;
        public float DirtyWaterIllnessHealthLoss = 15f;

        public DrinkContaminatedWaterActionSO()
        {
            id = "action_drink_contaminated_water";
            displayName = "Drink Contaminated Water";
            description = "Desperation fallback: drink straight from the dirty or irradiated catchment pool when no clean water is available.";
            basePriority = 0.05f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null || context.WaterStorage == null) return 0f;

            float thirst = context.Survivor.Needs.Thirst;
            if (thirst < MinThirstToConsider) return 0f;

            if (HasCleanWaterAvailable(context)) return 0f;

            bool hasDirty = context.WaterStorage.DirtyWater > 0f;
            bool hasIrradiated = context.WaterStorage.IrradiatedWater > 0f;
            if (!hasDirty && !hasIrradiated) return 0f;

            float urgency = Mathf.Clamp01(thirst / 100f);
            float riskWillingness = RiskWillingness(context.Survivor.RiskBias);
            return Mathf.Clamp01(urgency * riskWillingness);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.WaterStorage == null) return;

            var storage = context.WaterStorage;
            bool drankIrradiated = storage.IrradiatedWater > 0f && storage.ConsumeIrradiated(1f) > 0f;
            if (!drankIrradiated)
            {
                storage.ConsumeDirty(1f);
            }

            if (context.NeedsSystem != null)
                context.NeedsSystem.Modify(context.Survivor, NeedKind.Thirst, -(ThirstRestore));
            else
                context.Survivor.Needs.Thirst = Mathf.Max(0f, context.Survivor.Needs.Thirst - ThirstRestore);

            if (drankIrradiated)
            {
                context.RadiationSystem?.Expose(context.Survivor, IrradiatedDoseAmount, 1f);
            }
            else
            {
                // Prompt #190 — Iron Stomach: 90% reduced chance of Phase-1 illness from dirty water
                float chance = DirtyWaterIllnessChance;
                if (context.SurvivalPerks != null)
                    chance = context.SurvivalPerks.ScaleIllnessChance(context.Survivor, chance);

                double roll = (context.Random ?? FallbackRng).NextDouble();
                if (roll < chance)
                {
                    SurvivorNeedWrite.AdjustHealth(context.Survivor, -DirtyWaterIllnessHealthLoss);
                    // Prefer dysentery affliction when medical is available
                    context.MedicalSystem?.Inflict(context.Survivor, "dysentery");
                }
            }
        }

        private static bool HasCleanWaterAvailable(AIContext context)
        {
            if (context.WaterStorage.CleanWater > 0f) return true;

            if (context.Inventory?.Slots == null) return false;
            for (int i = 0; i < context.Inventory.Slots.Count; i++)
            {
                var slot = context.Inventory.Slots[i];
                if (slot != null && slot.Item != null && slot.Item.thirstRestore > 0f && slot.Amount > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private static float RiskWillingness(RiskBiasTrait trait) => trait switch
        {
            RiskBiasTrait.Paranoid => 0.05f,
            RiskBiasTrait.Cautious => 0.2f,
            RiskBiasTrait.Realist => 0.5f,
            RiskBiasTrait.Fatalist => 0.65f,
            RiskBiasTrait.Denialist => 0.9f,
            RiskBiasTrait.Reckless => 1f,
            _ => 0.5f
        };
    }
}
