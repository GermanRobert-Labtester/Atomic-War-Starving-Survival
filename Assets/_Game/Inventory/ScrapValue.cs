using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// Canonical scrap material ids for the workbench component economy (#workbench).
    /// snake_case — match items.json / ItemDefinition.id.
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
            this.amount = Mathf.Max(0, amount);
        }

        public ScrapYield Clone() => new ScrapYield(materialId, amount);
    }

    /// <summary>
    /// Cost to restore an item's durability / repair a hard-broken device at the workbench.
    /// Generated from items.json or filled with defaults by <see cref="Crafting.WorkbenchSystem"/>.
    /// </summary>
    [Serializable]
    public class RepairRecipe
    {
        public List<ScrapYield> costs = new List<ScrapYield>();
        /// <summary>Game-hours of workbench time (instant if 0 for tests/UI actions).</summary>
        public float hours = 0.5f;
        /// <summary>Requires an operational workbench (tools) when true.</summary>
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
}
