using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_Eat", menuName = "ASHFALL/AI/Eat Action")]
    public class EatActionSO : SurvivorAction
    {
        /// <summary>
        /// MISC-005: seeded last-resort stream. Callers should inject a campaign rng;
        /// without this, an un-injected host silently fell back to wall-clock
        /// UnityEngine.Random and made this roll unreplayable across loads.
        /// </summary>
        private static readonly System.Random FallbackRng =
            AtomicWar._Game.Utilities.SeededRandom.CreateFixed("eat_action");

        public string FoodItemId = "canned_food";

        /// <summary>Probability of botulism when eating ContaminatedFood (Internal Horror).</summary>
        public const float ContaminatedBotulismChance = 0.35f;

        public EatActionSO()
        {
            id = "action_eat";
            displayName = "Eat";
            description = "Consume food to reduce hunger.";
            basePriority = 0.2f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            // Comatose survivors cannot self-feed
            if (context.MedicalSystem != null && context.MedicalSystem.IsComatose(context.Survivor))
                return 0f;

            // #249 Fierce Mother: cancel Eat when a child need is critical.
            if (context.PersonalQuests != null && context.GetSurvivors != null
                && context.PersonalQuests.ShouldCancelEatOrSleepForChild(
                    context.Survivor, context.GetSurvivors()))
                return 0f;

            // #275 Pacifist hunger strike: refuses to eat.
            if (context.PersonalQuests != null
                && context.PersonalQuests.RefusesToEat(context.Survivor))
                return 0f;

            // #292 Penny Pincher: only eats when hunger is critical (≥95% on 0..100 scale).
            if (context.PersonalQuests != null
                && context.PersonalQuests.ShouldRefuseFoodUntilCritical(
                    context.Survivor, context.Survivor.Needs.Hunger / 100f))
                return 0f;

            float hunger = context.Survivor.Needs.Hunger;

            // If inventory check is required, check if food exists
            if (context.Inventory != null && context.Inventory.Slots != null)
            {
                bool hasFood = false;
                for (int i = 0; i < context.Inventory.Slots.Count; i++)
                {
                    var slot = context.Inventory.Slots[i];
                    if (slot != null && slot.Item != null
                        && (slot.Item.id == FoodItemId
                            || slot.Item.type == ItemType.Food
                            || slot.Item.type == ItemType.ContaminatedFood
                            || slot.Item.hungerRestore > 0f))
                    {
                        hasFood = true;
                        break;
                    }
                }
                if (!hasFood) return 0f;
            }

            return Mathf.Clamp01(hunger / 100f);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null) return;
            var survivor = context.Survivor;
            if (context.MedicalSystem != null && context.MedicalSystem.IsComatose(survivor))
                return;

            // #249 Fierce Mother: cancel Eat mid-select if a child needs care.
            if (context.PersonalQuests != null && context.GetSurvivors != null
                && context.PersonalQuests.ShouldCancelEatOrSleepForChild(
                    survivor, context.GetSurvivors()))
                return;

            // #275 Hunger strike.
            if (context.PersonalQuests != null
                && context.PersonalQuests.RefusesToEat(survivor))
                return;

            // #292 Penny Pincher.
            if (context.PersonalQuests != null
                && context.PersonalQuests.ShouldRefuseFoodUntilCritical(
                    survivor, survivor.Needs.Hunger / 100f))
                return;

            // #289 Microbiologist: 20% chance to refuse rations as tainted.
            if (context.PersonalQuests != null
                && context.PersonalQuests.RollRefuseRationsAsTainted(survivor, context.Random))
            {
                // Spikes own hunger by refusing — leave hunger high.
                survivor.Needs.Hunger = Mathf.Min(100f, survivor.Needs.Hunger + 15f);
                return;
            }

            // #272 Prepper: only eats own pre-war MREs until stash empty.
            if (context.PersonalQuests != null
                && context.PersonalQuests.WillOnlyEatOwnMres(survivor))
            {
                if (context.PersonalQuests.TryConsumePrepperMre(survivor))
                    return;
                // Stash empty — fall through to shared food.
            }

            ItemDefinition food = FindFood(context);
            if (food != null && context.Inventory != null)
            {
                // #256 Selfish: consumes 2× normal rations.
                int units = 1;
                if (context.PersonalQuests != null
                    && context.PersonalQuests.HasSelfish(survivor))
                    units = Mathf.Max(1, Mathf.RoundToInt(
                        context.PersonalQuests.GetRationConsumptionMultiplier(survivor)));

                for (int u = 0; u < units; u++)
                {
                    if (!context.Inventory.Consume(food, survivor, context.RadiationSystem, null))
                    {
                        // Missed full double-ration → morale hit for Selfish.
                        if (u == 0) return;
                        if (context.PersonalQuests != null)
                        {
                            float hit = context.PersonalQuests.GetSelfishMissedRationMoraleHit(survivor);
                            if (hit > 0f)
                                survivor.Needs.Morale = Mathf.Max(0f, survivor.Needs.Morale - hit);
                        }
                        break;
                    }
                }

                // Apply hunger via needs if available; Consume already applied hungerRestore
                // when NeedsSystem is passed — pass null above and apply here for parity.
                // #274 Animalistic: only raw meat.
                if (context.PersonalQuests != null
                    && context.PersonalQuests.EatsOnlyRawMeat(survivor)
                    && food.type != ItemType.Food
                    && food.id != null
                    && food.id.IndexOf("meat", System.StringComparison.OrdinalIgnoreCase) < 0)
                {
                    return; // spit it out
                }

                float restore = food.hungerRestore > 0f ? food.hungerRestore : 40f;
                survivor.Needs.Hunger = Mathf.Max(0f, survivor.Needs.Hunger - restore);
                if (food.moraleEffect != 0f)
                    survivor.Needs.Morale = Mathf.Clamp(survivor.Needs.Morale + food.moraleEffect, 0f, 100f);
                if (food.healthEffect != 0f)
                {
                    if (context.NeedsSystem != null)
                        context.NeedsSystem.AdjustHealth(survivor, food.healthEffect);
                    else
                        SurvivorNeedWrite.AdjustHealth(survivor, food.healthEffect);
                }

                // #273 Outcast room-meal morale hit on others sharing the room.
                if (context.PersonalQuests != null && context.GetSurvivors != null)
                {
                    var all = context.GetSurvivors();
                    if (all != null)
                    {
                        for (int oi = 0; oi < all.Count; oi++)
                        {
                            var other = all[oi];
                            if (other == null || !other.IsAlive) continue;
                            context.PersonalQuests.ApplyOutcastRoomMealMorale(other, survivor);
                        }
                    }
                }

                // ContaminatedFood / spoiled meat → Phase-1 gastric illness roll
                // Prompt #190 — Iron Stomach multiplies chance by 0.10
                if (context.MedicalSystem != null
                    && (food.type == ItemType.ContaminatedFood
                        || string.Equals(food.id, "spoiled_meat", System.StringComparison.OrdinalIgnoreCase)
                        || string.Equals(food.id, SurvivalPerkSystem.SpoiledMeatId,
                            System.StringComparison.OrdinalIgnoreCase)))
                {
                    float chance = ContaminatedBotulismChance;
                    if (context.SurvivalPerks != null)
                        chance = context.SurvivalPerks.ScaleIllnessChance(survivor, chance);
                    double roll = (context.Random ?? FallbackRng).NextDouble();
                    if (roll < chance)
                        context.MedicalSystem.Inflict(survivor, AfflictionSO.Ids.Botulism);
                }
                return;
            }

            // Fallback when no inventory food (tests / empty stock): just reduce hunger
            survivor.Needs.Hunger = Mathf.Max(0f, survivor.Needs.Hunger - 40f);
        }

        private ItemDefinition FindFood(AIContext context)
        {
            if (context.Inventory?.Slots == null) return null;
            ItemDefinition preferred = null;
            ItemDefinition any = null;
            for (int i = 0; i < context.Inventory.Slots.Count; i++)
            {
                var slot = context.Inventory.Slots[i];
                if (slot?.Item == null || slot.Amount <= 0) continue;
                var item = slot.Item;
                if (item.id == FoodItemId) preferred = item;
                if (item.type == ItemType.Food || item.type == ItemType.ContaminatedFood
                    || item.hungerRestore > 0f)
                {
                    // Prefer safe food over contaminated when both exist
                    if (item.type == ItemType.Food && any == null)
                        any = item;
                    else if (any == null)
                        any = item;
                }
            }
            return preferred ?? any;
        }
    }
}
