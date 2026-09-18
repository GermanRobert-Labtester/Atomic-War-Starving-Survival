// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 122-125 Phase 8 — save/store wiring for the late-tech
//                tranche. Setup* methods construct engines + sessions, fork
//                per-day RNG streams, and restore persisted state; Save*
//                methods capture typed sections. Panels arrive in Phase 9.
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using Ashfall.Core.Combat;
using Ashfall.Core.Expeditions;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SofcPowerHostSession? _sofcPower;
        private CvdDiamondHostSession? _cvdDiamond;
        private SoundRangingHostSession? _soundRanging;
        private AmphibiousDraisineHostSession? _amphibiousDraisine;
        private SolidOxideFuelCellPanel? _sofcPanel;
        private CvdDiamondPanel? _cvdPanel;
        private SoundRangingPanel? _soundRangingPanel;
        private AmphibiousDraisinePanel? _amphibiousPanel;

        public SofcPowerHostSession? SofcPowerSession => _sofcPower;
        public CvdDiamondHostSession? CvdDiamondSession => _cvdDiamond;
        public SoundRangingHostSession? SoundRangingSession => _soundRanging;
        public AmphibiousDraisineHostSession? AmphibiousDraisineSession => _amphibiousDraisine;

        // ── Setup (composition; called by SaveOrchestrator on boot/reload) ──

        private void SetupSofcPower()
        {
            if (_sofcPower != null) return;

            var catalog = SofcPowerCatalogLoader.Load(_dataDir, new FileSystemIO());
            var system = new SofcElectrochemistryEngine(catalog, new GodotLog());
            if (_campaignDay?.Rng != null)
                system.Rng = _campaignDay.Rng.Fork(CampaignStreamIds.SofcPower);

            _sofcPower = new SofcPowerHostSession(system)
            {
                // Wiring seams are bound here at composition time (Phase 7
                // contract): the canonical owners stay authoritative.
                ContributionApplier = (sourceId, watts) => _powerGrid?.System.SetGenerationContribution(sourceId, watts),
                FuelConsumer = units => ConsumeSofcFuel(units),
                FuelQualityProvider = () => ResolveSofcFuelQuality(),
                EngineeringSkillProvider = () => 50f,
                WasteHeatRouter = (roomId, kw) => _shelterThermal?.System.AddAuxiliaryHeat(roomId, kw),
                WasteHeatTargetRoomProvider = () => "room_kitchen"
            };

            var saved = SofcPowerSaveStore.TryLoad();
            if (saved != null)
            {
                _sofcPower.RestoreSave(saved);
                GD.Print("[Ashfall Godot] SOFC plant state restored.");
            }
        }

        private static readonly string[] SofcCleanFuelItemIds = { "item_biofuel_high_grade", "synthetic_fuel_canister" };
        private static readonly string[] SofcTreatedFuelItemIds = { "item_biofuel_generator_grade", "fuel_canister" };
        private static readonly string[] SofcDirtyFuelItemIds = { "item_biofuel_low_grade" };

        private string ResolveSofcFuelQuality()
        {
            if (_inventory == null) SetupInventory();
            var inv = _inventory?.Inventory;
            if (inv != null)
            {
                foreach (var id in SofcCleanFuelItemIds)
                    if (inv.CountById(id) > 0) return SofcPowerCatalog.QualityClean;
                foreach (var id in SofcTreatedFuelItemIds)
                    if (inv.CountById(id) > 0) return SofcPowerCatalog.QualityTreated;
                foreach (var id in SofcDirtyFuelItemIds)
                    if (inv.CountById(id) > 0) return SofcPowerCatalog.QualityDirty;
            }
            return SofcPowerCatalog.QualityClean;
        }

        private bool ConsumeSofcFuel(float units)
        {
            if (_inventory == null) SetupInventory();
            var inv = _inventory?.Inventory;

            if (units <= 0f)
            {
                // Availability probe: check inventory first, fall back to grid fuel units
                if (inv != null)
                {
                    foreach (var id in SofcCleanFuelItemIds)
                        if (inv.CountById(id) > 0) return true;
                    foreach (var id in SofcTreatedFuelItemIds)
                        if (inv.CountById(id) > 0) return true;
                    foreach (var id in SofcDirtyFuelItemIds)
                        if (inv.CountById(id) > 0) return true;
                }
                return (_powerGrid?.System != null && _powerGrid.System.FuelUnits > 0f);
            }

            // Consumption: draw canisters from inventory in order of quality
            int cansNeeded = Math.Max(1, (int)Math.Ceiling(units / 25.0f));
            if (inv != null)
            {
                string[][] qualityTiers = { SofcCleanFuelItemIds, SofcTreatedFuelItemIds, SofcDirtyFuelItemIds };
                foreach (var tier in qualityTiers)
                {
                    foreach (var id in tier)
                    {
                        int available = inv.CountById(id);
                        if (available <= 0) continue;
                        int take = Math.Min(available, cansNeeded);
                        if (inv.RemoveById(id, take))
                        {
                            cansNeeded -= take;
                            if (cansNeeded <= 0) return true;
                        }
                    }
                }
            }

            // Draw remaining fuel demand from power grid reserve if available
            if (cansNeeded > 0 && _powerGrid?.System != null && _powerGrid.System.FuelUnits > 0f)
            {
                float gridDraw = cansNeeded * 25.0f;
                if (_powerGrid.System.FuelUnits >= gridDraw)
                {
                    _powerGrid.System.State.FuelUnits -= gridDraw;
                    return true;
                }
                else
                {
                    _powerGrid.System.State.FuelUnits = 0f;
                    return true;
                }
            }

            return cansNeeded <= 0;
        }

        private void SetupCvdDiamond()
        {
            if (_cvdDiamond != null) return;

            var catalog = CvdDiamondCatalogLoader.Load(_dataDir, new FileSystemIO());
            var system = new CvdDiamondSynthesisEngine(catalog, new GodotLog());
            if (_campaignDay?.Rng != null)
                system.Rng = _campaignDay.Rng.Fork(CampaignStreamIds.CvdDiamond);

            _cvdDiamond = new CvdDiamondHostSession(system)
            {
                PowerAvailableProvider = () => _powerGrid?.System == null || !_powerGrid.System.IsBrownout,
                CoolingAvailableProvider = () => true,
                FeedstockAvailableProvider = _ => true,
                SubstrateItemAvailableProvider = _ => true,
                OperatorSkillProvider = () => 0f,
                RepairPartsAvailableProvider = () => true
            };
            // Register the authored high-wear consumers (plan §6.8–6.9).
            _cvdDiamond.System.RegisterConsumer("consumer_deep_excavation_cutter");
            _cvdDiamond.System.RegisterConsumer("consumer_precision_lathe_insert");

            var saved = CvdDiamondSaveStore.TryLoad();
            if (saved != null)
            {
                _cvdDiamond.RestoreSave(saved);
                GD.Print("[Ashfall Godot] CVD diamond reactor state restored.");
            }
        }

        private void SetupSoundRanging()
        {
            if (_soundRanging != null) return;

            var catalog = SoundRangingCatalogLoader.Load(_dataDir, new FileSystemIO());
            var system = new SoundRangingThreatEngine(catalog, new GodotLog());
            if (_campaignDay?.Rng != null)
                system.Rng = _campaignDay.Rng.Fork(CampaignStreamIds.SoundRanging);

            _soundRanging = new SoundRangingHostSession(system)
            {
                AtmosphericProfileProvider = () => "atmos_clear_cold",
                SensorNodeOperationalProvider = _ => true
            };

            var saved = SoundRangingSaveStore.TryLoad();
            if (saved != null)
            {
                _soundRanging.RestoreSave(saved);
                GD.Print("[Ashfall Godot] Sound-ranging station state restored.");
            }
        }

        private void SetupAmphibiousDraisine()
        {
            if (_amphibiousDraisine != null) return;

            var catalog = AmphibiousDraisineCatalogLoader.Load(_dataDir, new FileSystemIO());
            var system = new AmphibiousDraisineEngine(catalog, new GodotLog());
            if (_campaignDay?.Rng != null)
                system.Rng = _campaignDay.Rng.Fork(CampaignStreamIds.AmphibiousDraisine);

            _amphibiousDraisine = new AmphibiousDraisineHostSession(system)
            {
                VehicleStateProvider = id => (string.Empty, 10000),
                WorkshopAvailableProvider = () => true,
                PartsAvailableProvider = _ => true,
                MechanicSkillProvider = () => 0f,
                PumpPowerAvailableProvider = () => _powerGrid?.System == null || !_powerGrid.System.IsBrownout
            };

            var saved = AmphibiousDraisineSaveStore.TryLoad();
            if (saved != null)
            {
                _amphibiousDraisine.RestoreSave(saved);
                GD.Print("[Ashfall Godot] Amphibious kit state restored.");
            }
        }

        // ── Save (called by SaveOrchestrator) ─────────────────────────────

        private void SaveSofcPower()
        {
            if (_sofcPower == null) return;
            CaptureSection(
                SofcPowerSaveStore.SectionName,
                SofcPowerSaveStore.TryCapturePersisted(_sofcPower.CaptureSave()));
        }

        private void SaveCvdDiamond()
        {
            if (_cvdDiamond == null) return;
            CaptureSection(
                CvdDiamondSaveStore.SectionName,
                CvdDiamondSaveStore.TryCapturePersisted(_cvdDiamond.CaptureSave()));
        }

        private void SaveSoundRanging()
        {
            if (_soundRanging == null) return;
            CaptureSection(
                SoundRangingSaveStore.SectionName,
                SoundRangingSaveStore.TryCapturePersisted(_soundRanging.CaptureSave()));
        }

        private void SaveAmphibiousDraisine()
        {
            if (_amphibiousDraisine == null) return;
            CaptureSection(
                AmphibiousDraisineSaveStore.SectionName,
                AmphibiousDraisineSaveStore.TryCapturePersisted(_amphibiousDraisine.CaptureSave()));
        }

        // ── Panels (Plan 122-125 Phase 9): created hidden; opened via the
        //    expanded-panel route. Presentation only.
        private void EnsurePlans122to125Panels()
        {
            if (_sofcPower != null && _sofcPanel == null)
            {
                _sofcPanel = new SolidOxideFuelCellPanel();
                _sofcPanel.Bind(_sofcPower);
                _sofcPanel.Visible = false;
                AddChild(_sofcPanel);
            }
            if (_cvdDiamond != null && _cvdPanel == null)
            {
                _cvdPanel = new CvdDiamondPanel();
                _cvdPanel.Bind(_cvdDiamond);
                _cvdPanel.Visible = false;
                AddChild(_cvdPanel);
            }
            if (_soundRanging != null && _soundRangingPanel == null)
            {
                _soundRangingPanel = new SoundRangingPanel();
                _soundRangingPanel.Bind(_soundRanging);
                _soundRangingPanel.Visible = false;
                AddChild(_soundRangingPanel);
            }
            if (_amphibiousDraisine != null && _amphibiousPanel == null)
            {
                _amphibiousPanel = new AmphibiousDraisinePanel();
                _amphibiousPanel.Bind(_amphibiousDraisine);
                _amphibiousPanel.Visible = false;
                AddChild(_amphibiousPanel);
            }
        }

        private void OpenSofcPowerPanel()
        {
            SetupSofcPower();
            EnsurePlans122to125Panels();
            if (_sofcPanel != null) { _sofcPanel.Visible = true; _sofcPanel.RefreshView(); }
        }

        private void OpenCvdDiamondPanel()
        {
            SetupCvdDiamond();
            EnsurePlans122to125Panels();
            if (_cvdPanel != null) { _cvdPanel.Visible = true; _cvdPanel.RefreshView(); }
        }

        private void OpenSoundRangingPanel()
        {
            SetupSoundRanging();
            EnsurePlans122to125Panels();
            if (_soundRangingPanel != null) { _soundRangingPanel.Visible = true; _soundRangingPanel.RefreshView(); }
        }

        private void OpenAmphibiousDraisinePanel()
        {
            SetupAmphibiousDraisine();
            EnsurePlans122to125Panels();
            if (_amphibiousPanel != null) { _amphibiousPanel.Visible = true; _amphibiousPanel.RefreshView(); }
        }
    }
}
