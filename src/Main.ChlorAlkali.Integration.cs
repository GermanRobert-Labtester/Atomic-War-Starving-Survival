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
        private ChlorAlkaliHostSession? _chlorAlkali;

        // ─── Setup ───

        private void SetupChlorAlkali()
        {
            if (_chlorAlkali != null) return;
            SetupCampaignDay();
            SetupInventory();
            SetupPowerGrid();

            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var catalog = ChlorAlkaliSynthesisCatalogLoader.Load(_dataDir, fileIO, json, new GodotLog());

            var saved = ChlorAlkaliSaveStore.TryLoad() ?? new ChlorAlkaliPlantState();
            var system = new ChlorAlkaliSynthesisEngine(
                _inventory.Inventory,
                _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 20),
                () => _powerGrid?.System.NetWatts ?? 5000f,
                new GodotLog());
            system.LoadCatalog(catalog);
            system.RestoreState(saved);
            _chlorAlkali = new ChlorAlkaliHostSession(system);
        }

        // ─── Save (triad) ───

        private void SaveChlorAlkali()
        {
            if (_chlorAlkali != null)
                CaptureSection("chlor_alkali_synthesis", ChlorAlkaliSaveStore.TryCapturePersisted(_chlorAlkali.System.CaptureState()));
        }

    }
}
