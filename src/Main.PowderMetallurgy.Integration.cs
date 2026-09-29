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
        private PowderMetallurgyHostSession? _powderMetallurgy;

        private void SetupPowderMetallurgy()
        {
            if (_powderMetallurgy != null) return;
            SetupInventory();
            SetupPowerGrid();

            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.Foundry, 0, 24)
                : new SeededRng(130);
            var system = new PowderMetallurgySystem(
                _inventory.Inventory,
                rng,
                () => _powerGrid?.System.NetWatts ?? 0f,
                new GodotLog());
            system.LoadCatalog(PowderMetallurgyCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new GodotLog()));
            var saved = PowderMetallurgySaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _powderMetallurgy = new PowderMetallurgyHostSession(system);
            system.OnBatchCompleted += batch =>
                _journal?.TryAddRawEntry(
                    "powder_metallurgy_batch",
                    $"The material press completed {batch.output_units} abstract material unit(s).",
                    null!, _simDay);
        }

        private void SavePowderMetallurgy()
            => CaptureIfPresent("powder_metallurgy", _powderMetallurgy?.System.CaptureState(),
                PowderMetallurgySaveStore.TryCapturePersisted);

    }
}
