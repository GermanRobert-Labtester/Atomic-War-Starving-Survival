using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// ScriptableObject definition of a single item type. Authored as a .asset or
    /// imported from StreamingAssets/Data/items.json. Covers identity, stacking,
    /// weight, protective/equipment stats, contamination, and on-consume effects.
    /// </summary>
    [CreateAssetMenu(fileName = "NewItemDefinition", menuName = "ASHFALL/Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string id;
        public string displayName;
        [TextArea(2, 5)] public string description;
        public Sprite iconRef;

        [Header("Classification")]
        public ItemType type;
        public int stackMax = 1;
        public float weight;

        [Header("Protection / Equipment")]
        /// <summary>Dose-rate blocked while equipped/intact (same units as RadZoneProfile.radLevel).</summary>
        public float radProtection;
        /// <summary>Maximum durability; protection scales with remaining durability.</summary>
        public float durability;
        public bool isEquipable;
        public EquipSlot equipSlot;

        [Header("Contamination")]
        /// <summary>0..1 contamination level; ingesting contaminated items adds dose.</summary>
        [Range(0f, 1f)] public float contamination;

        [Header("Effects (applied on consume)")]
        public float hungerRestore;
        public float thirstRestore;
        public float healthEffect;
        /// <summary>Current radiation dose cleansed on consume (anti-rad).</summary>
        public float radCleanse;
        public float moraleEffect;

        [Header("EMP / Economy")]
        /// <summary>Survives EMP (Faraday-caged / hardened gear) if a Device; unaffected otherwise.</summary>
        public bool empShielded;
        /// <summary>Base barter value; phase-modulated by TradeEconomy.GetEffectiveValue.</summary>
        public float tradeValue;

        [Header("Workbench / Scrap")]
        /// <summary>
        /// Base materials recovered when this item is disassembled (before yield %).
        /// Empty → workbench uses type defaults.
        /// </summary>
        public List<ScrapYield> scrapValue = new List<ScrapYield>();

        /// <summary>Cost to restore durability / repair a hard-broken instance.</summary>
        public RepairRecipe repairRecipe = new RepairRecipe();

        /// <summary>
        /// Fraction of ScrapValue yielded on disassembly (0..1). Default 50%.
        /// </summary>
        [Range(0f, 1f)] public float disassembleYieldFraction = 0.5f;

        /// <summary>
        /// True for food/water/meds and pure scrap materials that cannot be broken down further.
        /// </summary>
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

        public static bool IsScrapMaterialId(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            return itemId == ScrapMaterialIds.MechanicalParts
                || itemId == ScrapMaterialIds.ElectronicScrap
                || itemId == ScrapMaterialIds.Chemicals;
        }

        /// <summary>Non-consumables may be disassembled at the workbench.</summary>
        public bool CanDisassemble => !IsConsumableOrScrapMaterial();
    }
}
