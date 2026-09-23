// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.IO;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Research
{
    [Serializable]
    public sealed class ResearchUnlockDef
    {
        public string id { get; set; } = string.Empty;
        public string research_node_id { get; set; } = string.Empty;
        public string unlock_type { get; set; } = "item"; // item, recipe, expedition, shelter, combat, medical
        public string unlock_target_id { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class ResearchUnlockState
    {
        public int schema_version { get; set; } = 1;
        public List<string> grantedUnlockIds { get; set; } = new List<string>();
        public List<string> grantedItems { get; set; } = new List<string>();
        public List<string> unlockedRecipeIds { get; set; } = new List<string>();
        public List<string> unlockedCapabilities { get; set; } = new List<string>();
    }

    /// <summary>
    /// Sink contract for downstream gameplay systems to receive research unlocks and breakthrough items.
    /// </summary>
    public interface IResearchUnlockSink
    {
        bool GrantBreakthroughItem(string itemId, int quantity = 1);
        bool UnlockRecipe(string recipeId);
        bool EnableCapability(string capabilityId, string domain);
    }

    /// <summary>
    /// Default adapter connecting research unlocks directly to player inventory without shadow stores.
    /// </summary>
    public sealed class InventoryResearchUnlockSink : IResearchUnlockSink
    {
        private readonly Inventory.Inventory _inventory;
        private readonly HashSet<string> _unlockedRecipes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _enabledCapabilities = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public InventoryResearchUnlockSink(Inventory.Inventory inventory)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
        }

        public IReadOnlyCollection<string> UnlockedRecipes => _unlockedRecipes;
        public IReadOnlyCollection<string> EnabledCapabilities => _enabledCapabilities;

        public bool GrantBreakthroughItem(string itemId, int quantity = 1)
        {
            if (string.IsNullOrEmpty(itemId) || quantity <= 0) return true;
            return _inventory.AddById(itemId, quantity);
        }

        public bool UnlockRecipe(string recipeId)
        {
            if (string.IsNullOrEmpty(recipeId)) return false;
            return _unlockedRecipes.Add(recipeId);
        }

        public bool EnableCapability(string capabilityId, string domain)
        {
            if (string.IsNullOrEmpty(capabilityId)) return false;
            return _enabledCapabilities.Add($"{domain}:{capabilityId}");
        }
    }

    /// <summary>
    /// Downstream unlocks bridge connecting ResearchSystem completions to crafting recipes,
    /// breakthrough items, expedition routes, shelter upgrades, combat doctrines, and medical procedures (Plan 141 / C2[31] / DEC-142).
    /// </summary>
    public sealed class ResearchUnlockBridge
    {
        public const string SystemId = "research_unlock_bridge";
        public const string DefaultCatalogFileName = "research_unlocks.json";

        private readonly List<ResearchUnlockDef> _catalog = new List<ResearchUnlockDef>();
        private ResearchUnlockState _state;

        public Action<ResearchUnlockDef>? OnUnlockGrantedSeam { get; set; }
        public Action<string, int>? OnBreakthroughItemAwardedSeam { get; set; }
        public Action<string>? OnRecipeUnlockedSeam { get; set; }
        public Action<string, string>? OnCapabilityEnabledSeam { get; set; }

        public ResearchUnlockBridge(ResearchUnlockState? state = null)
        {
            _state = state ?? new ResearchUnlockState();
        }

        public ResearchUnlockState State => _state;
        public IReadOnlyList<ResearchUnlockDef> Catalog => _catalog;

        public void RegisterUnlock(ResearchUnlockDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return;
            if (!_catalog.Any(u => u.id == def.id))
            {
                _catalog.Add(def);
            }
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var list = CatalogLocator.LoadWrappedList<ResearchUnlockDef>(json, SystemTextJsonSerializer.Options);
                if (list != null)
                {
                    foreach (var u in list)
                    {
                        RegisterUnlock(u);
                    }
                }
            }
            catch
            {
                // Catalog parse fallback
            }
        }

        public static ResearchUnlockBridge LoadFromDirectory(string dataDir, IFileIO fileIO)
        {
            var bridge = new ResearchUnlockBridge();
            if (fileIO != null && !string.IsNullOrEmpty(dataDir))
            {
                string path = fileIO.Combine(dataDir, DefaultCatalogFileName);
                if (fileIO.FileExists(path))
                {
                    bridge.LoadCatalog(fileIO.ReadAllText(path));
                }
            }
            return bridge;
        }

        public void BindResearchSystem(ResearchSystem system, IResearchUnlockSink? sink = null)
        {
            if (system == null) return;
            system.OnResearchCompleted += def =>
            {
                if (def != null)
                {
                    ProcessResearchCompletion(def.id, sink);
                }
            };
        }

        /// <summary>
        /// Evaluates and grants all unlocks mapped to a completed research node.
        /// </summary>
        public int ProcessResearchCompletion(string nodeId, IResearchUnlockSink? sink = null)
        {
            if (string.IsNullOrWhiteSpace(nodeId)) return 0;
            var matching = _catalog.Where(u => string.Equals(u.research_node_id, nodeId, StringComparison.OrdinalIgnoreCase)).ToList();

            int grantedCount = 0;
            foreach (var unlock in matching)
            {
                if (_state.grantedUnlockIds.Contains(unlock.id))
                    continue;

                _state.grantedUnlockIds.Add(unlock.id);
                grantedCount++;

                switch (unlock.unlock_type.ToLowerInvariant())
                {
                    case "item":
                        _state.grantedItems.Add(unlock.unlock_target_id);
                        sink?.GrantBreakthroughItem(unlock.unlock_target_id, 1);
                        OnBreakthroughItemAwardedSeam?.Invoke(unlock.unlock_target_id, 1);
                        break;
                    case "recipe":
                        _state.unlockedRecipeIds.Add(unlock.unlock_target_id);
                        sink?.UnlockRecipe(unlock.unlock_target_id);
                        OnRecipeUnlockedSeam?.Invoke(unlock.unlock_target_id);
                        break;
                    case "expedition":
                    case "shelter":
                    case "combat":
                    case "medical":
                        _state.unlockedCapabilities.Add(unlock.unlock_target_id);
                        sink?.EnableCapability(unlock.unlock_target_id, unlock.unlock_type);
                        OnCapabilityEnabledSeam?.Invoke(unlock.unlock_target_id, unlock.unlock_type);
                        break;
                }

                OnUnlockGrantedSeam?.Invoke(unlock);
            }

            return grantedCount;
        }

        /// <summary>
        /// Retroactively synchronizes previously completed research nodes from save data,
        /// ensuring past research grants corresponding downstream unlocks.
        /// </summary>
        public int SynchronizeCompletedResearch(IEnumerable<string> completedNodeIds, IResearchUnlockSink? sink = null)
        {
            if (completedNodeIds == null) return 0;
            int total = 0;
            foreach (var id in completedNodeIds)
            {
                total += ProcessResearchCompletion(id, sink);
            }
            return total;
        }

        public ResearchUnlockState CaptureState()
        {
            return new ResearchUnlockState
            {
                schema_version = _state.schema_version,
                grantedUnlockIds = new List<string>(_state.grantedUnlockIds),
                grantedItems = new List<string>(_state.grantedItems),
                unlockedRecipeIds = new List<string>(_state.unlockedRecipeIds),
                unlockedCapabilities = new List<string>(_state.unlockedCapabilities)
            };
        }

        public void RestoreState(ResearchUnlockState? state)
        {
            if (state == null)
            {
                _state = new ResearchUnlockState();
                return;
            }
            _state = new ResearchUnlockState
            {
                schema_version = state.schema_version,
                grantedUnlockIds = state.grantedUnlockIds != null ? new List<string>(state.grantedUnlockIds) : new List<string>(),
                grantedItems = state.grantedItems != null ? new List<string>(state.grantedItems) : new List<string>(),
                unlockedRecipeIds = state.unlockedRecipeIds != null ? new List<string>(state.unlockedRecipeIds) : new List<string>(),
                unlockedCapabilities = state.unlockedCapabilities != null ? new List<string>(state.unlockedCapabilities) : new List<string>()
            };
        }

        public bool HasCapability(string capabilityId)
        {
            if (string.IsNullOrEmpty(capabilityId)) return false;
            return _state.unlockedCapabilities.Any(c => string.Equals(c, capabilityId, StringComparison.OrdinalIgnoreCase) ||
                                                        c.EndsWith($":{capabilityId}", StringComparison.OrdinalIgnoreCase));
        }

        public bool HasRecipe(string recipeId)
        {
            if (string.IsNullOrEmpty(recipeId)) return false;
            return _state.unlockedRecipeIds.Any(r => string.Equals(r, recipeId, StringComparison.OrdinalIgnoreCase));
        }

        public bool HasItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            return _state.grantedItems.Any(i => string.Equals(i, itemId, StringComparison.OrdinalIgnoreCase));
        }

        public bool HasUnlock(string unlockId)
        {
            if (string.IsNullOrEmpty(unlockId)) return false;
            return _state.grantedUnlockIds.Any(u => string.Equals(u, unlockId, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<string> UnlockedCapabilities => _state.unlockedCapabilities;
        public IReadOnlyList<string> UnlockedRecipes => _state.unlockedRecipeIds;
        public IReadOnlyList<string> GrantedItems => _state.grantedItems;
        public IReadOnlyList<string> GrantedUnlockIds => _state.grantedUnlockIds;

        public ResearchUnlockCensus GetCensus()
        {
            return new ResearchUnlockCensus(
                _catalog.Count,
                _state.grantedUnlockIds.Count,
                _state.grantedItems.Count,
                _state.unlockedRecipeIds.Count,
                _state.unlockedCapabilities.Count);
        }
    }

    public readonly struct ResearchUnlockCensus
    {
        public readonly int TotalCatalogUnlocks;
        public readonly int GrantedUnlocksCount;
        public readonly int GrantedItemsCount;
        public readonly int UnlockedRecipesCount;
        public readonly int UnlockedCapabilitiesCount;

        public ResearchUnlockCensus(int catalog, int granted, int items, int recipes, int capabilities)
        {
            TotalCatalogUnlocks = catalog;
            GrantedUnlocksCount = granted;
            GrantedItemsCount = items;
            UnlockedRecipesCount = recipes;
            UnlockedCapabilitiesCount = capabilities;
        }
    }
}

