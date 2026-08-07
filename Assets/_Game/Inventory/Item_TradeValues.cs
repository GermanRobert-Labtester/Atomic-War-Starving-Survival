using System;

namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// Logical barter ladder. Numeric <see cref="BaseValue"/> lives in code only —
    /// player-facing UI must use <see cref="FormatWorthLabel"/> (no digits).
    /// Highest: weapons / self-defense. Lowest: scrap crafting materials.
    /// Shortage / demand multiplies base (see <see cref="Resolve"/>).
    /// Lives in Inventory so Data/Economy/Core can all reference it without cycles.
    /// </summary>
    public enum ItemTradeTier
    {
        /// <summary>Broken junk, sawdust, empty casings — floor of the market.</summary>
        Scrap = 0,
        /// <summary>Bulk craft feedstock: wood, nails, cloth, coal.</summary>
        BulkMaterial = 1,
        /// <summary>Cooking / chem ingredients: flour, sugar, yeast, herbs.</summary>
        Ingredient = 2,
        /// <summary>Ready food, water bottles, smokes, small consumables.</summary>
        Consumable = 3,
        /// <summary>Common tools: hammer, can opener, flashlight.</summary>
        UtilityTool = 4,
        /// <summary>Survival devices: filters, dosimeter, traps, heaters (basic).</summary>
        SurvivalGear = 5,
        /// <summary>Medicine and trauma gear.</summary>
        Medical = 6,
        /// <summary>Bunker stations / benches (scale by craft level).</summary>
        Station = 7,
        /// <summary>Body armour, helmets, coats.</summary>
        Protective = 8,
        /// <summary>Weapon attachments (rare loose).</summary>
        Attachment = 9,
        /// <summary>Firearms, grenades, flashbangs — top of the ladder.</summary>
        Weapon = 10,
        /// <summary>Gems / luxury trade goods — high but below armed kit.</summary>
        Precious = 11,
        /// <summary>Quest / plot items — not bartered for scrap.</summary>
        Quest = 12
    }

    /// <summary>
    /// Base trade values + shortage-aware resolution. Numbers never surface in UI;
    /// use worth labels. Integrates with DynamicEconomy demand multipliers.
    /// </summary>
    public static class Item_TradeValues
    {
        // ── Tier base values (code-only numerics) ─────────────────────────
        public const float BaseScrap = 0.4f;
        public const float BaseBulkMaterial = 1.2f;
        public const float BaseIngredient = 2.5f;
        public const float BaseConsumable = 4.5f;
        public const float BaseUtilityTool = 8f;
        public const float BaseSurvivalGear = 18f;
        public const float BaseMedical = 24f;
        public const float BaseStation = 35f;
        public const float BaseProtective = 40f;
        public const float BaseAttachment = 55f;
        public const float BaseWeapon = 100f;
        public const float BasePrecious = 38f;
        public const float BaseQuest = 0f;

        /// <summary>When supplies are short, survival categories spike harder than scrap.</summary>
        public const float ShortageScrapMul = 1.05f;
        public const float ShortageBulkMul = 1.15f;
        public const float ShortageIngredientMul = 1.45f;
        public const float ShortageConsumableMul = 1.70f;
        public const float ShortageUtilityMul = 1.25f;
        public const float ShortageSurvivalMul = 1.80f;
        public const float ShortageMedicalMul = 2.10f;
        public const float ShortageStationMul = 1.20f;
        public const float ShortageProtectiveMul = 1.55f;
        public const float ShortageAttachmentMul = 1.40f;
        public const float ShortageWeaponMul = 1.90f;
        public const float ShortagePreciousMul = 0.85f; // gems soften when people need food/guns
        public const float ShortageQuestMul = 1f;

        public static float BaseForTier(ItemTradeTier tier)
        {
            switch (tier)
            {
                case ItemTradeTier.Scrap: return BaseScrap;
                case ItemTradeTier.BulkMaterial: return BaseBulkMaterial;
                case ItemTradeTier.Ingredient: return BaseIngredient;
                case ItemTradeTier.Consumable: return BaseConsumable;
                case ItemTradeTier.UtilityTool: return BaseUtilityTool;
                case ItemTradeTier.SurvivalGear: return BaseSurvivalGear;
                case ItemTradeTier.Medical: return BaseMedical;
                case ItemTradeTier.Station: return BaseStation;
                case ItemTradeTier.Protective: return BaseProtective;
                case ItemTradeTier.Attachment: return BaseAttachment;
                case ItemTradeTier.Weapon: return BaseWeapon;
                case ItemTradeTier.Precious: return BasePrecious;
                case ItemTradeTier.Quest: return BaseQuest;
                default: return BaseBulkMaterial;
            }
        }

        public static float ShortageMultiplier(ItemTradeTier tier)
        {
            switch (tier)
            {
                case ItemTradeTier.Scrap: return ShortageScrapMul;
                case ItemTradeTier.BulkMaterial: return ShortageBulkMul;
                case ItemTradeTier.Ingredient: return ShortageIngredientMul;
                case ItemTradeTier.Consumable: return ShortageConsumableMul;
                case ItemTradeTier.UtilityTool: return ShortageUtilityMul;
                case ItemTradeTier.SurvivalGear: return ShortageSurvivalMul;
                case ItemTradeTier.Medical: return ShortageMedicalMul;
                case ItemTradeTier.Station: return ShortageStationMul;
                case ItemTradeTier.Protective: return ShortageProtectiveMul;
                case ItemTradeTier.Attachment: return ShortageAttachmentMul;
                case ItemTradeTier.Weapon: return ShortageWeaponMul;
                case ItemTradeTier.Precious: return ShortagePreciousMul;
                case ItemTradeTier.Quest: return ShortageQuestMul;
                default: return 1.2f;
            }
        }

        /// <summary>
        /// Infer tier from item type when catalog has no explicit tier.
        /// Weapons / self-defense top the ladder; materials sit at the bottom.
        /// </summary>
        public static ItemTradeTier InferTier(ItemType type, bool militaryGrade = false, bool extremelyRare = false)
        {
            switch (type)
            {
                case ItemType.Weapon: return ItemTradeTier.Weapon;
                case ItemType.Protective: return ItemTradeTier.Protective;
                case ItemType.Medical:
                case ItemType.AntiRad:
                case ItemType.Iodine:
                    return ItemTradeTier.Medical;
                case ItemType.Food:
                case ItemType.Water:
                    return ItemTradeTier.Consumable;
                case ItemType.ContaminatedFood:
                case ItemType.IrradiatedWater:
                    return ItemTradeTier.Scrap;
                case ItemType.Fuel: return ItemTradeTier.Ingredient;
                case ItemType.Filter:
                case ItemType.Device:
                    return ItemTradeTier.SurvivalGear;
                case ItemType.Tool:
                    return extremelyRare ? ItemTradeTier.Attachment : ItemTradeTier.UtilityTool;
                case ItemType.Trade: return ItemTradeTier.Precious;
                case ItemType.Comfort: return ItemTradeTier.Consumable;
                case ItemType.Quest: return ItemTradeTier.Quest;
                case ItemType.Material:
                default:
                    return ItemTradeTier.BulkMaterial;
            }
        }

        /// <summary>
        /// Resolve numeric trade value (code / economy only).
        /// <paramref name="demandMultiplier"/> is global supply pressure (1 = neutral, &gt;1 scarce).
        /// <paramref name="suppliesShort"/> applies category shortage spikes (food/meds/guns up, gems soft).
        /// </summary>
        public static float Resolve(
            float baseTradeValue,
            ItemTradeTier tier,
            float demandMultiplier = 1f,
            bool suppliesShort = false)
        {
            if (tier == ItemTradeTier.Quest) return 0f;
            float v = baseTradeValue > 0f ? baseTradeValue : BaseForTier(tier);
            if (demandMultiplier < 0.1f) demandMultiplier = 0.1f;
            v *= demandMultiplier;
            if (suppliesShort)
                v *= ShortageMultiplier(tier);
            return Math.Max(0f, v);
        }

        /// <summary>
        /// Resolve from an <see cref="ItemDefinition"/> using its tradeValue + inferred tier.
        /// Prefer catalog-aware resolution via world catalog when available at call site.
        /// </summary>
        public static float ResolveFromItem(
            ItemDefinition item,
            float demandMultiplier = 1f,
            bool suppliesShort = false,
            ItemTradeTier? explicitTier = null)
        {
            if (item == null) return 0f;
            var tier = explicitTier ?? InferTier(item.type);
            float bas = item.tradeValue > 0f ? item.tradeValue : BaseForTier(tier);
            return Resolve(bas, tier, demandMultiplier, suppliesShort);
        }

        /// <summary>
        /// Player-facing worth label — never includes digits.
        /// Maps resolved numeric value into cold barter language.
        /// </summary>
        public static string FormatWorthLabel(float resolvedValue)
        {
            if (resolvedValue <= 0.01f) return "worthless";
            if (resolvedValue < 1.5f) return "scrap";
            if (resolvedValue < 5f) return "common";
            if (resolvedValue < 15f) return "useful";
            if (resolvedValue < 35f) return "scarce";
            if (resolvedValue < 70f) return "valuable";
            if (resolvedValue < 110f) return "rare";
            return "priceless";
        }

        /// <summary>Station level bump: basic → intermediate → advanced → professional.</summary>
        public static float StationBase(int level)
        {
            // level 0 improvised, 1 basic, 2 intermediate, 3 advanced, 4 professional/tactical
            switch (Math.Max(0, level))
            {
                case 0: return 18f;
                case 1: return 28f;
                case 2: return 45f;
                case 3: return 70f;
                default: return 110f;
            }
        }

        /// <summary>Scale fill-state trade value by remaining fraction (empty ≈ container only).</summary>
        public static float ScaleByFill(float fullValue, float fill, float capacity, float emptyFloor = 0.15f)
        {
            if (capacity <= 0.01f) return fullValue;
            float frac = Math.Max(0f, Math.Min(1f, fill / capacity));
            return fullValue * (emptyFloor + (1f - emptyFloor) * frac);
        }
    }
}
