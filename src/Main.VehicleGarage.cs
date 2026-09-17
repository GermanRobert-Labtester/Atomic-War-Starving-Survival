// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Wave 8 B2 — Vehicle Garage player route (Plan 50)
// Subsystems   : presentation glue only. VehicleGarageSystem, its host drive
//                (Main.Plans50_53), save section, and the expedition
//                decoration/wear seam already exist; this file adds the
//                missing player-facing garage panel. No new authority.
// ============================================================================
using Godot;
using Ashfall.Core.Expeditions;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private UI.VehicleGaragePanel? _vehicleGaragePanel;
        private VehicleGarageSystem? _vehicleGaragePanelBoundSystem;

        private void EnsureVehicleGaragePanel()
        {
            var garage = EnsureVehicleGarage();
            SetupExpeditions();
            SetupInventory();

            if (_vehicleGaragePanel == null)
            {
                _vehicleGaragePanel = new UI.VehicleGaragePanel();
                _vehicleGaragePanel.Bind(garage, _expeditions.Vehicles, _inventory.Inventory);
                _vehicleGaragePanelBoundSystem = garage;
                _vehicleGaragePanel.Visible = false;
                AddChild(_vehicleGaragePanel);
                return;
            }

            if (!ReferenceEquals(_vehicleGaragePanelBoundSystem, garage))
            {
                _vehicleGaragePanel.Bind(garage, _expeditions.Vehicles, _inventory.Inventory);
                _vehicleGaragePanelBoundSystem = garage;
            }
        }

        private void OpenVehicleGaragePanel()
        {
            SetupInventory();
            SetupExpeditions();
            EnsureVehicleGaragePanel();
            if (_vehicleGaragePanel != null)
            {
                _vehicleGaragePanel.Visible = true;
                _vehicleGaragePanel.RefreshView();
            }
        }
    }
}