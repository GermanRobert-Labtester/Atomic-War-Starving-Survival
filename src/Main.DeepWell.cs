// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// B5–B8 Phase 6 (Plan 66 §9.8): shelter deep-well pump — standard triad
    /// (Setup / Save / Tick) plus the build/service routes. The build route is
    /// the §15.3 discipline in one place: live capability query (research is
    /// permission) → canonical item bill consumed atomically (the physical
    /// act) → Core build commit. Unlocking research never grants the well.
    /// </summary>
    public partial class Main
    {
        private DeepWellHostSession? _deepWell;

        private void SetupDeepWell()
        {
            if (_deepWell != null) return;
            SetupCampaignDay();
            SetupPowerGrid();
            SetupWaterTreatment();

            var saved = DeepWellSaveStore.TryLoad() ?? new DeepWellState();
            var system = new DeepWellSystem(
                _powerGrid.System,
                _waterTreatment.System,
                new GodotLog());
            system.RestoreState(saved);
            _deepWell = new DeepWellHostSession(system);
            // B5–B8 expansion (§27): the pump-worn edge journals once per wear
            // cycle — the fact is the well's; the journal owns reporting.
            system.OnPumpWorn += s =>
            {
                _journal?.TryAddRawEntry("deepwell_pump_worn",
                    "MAINTENANCE: The deep-well pump is wearing down — yield derating. A machine_oil service restores it.",
                    null!, _simDay);
            };
        }

        private void SaveDeepWell()
        {
            if (_deepWell != null)
                CaptureSection("deep_well", DeepWellSaveStore.TryCapturePersisted(_deepWell.System.CaptureState()));
        }

        /// <summary>Daily tick — pumps raw water into the treatment intake.</summary>
        private void TickDeepWell(int day)
        {
            _deepWell?.TickDay(day);
            if (_deepWell != null && _deepWell.IsDirty)
                SaveDeepWell();
        }

        /// <summary>
        /// Build route: live capability query → canonical bill (hydraulic
        /// actuator + mechanical parts) consumed atomically → Core commit.
        /// Blocked reasons name the exact missing requirement; nothing is
        /// consumed on a blocked build.
        /// </summary>
        public bool DeepWellTryBuild()
        {
            SetupDeepWell();
            var inv = _inventory.Inventory;

            if (_sharedResearch == null || !_sharedResearch.HasCapability(DeepWellSystem.RequiredKnowledgeId))
            {
                ObserveSigil("deepwell.build_blocked_missing_knowledge");
                return false;
            }
            if (inv.CountById(DeepWellSystem.BuildItemId) < 1
                || inv.CountById("mechanical_parts") < 2)
            {
                ObserveSigil("deepwell.build_blocked_missing_items");
                return false;
            }

            if (!_deepWell!.TryBuild(hasRequiredCapability: true, out var reason))
            {
                ObserveSigil("deepwell.build_blocked_" + reason);
                return false;
            }

            inv.TryConsumeById(DeepWellSystem.BuildItemId, 1);
            inv.TryConsumeById("mechanical_parts", 2);
            ObserveSigil("deepwell.built");
            return true;
        }

        /// <summary>Service route: canonical machine oil consumed once on commit.</summary>
        public bool DeepWellTryService()
        {
            SetupDeepWell();
            var inv = _inventory.Inventory;
            if (inv.CountById(DeepWellSystem.MaintenanceItemId) < 1)
            {
                ObserveSigil("deepwell.service_blocked_missing_item");
                return false;
            }
            if (!_deepWell!.PerformMaintenance(out var reason))
            {
                ObserveSigil("deepwell.service_blocked_" + reason);
                return false;
            }
            inv.TryConsumeById(DeepWellSystem.MaintenanceItemId, 1);
            ObserveSigil("deepwell.serviced");
            return true;
        }
    }
}
