// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ResearchUnlockSaveStore
// Core State : Ashfall.Core.Research.ResearchUnlockState
// Host Caller: Main.ResearchUnlock (SetupResearchUnlockBridge / SaveResearchUnlock)
// Purpose    : Plan 141 — Research downstream unlocks bridge: items, recipes,
//              shelter, expedition, combat, and medical capabilities.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Research;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class ResearchUnlockSaveStore
    {
        public const string FileName = "research_unlock_save.json";
        public const string SectionName = "research_unlock";

        private static readonly SaveStore<ResearchUnlockState> s_store =
            SaveStoreHub.Checksummed<ResearchUnlockState>(FileName, nameof(ResearchUnlockSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(ResearchUnlockState state) => s_store.CaptureBare(state);
        public static ResearchUnlockState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(ResearchUnlockState state) => s_store.TrySave(state);
        public static ResearchUnlockState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session manager for Plan 141 (Research → Downstream Unlocks Bridge).
    /// Connects ResearchSystem completions to inventory breakthroughs, crafting recipes,
    /// and shelter, expedition, combat, and medical capabilities.
    /// </summary>
    public sealed class ResearchUnlockHostSession : HostSessionBase
    {
        private readonly ResearchUnlockBridge _bridge;
        private readonly IResearchUnlockSink _sink;
        private string _lastEvent = string.Empty;

        public ResearchUnlockBridge Bridge => _bridge;
        public IResearchUnlockSink Sink => _sink;
        public string LastEvent => _lastEvent;

        public ResearchUnlockCensus Census => _bridge.GetCensus();
        public IReadOnlyList<string> UnlockedCapabilities => _bridge.UnlockedCapabilities;
        public IReadOnlyList<string> UnlockedRecipes => _bridge.UnlockedRecipes;
        public IReadOnlyList<string> GrantedItems => _bridge.GrantedItems;
        public IReadOnlyList<string> GrantedUnlockIds => _bridge.GrantedUnlockIds;

        public ResearchUnlockHostSession(
            string? dataDir = null,
            Ashfall.Core.Inventory.Inventory? inventory = null,
            ResearchUnlockBridge? bridge = null)
        {
            _bridge = bridge ?? new ResearchUnlockBridge();
            _sink = new InventoryResearchUnlockSink(inventory ?? new Ashfall.Core.Inventory.Inventory());

            _bridge.OnUnlockGrantedSeam = unlock =>
            {
                _lastEvent = $"Unlock granted: {unlock.id} ({unlock.unlock_type} -> {unlock.unlock_target_id})";
                RaiseStateChanged();
            };

            _bridge.OnBreakthroughItemAwardedSeam = (item, qty) =>
            {
                _lastEvent = $"Breakthrough awarded: {qty}x {item}";
                RaiseStateChanged();
            };

            _bridge.OnRecipeUnlockedSeam = recipe =>
            {
                _lastEvent = $"Recipe unlocked: {recipe}";
                RaiseStateChanged();
            };

            _bridge.OnCapabilityEnabledSeam = (cap, domain) =>
            {
                _lastEvent = $"Capability enabled: {domain}:{cap}";
                RaiseStateChanged();
            };

            if (!string.IsNullOrEmpty(dataDir))
            {
                LoadCatalog(dataDir);
            }
        }

        public static ResearchUnlockHostSession Create(
            string dataDir,
            Ashfall.Core.Inventory.Inventory? inventory = null,
            ResearchUnlockBridge? bridge = null)
        {
            return new ResearchUnlockHostSession(dataDir, inventory, bridge);
        }

        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            string path = Path.Combine(dataDir, ResearchUnlockBridge.DefaultCatalogFileName);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                _bridge.LoadCatalog(json);
                _lastEvent = $"Loaded {_bridge.Catalog.Count} research unlock definitions.";
                RaiseStateChanged();
            }
        }

        public void BindResearchSystem(ResearchSystem system)
        {
            if (system == null) return;
            _bridge.BindResearchSystem(system, _sink);
        }

        public int ProcessResearchCompletion(string nodeId)
        {
            int granted = _bridge.ProcessResearchCompletion(nodeId, _sink);
            if (granted > 0)
            {
                _lastEvent = $"Processed {granted} unlocks for completed node {nodeId}.";
                RaiseStateChanged();
            }
            return granted;
        }

        public int SynchronizeCompletedResearch(IEnumerable<string> completedNodeIds)
        {
            int granted = _bridge.SynchronizeCompletedResearch(completedNodeIds, _sink);
            if (granted > 0)
            {
                _lastEvent = $"Synchronized {granted} unlocks from existing completed research.";
                RaiseStateChanged();
            }
            return granted;
        }

        public bool HasCapability(string capabilityId) => _bridge.HasCapability(capabilityId);
        public bool HasRecipe(string recipeId) => _bridge.HasRecipe(recipeId);
        public bool HasItem(string itemId) => _bridge.HasItem(itemId);
        public bool HasUnlock(string unlockId) => _bridge.HasUnlock(unlockId);

        public ResearchUnlockState CaptureState() => _bridge.CaptureState();

        public void RestoreState(ResearchUnlockState state)
        {
            _bridge.RestoreState(state);
            _lastEvent = "Restored research unlock state.";
            RaiseStateChanged();
        }
    }
}
