using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Shelter
{
    public partial class HatchDefenseSystem
    {
        private const float LootStacksPerDeficit = 15f;
        private const float WholeStackLootDeficit = 40f;
        private const int MinimumLootStacks = 1;
        private const int MaximumLootStacks = 6;

        private sealed class BreachLootCandidate
        {
            public readonly ItemDefinition Item;
            public readonly string ItemId;
            public readonly string DisplayName;
            public readonly int SnapshotAmount;
            public readonly float Priority;
            public readonly int OriginalSlotIndex;

            public BreachLootCandidate(
                InventorySlot slot,
                float priority,
                int originalSlotIndex)
            {
                Item = slot.Item;
                ItemId = slot.Item.id;
                DisplayName = slot.Item.displayName;
                SnapshotAmount = slot.Amount;
                Priority = priority;
                OriginalSlotIndex = originalSlotIndex;
            }
        }

        private void StealLoot(RaidResolution result)
        {
            if (result == null) return;

            var inventory = _getInventory?.Invoke();
            if (inventory?.Slots == null) return;

            float deficit = Mathf.Max(1f, result.RaidStrength - result.DefenseScore);
            int stackTarget = CalculateLootStackTarget(deficit);
            var plan = BuildBreachLootPlan(inventory);

            int stolenStacks = 0;
            for (int i = 0; i < plan.Count && stolenStacks < stackTarget; i++)
            {
                if (TryStealLootCandidate(inventory, plan[i], deficit, result))
                    stolenStacks++;
            }
        }

        /// <summary>
        /// Captures value and quantity before any removal. Inventory.Remove compacts the
        /// live slot list, so the mutation phase must not retain indices into Slots.
        /// </summary>
        private static List<BreachLootCandidate> BuildBreachLootPlan(
            Inventory.Inventory inventory)
        {
            var plan = new List<BreachLootCandidate>();
            for (int i = 0; i < inventory.Slots.Count; i++)
            {
                var slot = inventory.Slots[i];
                if (slot?.Item == null || slot.Amount <= 0) continue;
                if (slot.Item.type == ItemType.Quest) continue;

                plan.Add(new BreachLootCandidate(slot, StealPriority(slot.Item), i));
            }

            plan.Sort(CompareBreachLootCandidates);
            return plan;
        }

        private static int CompareBreachLootCandidates(
            BreachLootCandidate left,
            BreachLootCandidate right)
        {
            int priorityComparison = right.Priority.CompareTo(left.Priority);
            return priorityComparison != 0
                ? priorityComparison
                : left.OriginalSlotIndex.CompareTo(right.OriginalSlotIndex);
        }

        private static bool TryStealLootCandidate(
            Inventory.Inventory inventory,
            BreachLootCandidate candidate,
            float deficit,
            RaidResolution result)
        {
            int currentlyAvailable = inventory.CountById(candidate.ItemId);
            int candidateAmount = Mathf.Min(candidate.SnapshotAmount, currentlyAvailable);
            int take = CalculateLootTakeAmount(candidateAmount, deficit);
            if (take <= 0 || !inventory.Remove(candidate.Item, take)) return false;

            result.StolenItems.Add(new StolenLootLine
            {
                ItemId = candidate.ItemId,
                Amount = take,
                DisplayName = candidate.DisplayName
            });
            return true;
        }

        private static int CalculateLootStackTarget(float deficit)
        {
            return Mathf.Clamp(
                Mathf.CeilToInt(deficit / LootStacksPerDeficit),
                MinimumLootStacks,
                MaximumLootStacks);
        }

        private static int CalculateLootTakeAmount(int available, float deficit)
        {
            if (available <= 0) return 0;
            if (deficit >= WholeStackLootDeficit) return available;

            return Mathf.Max(1, Mathf.Min(available, Mathf.CeilToInt(available * 0.5f)));
        }

        private static float StealPriority(ItemDefinition item)
        {
            if (item == null) return 0f;

            float priority = item.tradeValue;
            switch (item.type)
            {
                case ItemType.Food: priority += 20f; break;
                case ItemType.Water: priority += 25f; break;
                case ItemType.Medical: priority += 18f; break;
                case ItemType.Fuel: priority += 15f; break;
                case ItemType.AntiRad:
                case ItemType.Iodine: priority += 12f; break;
            }

            if (IsWeaponItem(item) || IsAmmoItem(item)) priority += 10f;
            return priority;
        }
    }
}
