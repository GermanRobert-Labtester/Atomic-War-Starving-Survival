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
        private GrainProcessingHostSession? _grainProcessing;

        private void SetupGrainProcessing()
        {
            if (_grainProcessing != null) return;
            SetupInventory();

            var system = new GrainProcessingSystem(_inventory.Inventory, new GodotLog());
            system.LoadCatalog(GrainProcessingCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new GodotLog()));
            var saved = GrainProcessingSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _grainProcessing = new GrainProcessingHostSession(system);
        }

        private void SaveGrainProcessing()
        {
            if (_grainProcessing != null)
                CaptureSection("grain_processing",
                    GrainProcessingSaveStore.TryCapturePersisted(_grainProcessing.System.CaptureState()));
        }

    }
}
