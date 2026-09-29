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
        private PrecisionOpticsHostSession? _precisionOptics;

        private void SetupPrecisionOptics()
        {
            if (_precisionOptics != null) return;
            SetupCampaignDay();
            SetupInventory();

            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var catalog = PrecisionOpticsCatalogLoader.Load(_dataDir, fileIO, json, new GodotLog());

            var saved = PrecisionOpticsSaveStore.TryLoad() ?? new PrecisionOpticsState();
            var system = new PrecisionOpticsEngine(
                _inventory.Inventory,
                _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 22),
                new GodotLog());
            system.LoadCatalog(catalog);
            system.RestoreState(saved);
            _precisionOptics = new PrecisionOpticsHostSession(system);
        }

        private void SavePrecisionOptics()
        {
            if (_precisionOptics != null)
                CaptureSection("precision_optics", PrecisionOpticsSaveStore.TryCapturePersisted(_precisionOptics.System.CaptureState()));
        }

    }
}
