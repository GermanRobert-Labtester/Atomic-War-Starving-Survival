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
        private SolarConcentratorHostSession? _solarConcentrator;

        private void SetupSolarConcentrator()
        {
            if (_solarConcentrator != null) return;
            SetupCampaignDay();
            SetupInventory();
            SetupWorld();

            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var catalog = SolarConcentratorCatalogLoader.Load(_dataDir, fileIO, json, new GodotLog());

            var saved = SolarConcentratorSaveStore.TryLoad() ?? new SolarConcentratorState();
            var system = new SolarConcentratorEngine(
                _inventory.Inventory,
                _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 21),
                () => _world?.Weather != null ? _world.Weather.VisibilityFactor : 1.0f,
                new GodotLog());
            system.LoadCatalog(catalog);
            system.RestoreState(saved);
            _solarConcentrator = new SolarConcentratorHostSession(system);

            // B5–B8 Phase 2 (Plan 65): publish the concentrator's electrical
            // output into the grid under the stable solar_concentrator source
            // id — but only once a grid-tie inverter is physically wired.
            // Republish after restore (same discipline as the nuclear core
            // contribution) and on every output change; weather already drives
            // the output itself through the engine's availability query.
            system.OnSolarOutputChanged += (_, _) => PublishSolarConcentratorGeneration();
            PublishSolarConcentratorGeneration();
        }

        /// <summary>
        /// B5–B8 Phase 2: publish (or clear) the solar grid feed. Idempotent —
        /// SetGenerationContribution replaces by source id, so daily
        /// republishing never accumulates duplicate output.
        /// </summary>
        private void PublishSolarConcentratorGeneration()
        {
            var solar = _solarConcentrator?.System;
            if (solar == null) return;
            SetupPowerGrid();
            _powerGrid?.System.SetGenerationContribution(
                SolarConcentratorEngine.PowerSourceId, solar.GridFeedWatts);
        }

        private void SaveSolarConcentrator()
        {
            if (_solarConcentrator != null)
                CaptureSection("solar_concentrator", SolarConcentratorSaveStore.TryCapturePersisted(_solarConcentrator.System.CaptureState()));
        }

    }
}
