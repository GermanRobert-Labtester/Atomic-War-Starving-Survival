using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Phase-modulated barter multipliers. Pre-Day 30 (CivilWar): food is dear,
    /// anti-rad/iodine are cheap. From Flashpoint day 30 onward: conventional Trade
    /// currency/jewelry collapses to 0, anti-rad and iodine spike 10×, clean water
    /// is the gold standard. Supply/demand and faction trust live on DynamicEconomySystem.
    /// </summary>
    public static class TradeEconomy
    {
        /// <summary>Anti-rad / iodine multiplier once the exchange happens.</summary>
        public const float PostFlashpointRadMedMultiplier = 10f;

        /// <summary>Clean water becomes the settlement currency after Flashpoint.</summary>
        public const float PostFlashpointWaterMultiplier = 8f;

        /// <summary>Pre-war food scarcity premium.</summary>
        public const float PreFlashpointFoodMultiplier = 2.5f;

        /// <summary>Nobody thinks they need anti-rad yet.</summary>
        public const float PreFlashpointAntiRadMultiplier = 0.35f;

        public const float PreFlashpointIodineMultiplier = 0.4f;

        /// <summary>True for Flashpoint day and every day of Nuclear Winter.</summary>
        public static bool IsPostFlashpoint(WorldPhase phase)
        {
            return phase == WorldPhase.Flashpoint || phase == WorldPhase.NuclearWinter;
        }

        /// <summary>
        /// Base trade value when ItemDefinition.tradeValue is unset (0). Keeps tests and
        /// unimported JSON items usable without requiring a full re-import.
        /// </summary>
        public static float GetBaseTradeValue(ItemDefinition item)
        {
            if (item == null) return 0f;
            if (item.tradeValue > 0f) return item.tradeValue;

            switch (item.type)
            {
                case ItemType.Food: return 12f;
                case ItemType.ContaminatedFood: return 1f;
                case ItemType.Water: return 15f;
                case ItemType.IrradiatedWater: return 2f;
                case ItemType.AntiRad: return 8f;
                case ItemType.Iodine: return 6f;
                case ItemType.Medical: return 10f;
                case ItemType.Trade: return 25f;
                case ItemType.Fuel: return 14f;
                case ItemType.Material: return 5f;
                case ItemType.Tool: return 18f;
                case ItemType.Protective: return 40f;
                case ItemType.Filter: return 20f;
                case ItemType.Device: return 30f;
                case ItemType.Comfort: return 8f;
                case ItemType.Corpse: return 0f;
                default: return 5f;
            }
        }

        /// <summary>Phase-only effective value (no supply/demand, no trust).</summary>
        public static float GetEffectiveValue(ItemDefinition item, WorldPhase phase)
        {
            if (item == null) return 0f;
            float baseVal = GetBaseTradeValue(item);

            if (IsPostFlashpoint(phase))
            {
                // Conventional currency / jewelry / luxury trade goods leave the pool.
                if (item.type == ItemType.Trade) return 0f;

                if (item.type == ItemType.AntiRad || item.type == ItemType.Iodine)
                    return baseVal * PostFlashpointRadMedMultiplier;

                if (item.type == ItemType.Water)
                    return baseVal * PostFlashpointWaterMultiplier;

                return baseVal;
            }

            // Pre-Day 30 (CivilWar / PreWar)
            if (item.type == ItemType.Food)
                return baseVal * PreFlashpointFoodMultiplier;
            if (item.type == ItemType.AntiRad)
                return baseVal * PreFlashpointAntiRadMultiplier;
            if (item.type == ItemType.Iodine)
                return baseVal * PreFlashpointIodineMultiplier;

            return baseVal;
        }
    }
}
