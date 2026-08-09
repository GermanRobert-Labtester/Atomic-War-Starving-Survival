using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    public partial class HatchDefenseSystem
    {
        private const int HatchUpgradeHardMaxLevel = 5;
        private const string HatchUpgradeScrapId = "scrap_metal";
        private const string HatchUpgradeMechanicalId = "mechanical_parts";
        private const string HatchUpgradeMechanicalFallbackId = "mechanical_components";

        private sealed class HatchUpgradePlan
        {
            public string ModuleId;
            public Inventory.Inventory Inventory;
            public Shelter Shelter;
            public ShelterModuleInstance Existing;
            public int TargetLevel;
            public int MaxLevel;
            public ItemDefinition Scrap;
            public int ScrapNeeded;
            public ItemDefinition Mechanical;
            public int MechanicalNeeded;
        }

        private bool TryCreateHatchUpgradePlan(
            string moduleId,
            Func<string, ItemDefinition> itemLookup,
            Inventory.Inventory inventory,
            out HatchUpgradePlan plan)
        {
            plan = null;
            if (!IsValidHatchUpgradeId(moduleId) || itemLookup == null) return false;

            var resolvedInventory = inventory ?? _getInventory?.Invoke();
            var shelter = _getShelter?.Invoke();
            if (resolvedInventory == null || shelter == null) return false;

            var existing = shelter.GetModule(moduleId);
            int targetLevel = existing != null ? existing.Level + 1 : 1;
            int maxLevel = GetHatchUpgradeMaxLevel(existing);
            if (targetLevel > maxLevel) return false;

            GetUpgradeMaterialCost(
                moduleId,
                targetLevel,
                out int scrapNeeded,
                out int mechanicalNeeded);
            if (!TryResolveHatchUpgradeMaterials(
                    itemLookup,
                    out ItemDefinition scrap,
                    out ItemDefinition mechanical)
                || !HasHatchUpgradeMaterials(
                    resolvedInventory,
                    scrap,
                    scrapNeeded,
                    mechanical,
                    mechanicalNeeded))
                return false;

            plan = new HatchUpgradePlan
            {
                ModuleId = moduleId,
                Inventory = resolvedInventory,
                Shelter = shelter,
                Existing = existing,
                TargetLevel = targetLevel,
                MaxLevel = maxLevel,
                Scrap = scrap,
                ScrapNeeded = scrapNeeded,
                Mechanical = mechanical,
                MechanicalNeeded = mechanicalNeeded
            };
            return true;
        }

        private static bool IsValidHatchUpgradeId(string moduleId)
        {
            return !string.IsNullOrEmpty(moduleId) && IsHatchModuleId(moduleId);
        }

        private static int GetHatchUpgradeMaxLevel(ShelterModuleInstance existing)
        {
            int definitionMax = existing?.Definition != null
                ? existing.Definition.MaxLevel
                : HatchUpgradeHardMaxLevel;
            return Mathf.Min(definitionMax, HatchUpgradeHardMaxLevel);
        }

        private static bool TryResolveHatchUpgradeMaterials(
            Func<string, ItemDefinition> itemLookup,
            out ItemDefinition scrap,
            out ItemDefinition mechanical)
        {
            scrap = itemLookup(HatchUpgradeScrapId);
            mechanical = itemLookup(HatchUpgradeMechanicalId)
                ?? itemLookup(HatchUpgradeMechanicalFallbackId);
            return scrap != null && mechanical != null;
        }

        private static bool HasHatchUpgradeMaterials(
            Inventory.Inventory inventory,
            ItemDefinition scrap,
            int scrapNeeded,
            ItemDefinition mechanical,
            int mechanicalNeeded)
        {
            if (AreSameInventoryItem(scrap, mechanical))
                return inventory.Count(scrap) >= scrapNeeded + mechanicalNeeded;

            return inventory.Count(scrap) >= scrapNeeded
                && inventory.Count(mechanical) >= mechanicalNeeded;
        }

        private static bool AreSameInventoryItem(
            ItemDefinition first,
            ItemDefinition second)
        {
            return string.Equals(first?.id, second?.id, StringComparison.Ordinal);
        }

        private static bool TryConsumeHatchUpgradeMaterials(HatchUpgradePlan plan)
        {
            if (!HasHatchUpgradeMaterials(
                    plan.Inventory,
                    plan.Scrap,
                    plan.ScrapNeeded,
                    plan.Mechanical,
                    plan.MechanicalNeeded))
                return false;

            if (AreSameInventoryItem(plan.Scrap, plan.Mechanical))
            {
                return plan.Inventory.Remove(
                    plan.Scrap,
                    plan.ScrapNeeded + plan.MechanicalNeeded);
            }

            if (!plan.Inventory.Remove(plan.Scrap, plan.ScrapNeeded)) return false;
            if (plan.Inventory.Remove(plan.Mechanical, plan.MechanicalNeeded)) return true;

            if (!plan.Inventory.Add(plan.Scrap, plan.ScrapNeeded))
                Debug.LogError("[HatchDefense] Failed to roll back an interrupted hatch upgrade.");
            return false;
        }

        private static void ApplyHatchUpgrade(HatchUpgradePlan plan)
        {
            if (plan.Existing != null)
            {
                InitializeHatchUpgradeModule(
                    plan.Existing,
                    plan.ModuleId,
                    plan.TargetLevel);
                return;
            }

            var installed = new ShelterModuleInstance(plan.ModuleId, plan.TargetLevel)
            {
                SecurityContribution = DefaultSecurityForModuleId(plan.ModuleId),
                FilterHealth = 100f,
                IsEnabled = true,
                RoomId = "entry"
            };
            plan.Shelter.AddModule(installed);
        }

        private static void InitializeHatchUpgradeModule(
            ShelterModuleInstance module,
            string moduleId,
            int targetLevel)
        {
            module.Level = targetLevel;
            if (module.SecurityContribution <= 0f)
                module.SecurityContribution = DefaultSecurityForModuleId(moduleId);
            module.FilterHealth = 100f;
            module.IsEnabled = true;
        }

        private void NotifyIronGateHatchUpgrade(int targetLevel, int maxLevel)
        {
            if (_personalQuests == null) return;

            IReadOnlyList<Survivor> crew = _getSurvivors?.Invoke();
            if (crew == null) return;

            int day = _getDay?.Invoke() ?? 0;
            for (int i = 0; i < crew.Count; i++)
            {
                Survivor survivor = crew[i];
                if (!IsIronGateUpgradeCandidate(survivor)) continue;

                _personalQuests.NotifyHatchUpgradeInstalled(
                    survivor,
                    targetLevel,
                    maxLevel,
                    day);
            }
        }

        private bool IsIronGateUpgradeCandidate(Survivor survivor)
        {
            if (survivor == null || !survivor.IsAlive) return false;

            return string.Equals(
                    survivor.ArchetypeId,
                    PersonalQuestSystem.WelderId,
                    StringComparison.Ordinal)
                || _personalQuests.HasCalloused(survivor)
                || _personalQuests.HasForgeMaster(survivor);
        }
    }
}
