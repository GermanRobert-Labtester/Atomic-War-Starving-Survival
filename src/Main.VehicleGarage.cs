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

using Ashfall.Core;
using Ashfall.Core.Audio;
using Ashfall.Core.Factions;
using Ashfall.Core.Needs;
using AtomicWar.GodotApp.Audio;
using System;
using System.IO;

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
        private VehicleGarageSystem? _vehicleGarage;

        private bool _vehicleGarageDirty;

        public VehicleGarageSystem? VehicleGarageSystem => _vehicleGarage;

        // ── Plan 50: Vehicle Garage ───────────────────────────────────────

        public VehicleGarageSystem EnsureVehicleGarage()
        {
            if (_vehicleGarage != null) return _vehicleGarage;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("vehicle_garage") : new SeededRng(50);
            _vehicleGarage = new VehicleGarageSystem(null, rng);

            string path = Path.Combine(_dataDir, "vehicle_modifications.json");
            if (System.IO.File.Exists(path))
            {
                try
                {
                    string json = System.IO.File.ReadAllText(path);
                    var catalog = VehicleGarageCatalogLoader.Load(json, new SystemTextJsonSerializer());
                    _vehicleGarage.LoadCatalog(catalog);
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[Ashfall Godot] Failed to load vehicle_modifications.json: {ex.Message}");
                }
            }

            string armorPath = Path.Combine(_dataDir, VehicleArmorGradeCatalogLoader.FileName);
            if (System.IO.File.Exists(armorPath))
            {
                try
                {
                    string armorJson = System.IO.File.ReadAllText(armorPath);
                    var loaded = VehicleArmorGradeCatalogLoader.LoadJson(armorJson, new SystemTextJsonSerializer());
                    if (loaded.Catalog != null && !loaded.HasErrors)
                        _vehicleGarage.LoadArmorCatalog(loaded.Catalog);
                    else
                        GD.PrintErr($"[Ashfall Godot] Failed to load {VehicleArmorGradeCatalogLoader.FileName}: {string.Join("; ", loaded.Errors)}");
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[Ashfall Godot] Failed to load {VehicleArmorGradeCatalogLoader.FileName}: {ex.Message}");
                }
            }

            // Terrain is a read-only classification from the existing vehicle
            // owner. The optional resolver keeps early pure-Core/bootstrap paths
            // neutral while enforcing authored terrain gates in the live garage.
            _vehicleGarage.VehicleTerrainResolver = id => _expeditions?.Vehicles.GetDefinition(id)?.terrain_type;
            BindVehicleGarageArmorMaterialQuality();

            var saved = VehicleGarageSaveStore.TryLoad();
            if (saved != null)
            {
                _vehicleGarage.RestoreState(saved);
            }

            return _vehicleGarage;
        }

        private void SetupVehicleGarage() => EnsureVehicleGarage();

        private void SaveVehicleGarage()
        {
            if (_vehicleGarage == null) return;
            var state = _vehicleGarage.CaptureState();
            string payload = VehicleGarageSaveStore.TryCapturePersisted(state);
            if (CaptureSection(VehicleGarageSaveStore.SectionName, payload))
            {
                _vehicleGarageDirty = false;
            }
        }

    }
}