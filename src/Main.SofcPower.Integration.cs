// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using AtomicWar.GodotApp.UI;
using Godot;
using System;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SofcPowerHostSession? _sofcPower;
        private SolidOxideFuelCellPanel? _sofcPanel;

        public SofcPowerHostSession? SofcPowerSession => _sofcPower;

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

        // ── Save (called by SaveOrchestrator) ─────────────────────────────

        private void SaveSofcPower()
        {
            if (_sofcPower == null) return;
            CaptureSection(
                SofcPowerSaveStore.SectionName,
                SofcPowerSaveStore.TryCapturePersisted(_sofcPower.CaptureSave()));
        }

        private void OpenSofcPowerPanel()
        {
            SetupSofcPower();
            EnsurePlans122to125Panels();
            if (_sofcPanel != null) { _sofcPanel.Visible = true; _sofcPanel.RefreshView(); }
        }

    }
}
