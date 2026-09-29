// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Godot;
using System;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private KineticStorageHostSession? _kineticStorage;

        private void SetupKineticStorage()
        {
            if (_kineticStorage != null) return;
            SetupCampaignDay();
            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var catalog = KineticFlywheelCatalogLoader.Load(_dataDir, fileIO, json);

            var ksState = KineticStorageSaveStore.TryLoad() ?? new KineticStorageState();
            var ksSys = new KineticStorageSystem(
                catalog,
                _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 15),
                new GodotLog());
            ksSys.RestoreState(ksState);
            _kineticStorage = new KineticStorageHostSession(ksSys);
        }

        private void SaveKineticStorage()
        {
            if (_kineticStorage != null)
                CaptureSection("kinetic_storage", KineticStorageSaveStore.TryCapturePersisted(_kineticStorage.System.CaptureState()));
        }

    }
}
