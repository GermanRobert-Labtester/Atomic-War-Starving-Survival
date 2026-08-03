using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Shelter
{
    [System.Serializable]
    public class ModuleUpgradeCost
    {
        public ItemDefinition Item;
        public int Amount;
    }

    [System.Serializable]
    public class ModuleLevelRequirement
    {
        public int TargetLevel;
        public List<ModuleUpgradeCost> Materials = new List<ModuleUpgradeCost>();
    }

    public abstract class ShelterModule : ScriptableObject
    {
        [Header("Module Identification")]
        public string ModuleId;
        public string DisplayName;
        [TextArea(2, 5)] public string Description;

        [Header("Upgrade Settings")]
        public int MaxLevel = 5;
        public List<ModuleLevelRequirement> UpgradeRequirements = new List<ModuleLevelRequirement>();

        [Header("EMP")]
        /// <summary>Survives an EMP event (Faraday-caged / hardened installation).</summary>
        public bool EmpShielded;

        public virtual List<ModuleUpgradeCost> GetUpgradeCosts(int targetLevel)
        {
            if (UpgradeRequirements != null)
            {
                for (int i = 0; i < UpgradeRequirements.Count; i++)
                {
                    if (UpgradeRequirements[i] != null && UpgradeRequirements[i].TargetLevel == targetLevel)
                    {
                        return UpgradeRequirements[i].Materials;
                    }
                }
            }
            return new List<ModuleUpgradeCost>();
        }
    }
}
