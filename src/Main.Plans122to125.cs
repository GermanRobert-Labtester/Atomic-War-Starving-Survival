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
                FuelConsumer = units => true, // inventory owner binds the real check in Phase 9
                FuelQualityProvider = () => SofcPowerCatalog.QualityClean,
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
