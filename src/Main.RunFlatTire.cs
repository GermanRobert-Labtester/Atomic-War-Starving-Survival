// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 141 Phase 2 — run-flat tire host wire
// Subsystems   : catalog load, workshop/inventory gating, hazard + heat
//                commands, weather ambient, dedicated save section. Vehicle
//                state only — expedition movement stays with the expedition.
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Random;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private RunFlatTireHostSession? _runFlatTire;
        private RunFlatTirePanel? _runFlatTirePanel;

        /// <summary>Panel-facing session (Plan 141 Phase 3).</summary>
        public RunFlatTireHostSession EnsureRunFlatTireSession()
        {
            SetupRunFlatTire();
            return _runFlatTire!;
        }

        private void SetupRunFlatTire()
        {
            if (_runFlatTire != null) return;

            var catalog = RunFlatTireCatalogLoader.Load(_dataDir, new FileSystemIO());
            var system = new RunFlatTireEngine(catalog, new GodotLog());

            if (_campaignDay?.Rng != null)
                system.Rng = _campaignDay.Rng.Fork(CampaignStreamIds.RunFlatTire);

            _runFlatTire = new RunFlatTireHostSession(system)
            {
                WorkshopAvailableProvider = ResolveRunFlatWorkshopAvailable,
                PartsAvailableProvider = RunFlatPartsAvailable,
                AmbientTempProvider = ResolveRunFlatAmbientC
            };

            var saved = RunFlatTireSaveStore.TryLoad();
            if (saved != null)
            {
                _runFlatTire.RestoreSave(saved);
                GD.Print("[Ashfall Godot] Run-flat wheel state restored.");
            }

            if (_runFlatTirePanel != null && _runFlatTirePanel.IsInsideTree())
                RemoveChild(_runFlatTirePanel);
            _runFlatTirePanel = new RunFlatTirePanel();
            _runFlatTirePanel.Bind(_runFlatTire);
            _runFlatTirePanel.Visible = false;
            AddChild(_runFlatTirePanel);
        }

        /// <summary>A workshop requires a stable grid; a brownout stops tire fitting.</summary>
        private bool ResolveRunFlatWorkshopAvailable()
        {
            var grid = _powerGrid?.System;
            return grid == null || !grid.IsBrownout;
        }

        /// <summary>True when every authored install item is on hand in the inventory.</summary>
        private bool RunFlatPartsAvailable(string profileId)
        {
            var profile = _runFlatTire?.System.Catalog.Get(profileId);
            if (profile == null || profile.required_item_ids.Count == 0) return true;
            if (_inventory == null) return true;
            foreach (var itemId in profile.required_item_ids)
            {
                if (!_inventory.Inventory.HasSufficient(itemId, 1)) return false;
            }
            return true;
        }

        /// <summary>Ambient temperature from the weather authority (cold penalties lower it).</summary>
        private int ResolveRunFlatAmbientC()
        {
            float penalty = _world?.Weather?.GetTemperaturePenaltyCelsius() ?? 0f;
            return RunFlatTireEngine.AmbientC + (int)penalty;
        }

        /// <summary>
        /// Fits a run-flat profile and, on success, consumes the authored install
        /// kit from the canonical inventory. Inventory remains the stock authority.
        /// </summary>
        public string InstallRunFlatUpgrade(string vehicleId, string vehicleTag, string profileId)
        {
            SetupRunFlatTire();
            if (_runFlatTire == null) return "Run-flat workshop is unavailable.";

            string message = _runFlatTire.Install(vehicleId, vehicleTag, profileId, skill: 0.5);
            if (_runFlatTire.LastInstallCommitted && _inventory != null)
            {
                var profile = _runFlatTire.System.Catalog.Get(profileId);
                if (profile != null)
                {
                    foreach (var itemId in profile.required_item_ids)
                        _inventory.Inventory.TryConsumeById(itemId, 1);
                }
            }
            return message;
        }

        public string ApplyRunFlatHazard(string vehicleId, string hazardClass, int speedKph)
        {
            SetupRunFlatTire();
            if (_runFlatTire == null) return "Run-flat workshop is unavailable.";
            return _runFlatTire.ApplyHazard(vehicleId, hazardClass, speedKph);
        }

        public string TickRunFlatHeat(string vehicleId, int speedKph, int loadBp)
        {
            SetupRunFlatTire();
            if (_runFlatTire == null) return "Run-flat workshop is unavailable.";
            return _runFlatTire.TickHeat(vehicleId, speedKph, loadBp);
        }

        public string ServiceRunFlat(string vehicleId)
        {
            SetupRunFlatTire();
            if (_runFlatTire == null) return "Run-flat workshop is unavailable.";
            return _runFlatTire.Repair(vehicleId, skill: 0.5);
        }

        private void SaveRunFlatTire()
        {
            if (_runFlatTire == null) return;
            CaptureSection(
                RunFlatTireSaveStore.SectionName,
                RunFlatTireSaveStore.TryCapturePersisted(_runFlatTire.CaptureSave()));
        }
    }
}
