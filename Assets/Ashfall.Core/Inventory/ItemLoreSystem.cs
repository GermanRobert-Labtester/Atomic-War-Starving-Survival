// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private ItemLoreState _state;

        public event Action<ItemLoreEntry>? OnLoreAdded;
        public event Action<ItemProvenanceChain, SignificanceLevel>? OnSignificanceChanged;
        public event Action<string, string>? OnOwnershipTransferred;

        public int TrackedItemCount => _state.Provenances.Count;
        public int TotalLoreEntriesCount => _state.LoreEntries.Count;

        public ItemLoreSystem(ItemLoreState? state = null)
        {
            _state = CloneState(state);
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
            string canonicalItemId = itemInstanceId.Trim();

            var existing = _state.Provenances.FirstOrDefault(p => string.Equals(p.ItemInstanceId, canonicalItemId, StringComparison.OrdinalIgnoreCase));
            if (existing != null) return existing;

            var provenance = new ItemProvenanceChain
            {
                ItemInstanceId = canonicalItemId,
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
            string canonicalItemId = itemInstanceId.Trim();

            var prov = _state.Provenances.FirstOrDefault(p => string.Equals(p.ItemInstanceId, canonicalItemId, StringComparison.OrdinalIgnoreCase));
            if (prov == null)
            {
                prov = RegisterItem(canonicalItemId);
            }

            string cleanOwner = newOwnerId.Trim();
            if (prov.OwnershipChain.Count == 0 || !string.Equals(prov.OwnershipChain.Last(), cleanOwner, StringComparison.OrdinalIgnoreCase))
            {
                prov.OwnershipChain.Add(cleanOwner);
                AddLoreInternal(prov, LoreTriggerType.Gift, $"Ownership transferred to {cleanOwner} on Day {day}.", day, cleanOwner);
                OnOwnershipTransferred?.Invoke(canonicalItemId, cleanOwner);
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
            string canonicalItemId = itemInstanceId.Trim();

            var prov = _state.Provenances.FirstOrDefault(p => string.Equals(p.ItemInstanceId, canonicalItemId, StringComparison.OrdinalIgnoreCase));
            if (prov == null)
            {
                prov = RegisterItem(canonicalItemId);
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
            if (_state.NextSequence == int.MaxValue)
                throw new InvalidOperationException("Item lore sequence exhausted.");
            if (_state.NextSequence < 1) _state.NextSequence = 1;

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
            OnLoreAdded?.Invoke(CloneLoreEntry(entry));
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
                OnSignificanceChanged?.Invoke(CloneProvenance(prov), newLevel);
            }
        }

        public ItemProvenanceChain? GetProvenance(string itemInstanceId)
        {
            if (string.IsNullOrWhiteSpace(itemInstanceId)) return null;
            var provenance = _state.Provenances.FirstOrDefault(p =>
                string.Equals(p.ItemInstanceId, itemInstanceId.Trim(), StringComparison.OrdinalIgnoreCase));
            return provenance == null ? null : CloneProvenance(provenance);
        }

        public IReadOnlyList<ItemLoreEntry> GetLoreEntries(string itemInstanceId)
        {
            if (string.IsNullOrWhiteSpace(itemInstanceId)) return Array.Empty<ItemLoreEntry>();
            string canonicalItemId = itemInstanceId.Trim();
            return _state.LoreEntries
                .Where(entry => entry != null
                    && string.Equals(entry.ItemInstanceId, canonicalItemId, StringComparison.OrdinalIgnoreCase))
                .Select(CloneLoreEntry)
                .ToList();
        }

        public ItemLoreState CaptureState() => CloneState(_state);

        public void RestoreState(ItemLoreState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state = CloneState(state);
        }

        private static ItemLoreState CloneState(ItemLoreState? source)
        {
            var copy = new ItemLoreState
            {
                SchemaVersion = source?.SchemaVersion ?? 1,
                NextSequence = source?.NextSequence ?? 1,
                LoreEntries = new List<ItemLoreEntry>(),
                Provenances = new List<ItemProvenanceChain>()
            };
            if (source == null) return copy;

            var loreIds = new HashSet<string>(StringComparer.Ordinal);
            int maxSequence = 0;
            if (source.LoreEntries != null)
            {
                foreach (var entry in source.LoreEntries)
                {
                    if (entry == null
                        || string.IsNullOrWhiteSpace(entry.LoreId)
                        || string.IsNullOrWhiteSpace(entry.ItemInstanceId)
                        || string.IsNullOrWhiteSpace(entry.Text)
                        || !loreIds.Add(entry.LoreId))
                        continue;

                    copy.LoreEntries.Add(CloneLoreEntry(entry));
                    if (TryParseLoreSequence(entry.LoreId, out int sequence))
                        maxSequence = Math.Max(maxSequence, sequence);
                }
            }

            var provenanceIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (source.Provenances != null)
            {
                foreach (var provenance in source.Provenances)
                {
                    if (provenance == null
                        || string.IsNullOrWhiteSpace(provenance.ItemInstanceId)
                        || !provenanceIds.Add(provenance.ItemInstanceId))
                        continue;

                    var cloned = CloneProvenance(provenance);
                    cloned.OwnershipChain = NormalizeStrings(cloned.OwnershipChain);
                    var linkedLoreIds = new HashSet<string>(StringComparer.Ordinal);
                    cloned.LoreEntryIds = cloned.LoreEntryIds
                        .Where(loreId => loreIds.Contains(loreId) && linkedLoreIds.Add(loreId))
                        .ToList();
                    cloned.Significance = SignificanceForLoreCount(cloned.LoreEntryIds.Count);
                    copy.Provenances.Add(cloned);
                }
            }

            if (copy.NextSequence < 1) copy.NextSequence = 1;
            long requiredSequence = (long)maxSequence + 1;
            if (requiredSequence > copy.NextSequence && requiredSequence <= int.MaxValue)
                copy.NextSequence = (int)requiredSequence;
            return copy;
        }

        private static ItemLoreEntry CloneLoreEntry(ItemLoreEntry source)
        {
            return new ItemLoreEntry
            {
                LoreId = source.LoreId,
                ItemInstanceId = source.ItemInstanceId,
                TriggerType = source.TriggerType,
                Text = source.Text,
                Day = source.Day,
                AssociatedSurvivorId = source.AssociatedSurvivorId,
                AssociatedLocationId = source.AssociatedLocationId
            };
        }

        private static ItemProvenanceChain CloneProvenance(ItemProvenanceChain source)
        {
            return new ItemProvenanceChain
            {
                ItemInstanceId = source.ItemInstanceId,
                CrafterSurvivorId = source.CrafterSurvivorId,
                CraftingDay = source.CraftingDay,
                DiscoveryLocationId = source.DiscoveryLocationId,
                DiscoveryDay = source.DiscoveryDay,
                DiscoveryContext = source.DiscoveryContext,
                OwnershipChain = new List<string>(source.OwnershipChain ?? Enumerable.Empty<string>()),
                LoreEntryIds = new List<string>(source.LoreEntryIds ?? Enumerable.Empty<string>()),
                Significance = source.Significance
            };
        }

        private static List<string> NormalizeStrings(IEnumerable<string>? values)
        {
            if (values == null) return new List<string>();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            return values
                .Where(value => !string.IsNullOrWhiteSpace(value) && seen.Add(value))
                .ToList();
        }

        private static bool TryParseLoreSequence(string loreId, out int sequence)
        {
            sequence = 0;
            const string prefix = "lore_";
            if (string.IsNullOrEmpty(loreId)
                || !loreId.StartsWith(prefix, StringComparison.Ordinal)
                || loreId.Length == prefix.Length)
                return false;

            return int.TryParse(
                loreId.Substring(prefix.Length),
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out sequence);
        }

        private static SignificanceLevel SignificanceForLoreCount(int count)
        {
            return count switch
            {
                >= 6 => SignificanceLevel.Legendary,
                >= 4 => SignificanceLevel.Important,
                >= 2 => SignificanceLevel.Notable,
                _ => SignificanceLevel.Mundane
            };
        }
    }
}
