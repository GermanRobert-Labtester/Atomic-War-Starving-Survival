// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Shelter;
using Godot;
using System;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private BallisticShieldHostSession? _ballisticShield;

        private void SetupBallisticShield()
        {
            if (_ballisticShield != null) return;
            SetupCampaignDay();
            SetupInventory();

            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var catalog = BallisticShieldCatalogLoader.Load(_dataDir, fileIO, json, new GodotLog());

            var saved = BallisticShieldSaveStore.TryLoad() ?? new BallisticShieldState();
            var system = new BallisticShieldEngine(
                _inventory.Inventory,
                _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Combat, 0, 23),
                new GodotLog());
            system.LoadCatalog(catalog);
            system.RestoreState(saved);
            _ballisticShield = new BallisticShieldHostSession(system);
        }

        private void SaveBallisticShield()
        {
            if (_ballisticShield != null)
                CaptureSection("ballistic_shield", BallisticShieldSaveStore.TryCapturePersisted(_ballisticShield.System.CaptureState()));
        }

    }
}
