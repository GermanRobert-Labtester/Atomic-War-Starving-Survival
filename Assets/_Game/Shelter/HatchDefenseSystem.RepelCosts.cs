using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    public partial class HatchDefenseSystem
    {
        private const int DefaultAmmoSpendPriority = 50;

        private sealed class AmmoSpendCandidate
        {
            public readonly ItemDefinition Item;
            public readonly string ItemId;
            public readonly int Priority;
            public readonly int FirstSlotIndex;
            public int Available;

            public AmmoSpendCandidate(
                ItemDefinition item,
                int available,
                int priority,
                int firstSlotIndex)
            {
                Item = item;
                ItemId = item.id;
                Available = available;
                Priority = priority;
                FirstSlotIndex = firstSlotIndex;
            }
        }

        private void ApplyRepelCosts(RaidResolution result)
        {
            if (result == null) return;

            var inventory = _getInventory?.Invoke();
            if (inventory?.Slots == null) return;

            int ammoNeeded = Mathf.Clamp(Mathf.CeilToInt(result.RaidStrength / 10f), 1, 12);
            int ammoConsumed = ConsumeRaidAmmo(inventory, ammoNeeded);
            result.AmmoConsumed += ammoConsumed;

            RecordGuardAmmoExpenditure(ammoConsumed);
            ApplyRepelWeaponWear(inventory, result, ammoNeeded - ammoConsumed);
        }

        /// <summary>
        /// Takes an immutable spend plan before mutating the inventory. Inventory.Remove
        /// deletes exhausted stacks and shifts Slots, so retaining live indices is unsafe.
        /// </summary>
        private int ConsumeRaidAmmo(Inventory.Inventory inventory, int requested)
        {
            if (requested <= 0) return 0;

            int remaining = requested;
            var spendPlan = BuildAmmoSpendPlan(inventory);
            for (int i = 0; i < spendPlan.Count && remaining > 0; i++)
            {
                var candidate = spendPlan[i];
                int currentlyAvailable = inventory.CountById(candidate.ItemId);
                int take = Mathf.Min(remaining, Mathf.Min(candidate.Available, currentlyAvailable));
                if (take <= 0) continue;
                if (!inventory.Remove(candidate.Item, take)) continue;

                remaining -= take;
            }

            return requested - remaining;
        }

        private List<AmmoSpendCandidate> BuildAmmoSpendPlan(Inventory.Inventory inventory)
        {
            var plan = new List<AmmoSpendCandidate>();
            var byItemId = new Dictionary<string, AmmoSpendCandidate>(StringComparer.Ordinal);

            for (int i = 0; i < inventory.Slots.Count; i++)
            {
                var slot = inventory.Slots[i];
                if (!IsSpendableAmmo(slot)) continue;

                string itemId = slot.Item.id;
                if (byItemId.TryGetValue(itemId, out var candidate))
                {
                    candidate.Available += slot.Amount;
                    continue;
                }

                int priority = AmmoSpendPriorityResolver?.Invoke(itemId)
                    ?? DefaultAmmoSpendPriority;
                candidate = new AmmoSpendCandidate(slot.Item, slot.Amount, priority, i);
                byItemId.Add(itemId, candidate);
                plan.Add(candidate);
            }

            plan.Sort(CompareAmmoSpendCandidates);
            return plan;
        }

        private static bool IsSpendableAmmo(InventorySlot slot)
        {
            return slot?.Item != null
                && !string.IsNullOrEmpty(slot.Item.id)
                && slot.Amount > 0
                && (IsAmmoItem(slot.Item) || IsAmmoId(slot.Item.id));
        }

        private static int CompareAmmoSpendCandidates(
            AmmoSpendCandidate left,
            AmmoSpendCandidate right)
        {
            int priorityComparison = left.Priority.CompareTo(right.Priority);
            return priorityComparison != 0
                ? priorityComparison
                : left.FirstSlotIndex.CompareTo(right.FirstSlotIndex);
        }

        private void RecordGuardAmmoExpenditure(int ammoConsumed)
        {
            if (ammoConsumed <= 0 || _combatPerks == null) return;

            var survivors = _getSurvivors?.Invoke();
            if (survivors == null) return;

            int day = _getDay?.Invoke() ?? 0;
            for (int i = 0; i < survivors.Count; i++)
            {
                var survivor = survivors[i];
                if (survivor == null || !_activeGuards.ContainsKey(survivor.Id)) continue;
                _combatPerks.RecordAmmoExpended(survivor, ammoConsumed, day);
            }
        }

        private void ApplyRepelWeaponWear(
            Inventory.Inventory inventory,
            RaidResolution result,
            int ammoShortfall)
        {
            var weaponSlot = FindFirstStoredWeapon(inventory);
            if (weaponSlot == null) return;

            float wear = 8f + (ammoShortfall > 0 ? ammoShortfall * 2f : 2f);
            float maxDurability = weaponSlot.Item.durability > 0f
                ? weaponSlot.Item.durability
                : 100f;
            if (weaponSlot.CurrentDurability < 0f)
                weaponSlot.CurrentDurability = maxDurability;

            float durabilityBefore = weaponSlot.CurrentDurability;
            weaponSlot.CurrentDurability = Mathf.Max(0f, durabilityBefore - wear);
            result.WeaponDurabilityLost += durabilityBefore - weaponSlot.CurrentDurability;

            TryRecordRepelWeaponJam(weaponSlot, maxDurability);
        }

        private static InventorySlot FindFirstStoredWeapon(Inventory.Inventory inventory)
        {
            for (int i = 0; i < inventory.Slots.Count; i++)
            {
                var slot = inventory.Slots[i];
                if (slot?.Item == null) continue;
                if (IsAmmoItem(slot.Item) || IsAmmoId(slot.Item.id)) continue;
                if (!IsWeaponItem(slot.Item) && !IsWeaponId(slot.Item.id)) continue;
                if (slot.Item.id == "kevlar_vest") continue;
                return slot;
            }

            return null;
        }

        private void TryRecordRepelWeaponJam(InventorySlot weaponSlot, float maxDurability)
        {
            if (TryJamWeapon == null) return;
            if (weaponSlot.CurrentDurability >= maxDurability * 0.5f) return;

            Survivor guard = FindFirstActiveGuard();
            int clearTicks = guard != null && _combatPerks != null
                ? _combatPerks.GetJamClearTicks(guard)
                : CombatPerkSystem.DefaultJamClearTicks;

            if (!TryJamWeapon(weaponSlot.Item.id, clearTicks)) return;
            if (guard == null || _combatPerks == null) return;

            int day = _getDay?.Invoke() ?? 0;
            _combatPerks.RecordWeaponJamSurvived(guard, day);
        }

        private Survivor FindFirstActiveGuard()
        {
            var survivors = _getSurvivors?.Invoke();
            if (survivors == null) return null;

            for (int i = 0; i < survivors.Count; i++)
            {
                var survivor = survivors[i];
                if (survivor != null && _activeGuards.ContainsKey(survivor.Id))
                    return survivor;
            }

            return null;
        }
    }
}
