using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Phase-modulated barter multipliers. Pre-Day 30 (CivilWar): food is dear,
    /// anti-rad/iodine are cheap. From Flashpoint day 30 onward: conventional Trade
    /// currency/jewelry collapses to 0, anti-rad and iodine spike 10×, clean water
    /// is the gold standard. Supply/demand and faction trust live on DynamicEconomySystem.
    /// Base ladder lives on <see cref="Item_TradeValues"/> / world catalog (code-only numerics).
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
        /// Base trade value: ItemDefinition.tradeValue, else logical tier defaults.
        /// Numbers stay in code — UI must use <see cref="Item_TradeValues.FormatWorthLabel"/>.
        /// </summary>
        public static float GetBaseTradeValue(ItemDefinition item)
        {
            if (item == null) return 0f;
            if (item.tradeValue > 0f) return item.tradeValue;

            // Fall back to the code-only barter ladder. An explicitly authored
            // tradeTier wins over inference: this path previously went straight to
            // InferTier(item.type), so an item that declared a tier but no explicit
            // tradeValue was silently priced at the wrong rung of the ladder — an
            // Attachment-tier item, for instance, resolved as a UtilityTool.
            // Scrap is tier 0, i.e. "unset", so it is the one value that defers.
            var tier = item.tradeTier != ItemTradeTier.Scrap
                ? item.tradeTier
                : Item_TradeValues.InferTier(item.type);
            return Item_TradeValues.BaseForTier(tier);
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
