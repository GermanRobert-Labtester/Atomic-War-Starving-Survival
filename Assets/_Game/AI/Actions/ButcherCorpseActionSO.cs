using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action that drives a survivor to break down a stored corpse into
    /// bones and meat (Prompt #192 — CorpseManagementSystem.ProcessForParts).
    /// Scores when a corpse is in storage AND the shared food stock is low;
    /// a grim last resort, priced below Cook's basePriority to reflect the
    /// morale cost. The actual process (yields, perk rolls) is delegated to
    /// CorpseManagementSystem via AIContext.OnRequestButcherCorpse, since the
    /// AI assembly cannot reference Core types.
    /// </summary>
    [CreateAssetMenu(fileName = "NewButcherCorpseAction", menuName = "ASHFALL/AI Actions/Butcher Corpse")]
    public class ButcherCorpseActionSO : SurvivorAction
    {
        private const int LowFoodThreshold = 3;

        public ButcherCorpseActionSO()
        {
            id = "action_butcher_corpse";
            displayName = "Butcher the Dead";
            description = "Break down a stored body for bones and meat.";
            basePriority = 0.25f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (!HasLivingSurvivor(context) || context.CorpseCount <= 0) return 0f;
            int foodUnits = CountFoodUnits(context);
            if (foodUnits >= LowFoodThreshold) return 0f;
            return Mathf.Clamp01(1f - (float)foodUnits / LowFoodThreshold) * 0.5f;
        }

        private static int CountFoodUnits(AIContext context)
        {
            if (context.Inventory?.Slots == null) return 0;
            int total = 0;
            for (int i = 0; i < context.Inventory.Slots.Count; i++)
            {
                var slot = context.Inventory.Slots[i];
                if (slot?.Item != null && slot.Item.type == ItemType.Food)
                    total += slot.Amount;
            }
            return total;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null) return;
            context.OnRequestButcherCorpse?.Invoke(context.Survivor);
        }
    }
}
