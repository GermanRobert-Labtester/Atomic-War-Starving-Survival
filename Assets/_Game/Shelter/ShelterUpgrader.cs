using System;
using System.Collections.Generic;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Upgrades shelter modules by consuming required materials from inventory.
    /// </summary>
    public class ShelterUpgrader
    {
        public event Action<ShelterModuleInstance, int> OnModuleUpgraded;

        public bool CanUpgrade(ShelterModuleInstance module, Inventory.Inventory inventory)
        {
            if (module == null || inventory == null) return false;
            if (module.Definition != null && module.Level >= module.Definition.MaxLevel) return false;

            int targetLevel = module.Level + 1;
            if (module.Definition != null)
            {
                List<ModuleUpgradeCost> costs = module.Definition.GetUpgradeCosts(targetLevel);
                if (costs != null && costs.Count > 0)
                {
                    for (int i = 0; i < costs.Count; i++)
                    {
                        var cost = costs[i];
                        if (cost != null && cost.Item != null)
                        {
                            if (inventory.Count(cost.Item) < cost.Amount)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool TryUpgrade(ShelterModuleInstance module, Inventory.Inventory inventory, Shelter shelter = null)
        {
            if (!CanUpgrade(module, inventory)) return false;

            int targetLevel = module.Level + 1;
            if (module.Definition != null)
            {
                List<ModuleUpgradeCost> costs = module.Definition.GetUpgradeCosts(targetLevel);
                if (costs != null)
                {
                    for (int i = 0; i < costs.Count; i++)
                    {
                        var cost = costs[i];
                        if (cost != null && cost.Item != null && cost.Amount > 0)
                        {
                            inventory.Remove(cost.Item, cost.Amount);
                        }
                    }
                }
            }

            module.Level = targetLevel;
            shelter?.NotifyModuleUpgraded(module, targetLevel);
            OnModuleUpgraded?.Invoke(module, targetLevel);
            return true;
        }
    }
}
