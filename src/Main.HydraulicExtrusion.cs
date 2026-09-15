// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 140 Phase 2 — hydraulic extrusion host wire
// Subsystems   : catalog load, machine registration, power/water availability,
//                batch lifecycle, dedicated save section. Manufacturing only —
//                inventory owns the produced stock.
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Foundry;
using Ashfall.Core.Random;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private HydraulicExtrusionHostSession? _hydraulicExtrusion;
        private HydraulicExtrusionPanel? _hydraulicExtrusionPanel;

        /// <summary>Panel-facing session (Plan 140 Phase 3).</summary>
        public HydraulicExtrusionHostSession EnsureHydraulicExtrusionSession()
        {
            SetupHydraulicExtrusion();
            return _hydraulicExtrusion!;
        }

        private void SetupHydraulicExtrusion()
        {
            if (_hydraulicExtrusion != null) return;

            var catalog = HydraulicExtrusionCatalogLoader.Load(_dataDir, new FileSystemIO());
            var system = new HydraulicExtrusionEngine(catalog, new GodotLog());

            if (_campaignDay?.Rng != null)
                system.Rng = _campaignDay.Rng.Fork(CampaignStreamIds.HydraulicExtrusion);

            system.RegisterMachine("standard_press");
            system.RegisterMachine("heavy_press");
            system.RegisterMachine("precision_press");

            _hydraulicExtrusion = new HydraulicExtrusionHostSession(system)
            {
                EnergyAvailableBpProvider = ResolveExtrusionPowerBp,
                CoolingAvailableBpProvider = ResolveExtrusionCoolingBp
            };

            var saved = HydraulicExtrusionSaveStore.TryLoad();
            if (saved != null)
            {
                _hydraulicExtrusion.RestoreSave(saved);
                GD.Print("[Ashfall Godot] Hydraulic extrusion restored.");
            }

            if (_hydraulicExtrusionPanel != null && _hydraulicExtrusionPanel.IsInsideTree())
                RemoveChild(_hydraulicExtrusionPanel);
            _hydraulicExtrusionPanel = new HydraulicExtrusionPanel
            {
                AppDayProvider = () => _simDay
            };
            _hydraulicExtrusionPanel.Bind(_hydraulicExtrusion);
            _hydraulicExtrusionPanel.Visible = false;
            AddChild(_hydraulicExtrusionPanel);
        }

        /// <summary>
        /// Available power for the extrusion press: full when the grid is stable,
        /// constrained under brownout. The press cannot run heavy classes on a
        /// sagging grid.
        /// </summary>
        private int ResolveExtrusionPowerBp()
        {
            var grid = _powerGrid?.System;
            if (grid == null) return 100;
            return grid.IsBrownout ? 40 : 100;
        }

        /// <summary>
        /// Available cooling water from the canonical water authority. No clean or
        /// raw water means no cooling, which blocks the cooling-heavy profiles.
        /// </summary>
        private int ResolveExtrusionCoolingBp()
        {
            var water = _waterTreatment?.System?.State;
            if (water == null) return 100;
            float available = water.cleanWater + water.rawWater;
            return Math.Clamp((int)(available / 40f * 100f), 0, 100);
        }

        public string StartHydraulicExtrusionBatch(string productProfileId, string machineId, int units)
        {
            SetupHydraulicExtrusion();
            if (_hydraulicExtrusion == null) return "Hydraulic extrusion is unavailable.";
            return _hydraulicExtrusion.StartBatch(productProfileId, machineId, units, _simDay);
        }

        public string AdvanceHydraulicExtrusionBatch(string batchId)
        {
            SetupHydraulicExtrusion();
            if (_hydraulicExtrusion == null) return "Hydraulic extrusion is unavailable.";
            return _hydraulicExtrusion.AdvanceBatch(batchId);
        }

        public string CompleteHydraulicExtrusionBatch(string batchId)
        {
            SetupHydraulicExtrusion();
            if (_hydraulicExtrusion == null) return "Hydraulic extrusion is unavailable.";
            return _hydraulicExtrusion.CompleteBatch(batchId, operatorSkill: 0.5);
        }

        private void SaveHydraulicExtrusion()
        {
            if (_hydraulicExtrusion == null) return;
            CaptureSection(
                HydraulicExtrusionSaveStore.SectionName,
                HydraulicExtrusionSaveStore.TryCapturePersisted(_hydraulicExtrusion.CaptureSave()));
        }
    }
}
