// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private WaterSourcesHostSession? _waterSources;

        /// <summary>
        /// Returns the panel adapter over the existing well, condenser, and
        /// piezometer authorities. Their individual save owners remain intact.
        /// </summary>
        public WaterSourcesHostSession EnsureWaterSourcesSession()
        {
            if (_waterSources != null) return _waterSources;

            SetupWaterTreatment();
            SetupInventory();
            SetupPowerGrid();
            SetupWorld();
            EnsureSharedResearch();
            SetupDeepWell();
            SetupWaterCondenser();
            SetupPiezometer();

            _waterSources = new WaterSourcesHostSession(
                _inventory.Inventory,
                _powerGrid.System,
                _sharedResearch,
                _deepWell!,
                _waterCondenser!,
                _piezometer!);
            return _waterSources;
        }

        private void BindWaterSourcesPanel()
        {
            if (_waterTreatmentPanel == null) return;
            _waterTreatmentPanel.BindWaterSources(EnsureWaterSourcesSession());
        }
    }
}
