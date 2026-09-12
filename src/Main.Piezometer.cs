// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 189 — Piezometer intake advisory host wire
// Subsystems   : aquifer monitoring network construction, daily advisory
//                handoff into the water-treatment intake gate, dedicated save.
// Contract     : the piezometer forecasts; WaterTreatmentSystem owns liters and
//                admits/refuses intake. No new liter ledger is created here.
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PiezometerHostSession? _piezometer;

        /// <summary>Panel/CLI-facing session.</summary>
        public PiezometerHostSession EnsurePiezometerSession()
        {
            SetupPiezometer();
            return _piezometer!;
        }

        private void SetupPiezometer()
        {
            if (_piezometer != null) return;

            var engine = new AquiferPiezometerEngine(log: new GodotLog())
            {
                DayProvider = () => _simDay
            };

            var inv = _inventory?.Inventory;
            if (inv != null)
            {
                engine.BindInventory(
                    itemId => inv.CountById(itemId),
                    (itemId, count) => inv.TryConsumeById(itemId, count));
            }

            _piezometer = new PiezometerHostSession(engine);
            _piezometer.LoadCatalog(_dataDir);

            var saved = PiezometerSaveStore.TryLoad();
            if (saved != null)
            {
                _piezometer.RestoreSave(saved);
                GD.Print("[Ashfall Godot] Aquifer piezometer network restored.");
            }
        }

        /// <summary>Install the monitoring network (consumes catalog install materials).</summary>
        public string ConstructPiezometerNetwork()
        {
            SetupPiezometer();
            return _piezometer?.ConstructNetwork() ?? "Piezometer network unavailable.";
        }

        /// <summary>
        /// Plan 189 bridge: piezometer <c>BuildAdvisory</c> → water-treatment
        /// <c>RegisterContaminationAdvisory</c>. Runs before the water tick so a
        /// blocked source refuses intake in the same day.
        /// </summary>
        private void TickPiezometerAdvisoryBridge(int day)
        {
            SetupPiezometer();
            if (_piezometer == null || _waterTreatment?.System == null) return;
            _piezometer.PublishAdvisory(_waterTreatment.System, day);
        }

        private void SavePiezometer()
        {
            if (_piezometer == null) return;
            CaptureSection(
                PiezometerSaveStore.SectionName,
                PiezometerSaveStore.TryCapturePersisted(_piezometer.CaptureSave()));
        }
    }
}
