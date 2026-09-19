// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Inventory
{
    public enum LoreTriggerType
    {
        Crafting = 0,
        Discovery = 1,
        Combat = 2,
        Gift = 3,
        Trade = 4,
        LossRecovery = 5,
        SignificantMoment = 6
    }

    public enum SignificanceLevel
    {
        Mundane = 0,
        Notable = 1,
        Important = 2,
        Legendary = 3
    }

    [Serializable]
    public sealed class ItemLoreEntry
    {
        public string LoreId { get; set; } = string.Empty;
        public string ItemInstanceId { get; set; } = string.Empty;
        public LoreTriggerType TriggerType { get; set; } = LoreTriggerType.Crafting;
        public string Text { get; set; } = string.Empty;
        public int Day { get; set; } = 1;
        public string AssociatedSurvivorId { get; set; } = string.Empty;
        public string AssociatedLocationId { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class ItemProvenanceChain
    {
        public string ItemInstanceId { get; set; } = string.Empty;
        public string CrafterSurvivorId { get; set; } = string.Empty;
        public int CraftingDay { get; set; } = 0;
        public string DiscoveryLocationId { get; set; } = string.Empty;
        public int DiscoveryDay { get; set; } = 0;
        public string DiscoveryContext { get; set; } = string.Empty;
        public List<string> OwnershipChain { get; set; } = new List<string>();
        public List<string> LoreEntryIds { get; set; } = new List<string>();
        public SignificanceLevel Significance { get; set; } = SignificanceLevel.Mundane;
    }

    [Serializable]
    public sealed class ItemLoreState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<ItemLoreEntry> LoreEntries { get; set; } = new List<ItemLoreEntry>();
        public List<ItemProvenanceChain> Provenances { get; set; } = new List<ItemProvenanceChain>();
    }

    /// <summary>
    /// Plan 190 — Item Lore & Provenance Tracking System.
    /// Tracks individual item narrative history, ownership chains, crafting/discovery origins,
    /// combat or significant event lore, and calculates item significance levels.
    /// </summary>
    public sealed class ItemLoreSystem
    {
        private readonly ItemLoreState _state;

        public event Action<ItemLoreEntry>? OnLoreAdded;
        public event Action<ItemProvenanceChain, SignificanceLevel>? OnSignificanceChanged;
        public event Action<string, string>? OnOwnershipTransferred;

        public int TrackedItemCount => _state.Provenances.Count;
        public int TotalLoreEntriesCount => _state.LoreEntries.Count;

        public ItemLoreSystem(ItemLoreState? state = null)
        {
            _state = state ?? new ItemLoreState();
        }

        public ItemProvenanceChain RegisterItem(
            string itemInstanceId,
            string? crafterId = null,
            int craftingDay = 0,
            string? discoveryLocationId = null,
            int discoveryDay = 0,
            string? context = null)
        {
            if (string.IsNullOrWhiteSpace(itemInstanceId)) throw new ArgumentNullException(nameof(itemInstanceId));

            var existing = _state.Provenances.FirstOrDefault(p => string.Equals(p.ItemInstanceId, itemInstanceId, StringComparison.OrdinalIgnoreCase));
            if (existing != null) return existing;

            var provenance = new ItemProvenanceChain
            {
                ItemInstanceId = itemInstanceId.Trim(),
                CrafterSurvivorId = crafterId ?? string.Empty,
                CraftingDay = Math.Max(0, craftingDay),
                DiscoveryLocationId = discoveryLocationId ?? string.Empty,
                DiscoveryDay = Math.Max(0, discoveryDay),
                DiscoveryContext = context ?? string.Empty,
                Significance = SignificanceLevel.Mundane
            };

            if (!string.IsNullOrWhiteSpace(crafterId))
            {
                provenance.OwnershipChain.Add(crafterId!.Trim());
                AddLoreInternal(provenance, LoreTriggerType.Crafting, $"Crafted by {crafterId} on Day {craftingDay}.", craftingDay, crafterId);
            }
            else if (!string.IsNullOrWhiteSpace(discoveryLocationId))
            {
                AddLoreInternal(provenance, LoreTriggerType.Discovery, $"Discovered in {discoveryLocationId} on Day {discoveryDay} ({context}).", discoveryDay, locationId: discoveryLocationId);
            }

            _state.Provenances.Add(provenance);
            return provenance;
        }

        public bool TransferOwnership(string itemInstanceId, string newOwnerId, int day = 1)
        {
            if (string.IsNullOrWhiteSpace(itemInstanceId) || string.IsNullOrWhiteSpace(newOwnerId)) return false;

            var prov = _state.Provenances.FirstOrDefault(p => string.Equals(p.ItemInstanceId, itemInstanceId, StringComparison.OrdinalIgnoreCase));
            if (prov == null)
            {
                prov = RegisterItem(itemInstanceId);
            }

            string cleanOwner = newOwnerId.Trim();
            if (prov.OwnershipChain.Count == 0 || !string.Equals(prov.OwnershipChain.Last(), cleanOwner, StringComparison.OrdinalIgnoreCase))
            {
                prov.OwnershipChain.Add(cleanOwner);
                AddLoreInternal(prov, LoreTriggerType.Gift, $"Ownership transferred to {cleanOwner} on Day {day}.", day, cleanOwner);
                OnOwnershipTransferred?.Invoke(itemInstanceId, cleanOwner);
                return true;
            }

            return false;
        }

        public ItemLoreEntry AddLore(
            string itemInstanceId,
            LoreTriggerType trigger,
            string text,
            int day,
            string? survivorId = null,
            string? locationId = null)
        {
            if (string.IsNullOrWhiteSpace(itemInstanceId)) throw new ArgumentNullException(nameof(itemInstanceId));
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));

            var prov = _state.Provenances.FirstOrDefault(p => string.Equals(p.ItemInstanceId, itemInstanceId, StringComparison.OrdinalIgnoreCase));
            if (prov == null)
            {
                prov = RegisterItem(itemInstanceId);
            }

            return AddLoreInternal(prov, trigger, text, day, survivorId, locationId);
        }

        private ItemLoreEntry AddLoreInternal(
            ItemProvenanceChain prov,
            LoreTriggerType trigger,
            string text,
            int day,
            string? survivorId = null,
            string? locationId = null)
        {
            var entry = new ItemLoreEntry
            {
                LoreId = $"lore_{_state.NextSequence++}",
                ItemInstanceId = prov.ItemInstanceId,
                TriggerType = trigger,
                Text = text.Trim(),
                Day = Math.Max(1, day),
                AssociatedSurvivorId = survivorId ?? string.Empty,
                AssociatedLocationId = locationId ?? string.Empty
            };

            _state.LoreEntries.Add(entry);
            prov.LoreEntryIds.Add(entry.LoreId);

            RecalculateSignificance(prov);
            OnLoreAdded?.Invoke(entry);
            return entry;
        }

        private void RecalculateSignificance(ItemProvenanceChain prov)
        {
            int count = prov.LoreEntryIds.Count;
            SignificanceLevel newLevel = count switch
            {
                >= 6 => SignificanceLevel.Legendary,
                >= 4 => SignificanceLevel.Important,
                >= 2 => SignificanceLevel.Notable,
                _ => SignificanceLevel.Mundane
            };

            if (newLevel != prov.Significance)
            {
                prov.Significance = newLevel;
                OnSignificanceChanged?.Invoke(prov, newLevel);
            }
        }

        public ItemProvenanceChain? GetProvenance(string itemInstanceId)
        {
            return _state.Provenances.FirstOrDefault(p => string.Equals(p.ItemInstanceId, itemInstanceId, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<ItemLoreEntry> GetLoreEntries(string itemInstanceId)
        {
            return _state.LoreEntries.Where(e => string.Equals(e.ItemInstanceId, itemInstanceId, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public ItemLoreState CaptureState()
        {
            var state = new ItemLoreState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                LoreEntries = new List<ItemLoreEntry>(_state.LoreEntries.Count),
                Provenances = new List<ItemProvenanceChain>(_state.Provenances.Count)
            };

            foreach (var e in _state.LoreEntries)
            {
                state.LoreEntries.Add(new ItemLoreEntry
                {
                    LoreId = e.LoreId,
                    ItemInstanceId = e.ItemInstanceId,
                    TriggerType = e.TriggerType,
                    Text = e.Text,
                    Day = e.Day,
                    AssociatedSurvivorId = e.AssociatedSurvivorId,
                    AssociatedLocationId = e.AssociatedLocationId
                });
            }

            foreach (var p in _state.Provenances)
            {
                state.Provenances.Add(new ItemProvenanceChain
                {
                    ItemInstanceId = p.ItemInstanceId,
                    CrafterSurvivorId = p.CrafterSurvivorId,
                    CraftingDay = p.CraftingDay,
                    DiscoveryLocationId = p.DiscoveryLocationId,
                    DiscoveryDay = p.DiscoveryDay,
                    DiscoveryContext = p.DiscoveryContext,
                    OwnershipChain = new List<string>(p.OwnershipChain),
                    LoreEntryIds = new List<string>(p.LoreEntryIds),
                    Significance = p.Significance
                });
            }

            return state;
        }

        public void RestoreState(ItemLoreState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.LoreEntries.Clear();
            _state.Provenances.Clear();

            if (state.LoreEntries != null)
            {
                foreach (var e in state.LoreEntries)
                {
                    _state.LoreEntries.Add(new ItemLoreEntry
                    {
                        LoreId = e.LoreId,
                        ItemInstanceId = e.ItemInstanceId,
                        TriggerType = e.TriggerType,
                        Text = e.Text,
                        Day = e.Day,
                        AssociatedSurvivorId = e.AssociatedSurvivorId,
                        AssociatedLocationId = e.AssociatedLocationId
                    });
                }
            }

            if (state.Provenances != null)
            {
                foreach (var p in state.Provenances)
                {
                    _state.Provenances.Add(new ItemProvenanceChain
                    {
                        ItemInstanceId = p.ItemInstanceId,
                        CrafterSurvivorId = p.CrafterSurvivorId,
                        CraftingDay = p.CraftingDay,
                        DiscoveryLocationId = p.DiscoveryLocationId,
                        DiscoveryDay = p.DiscoveryDay,
                        DiscoveryContext = p.DiscoveryContext,
                        OwnershipChain = new List<string>(p.OwnershipChain ?? Enumerable.Empty<string>()),
                        LoreEntryIds = new List<string>(p.LoreEntryIds ?? Enumerable.Empty<string>()),
                        Significance = p.Significance
                    });
                }
            }
        }
    }
}
