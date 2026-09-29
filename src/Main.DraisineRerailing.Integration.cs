// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Medical;
using Ashfall.Core.Radio;
using Ashfall.Core.Random;
using AtomicWar.GodotApp.UI;
using Godot;
using System;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private DraisineRerailingHostSession? _draisineRerailing;

        private void SetupDraisineRerailing()
        {
            if (_draisineRerailing != null) return;
            SetupInventory();
            SetupPowerGrid();
            SetupRailway();

            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.Expedition, 0, 27)
                : new SeededRng(133);
            var system = new DraisineRerailingSystem(
                _inventory.Inventory,
                EnsureRailway(),
                rng,
                () => _powerGrid?.System.NetWatts ?? 0f,
                new GodotLog());
            system.LoadCatalog(RerailingEquipmentCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new GodotLog()));
            var saved = DraisineRerailingSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _draisineRerailing = new DraisineRerailingHostSession(system);
            system.OnRecoveryCompleted += state =>
                _journal?.TryAddRawEntry(
                    "draisine_rerailing",
                    $"Armored draisine {state.train_id} was returned to the rail.",
                    null!, _simDay);
        }

        private void SaveDraisineRerailing()
            => CaptureIfPresent("draisine_recovery", _draisineRerailing?.System.CaptureState(),
                DraisineRerailingSaveStore.TryCapturePersisted);

    }
}
