using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Canonical scrap material ids for the workbench component economy.
    /// snake_case — match items.json / ItemDefinition.Id.
    /// </summary>
    public static class ScrapMaterialIds
    {
        public const string MechanicalParts = "mechanical_parts";
        public const string ElectronicScrap = "electronic_scrap";
        public const string Chemicals = "chemicals";
    }

    /// <summary>One line of scrap: material id + amount.</summary>
    [Serializable]
    public class ScrapYield
    {
        public string materialId;
        public int amount = 1;

        public ScrapYield() { }

        public ScrapYield(string materialId, int amount)
        {
            this.materialId = materialId;
            this.amount = MathfCompat.Max(0, amount);
        }

        public ScrapYield Clone() => new ScrapYield(materialId, amount);
    }

    /// <summary>Cost to restore durability / repair a hard-broken device at the workbench.</summary>
    [Serializable]
    public class RepairRecipe
    {
        public List<ScrapYield> costs = new List<ScrapYield>();
        public float hours = 0.5f;
        public bool requiresTools = true;

        public RepairRecipe Clone()
        {
            var copy = new RepairRecipe
            {
                hours = hours,
                requiresTools = requiresTools,
                costs = new List<ScrapYield>()
            };
            if (costs != null)
            {
                for (int i = 0; i < costs.Count; i++)
                {
                    if (costs[i] != null)
                        copy.costs.Add(costs[i].Clone());
                }
            }
            return copy;
        }
    }

    /// <summary>
    /// Fully engine-agnostic item definition (port of Unity's ItemDefinition
    /// ScriptableObject). All fields plain public for cross-host serialization.
    /// </summary>
    public class ItemDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string description = string.Empty;
        public string iconPath = string.Empty;

        public ItemType type;
        public int stackMax = 1;
        public float weight;

        public float radProtection;
        public float durability;
        public float degradeRate;
        public bool isEquipable;
        public EquipSlot equipSlot;

        public float contamination;

        public float hungerRestore;
        public float thirstRestore;
        public float healthEffect;
        public float radCleanse;
        public float moraleEffect;

        /// <summary>
        /// Additive morale granted once per day to an alive survivor assigned to
        /// the room where this item is mounted. A value of zero means the item
        /// is not a shelter-decor morale source. The JSON field is deliberately
        /// on the item definition so the item catalog remains the only data
        /// authority for decor modifiers.
        /// </summary>
        public float decorLocalizedMoraleDelta;

        public bool empShielded;
        public float tradeValue;
        public int tradeTier;

        public List<ScrapYield> scrapValue = new List<ScrapYield>();
        public RepairRecipe repairRecipe = new RepairRecipe();
        public float disassembleYieldFraction = 0.5f;

        public bool IsConsumableOrScrapMaterial()
        {
            if (IsScrapMaterialId(id)) return true;
            switch (type)
            {
                case ItemType.Food:
                case ItemType.ContaminatedFood:
                case ItemType.Water:
                case ItemType.IrradiatedWater:
                case ItemType.Medical:
                case ItemType.AntiRad:
                case ItemType.Iodine:
                case ItemType.Comfort:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsConsumable()
        {
            if (hungerRestore != 0f || thirstRestore != 0f || healthEffect != 0f || radCleanse > 0f || moraleEffect != 0f || contamination > 0f)
                return true;
            switch (type)
            {
                case ItemType.Food:
                case ItemType.ContaminatedFood:
                case ItemType.Water:
                case ItemType.IrradiatedWater:
                case ItemType.Medical:
                case ItemType.AntiRad:
                case ItemType.Iodine:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsScrapMaterialId(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            return itemId == ScrapMaterialIds.MechanicalParts
                || itemId == ScrapMaterialIds.ElectronicScrap
                || itemId == ScrapMaterialIds.Chemicals;
        }

        public bool CanDisassemble => !IsConsumableOrScrapMaterial();

        /// <summary>
        /// Effective degradation rate (durability loss per hour of active radiation exposure).
        /// If explicitly authored (> 0), uses authored degradeRate.
        /// Otherwise, derives sensible default based on gear category and slot for equipable protective gear.
        /// Returns 0 for non-protective or zero-durability items (intentional zero-rate).
        /// </summary>
        public float GetEffectiveDegradeRate()
        {
            if (degradeRate > 0f)
                return Math.Clamp(degradeRate, 0f, 100f);

            if (!isEquipable || radProtection <= 0f || durability <= 0f)
                return 0f;

            // Derived baseline by gear category/slot:
            // Face filters (gas masks, respirators) wear faster under airborne particulate (1.0 durability/hr).
            // Heavy body suits (hazmat, lead aprons) wear at 0.5 durability/hr.
            // Minor protective gear wears at 0.25 durability/hr.
            if (equipSlot == EquipSlot.Face)
                return 1.0f;
            if (equipSlot == EquipSlot.Body)
                return 0.5f;

            return 0.25f;
        }

        /// <summary>Deep copy for host use.</summary>
        public ItemDefinition Clone()
        {
            var copy = new ItemDefinition
            {
                id = id,
                displayName = displayName,
                description = description,
                iconPath = iconPath,
                type = type,
                stackMax = stackMax,
                weight = weight,
                radProtection = radProtection,
                durability = durability,
                degradeRate = degradeRate,
                isEquipable = isEquipable,
                equipSlot = equipSlot,
                contamination = contamination,
                hungerRestore = hungerRestore,
                thirstRestore = thirstRestore,
                healthEffect = healthEffect,
                radCleanse = radCleanse,
                moraleEffect = moraleEffect,
                decorLocalizedMoraleDelta = decorLocalizedMoraleDelta,
                empShielded = empShielded,
                tradeValue = tradeValue,
                tradeTier = tradeTier,
                disassembleYieldFraction = disassembleYieldFraction,
                scrapValue = new List<ScrapYield>(),
                repairRecipe = repairRecipe != null ? repairRecipe.Clone() : new RepairRecipe()
            };
            if (scrapValue != null)
            {
                for (int i = 0; i < scrapValue.Count; i++)
                    if (scrapValue[i] != null) copy.scrapValue.Add(scrapValue[i].Clone());
            }
            return copy;
        }
    }

    /// <summary>Single source of truth for equipSlot string parsing (port of Unity EquipSlots).</summary>
    public static class EquipSlots
    {
        private static readonly (string Alias, EquipSlot Slot)[] LegacyAliases =
        {
            ("torso", EquipSlot.Body),
            ("chest", EquipSlot.Body),
        };

        private static readonly string[] s_canonicalNames = Enum.GetNames(typeof(EquipSlot));

        public static string[] CanonicalNames => s_canonicalNames;

        public static bool TryParse(string raw, out EquipSlot slot)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                slot = EquipSlot.None;
                return true;
            }
            string key = raw.Trim();
            if (Enum.TryParse(key, true, out slot))
                return true;
            for (int i = 0; i < LegacyAliases.Length; i++)
            {
                if (string.Equals(key, LegacyAliases[i].Alias, StringComparison.OrdinalIgnoreCase))
                {
                    slot = LegacyAliases[i].Slot;
                    return true;
                }
            }
            slot = EquipSlot.None;
            return false;
        }

        public static EquipSlot Parse(string raw, EquipSlot fallback = EquipSlot.None) =>
            TryParse(raw, out var slot) ? slot : fallback;

        public static bool IsCanonicalName(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return true;
            return Enum.TryParse<EquipSlot>(raw.Trim(), true, out _);
        }

        public static string? CanonicalNameForAlias(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;
            string key = raw.Trim();
            for (int i = 0; i < LegacyAliases.Length; i++)
            {
                if (string.Equals(key, LegacyAliases[i].Alias, StringComparison.OrdinalIgnoreCase))
                    return LegacyAliases[i].Slot.ToString();
            }
            return null;
        }
    }
}
