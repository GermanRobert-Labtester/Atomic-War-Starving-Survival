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
        private ChemicalReconHostSession? _chemicalRecon;

        private void SetupChemicalRecon()
        {
            if (_chemicalRecon != null) return;
            SetupCampaignDay();
            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var catalog = ToxicChemicalCatalogLoader.Load(_dataDir, fileIO, json);

            var crState = ChemicalReconSaveStore.TryLoad() ?? new ChemicalReconState();
            var crSys = new ChemicalReconEngine(
                catalog,
                _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Expedition, 0, 15),
                new GodotLog());
            crSys.RestoreState(crState);
            _chemicalRecon = new ChemicalReconHostSession(crSys);
            // Plan 81: filter breakthrough reads the canonical equipped-respirator
            // condition (gas mask in the Face slot). No parallel filter state.
            _chemicalRecon.FilterRemainingCapacityProvider = () =>
                _inventory?.Inventory?.GetEquipped(EquipSlot.Face)?.CurrentDurability ?? 0f;
        }

        private void SaveChemicalRecon()
        {
            if (_chemicalRecon != null)
                CaptureSection("chemical_recon", ChemicalReconSaveStore.TryCapturePersisted(_chemicalRecon.System.CaptureState()));
        }

    }
}
