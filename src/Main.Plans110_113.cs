using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plans 110–113 flagship wiring:
    /// - Plan 110: ChlorAlkaliSynthesisEngine (Industrial Sanitation Chemistry)
    /// - Plan 111: SolarConcentratorEngine (Solar-Thermal Infrastructure)
    /// - Plan 112: PrecisionOpticsEngine (Precision Optics Manufacturing)
    /// - Plan 113: BallisticShieldEngine (Defensive Shield Systems)
    /// Follows the standard triad pattern (SetupXxx / SaveXxx / TickPlans110To113).
    /// </summary>
    public sealed partial class Main
    {
        private ChlorAlkaliHostSession? _chlorAlkali;
        private SolarConcentratorHostSession? _solarConcentrator;
        private PrecisionOpticsHostSession? _precisionOptics;
        private BallisticShieldHostSession? _ballisticShield;

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
        }

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

        private void SetupPlans110To113()
        {
            SetupChlorAlkali();
            SetupSolarConcentrator();
            SetupPrecisionOptics();
            SetupBallisticShield();
        }

        // ─── Save (triad) ───

        private void SaveChlorAlkali()
        {
            if (_chlorAlkali != null)
                CaptureSection("chlor_alkali_synthesis", ChlorAlkaliSaveStore.TryCapturePersisted(_chlorAlkali.System.CaptureState()));
        }

        private void SaveSolarConcentrator()
        {
            if (_solarConcentrator != null)
                CaptureSection("solar_concentrator", SolarConcentratorSaveStore.TryCapturePersisted(_solarConcentrator.System.CaptureState()));
        }

        private void SavePrecisionOptics()
        {
            if (_precisionOptics != null)
                CaptureSection("precision_optics", PrecisionOpticsSaveStore.TryCapturePersisted(_precisionOptics.System.CaptureState()));
        }

        private void SaveBallisticShield()
        {
            if (_ballisticShield != null)
                CaptureSection("ballistic_shield", BallisticShieldSaveStore.TryCapturePersisted(_ballisticShield.System.CaptureState()));
        }

        private void SavePlans110To113()
        {
            SaveChlorAlkali();
            SaveSolarConcentrator();
            SavePrecisionOptics();
            SaveBallisticShield();
        }

        // ─── Tick (cadence) ───

        private void TickPlans110To113(int day)
        {
            _chlorAlkali?.System.TickDay(day);
            _solarConcentrator?.System.TickDay(day);
        }
    }
}
