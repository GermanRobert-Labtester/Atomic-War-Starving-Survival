// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Radio;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private CryogenicAirSeparationHostSession? _cryogenicAirSeparation;

        private void SetupCryogenicAirSeparation()
        {
            if (_cryogenicAirSeparation != null) return;
            SetupInventory();
            SetupPowerGrid();

            var system = new CryogenicAirSeparationSystem(
                _inventory.Inventory,
                _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 17),
                () => _powerGrid?.System.NetWatts ?? 0f,
                new GodotLog());
            system.LoadCatalog(CryogenicAirSeparationCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new GodotLog()));
            var saved = CryogenicAirSeparationSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _cryogenicAirSeparation = new CryogenicAirSeparationHostSession(system);
        }

        private void SaveCryogenicAirSeparation()
        {
            if (_cryogenicAirSeparation != null)
                CaptureSection("cryogenic_air_separation",
                    CryogenicAirSeparationSaveStore.TryCapturePersisted(
                        _cryogenicAirSeparation.System.CaptureState()));
        }

    }
}
