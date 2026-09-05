// SPDX-License-Identifier: MIT
// ASHFALL collectible discovery + unique-claim host triad
// (save enrollment for collectible_discovery and unique_claims sections).

using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private CollectibleCatalog? _collectibleCatalog;
        private CollectibleDiscoveryState? _collectibleDiscovery;
        private UniqueItemClaimRegistry? _uniqueClaims;
        private CollectibleEffectDispatcher? _collectibleDispatcher;
        private bool _collectiblesDirty;
        private bool _collectibleInventoryWired;

        /// <summary>
        /// Live discovery ledger used by collectible effect dispatch.
        /// Null until <see cref="SetupCollectibles"/> runs.
        /// </summary>
        public CollectibleDiscoveryState? CollectibleDiscovery => _collectibleDiscovery;

        /// <summary>
        /// Live unique-claim registry used by loot generation channels.
        /// Null until <see cref="SetupCollectibles"/> runs.
        /// </summary>
        public UniqueItemClaimRegistry? UniqueClaims => _uniqueClaims;

        /// <summary>
        /// Production collectible effect feeder (audit #27). Null until setup.
        /// </summary>
        public CollectibleEffectDispatcher? CollectibleDispatcher => _collectibleDispatcher;

        private void SetupCollectibles()
        {
            if (_collectibleDiscovery != null && _uniqueClaims != null && _collectibleDispatcher != null)
            {
                WireCollectibleInventoryFeeder();
                return;
            }

            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            _collectibleCatalog = CollectibleCatalogLoader.Load(_dataDir, fileIO, json)
                                  ?? new CollectibleCatalog(null);

            var uniqueIds = new List<string>();
            foreach (var kv in _collectibleCatalog.ByItemId)
            {
                if (kv.Value != null && kv.Value.unique)
                    uniqueIds.Add(kv.Key);
            }

            _collectibleDiscovery ??= new CollectibleDiscoveryState();
            _uniqueClaims ??= new UniqueItemClaimRegistry(uniqueIds);

            var discoverySaved = CollectibleDiscoverySaveStore.TryLoad();
            if (discoverySaved != null)
                _collectibleDiscovery.RestoreState(discoverySaved);

            var claimsSaved = UniqueClaimSaveStore.TryLoad();
            if (claimsSaved != null)
                _uniqueClaims.RestoreState(claimsSaved);

            _collectibleDispatcher ??= new CollectibleEffectDispatcher(
                _collectibleCatalog,
                _collectibleDiscovery,
                needsProvider: () => _survivors?.Needs,
                researchProvider: () =>
                {
                    _sharedResearch = EnsureSharedResearch();
                    return _sharedResearch;
                },
                journalProvider: () => _journal,
                mapProvider: () => _world?.WastelandMap,
                dayProvider: () => _simDay);

            WireCollectibleInventoryFeeder();
        }

        /// <summary>
        /// Subscribe the collectible dispatcher to inventory acquisitions.
        /// Safe to call before or after <see cref="SetupInventory"/>; re-entrant.
        /// </summary>
        private void WireCollectibleInventoryFeeder()
        {
            if (_collectibleInventoryWired || _collectibleDispatcher == null || _inventory == null)
                return;

            _inventory.Inventory.OnItemAdded += OnCollectibleItemAdded;
            _collectibleInventoryWired = true;
        }

        private void OnCollectibleItemAdded(ItemDefinition item, int amount)
        {
            if (_collectibleDispatcher == null || item == null || string.IsNullOrEmpty(item.id))
                return;

            var result = _collectibleDispatcher.DispatchOnAcquire(item.id);
            if (result.DiscoveryRegistered)
                MarkCollectiblesDirty();
        }

        private void SaveCollectibles()
        {
            bool ok = true;

            if (_collectibleDiscovery != null)
            {
                ok &= CaptureSection(
                    "collectible_discovery",
                    CollectibleDiscoverySaveStore.TryCapturePersisted(_collectibleDiscovery.CaptureState()));
            }

            if (_uniqueClaims != null)
            {
                ok &= CaptureSection(
                    "unique_claims",
                    UniqueClaimSaveStore.TryCapturePersisted(_uniqueClaims.CaptureState()));
            }

            if (ok)
                _collectiblesDirty = false;
        }

        private void FlushCollectiblesIfDirty()
        {
            if (_collectiblesDirty) SaveCollectibles();
        }

        /// <summary>Marks collectible ledgers dirty after a discovery or unique claim.</summary>
        public void MarkCollectiblesDirty() => _collectiblesDirty = true;
    }
}
