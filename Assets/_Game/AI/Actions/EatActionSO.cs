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
    private static System.Random FallbackRng =>
        AtomicWar._Game.Utilities.SeededRandom.Stream("eat_action");

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
            if (context.BunkerRationsScheduled) return 0f;
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
            if (RefusesMeal(context, survivor)) return;

            ItemDefinition food = FindFood(context);
            if (food == null || context.Inventory == null)
            {
                // Fallback when no inventory food (tests / empty stock): just reduce hunger
                if (context.NeedsSystem != null)
                    context.NeedsSystem.Modify(survivor, NeedKind.Hunger, -(40f));
                else
                    survivor.Needs.Hunger = Mathf.Max(0f, survivor.Needs.Hunger - 40f);
                return;
            }

            // #274 Animalistic: only raw meat, checked before consumption. The old
            // order drew the ration first and spat it out after — the survivor's
            // stock was spent on a meal they refused to eat. Checking first means a
            // refusal costs nothing and the shared food stays in the inventory.
            if (SpitsOutNonMeat(context, survivor, food)) return;

            if (!ConsumeRations(context, survivor, food)) return;

            ApplyFoodEffects(context, survivor, food);
            ApplyOutcastRoomMealMorale(context, survivor);
            RollGastricIllness(context, survivor, food);
        }

        /// <summary>
        /// Trait and quest gates that abort the meal before any food is drawn.
        /// Returns true when the survivor will not eat this tick.
        /// </summary>
        private static bool RefusesMeal(AIContext context, Survivor survivor)
        {
            if (context.MedicalSystem != null && context.MedicalSystem.IsComatose(survivor))
                return true;

            var quests = context.PersonalQuests;
            if (quests == null) return false;

            // #249 Fierce Mother: cancel Eat mid-select if a child needs care.
            if (context.GetSurvivors != null
                && quests.ShouldCancelEatOrSleepForChild(survivor, context.GetSurvivors()))
                return true;

            // #275 Hunger strike.
            if (quests.RefusesToEat(survivor)) return true;

            // #292 Penny Pincher.
            if (quests.ShouldRefuseFoodUntilCritical(survivor, survivor.Needs.Hunger / 100f))
                return true;

            // #289 Microbiologist: 20% chance to refuse rations as tainted.
            if (quests.RollRefuseRationsAsTainted(survivor, context.Random))
            {
                // Spikes own hunger by refusing — leave hunger high.
                if (context.NeedsSystem != null)
                    context.NeedsSystem.Modify(survivor, NeedKind.Hunger, 15f);
                else
                    survivor.Needs.Hunger = Mathf.Min(100f, survivor.Needs.Hunger + 15f);
                return true;
            }

            // #272 Prepper: only eats own pre-war MREs until stash empty. An empty
            // stash falls through to the shared food below.
            return quests.WillOnlyEatOwnMres(survivor)
                && quests.TryConsumePrepperMre(survivor);
        }

        /// <summary>
        /// Draw the survivor's rations from the inventory. Returns false when not
        /// even the first unit was available, which aborts the meal entirely.
        /// </summary>
        private static bool ConsumeRations(AIContext context, Survivor survivor, ItemDefinition food)
        {
            // #256 Selfish: consumes 2× normal rations.
            int units = 1;
            if (context.PersonalQuests != null
                && context.PersonalQuests.HasSelfish(survivor))
                units = Mathf.Max(1, Mathf.RoundToInt(
                    context.PersonalQuests.GetRationConsumptionMultiplier(survivor)));

            for (int u = 0; u < units; u++)
            {
                if (context.Inventory.Consume(food, survivor, context.RadiationSystem, null))
                    continue;

                // Missed full double-ration → morale hit for Selfish.
                if (u == 0) return false;
                if (context.PersonalQuests != null)
                {
                    float hit = context.PersonalQuests.GetSelfishMissedRationMoraleHit(survivor);
                    if (hit > 0f)
                        if (context.NeedsSystem != null)
                            context.NeedsSystem.Modify(survivor, NeedKind.Morale, -(hit));
                        else
                            survivor.Needs.Morale = Mathf.Max(0f, survivor.Needs.Morale - hit);
                }
                break;
            }
            return true;
        }

        /// <summary>#274 Animalistic: anything that is not raw meat gets spat out.</summary>
        private static bool SpitsOutNonMeat(AIContext context, Survivor survivor, ItemDefinition food)
        {
            return context.PersonalQuests != null
                && context.PersonalQuests.EatsOnlyRawMeat(survivor)
                && food.type != ItemType.Food
                && food.id != null
                && food.id.IndexOf("meat", System.StringComparison.OrdinalIgnoreCase) < 0;
        }

        /// <summary>
        /// Hunger, morale and health from the ration. Consume above is passed a null
        /// NeedsSystem so hungerRestore is applied here instead, keeping one code path.
        /// </summary>
        private static void ApplyFoodEffects(AIContext context, Survivor survivor, ItemDefinition food)
        {
            float restore = food.hungerRestore > 0f ? food.hungerRestore : 40f;
            if (context.NeedsSystem != null)
                context.NeedsSystem.Modify(survivor, NeedKind.Hunger, -(restore));
            else
                survivor.Needs.Hunger = Mathf.Max(0f, survivor.Needs.Hunger - restore);
            if (food.moraleEffect != 0f)
                if (context.NeedsSystem != null)
                    context.NeedsSystem.Modify(survivor, NeedKind.Morale, food.moraleEffect);
                else
                    survivor.Needs.Morale = Mathf.Clamp(survivor.Needs.Morale + food.moraleEffect, 0f, 100f);
            if (food.healthEffect == 0f) return;

            if (context.NeedsSystem != null)
                context.NeedsSystem.AdjustHealth(survivor, food.healthEffect);
            else
                SurvivorNeedWrite.AdjustHealth(survivor, food.healthEffect);
        }

        /// <summary>#273 Outcast room-meal morale hit on others sharing the room.</summary>
        private static void ApplyOutcastRoomMealMorale(AIContext context, Survivor survivor)
        {
            if (context.PersonalQuests == null || context.GetSurvivors == null) return;
            var all = context.GetSurvivors();
            if (all == null) return;

            for (int oi = 0; oi < all.Count; oi++)
            {
                var other = all[oi];
                if (other == null || !other.IsAlive) continue;
                context.PersonalQuests.ApplyOutcastRoomMealMorale(other, survivor);
            }
        }

        /// <summary>
        /// ContaminatedFood / spoiled meat → Phase-1 gastric illness roll.
        /// Prompt #190 — Iron Stomach multiplies chance by 0.10.
        /// </summary>
        private static void RollGastricIllness(AIContext context, Survivor survivor, ItemDefinition food)
        {
            if (context.MedicalSystem == null) return;
            bool suspect = food.type == ItemType.ContaminatedFood
                || string.Equals(food.id, "spoiled_meat", System.StringComparison.OrdinalIgnoreCase)
                || string.Equals(food.id, SurvivalPerkSystem.SpoiledMeatId,
                    System.StringComparison.OrdinalIgnoreCase);
            if (!suspect) return;

            float chance = ContaminatedBotulismChance;
            if (context.SurvivalPerks != null)
                chance = context.SurvivalPerks.ScaleIllnessChance(survivor, chance);
            double roll = (context.Random ?? FallbackRng).NextDouble();
            if (roll < chance)
                context.MedicalSystem.Inflict(survivor, AfflictionSO.Ids.Botulism);
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
